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
      private readonly IObjectPathFactory _objectPathFactory;
      private readonly IObjectTypeResolver _objectTypeResolver;

      public EntityTracker(IObjectPathFactory objectPathFactory, IObjectTypeResolver objectTypeResolver)
      {
         _objectPathFactory = objectPathFactory;
         _objectTypeResolver = objectTypeResolver;
      }

      public void Track(IEntity entityToTrack, IEntity sourceBuilder, SimulationBuilder simulationBuilder)
      {
         var sourcePath = _objectPathFactory.CreateAbsoluteObjectPath(sourceBuilder).ToString();
         var sourceType = _objectTypeResolver.TypeFor(sourceBuilder);

         var builderSource = simulationBuilder.BuilderSourceFor(sourceBuilder);
         if (builderSource != null)
         {
            var objectSource = new EntitySource(entityToTrack.Id, builderSource.BuildingBlock.Id, sourceType, sourceBuilder.Id, sourceBuilder);
            simulationBuilder.AddObjectSource(objectSource);
            return;
         }

         //in this case, we might have clone the object. We need to find the source of the source
         var objectSourceOrigin = simulationBuilder.EntitySources.SourceById(sourceBuilder.Id);
         if (objectSourceOrigin != null)
         {
            var newObjectSource = new EntitySource(entityToTrack.Id, objectSourceOrigin);
            simulationBuilder.AddObjectSource(newObjectSource);
            return;
         }

         //Error. This should never happen. Log for now
         Console.WriteLine($"Cannot find builder source for {sourcePath}");
      }

   }
}