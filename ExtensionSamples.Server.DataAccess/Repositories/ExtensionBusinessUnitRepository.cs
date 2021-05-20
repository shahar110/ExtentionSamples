using Storix.Contract.Entities;
using Storix.DataAccess;
using Storix.DataAccess.Repositories;
using Storix.Model.Entities;
using Storix.Model.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExtensionSamples.Server.DataAccess.Repositories
{
    public class ExtensionBusinessUnitRepository : IBusinessUnitRepository
    {
        private readonly IBusinessUnitRepository _coreImplementation;
        private readonly Storix_PortalEntities _context;
        public ExtensionBusinessUnitRepository(Storix_PortalEntities context, IBusinessUnitRepository coreImplementation)
        {
            _coreImplementation = coreImplementation;
            _context = context;
        }

        public IBusinessUnit Delete(int id)
        {
            return _coreImplementation.Delete(id);
        }

        public IBusinessUnit Fetch(int id)
        {
            var coreBu = _coreImplementation.Fetch(id);
            var extensionBu = _context.BusinessUnitExtensions.FirstOrDefault(b => b.BusinessUnitId == id);
            if (extensionBu != null)
            {
                coreBu.ExtraData = new Dictionary<string, string>()
                {
                    { "StationType", extensionBu.StationType },
                    { "StationFormat", extensionBu.StationFormat },
                    { "CustomerId", extensionBu.CustomerId },
                    { "RecipientId", extensionBu.RecipientId },
                    { "ChannelId", extensionBu.ChannelId },
                    { "CashBookId", extensionBu.CashBookId },
                    { "ProfitCenterId", extensionBu.ProfitCenterId },
                    { "CostCenterId", extensionBu.CostCenterId }
                };
            }

            return coreBu;
        }

        public IEnumerable<IBusinessUnit> FetchAll()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<IBusinessUnit> FetchByPaging(int startIndex, int endIndex)
        {
            throw new NotImplementedException();
        }

        public IBusinessUnitHeader FetchHeader(int businessUnitID)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<IBusinessUnit> GetAllTenantsRootBusinessUnits()
        {
            throw new NotImplementedException();
        }

        public IBusinessUnit GetRootBusinessUnit()
        {
            throw new NotImplementedException();
        }

        public IBusinessUnit Save(IBusinessUnit entity)
        {
            var coreBu = _coreImplementation.Save(entity);
            var buExtensionDTO = new BusinessUnitExtension
            {
                StationType = entity.ExtraData["StationType"],
                StationFormat = entity.ExtraData["StationFormat"],
                CustomerId = entity.ExtraData["CustomerId"],
                RecipientId = entity.ExtraData["RecipientId"],
                ChannelId = entity.ExtraData["ChannelId"],
                CashBookId = entity.ExtraData["CashBookId"],
                ProfitCenterId = entity.ExtraData["ProfitCenterId"],
                CostCenterId = entity.ExtraData["CostCenterId"]
            };

            //businessUnitDTO.TS_REC = DateTime.Now;
            _context.BusinessUnitExtensions.Add(buExtensionDTO);
            _context.SaveChanges();

            return coreBu;
        }

        public IEnumerable<IBusinessUnit> Search(string externalCode)
        {
            throw new NotImplementedException();
        }

        public IBusinessUnit Update(IBusinessUnit entity)
        {
            _coreImplementation.Update(entity);
            var buExtensionDTO = _context.BusinessUnitExtensions.FirstOrDefault(o => o.BusinessUnitId == entity.ID);

            buExtensionDTO.StationType = entity.ExtraData["StationType"];
            buExtensionDTO.StationFormat = entity.ExtraData["StationFormat"];
            buExtensionDTO.CustomerId = entity.ExtraData["CustomerId"];
            buExtensionDTO.RecipientId = entity.ExtraData["RecipientId"];
            buExtensionDTO.ChannelId = entity.ExtraData["ChannelId"];
            buExtensionDTO.CashBookId = entity.ExtraData["CashBookId"];
            buExtensionDTO.ProfitCenterId = entity.ExtraData["ProfitCenterId"];
            buExtensionDTO.CostCenterId = entity.ExtraData["CostCenterId"];
            //buExtensionDTO.TS_REC = DateTime.Now;

            _context.Entry(buExtensionDTO).State = System.Data.Entity.EntityState.Modified;
            _context.Entry(buExtensionDTO).Property(x => x.BusinessUnitId).IsModified = false;
            return null;
        }
    }
}
