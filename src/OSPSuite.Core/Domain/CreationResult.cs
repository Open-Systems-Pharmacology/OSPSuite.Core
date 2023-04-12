using System.Linq;
using OSPSuite.Core.Domain.Builder;

namespace OSPSuite.Core.Domain
{
   public class CreationResult
   {
      public virtual IModel Model { get; }
      public SimulationBuilder SimulationBuilder { get; }
      public virtual ValidationResult ValidationResult { get; private set; }

      public CreationResult(IModel model, SimulationBuilder simulationBuilder)
      {
         Model = model;
         SimulationBuilder = simulationBuilder;
         ValidationResult = new ValidationResult();
      }

      public virtual ValidationState State => ValidationResult.ValidationState;

      public virtual bool IsInvalid => State == ValidationState.Invalid;

      public virtual void Add(ValidationResult validationResult)
      {
         ValidationResult = new ValidationResult(validationResult.Messages.Union(ValidationResult.Messages));
      }

      public void Deconstruct(out IModel model, out ValidationResult validationResult)
      {
         model = Model;
         validationResult = ValidationResult;
      }
   }
}