using OSPSuite.Core.Domain.Builder;

namespace OSPSuite.Core.Comparison
{
   public class TransporterBuilderDiffBuilder : DiffBuilder<ITransportBuilder>
   {
      private readonly ContainerDiffBuilder _containerDiffBuilder;
      private readonly IObjectComparer _objectComparer;
      private readonly MoleculeDependentBuilderDiffBuilder _moleculeDependentBuilderDiffBuilder;

      public TransporterBuilderDiffBuilder(ContainerDiffBuilder containerDiffBuilder, IObjectComparer objectComparer, MoleculeDependentBuilderDiffBuilder moleculeDependentBuilderDiffBuilder)
      {
         _containerDiffBuilder = containerDiffBuilder;
         _objectComparer = objectComparer;
         _moleculeDependentBuilderDiffBuilder = moleculeDependentBuilderDiffBuilder;
      }

      public override void Compare(IComparison<ITransportBuilder> comparison)
      {
         _containerDiffBuilder.Compare(comparison);
         CompareValues(x => x.SourceCriteria, x => x.SourceCriteria, comparison);
         CompareValues(x => x.TargetCriteria, x => x.TargetCriteria, comparison);
         CompareValues(x => x.Dimension, x => x.Dimension, comparison);
         _moleculeDependentBuilderDiffBuilder.Compare(comparison);
         _objectComparer.Compare(comparison.FormulaComparison());
      }
   }
}