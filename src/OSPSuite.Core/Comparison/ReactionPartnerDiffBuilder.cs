using OSPSuite.Assets;
using OSPSuite.Core.Domain;

namespace OSPSuite.Core.Comparison
{
   public class ReactionPartnerDiffBuilder : DiffBuilder<ReactionPartner>
   {
      public override void Compare(IComparison<ReactionPartner> comparison)
      {
         CompareStringValues(x => x.Partner.Name,Captions.Diff.ReactionPartnerName, comparison);
         CompareValues(x => x.StoichiometricCoefficient, x => x.StoichiometricCoefficient, comparison);
      }
   }
}