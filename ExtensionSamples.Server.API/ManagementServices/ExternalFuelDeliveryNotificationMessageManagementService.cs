using System;
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
using Storix.Model.Engine;
using Storix.Model.Entities;
using Storix.Model.ExtensionMethods;
using Storix.Model.Repositories;
using Storix.Model.ServiceActionExecuters;

namespace ExtensionSamples.Server.API.ManagementServices
{
    public class ExternalFuelDeliveryNotificationMessageManagementService : ProcessManagementService<ExternalFuelDeliveryNotificationMessageRequest,
                                                                            ExternalFuelDeliveryNotificationMessageResponse>
    {
        private readonly IRequestValidatorFactory _requestValidatorFactory;
        private readonly IEntityActionExecuterFactory _entityActionExecuterFactory;
        private readonly IBusinessUnitRepository _buRepository;
        private readonly IDateTimeProvider _timeProvider;
        //private readonly IOrganizationIdRetriever _organizationIdRetriever;
        private IRemoteNotificationMessage _entityAfter;
        private readonly IEnvironmentContext _environmentContext;

        private RemoteNotificationMessageManagementRequest _coreRemoteNotificationMessage;
        private readonly string CoreValidatorCreateService = "RemoteNotificationMessageCreateRequestValidator";
        private readonly string CoreExecuterCreateService = "RemoteNotificationMessageCreateActionExecuter";

        public ExternalFuelDeliveryNotificationMessageManagementService(IBusinessUnitRepository buRepository,
                                                                        IEntityActionExecuterFactory entityActionExecuterFactory,
                                                                        IRequestValidatorFactory requestValidatorFactory,
                                                                        IDateTimeProvider timeProvider,
                                                                        //IOrganizationIdRetriever organizationIdRetriever,
                                                                        IEnvironmentContext environmentContext)
        {
            _requestValidatorFactory = requestValidatorFactory;
            _entityActionExecuterFactory = entityActionExecuterFactory;
            _buRepository = buRepository;
            _timeProvider = timeProvider;
            //_organizationIdRetriever = organizationIdRetriever;
            _environmentContext = environmentContext;
        }

        protected override void Execute(RequestHeader header, ExternalFuelDeliveryNotificationMessageRequest request)
        {
            var entity = _coreRemoteNotificationMessage.Entity.MapTo<IRemoteNotificationMessage>();

            entity.FillOrganizationID(_environmentContext.OrganizationId);


            var entityActionExecuter = _entityActionExecuterFactory.Resolve<IRemoteNotificationMessage>(CoreExecuterCreateService);

            try
            {
                _entityAfter = entityActionExecuter.Execute(entity);
            }
            finally
            {
                _entityActionExecuterFactory.Release(entityActionExecuter);
            }
        }

        protected override void ValidateRequest(ExternalFuelDeliveryNotificationMessageRequest request)
        {
            if (request.MessageSubject.IsEmpty())
            {
                //request.MessageSubject = Constants.ExternalNotificationMessage.RemoteNotificationDefaultMessage;
                request.MessageSubject = "DefaultMessage";
            }

            if (request.MessageBody.IsEmpty())
            {
                //request.MessageBody = Constants.ExternalNotificationMessage.RemoteNotificationDefaultBody;
                request.MessageSubject = "DefaultBody";
            }

            var bu = _buRepository.Search(request.DestinationExternalBusinessUnitId.ToString()).FirstOrDefault();
            if (bu == null)
            {
                throw new RequestValidationFailedException(nameof(BusinessUnit));
            }


            _coreRemoteNotificationMessage = new RemoteNotificationMessageManagementRequest
            {
                Entity = new RemoteNotificationMessage
                {
                    RemoteNotificationMessageID = Guid.NewGuid(),
                    FromBusinessUnit = 0,
                    DisplayArea = "Notification",
                    DestinationBusinessUnit = bu.ID,
                    DestinationTouchPoint = 1,
                    MessageType = string.Empty,
                    Subject = request.MessageSubject,
                    MessageBody = request.MessageBody,
                    IncomingTimeStamp = _timeProvider.Now
                },

                ActionType = "Create",
            };

            var validator = _requestValidatorFactory
                .Resolve<RemoteNotificationMessageManagementRequest>(CoreValidatorCreateService);
            try
            {
                validator.Validate(_coreRemoteNotificationMessage);
            }
            finally
            {
                _requestValidatorFactory.Release(validator);
            }
        }
    }
}
