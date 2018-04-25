using OSPSuite.Assets;
using OSPSuite.Core.Domain.Builder;

namespace OSPSuite.Core.Comparison
{
   public class MoleculeDependentBuilderDiffBuilder : DiffBuilder<IMoleculeDependentBuilder>
   {
      private readonly EnumerableComparer _enumerableComparer;

      public MoleculeDependentBuilderDiffBuilder(EnumerableComparer enumerableComparer)
      {
         _enumerableComparer = enumerableComparer;
      }

      public override void Compare(IComparison<IMoleculeDependentBuilder> comparison)
      {
         CompareValues(x => x.ForAll, x => x.ForAll, comparison);

         //Only Compare In and Exclude list if the ForAll property is the same, otherwise the comparrison dor the lists is misleading
         if (comparison.Object1.ForAll != comparison.Object2.ForAll)
            return;

         if (comparison.Object1.ForAll) 
            // For All = true so only compare exclude list, include is not used
            _enumerableComparer.CompareEnumerables(comparison, x => x.MoleculeList.MoleculeNamesToExclude, x => x, missingItemType: Captions.Diff.ExcludeMolecule);
         else 
            // For All = True so only compare include list, exclude is not used
            _enumerableComparer.CompareEnumerables(comparison, x => x.MoleculeList.MoleculeNames, x => x, missingItemType: Captions.Diff.IncludeMolecule);
      }
   }
}