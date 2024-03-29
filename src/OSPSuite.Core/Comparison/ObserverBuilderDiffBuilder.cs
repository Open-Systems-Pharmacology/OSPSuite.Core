﻿using OSPSuite.Core.Domain.Builder;

namespace OSPSuite.Core.Comparison
{
   public class ObserverBuilderDiffBuilder : DiffBuilder<ObserverBuilder>
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

      public override void Compare(IComparison<ObserverBuilder> comparison)
      {
         _entityDiffBuilder.Compare(comparison);
         _moleculeDependentDiffBuilder.Compare(comparison);
         CompareValues(x => x.ContainerCriteria, x => x.ContainerCriteria, comparison);
         _objectComparer.Compare(comparison.DimensionComparison());
         _objectComparer.Compare(comparison.FormulaComparison());
      }
   }
}