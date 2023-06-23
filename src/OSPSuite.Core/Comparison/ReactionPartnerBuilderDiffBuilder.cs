using OSPSuite.Core.Domain.Builder;

namespace OSPSuite.Core.Comparison
{
   public class ReactionPartnerBuilderDiffBuilder : DiffBuilder<ReactionPartnerBuilder>
   {
      public override void Compare(IComparison<ReactionPartnerBuilder> comparison)
      {
         CompareStringValues(x => x.MoleculeName, x => x.MoleculeName, comparison);
         CompareDoubleValues(x => x.StoichiometricCoefficient, x => x.StoichiometricCoefficient, comparison);
      }
   }
}