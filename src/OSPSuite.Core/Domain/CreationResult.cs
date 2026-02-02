using System.Linq;
using OSPSuite.Core.Domain.Builder;

namespace OSPSuite.Core.Domain
{
   public class WithValidationResult
   {
      public virtual ValidationResult ValidationResult { get; private set; } = new ValidationResult();

      public virtual ValidationState State => ValidationResult.ValidationState;

      public virtual bool IsInvalid => State == ValidationState.Invalid;

      public virtual void Add(ValidationResult validationResult)
      {
         ValidationResult = new ValidationResult(validationResult.Messages.Union(ValidationResult.Messages));
      }
   }

   public class CreationResult : WithValidationResult
   {
      public virtual IModel Model { get; }
      public SimulationBuilder SimulationBuilder { get; }

      public CreationResult(IModel model, SimulationBuilder simulationBuilder)
      {
         Model = model;
         SimulationBuilder = simulationBuilder;
      }

      public void Deconstruct(out IModel model, out ValidationResult validationResult)
      {
         model = Model;
         validationResult = ValidationResult;
      }
   }

   public class RunValidationResult : WithValidationResult
   {

   }
}