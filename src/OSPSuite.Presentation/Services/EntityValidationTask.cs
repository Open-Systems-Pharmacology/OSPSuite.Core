using OSPSuite.Assets;
using OSPSuite.Core.Commands;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Presentation.Core;
using OSPSuite.Presentation.Presenters;

namespace OSPSuite.Presentation.Services
{
   public class EntityValidationTask : IEntityValidationTask
   {
      private readonly IEntityValidator _entityValidator;
      private readonly IApplicationController _applicationController;
      private readonly IOSPSuiteExecutionContext _executionContext;

      public EntityValidationTask(IEntityValidator entityValidator, IApplicationController applicationController, IOSPSuiteExecutionContext executionContext)
      {
         _entityValidator = entityValidator;
         _applicationController = applicationController;
         _executionContext = executionContext;
      }

      public bool Validate(IObjectBase objectToValidate)
      {
         var validationResult = _entityValidator.Validate(objectToValidate);
         if (validationResult.ValidationState == ValidationState.Valid)
            return true;

         using (var validationMessagesPresenter = _applicationController.Start<IValidationMessagesPresenter>())
         {
            validationMessagesPresenter.Caption = Error.EntityIsInvalid(_executionContext.TypeFor(objectToValidate), objectToValidate.Name);
            return validationMessagesPresenter.Display(validationResult);
         }
      }
   }
}