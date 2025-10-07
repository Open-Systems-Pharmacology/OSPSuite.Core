using OSPSuite.Assets;
using OSPSuite.Core.Commands;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Services;

namespace OSPSuite.CLI.Core.MinimalImplementations
{
   public class EntityValidationTask : IEntityValidationTask
   {
      private readonly IEntityValidatorFactory _entityValidatorFactory;
      private readonly IOSPSuiteExecutionContext _executionContext;
      private readonly IOSPSuiteLogger _logger;

      public EntityValidationTask(IEntityValidatorFactory entityValidatorFactory, IOSPSuiteExecutionContext executionContext, IOSPSuiteLogger logger)
      {
         _entityValidatorFactory = entityValidatorFactory;
         _executionContext = executionContext;
         _logger = logger;
      }

      public bool Validate(IObjectBase objectToValidate)
      {
         var validationResult = _entityValidatorFactory.Validate(objectToValidate);
         if (validationResult.ValidationState == ValidationState.Valid)
            return true;

         var error = Error.EntityIsInvalid(_executionContext.TypeFor(objectToValidate), objectToValidate.Name);
         _logger.AddError(error);
         return false;
      }
   }
}