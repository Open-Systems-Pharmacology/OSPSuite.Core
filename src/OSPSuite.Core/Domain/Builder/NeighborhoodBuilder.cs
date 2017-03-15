using System.Collections.Generic;

namespace OSPSuite.Core.Domain.Builder
{
   public interface INeighborhoodBuilder : INeighborhoodBase
   {
      IContainer MoleculeProperties { get; }
      IEnumerable<IParameter> Parameters { get; }
      void AddParameter(IParameter newParameter);
      void RemoveParameter(IParameter parameterToRemove);
      bool IsConnectedTo(IContainer container);
   }

   public class NeighborhoodBuilder : Container, INeighborhoodBuilder
   {
      public IContainer FirstNeighbor { get; set; }
      public IContainer SecondNeighbor { get; set; }

      public NeighborhoodBuilder()
      {
         ContainerType = ContainerType.Neighborhood;
      }

      public IContainer MoleculeProperties
      {
         get { return this.GetSingleChildByName<IContainer>(Constants.MOLECULE_PROPERTIES); }
      }

      public IEnumerable<IParameter> Parameters
      {
         get { return GetChildren<IParameter>(); }
      }

      public void AddParameter(IParameter newParameter)
      {
         Add(newParameter);
      }

      public void RemoveParameter(IParameter parameterToRemove)
      {
         RemoveChild(parameterToRemove);
      }

      public bool IsConnectedTo(IContainer container)
      {
         return Equals(FirstNeighbor, container) || Equals(SecondNeighbor, container);
      }
   }
}