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

      public ObjectPath FirstNeighborPath
      {
         get => _firstNeighborPath;
         set
         {
            _firstNeighborPath = value;
            FirstNeighbor = null;
         }
      }

      public ObjectPath SecondNeighborPath
      {
         get => _secondNeighborPath;
         set
         {
            _secondNeighborPath = value;
            SecondNeighbor = null;
         }
      }

      public NeighborhoodBuilder()
      {
         ContainerType = ContainerType.Neighborhood;
      }

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
         //only update if undefined
         FirstNeighbor = FirstNeighbor ?? FirstNeighborPath.TryResolve<IContainer>(container);
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

         FirstNeighborPath = sourceNeighborhood.FirstNeighborPath;
         SecondNeighborPath = sourceNeighborhood.SecondNeighborPath;
      }
   }
}