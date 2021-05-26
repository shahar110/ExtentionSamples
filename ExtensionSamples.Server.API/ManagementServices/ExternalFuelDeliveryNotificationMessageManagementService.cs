using System;
using System.Linq;
using ExtensionSamples.Server.Contract.Requests;
using Storix.API.Model.ManagementServices;
using Storix.API.Model.RequestValidation;
using Storix.Common.ExtensionMethods;
using Storix.Common.Model.Exceptions;
using Storix.Contract.Base.Requests;
using Storix.Contract.Base.Responses;
using Storix.Contract.Entities;
using Storix.Contract.Requests.ManagementRequests.EntityManagementRequests;
using Storix.Model;
using Storix.Model.Entities;
using Storix.Model.ExtensionMethods;
using Storix.Model.Repositories;
using Storix.Model.ServiceActionExecuters;

namespace ExtensionSamples.Server.API.ManagementServices
{
    public class ExternalFuelDeliveryNotificationMessageManagementService : ProcessManagementService<ExternalFuelDeliveryNotificationMessageRequest, Response>
    {
        private readonly IRequestValidatorFactory _requestValidatorFactory;
        private readonly IEntityActionExecuterFactory _entityActionExecuterFactory;
        private readonly IBusinessUnitRepository _buRepository;
        private readonly IDateTimeProvider _timeProvider;
        private readonly IEnvironmentContext _environmentContext;
        private RemoteNotificationMessageManagementRequest _coreRemoteNotificationRequest;
        private const string CoreValidatorCreateService = "RemoteNotificationMessageCreateRequestValidator";
        private const string CoreExecuterCreateService = "RemoteNotificationMessageCreateActionExecuter";

        public ExternalFuelDeliveryNotificationMessageManagementService(IBusinessUnitRepository buRepository,
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

        protected override void Execute(RequestHeader header, ExternalFuelDeliveryNotificationMessageRequest request)
        {
            var entity = _coreRemoteNotificationRequest.Entity.MapTo<IRemoteNotificationMessage>();
            entity.FillOrganizationID(_environmentContext.OrganizationId);
            var entityActionExecuter = _entityActionExecuterFactory.Resolve<IRemoteNotificationMessage>(CoreExecuterCreateService);

            try
            {
                entityActionExecuter.Execute(entity);
            }
            finally
            {
                _entityActionExecuterFactory.Release(entityActionExecuter);
            }
        }

        protected override void ValidateRequest(ExternalFuelDeliveryNotificationMessageRequest request)
        {
            var internalBu = _buRepository.Search(request.DestinationExternalBusinessUnitId.ToString()).FirstOrDefault();
            if (internalBu == null)
            {
                throw new RequestValidationFailedException(nameof(ExternalFuelDeliveryNotificationMessageRequest.DestinationExternalBusinessUnitId));
            }

            _coreRemoteNotificationRequest = CreateCoreRequest(request, internalBu.ID);
            ValidateCoreRequest();
        }

        private RemoteNotificationMessageManagementRequest CreateCoreRequest(ExternalFuelDeliveryNotificationMessageRequest request, int internalBu)
        {
            return new RemoteNotificationMessageManagementRequest
            {
                Entity = new RemoteNotificationMessage
                {
                    RemoteNotificationMessageID = Guid.NewGuid(),
                    FromBusinessUnit = 0,
                    DisplayArea = Storix.Common.Model.Constants.DisplayArea.Notification,
                    DestinationBusinessUnit = internalBu,
                    DestinationTouchPoint = 1,
                    MessageType = string.Empty,
                    Subject = request.MessageSubject.IsEmpty() ? Resources.Resource.FuelDeliveryNotification_DefaultMessageSubject : request.MessageSubject,
                    MessageBody = request.MessageBody.IsEmpty() ? Resources.Resource.FuelDeliveryNotification_DefaultMessageBody : request.MessageBody,
                    IncomingTimeStamp = _timeProvider.Now
                },

                ActionType = Storix.Contract.Constants.ActionType.Create,
            };
        }

        private void ValidateCoreRequest()
        {
            var validator = _requestValidatorFactory
                    .Resolve<RemoteNotificationMessageManagementRequest>(CoreValidatorCreateService);
            try
            {
                validator.Validate(_coreRemoteNotificationRequest);
            }
            finally
            {
                _requestValidatorFactory.Release(validator);
            }
        }
    }
}
