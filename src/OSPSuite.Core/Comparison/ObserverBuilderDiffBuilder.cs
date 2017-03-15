using OSPSuite.Core.Domain.Builder;

namespace OSPSuite.Core.Comparison
{
   public class ObserverBuilderDiffBuilder : DiffBuilder<IObserverBuilder>
   {
      private readonly EntityDiffBuilder _entityDiffBuilder;
      private readonly MoleculeDependentBuilderDiffBuilder _moleculeDependentDiffBuilder;
      private readonly IObjectComparer _objectComparer;

      public ObserverBuilderDiffBuilder(EntityDiffBuilder entityDiffBuilder, MoleculeDependentBuilderDiffBuilder moleculeDependentDiffBuilder, IObjectComparer objectComparer)
      {
         _entityDiffBuilder = entityDiffBuilder;
         _moleculeDependentDiffBuilder = moleculeDependentDiffBuilder;
         _objectComparer = objectComparer;
      }

      public override void Compare(IComparison<IObserverBuilder> comparison)
      {
         _entityDiffBuilder.Compare(comparison);
         _moleculeDependentDiffBuilder.Compare(comparison);
         CompareValues(x => x.ContainerCriteria, x => x.ContainerCriteria, comparison);
         CompareValues(x => x.Dimension, x => x.Dimension, comparison);
         _objectComparer.Compare(comparison.FormulaComparison());
      }
   }
}