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
      private readonly IEntityValidatorFactory _entityValidatorFactory;
      private readonly IApplicationController _applicationController;
      private readonly IOSPSuiteExecutionContext _executionContext;

      public EntityValidationTask(IEntityValidatorFactory entityValidatorFactory, IApplicationController applicationController, IOSPSuiteExecutionContext executionContext)
      {
         _entityValidatorFactory = entityValidatorFactory;
         _applicationController = applicationController;
         _executionContext = executionContext;
      }

      public bool Validate(IObjectBase objectToValidate)
      {
         var validationResult = _entityValidatorFactory.Create().Validate(objectToValidate);
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