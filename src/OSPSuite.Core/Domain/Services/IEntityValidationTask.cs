namespace OSPSuite.Core.Domain.Services
{
   public interface IEntityValidationTask
   {
      /// <summary>
      ///    Validates the current entity. Returns true if the entity is valid or if the user accepted the validation errors
      /// </summary>
      bool Validate(IObjectBase objectToValidate);
   }
}