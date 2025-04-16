using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Utility.Extensions;

namespace OSPSuite.Core.Services
{
   public interface IEntitySourcePathUpdater
   {
      void UpdateEntityPaths(IModel model, SimulationBuilder simulationBuilder);
   }

   public class EntitySourcePathUpdater : IEntitySourcePathUpdater
   {
      protected readonly IEntityPathResolver _entityPathResolver;

      public EntitySourcePathUpdater(IEntityPathResolver entityPathResolver)
      {
         _entityPathResolver = entityPathResolver;
      }

      public void UpdateEntityPaths(IModel model, SimulationBuilder simulationBuilder)
      {
         var allEntitiesInModel = model.Root.GetAllChildrenAndSelf<IEntity>();

         allEntitiesInModel.Each(x =>
         {
            var entitySource = simulationBuilder.SimulationEntitySourceFor(x);
            if (entitySource == null)
               return;

            entitySource.SimulationEntityPath = _entityPathResolver.PathFor(x);
         });
      }
   }
}