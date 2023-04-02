using System.Linq;

namespace OSPSuite.Core.Domain
{
   public class CreationResult
   {
      public virtual IModel Model { get; }
      public virtual ValidationResult ValidationResult { get; private set; }

      public CreationResult(IModel model)
      {
         Model = model;
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