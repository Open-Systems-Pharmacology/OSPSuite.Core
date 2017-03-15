using System.Linq;

namespace OSPSuite.Core.Domain.Services
{
   /// <summary>
   ///    Purpose of this class is to consolidate the absolute object pathes defined in
   ///    1- an Individual (starts with ROOT)
   ///    2- a Spatial Structure (starts with Organism or Neighborhood)
   ///    3- a Simulation (starts with name of simulation)
   /// </summary>
   public interface IEntityPathResolver
   {
      /// <summary>
      ///    Returns a consolidated absolute path as string for the given entity where the simulation name was removed
      /// </summary>
      string PathFor(IEntity entity);

      /// <summary>
      ///    Returns an absolute path as string for the given entity where the simulation name was kept
      /// </summary>
      string FullPathFor(IEntity entity);

      /// <summary>
      ///    Returns a consolidated absolute object path for the given entity
      /// </summary>
      IObjectPath ObjectPathFor(IEntity entity, bool addSimulationName = false);
   }

   public class EntityPathResolver : IEntityPathResolver
   {
      private readonly IObjectPathFactory _objectPathFactory;

      public EntityPathResolver(IObjectPathFactory objectPathFactory)
      {
         _objectPathFactory = objectPathFactory;
      }

      public string PathFor(IEntity entity)
      {
         return ObjectPathFor(entity, addSimulationName: false).ToString();
      }

      public string FullPathFor(IEntity entity)
      {
         return ObjectPathFor(entity, addSimulationName: true).ToString();
      }

      public virtual IObjectPath ObjectPathFor(IEntity entity, bool addSimulationName = false)
      {
         var objectPath = _objectPathFactory.CreateAbsoluteObjectPath(entity);
         return convertedPath(objectPath, entity.RootContainer, addSimulationName);
      }

      private IObjectPath convertedPath(IObjectPath objectPath, IContainer rootContainer, bool addSimulationName)
      {
         if (!objectPath.Any())
            return objectPath;

         //Path starts with the root element, this needs to be remove no matter what
         if (string.Equals(objectPath.ElementAt(0), Constants.ROOT))
            return removeFirstEntryOf(objectPath);

         if (rootContainer == null)
            return objectPath;

         //now this is an simulation absolute path (the second entry in the path is either Organism,  neighborhoods)
         if (rootContainer.ContainerType == ContainerType.Simulation && !addSimulationName)
            return removeFirstEntryOf(objectPath);

         //the path is either relative or should stay as is. Return
         return objectPath;
      }

      private IObjectPath removeFirstEntryOf(IObjectPath objectPath)
      {
         objectPath.Remove(objectPath[0]);
         return objectPath;
      }
   }
}