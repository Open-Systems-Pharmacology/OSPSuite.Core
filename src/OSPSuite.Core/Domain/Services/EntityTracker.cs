using System;
using OSPSuite.Core.Domain.Builder;

namespace OSPSuite.Core.Domain.Services
{
   public interface IEntityTracker
   {
      void Track(IEntity entityToTrack, IEntity sourceBuilder, SimulationBuilder simulationBuilder);
   }

   public class EntityTracker : IEntityTracker
   {
      private readonly IEntityPathResolver _entityPathResolver;

      public EntityTracker(IEntityPathResolver entityPathResolver)
      {
         _entityPathResolver = entityPathResolver;
      }

      public void Track(IEntity entityToTrack, IEntity sourceBuilder, SimulationBuilder simulationBuilder)
      {
         var sourcePath = sourcePathFor(sourceBuilder);

         if (entityToTrack.Name == "Ontogeny factor")
         {
            entityToTrack.Name = entityToTrack.Name;
         }

         var builderSource = simulationBuilder.BuilderSourceFor(sourceBuilder);
         if (builderSource != null)
         {
            var objectSource = new EntitySource(builderSource.BuildingBlock, sourcePath, sourceBuilder);
            simulationBuilder.AddEntitySource(entityToTrack.Id, objectSource);
            return;
         }

         //in this case, we might have clone the object. We need to find the source of the source
         var objectSourceOrigin = simulationBuilder.EntitySourceFor(sourceBuilder);
         if (objectSourceOrigin != null)
         {
            var newObjectSource = new EntitySource(objectSourceOrigin);
            simulationBuilder.AddEntitySource(entityToTrack.Id, newObjectSource);
            return;
         }

         //Error. This should never happen. Log for now
         Console.WriteLine($"Cannot find builder source for {sourcePath}");
      }

      private string sourcePathFor(IEntity sourceBuilder)
      {
         switch (sourceBuilder)
         {
            case PathAndValueEntity pathAndValueEntity:
               return pathAndValueEntity.Path;
            default:
               return _entityPathResolver.PathFor(sourceBuilder);
         }
      }
   }
}