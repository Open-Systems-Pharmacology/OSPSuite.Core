using OSPSuite.Core.Services;

namespace OSPSuite.Core.Domain.Services
{
   public interface IEntityValidatorFactory
   {
      ValidationResult Validate(IObjectBase objectBase);
   }
   
   public class EntityValidatorFactory : DynamicFactory<IEntityValidator>, IEntityValidatorFactory
   {
      public EntityValidatorFactory(Utility.Container.IContainer container) : base(container)
      {
      }

      public ValidationResult Validate(IObjectBase objectBase)
      {
         return Create().Validate(objectBase);
      }
   }
}