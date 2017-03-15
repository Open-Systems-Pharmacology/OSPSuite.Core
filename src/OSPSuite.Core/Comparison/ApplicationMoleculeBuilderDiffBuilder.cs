using OSPSuite.Core.Domain.Builder;

namespace OSPSuite.Core.Comparison
{
   public class ApplicationMoleculeBuilderDiffBuilder : DiffBuilder<IApplicationMoleculeBuilder>
   {
      private readonly EntityDiffBuilder _entityDiffBuilder;
      private readonly IObjectComparer _objectComparer;

      public ApplicationMoleculeBuilderDiffBuilder(EntityDiffBuilder entityDiffBuilder, IObjectComparer objectComparer)
      {
         _entityDiffBuilder = entityDiffBuilder;
         _objectComparer = objectComparer;
      }

      public override void Compare(IComparison<IApplicationMoleculeBuilder> comparison)
      {
         _entityDiffBuilder.Compare(comparison);
         CompareValues(x => x.RelativeContainerPath, x => x.RelativeContainerPath, comparison, Equals, (molecule, path) => path.PathAsString);
         _objectComparer.Compare(comparison.FormulaComparison());
      }
   }
}