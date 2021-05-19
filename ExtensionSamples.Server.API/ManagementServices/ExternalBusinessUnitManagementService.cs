﻿using System;
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
            var coreBuRequest = generateCoreBuRequest(request);
            validateCoreBuRequest(coreBuRequest);

            var entity = coreBuRequest.Entity.MapTo<IBusinessUnit>();
            if (coreBuRequest.ExtraData.IsNotEmpty())
                entity.ExtraData = coreBuRequest.ExtraData;

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
            if (request.ExternalId.IsEmpty())
            {
                throw new RequestValidationFailedException(nameof(request.ExternalId));
            }
        }

        private BusinessUnitManagementRequest generateCoreBuRequest(ExternalBusinessUnitManagementRequest request)
        {
            var bu = _buRepository.Search(request.ExternalId).FirstOrDefault();

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
                            ParentId = request.ParentId,
                            Level = request.LevelId
                        },
                    },

                    ExternalCode = request.ExternalId,
                    OpenDate = _timeProvider.Now,
                },

                ExtraData = request.ExtraData,
                ActionType = "CreateOrUpdate"
            };
        }

        private void validateCoreBuRequest(BusinessUnitManagementRequest coreBuRequest)
        {
            var validator = _requestValidatorFactory
                    .Resolve<BusinessUnitManagementRequest>(CoreValidatorCreateOrUpdateService);
            try
            {
                validator.Validate(coreBuRequest);
            }
            finally
            {
                _requestValidatorFactory.Release(validator);
            }
        }
    }
}