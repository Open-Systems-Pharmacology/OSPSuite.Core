using System;
using OSPSuite.Core.Domain.Builder;

namespace OSPSuite.Core.Domain.Services
{
   public interface IObjectTracker
   {
      void TrackObject(IEntity objectToTrack, IEntity sourceBuilder, SimulationBuilder simulationBuilder);
   }

   public class ObjectTracker : IObjectTracker
   {
      private readonly IObjectPathFactory _objectPathFactory;
      private readonly IObjectTypeResolver _objectTypeResolver;

      public ObjectTracker(IObjectPathFactory objectPathFactory, IObjectTypeResolver objectTypeResolver)
      {
         _objectPathFactory = objectPathFactory;
         _objectTypeResolver = objectTypeResolver;
      }

      public void TrackObject(IEntity objectToTrack, IEntity sourceBuilder, SimulationBuilder simulationBuilder)
      {
         var sourcePath = _objectPathFactory.CreateAbsoluteObjectPath(sourceBuilder).ToString();
         var sourceType = _objectTypeResolver.TypeFor(sourceBuilder);

         var builderSource = simulationBuilder.BuilderSourceFor(sourceBuilder);
         if (builderSource != null)
         {
            var objectSource = new ObjectSource(objectToTrack.Id, builderSource.BuildingBlock.Module?.Id, builderSource.BuildingBlock.Id, sourcePath, sourceType, sourceBuilder.Id);
            simulationBuilder.AddObjectSource(objectSource);
            return;
         }

         //in this case, we might have clone the object. We need to find the source of the source
         var objectSourceOrigin = simulationBuilder.ObjectSources.SourceById(sourceBuilder.Id);
         if (objectSourceOrigin != null)
         {
            var newObjectSource = new ObjectSource(objectToTrack.Id, objectSourceOrigin);
            simulationBuilder.AddObjectSource(newObjectSource);
            return;
         }

         //Error. This should never happen. Log for now
         Console.WriteLine($"Cannot find builder source for {sourcePath}");
      }
   }
}