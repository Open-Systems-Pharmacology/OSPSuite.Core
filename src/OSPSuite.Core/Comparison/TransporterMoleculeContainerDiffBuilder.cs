using OSPSuite.Core.Domain.Builder;

namespace OSPSuite.Core.Comparison
{
   public class TransporterMoleculeContainerDiffBuilder : DiffBuilder<TransporterMoleculeContainer>
   {
      private readonly ContainerDiffBuilder _containerDiffBuilder;

      public TransporterMoleculeContainerDiffBuilder(ContainerDiffBuilder containerDiffBuilder)
      {
         _containerDiffBuilder = containerDiffBuilder;
      }

      public override void Compare(IComparison<TransporterMoleculeContainer> comparison)
      {
         _containerDiffBuilder.Compare(comparison);
         CompareStringValues(x => x.TransportName, x => x.TransportName, comparison);
      }
   }
}
