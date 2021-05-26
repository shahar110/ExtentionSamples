using System;
using System.Collections.Generic;
using System.Linq;
using ExtensionSamples.Server.Contract.Requests;
using ExtensionSamples.Server.Contract.Responses;
using Storix.API.Model.ManagementServices;
using Storix.API.Model.RequestValidation;
using Storix.Common.ExtensionMethods;
using Storix.Common.Model.Exceptions;
using Storix.Contract.Base.Requests;
using Storix.Contract.Entities;
using Storix.Contract.Requests.ManagementRequests.EntityManagementRequests;
using Storix.Model;
using Storix.Model.Entities;
using Storix.Model.ExtensionMethods;
using Storix.Model.Repositories;
using Storix.Model.ServiceActionExecuters;

namespace ExtensionSamples.Server.API.ManagementServices
{
    public class ExternalBusinessUnitManagementService : ProcessManagementService<ExternalBusinessUnitManagementRequest,
                                                                            ExternalBusinessUnitManagementResponse>
    {
        private readonly IRequestValidatorFactory _requestValidatorFactory;
        private readonly IEntityActionExecuterFactory _entityActionExecuterFactory;
        private readonly IBusinessUnitRepository _buRepository;
        private readonly IDateTimeProvider _timeProvider;
        private IBusinessUnit _entityAfter;
        private readonly IEnvironmentContext _environmentContext;
        private BusinessUnitManagementRequest _coreBuRequest;
        private readonly string CoreValidatorCreateOrUpdateService = "BusinessUnitCreateOrUpdateRequestValidator";
        private readonly string CoreExecuterCreateOrUpdateService = "BusinessUnitCreateOrUpdateActionExecuter";

        public ExternalBusinessUnitManagementService(IBusinessUnitRepository buRepository,
                                                                        IEntityActionExecuterFactory entityActionExecuterFactory,
                                                                        IRequestValidatorFactory requestValidatorFactory,
                                                                        IDateTimeProvider timeProvider,
                                                                        IEnvironmentContext environmentContext)
        {
            _requestValidatorFactory = requestValidatorFactory;
            _entityActionExecuterFactory = entityActionExecuterFactory;
            _buRepository = buRepository;
            _timeProvider = timeProvider;
            _environmentContext = environmentContext;
        }

        protected override void Execute(RequestHeader header, ExternalBusinessUnitManagementRequest request)
        {
            var entity = _coreBuRequest.Entity.MapTo<IBusinessUnit>();
            if (_coreBuRequest.ExtraData.IsNotEmpty())
                entity.ExtraData = _coreBuRequest.ExtraData;

            entity.FillOrganizationID(_environmentContext.OrganizationId);

            var entityActionExecuter = _entityActionExecuterFactory.Resolve<IBusinessUnit>(CoreExecuterCreateOrUpdateService);
            try
            {
                _entityAfter = entityActionExecuter.Execute(entity);
            }
            finally
            {
                _entityActionExecuterFactory.Release(entityActionExecuter);
            }
        }

        protected override void ValidateRequest(ExternalBusinessUnitManagementRequest request)
        {
            var parentBu = _buRepository.Search(request.ParentId).FirstOrDefault();
            if (parentBu == null || parentBu.HasNoValue())
            {
                throw new RequestValidationFailedException(nameof(request.ParentId));
            }

            if (parentBu.Level.HasNoValue())
            {
                throw new RequestValidationFailedException(nameof(parentBu.Level));
            }

            var currentBuLevel = parentBu.Level.Value - 1;
            if (currentBuLevel == 1)
            {
                if (request.ExternalId.IsEmpty())
                {
                    throw new RequestValidationFailedException(nameof(request.ExternalId));
                }

                if (request.StationType.IsEmpty())
                {
                    throw new RequestValidationFailedException(nameof(request.StationType));
                }

                if (request.StationFormat.IsEmpty())
                {
                    throw new RequestValidationFailedException(nameof(request.StationFormat));
                }

                if (request.CustomerId.IsEmpty())
                {
                    throw new RequestValidationFailedException(nameof(request.CustomerId));
                }

                if (request.RecipientId.IsEmpty())
                {
                    throw new RequestValidationFailedException(nameof(request.RecipientId));
                }

                if (request.ChannelId.IsEmpty())
                {
                    throw new RequestValidationFailedException(nameof(request.ChannelId));
                }

                if (request.CashBookId.IsEmpty())
                {
                    throw new RequestValidationFailedException(nameof(request.CashBookId));
                }

                if (request.ProfitCenterId.IsEmpty())
                {
                    throw new RequestValidationFailedException(nameof(request.ProfitCenterId));
                }

                if (request.CostCenterId.IsEmpty())
                {
                    throw new RequestValidationFailedException(nameof(request.CostCenterId));
                }
            }

            _coreBuRequest = CreateCoreBuRequest(request, parentBu);
            ValidateCoreBuRequest();
        }

        private BusinessUnitManagementRequest CreateCoreBuRequest(ExternalBusinessUnitManagementRequest request, IBusinessUnit parentBu)
        {
            var bu = _buRepository.Search(request.ExternalId).FirstOrDefault();
            var buExtraData = new Dictionary<string, string>()
            {
                { "StationType", request.StationType },
                { "StationFormat", request.StationFormat },
                { "CustomerId", request.CustomerId },
                { "RecipientId", request.RecipientId },
                { "ChannelId", request.ChannelId },
                { "CashBookId", request.CashBookId },
                { "ProfitCenterId", request.ProfitCenterId },
                { "CostCenterId", request.CostCenterId }
            };

            

            return new BusinessUnitManagementRequest
            {
                Entity = new BusinessUnit
                {
                    Header = new BusinessUnitHeader
                    {
                        ID = bu?.ID ?? 0,
                        Name = new Storix.Contract.ValueObjects.Name
                        {
                            ShortName = request.Name
                        },

                        HierarchyNode = new Storix.Contract.ValueObjects.HierarchyNode
                        {
                            ParentId = Convert.ToUInt64(parentBu.ID),
                            Level = (parentBu.Level.Value - 1)
                        },
                    },

                    ExternalCode = request.ExternalId,
                    OpenDate = _timeProvider.Now,
                },

                ExtraData = buExtraData,
                ActionType = "CreateOrUpdate"
            };
        }

        private void ValidateCoreBuRequest()
        {
            var validator = _requestValidatorFactory
                    .Resolve<BusinessUnitManagementRequest>(CoreValidatorCreateOrUpdateService);
            try
            {
                validator.Validate(_coreBuRequest);
            }
            finally
            {
                _requestValidatorFactory.Release(validator);
            }
        }
    }
}
