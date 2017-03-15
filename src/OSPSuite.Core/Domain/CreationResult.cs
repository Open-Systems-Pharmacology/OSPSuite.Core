using System.Linq;

namespace OSPSuite.Core.Domain
{
   public class CreationResult
   {
      public virtual IModel Model { get; private set; }
      public virtual ValidationResult ValidationResult { get; private set; }

      public CreationResult(IModel model)
      {
         Model = model;
         ValidationResult = new ValidationResult();
      }

      public virtual ValidationState State
      {
         get { return ValidationResult.ValidationState; }
      }

      public virtual bool IsInvalid
      {
         get { return State == ValidationState.Invalid; }
      }

      public virtual void Add(ValidationResult validationResult)
      {
         ValidationResult = new ValidationResult(validationResult.Messages.Union(ValidationResult.Messages));
      }
   }
}