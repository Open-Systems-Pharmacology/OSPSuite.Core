using OSPSuite.Core.Services;

namespace OSPSuite.Core.Domain.Services
{
   public interface IEntityValidatorFactory
   {
      EntityValidator Create();
   }
   
   public class EntityValidatorFactory : DynamicFactory<EntityValidator>, IEntityValidatorFactory
   {
      public EntityValidatorFactory(Utility.Container.IContainer container) : base(container)
      {
      }
   }
}