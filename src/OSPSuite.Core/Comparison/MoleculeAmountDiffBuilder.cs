using OSPSuite.Core.Domain;

namespace OSPSuite.Core.Comparison
{
   public class MoleculeAmountDiffBuilder:DiffBuilder<IMoleculeAmount>
   {
      private readonly QuantityDiffBuilder _quantityDiffBuilder;
      private readonly ContainerDiffBuilder _containerDiffBuilder;

      public MoleculeAmountDiffBuilder(QuantityDiffBuilder quantityDiffBuilder, ContainerDiffBuilder containerDiffBuilder)
      {
         _quantityDiffBuilder = quantityDiffBuilder;
         _containerDiffBuilder = containerDiffBuilder;
      }

      public override void Compare(IComparison<IMoleculeAmount> comparison)
      {
         _quantityDiffBuilder.Compare(comparison);
         _containerDiffBuilder.Compare(comparison);
      }
   }
}