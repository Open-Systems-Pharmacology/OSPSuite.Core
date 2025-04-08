using System;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Utility.Extensions;

namespace OSPSuite.Core.Services
{
   public interface IEntitySourcePathUpdater
   {
      void UpdateEntityPath(IModel model, SimulationBuilder simulationBuilder);
   }

   public class EntitySourcePathUpdater : IEntitySourcePathUpdater
   {
      protected readonly IEntityPathResolver _entityPathResolver;

      public EntitySourcePathUpdater(IEntityPathResolver entityPathResolver)
      {
         _entityPathResolver = entityPathResolver;
      }

      public void UpdateEntityPath(IModel model, SimulationBuilder simulationBuilder)
      {
         var allEntitiesInModel = model.Root.GetAllChildrenAndSelf<IEntity>();

         allEntitiesInModel.Each(x =>
         {
            var entitySource = simulationBuilder.EntitySourceFor(x);
            if (entitySource == null)
               return;

            entitySource.EntityPath = _entityPathResolver.PathFor(x);
         });
      }
   }
}