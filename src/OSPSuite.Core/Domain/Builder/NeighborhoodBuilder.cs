using System.Collections.Generic;
using OSPSuite.Core.Domain.Services;

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
      //We define a property set for the first neighbor only to be compatible with serialization prior to v12
      public IContainer FirstNeighbor { get; set; }

      //We define a property set for the second neighbor only to be compatible with serialization prior to v12
      public IContainer SecondNeighbor { get; set; }

      public ObjectPath FirstNeighborPath { get; set; }
      public ObjectPath SecondNeighborPath { get; set; }

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

      public void ResolveReference(IContainer container)
      {
         FirstNeighbor = FirstNeighborPath.Resolve<IContainer>(container);
         SecondNeighbor = SecondNeighborPath.Resolve<IContainer>(container);
      }

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