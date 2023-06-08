﻿using OSPSuite.Core.Domain.Builder;

namespace OSPSuite.Core.Comparison
{
   public class TransporterBuilderDiffBuilder : DiffBuilder<TransportBuilder>
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

      public override void Compare(IComparison<TransportBuilder> comparison)
      {
         _containerDiffBuilder.Compare(comparison);
         CompareValues(x => x.SourceCriteria, x => x.SourceCriteria, comparison);
         CompareValues(x => x.TargetCriteria, x => x.TargetCriteria, comparison);
         _objectComparer.Compare(comparison.ChildComparison(x => x.Dimension));
         _moleculeDependentBuilderDiffBuilder.Compare(comparison);
         _objectComparer.Compare(comparison.FormulaComparison());
      }
   }
}