using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;

namespace OSPSuite.Core.Comparison
{
   public abstract class PathAndValueEntityDiffBuilder<T> : DiffBuilder<T> where T : PathAndValueEntity
   {
      protected IObjectComparer _objectComparer;
      protected EntityDiffBuilder _entityDiffBuilder;

      protected PathAndValueEntityDiffBuilder(IObjectComparer objectComparer, EntityDiffBuilder entityDiffBuilder)
      {
         _objectComparer = objectComparer;
         _entityDiffBuilder = entityDiffBuilder;
      }

      public override void Compare(IComparison<T> comparison)
      {
         ComparePathAndEntityValues(comparison);
      }

      protected virtual void ComparePathAndEntityValues(IComparison<T> comparison)
      {
         _entityDiffBuilder.Compare(comparison);
         CompareValues(x => x.Dimension, x => x.Dimension, comparison);
         CompareValues(x => x.ContainerPath, x => x.ContainerPath, comparison);

         // Always Compare Value and Formula, independent from settings as these are two different properties of a start value
         CompareNullableDoubleValues(x => x.Value, x => x.Value, comparison, x => x.DisplayUnit);
         _objectComparer.Compare(comparison.FormulaComparison());
      }
   }

   internal abstract class StartValueDiffBuilder<T> : PathAndValueEntityDiffBuilder<T> where T : PathAndValueEntity, IWithValueOrigin
   {
      private readonly WithValueOriginComparison<T> _valueOriginComparison;

      protected StartValueDiffBuilder(IObjectComparer objectComparer, EntityDiffBuilder entityDiffBuilder, WithValueOriginComparison<T> valueOriginComparison) :base(objectComparer, entityDiffBuilder)
      {

         _valueOriginComparison = valueOriginComparison;
      }

      public override void Compare(IComparison<T> comparison)
      {
         _valueOriginComparison.AddValueOriginToComparison(comparison, this, ComparePathAndEntityValues);
      }
   }

   internal class MoleculeStartValueDiffBuilder : StartValueDiffBuilder<MoleculeStartValue>
   {
      public MoleculeStartValueDiffBuilder(IObjectComparer objectComparer, EntityDiffBuilder entityDiffBuilder, WithValueOriginComparison<MoleculeStartValue> valueOriginComparison)
         : base(objectComparer, entityDiffBuilder, valueOriginComparison)
      {
      }

      protected override void ComparePathAndEntityValues(IComparison<MoleculeStartValue> comparison)
      {
         base.ComparePathAndEntityValues(comparison);
         CompareValues(x => x.IsPresent, x => x.IsPresent, comparison);
         CompareValues(x => x.ScaleDivisor, x => x.ScaleDivisor, comparison);
         CompareValues(x => x.NegativeValuesAllowed, x => x.NegativeValuesAllowed, comparison);
      }
   }

   internal class ParameterStartValueDiffBuilder : StartValueDiffBuilder<ParameterStartValue>
   {
      public ParameterStartValueDiffBuilder(IObjectComparer objectComparer, EntityDiffBuilder entityDiffBuilder, WithValueOriginComparison<ParameterStartValue> valueOriginComparison)
         : base(objectComparer, entityDiffBuilder, valueOriginComparison)
      {
      }
   }
}