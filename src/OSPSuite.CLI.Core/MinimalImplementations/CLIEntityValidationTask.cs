using OSPSuite.Assets;
using OSPSuite.Core.Commands;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Services;

namespace OSPSuite.CLI.Core.MinimalImplementations
{
   public class CLIEntityValidationTask : IEntityValidationTask
   {
      private readonly IEntityValidator _entityValidator;
      private readonly IOSPSuiteExecutionContext _executionContext;
      private readonly IOSPSuiteLogger _logger;

      public CLIEntityValidationTask(IEntityValidator entityValidator, IOSPSuiteExecutionContext executionContext, IOSPSuiteLogger logger)
      {
         _entityValidator = entityValidator;
         _executionContext = executionContext;
         _logger = logger;
      }

      public bool Validate(IObjectBase objectToValidate)
      {
         var validationResult = _entityValidator.Validate(objectToValidate);
         if (validationResult.ValidationState == ValidationState.Valid)
            return true;

         var error = Error.EntityIsInvalid(_executionContext.TypeFor(objectToValidate), objectToValidate.Name);
         _logger.AddError(error);
         return false;
      }
   }
}