using OSPSuite.Core.Domain.Builder;

namespace OSPSuite.Core.Comparison
{
   public class ReactionPartnerBuilderDiffBuilder : DiffBuilder<IReactionPartnerBuilder>
   {
      public override void Compare(IComparison<IReactionPartnerBuilder> comparison)
      {
         CompareStringValues(x => x.MoleculeName, x => x.MoleculeName, comparison);
         CompareDoubleValues(x => x.StoichiometricCoefficient, x => x.StoichiometricCoefficient, comparison);
      }
   }
}