using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Services;

namespace OSPSuite.Starter.Services
{
   public class FullPathDisplayResolver : IFullPathDisplayResolver
   {
      private readonly IEntityPathResolver _entityPathResolver;

      public FullPathDisplayResolver(IEntityPathResolver entityPathResolver)
      {
         _entityPathResolver = entityPathResolver;
      }

      public string FullPathFor(IObjectBase objectBase, bool addSimulationName = false)
      {
         var entity = objectBase as IEntity;
         if (entity == null)
            return objectBase.Name;

         if (addSimulationName)
            return _entityPathResolver.FullPathFor(entity);

         return _entityPathResolver.PathFor(entity);
      }
   }
}