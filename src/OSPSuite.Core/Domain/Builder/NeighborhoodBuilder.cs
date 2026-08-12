using System.Collections.Generic;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Utility.Extensions;

namespace OSPSuite.Core.Domain.Builder
{
   public interface INeighborhoodBase : IContainer
   {
      /// <summary>
      ///    First neighbor in the neighborhood.
      /// </summary>
      IContainer FirstNeighbor { get; }

      /// <summary>
      ///    Second neighbor in the neighborhood.
      /// </summary>
      IContainer SecondNeighbor { get; }
   }

   public class NeighborhoodBuilder : Container, INeighborhoodBase
   {
      private ObjectPath _firstNeighborPath;
      private ObjectPath _secondNeighborPath;

      public IContainer FirstNeighbor { get; private set; }

      public IContainer SecondNeighbor { get; private set; }

      /// <summary>
      ///    Path of the first neighbor in the spatial structure. Never <c>null</c>: an empty path indicates that the neighbor
      ///    is not defined
      /// </summary>
      public ObjectPath FirstNeighborPath
      {
         get => _firstNeighborPath;
         set
         {
            _firstNeighborPath = value ?? new ObjectPath();
            FirstNeighbor = null;
         }
      }

      /// <summary>
      ///    Path of the second neighbor in the spatial structure. Never <c>null</c>: an empty path indicates that the neighbor
      ///    is not defined
      /// </summary>
      public ObjectPath SecondNeighborPath
      {
         get => _secondNeighborPath;
         set
         {
            _secondNeighborPath = value ?? new ObjectPath();
            SecondNeighbor = null;
         }
      }

      public NeighborhoodBuilder()
      {
         ContainerType = ContainerType.Neighborhood;
         _firstNeighborPath = new ObjectPath();
         _secondNeighborPath = new ObjectPath();
      }

      /// <summary>
      ///    Returns <c>true</c> if neither the first nor the second neighbor path is defined otherwise <c>false</c>.
      ///    A neighborhood without neighbors removes the neighborhood with the same name from the simulation when merging
      ///    modules.
      /// </summary>
      public bool HasNoNeighbors => !firstNeighborPathIsDefined && !secondNeighborPathIsDefined;

      /// <summary>
      ///    Returns <c>true</c> if both the first and the second neighbor path are defined otherwise <c>false</c>.
      /// </summary>
      public bool HasDefinedNeighborPaths => firstNeighborPathIsDefined && secondNeighborPathIsDefined;

      private bool firstNeighborPathIsDefined => pathIsDefined(FirstNeighborPath);

      private bool secondNeighborPathIsDefined => pathIsDefined(SecondNeighborPath);

      private static bool pathIsDefined(ObjectPath path) => !string.IsNullOrEmpty(path.PathAsString);

      public IContainer MoleculeProperties => this.Container(Constants.MOLECULE_PROPERTIES);

      public IEnumerable<IParameter> Parameters => GetChildren<IParameter>();

      public void AddParameter(IParameter newParameter) => Add(newParameter);

      public void RemoveParameter(IParameter parameterToRemove) => RemoveChild(parameterToRemove);

      public bool IsConnectedTo(ObjectPath containerPath)
      {
         return Equals(FirstNeighborPath, containerPath) || Equals(SecondNeighborPath, containerPath);
      }

      /// <summary>
      ///    Tries to resolve the reference to the first and second neighbor.
      ///    This method should be called after deserialization or when a new container is added to the structure
      /// </summary>
      public void ResolveReference(IReadOnlyList<IContainer> topContainers) => topContainers.Each(resolveReferenceIfRequired);

      /// <summary>
      ///    Tries to resolve the reference to the first and second neighbor.
      ///    This method should be called after deserialization or when a new container is added to the structure
      /// </summary>
      private void resolveReferenceIfRequired(IContainer container)
      {
         //only update if undefined. An undefined path cannot be resolved
         if (firstNeighborPathIsDefined)
            FirstNeighbor = FirstNeighbor ?? FirstNeighborPath.TryResolve<IContainer>(container);

         if (secondNeighborPathIsDefined)
            SecondNeighbor = SecondNeighbor ?? SecondNeighborPath.TryResolve<IContainer>(container);
      }

      /// <summary>
      ///    Tries to resolve the reference to the first and second neighbor.
      ///    This method should be called after deserialization or when a new container is added to the structure
      /// </summary>
      /// <param name="spatialStructure">Spatial structure used to resolve containers (in all top containers)</param>
      public void ResolveReference(SpatialStructure spatialStructure) => ResolveReference(spatialStructure.TopContainers);

      public override void UpdatePropertiesFrom(IUpdatable source, ICloneManager cloneManager)
      {
         base.UpdatePropertiesFrom(source, cloneManager);
         var sourceNeighborhood = source as NeighborhoodBuilder;
         if (sourceNeighborhood == null)
            return;

         FirstNeighborPath = sourceNeighborhood.FirstNeighborPath.Clone<ObjectPath>();
         SecondNeighborPath = sourceNeighborhood.SecondNeighborPath.Clone<ObjectPath>();
      }
   }
}