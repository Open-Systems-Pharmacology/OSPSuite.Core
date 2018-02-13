using OSPSuite.Core.Domain.Builder;

namespace OSPSuite.Core.Comparison
{
   internal abstract class StartValueDiffBuilder<T> : DiffBuilder<T> where T : class, IStartValue
   {
      private readonly IObjectComparer _objectComparer;
      private readonly EntityDiffBuilder _entityDiffBuilder;
      private readonly WithValueOriginComparison<T> _valueOriginComparison;

      protected StartValueDiffBuilder(IObjectComparer objectComparer, EntityDiffBuilder entityDiffBuilder, WithValueOriginComparison<T> valueOriginComparison)
      {
         _objectComparer = objectComparer;
         _entityDiffBuilder = entityDiffBuilder;
         _valueOriginComparison = valueOriginComparison;
      }

      public override void Compare(IComparison<T> comparison)
      {
         _valueOriginComparison.AddValueOriginToComparison(comparison, this, CompareStartValue);
      }

      protected virtual void CompareStartValue(IComparison<T> comparison)
      {
         _entityDiffBuilder.Compare(comparison);
         CompareValues(x => x.Dimension, x => x.Dimension, comparison);
         CompareValues(x => x.ContainerPath, x => x.ContainerPath, comparison);

         // Always Compare Value and Formula, independent from settings as these are two different properties of a start value
         CompareNullableDoubleValues(x => x.StartValue, x => x.StartValue, comparison, x => x.DisplayUnit);
         _objectComparer.Compare(comparison.FormulaComparison());
      }
   }

   internal class MoleculeStartValueDiffBuilder : StartValueDiffBuilder<IMoleculeStartValue>
   {
      public MoleculeStartValueDiffBuilder(IObjectComparer objectComparer, EntityDiffBuilder entityDiffBuilder, WithValueOriginComparison<IMoleculeStartValue> valueOriginComparison)
         : base(objectComparer, entityDiffBuilder, valueOriginComparison)
      {
      }

      protected override void CompareStartValue(IComparison<IMoleculeStartValue> comparison)
      {
         base.CompareStartValue(comparison);
         CompareValues(x => x.IsPresent, x => x.IsPresent, comparison);
         CompareValues(x => x.ScaleDivisor, x => x.ScaleDivisor, comparison);
         CompareValues(x => x.NegativeValuesAllowed, x => x.NegativeValuesAllowed, comparison);
      }
   }

   internal class ParameterStartValueDiffBuilder : StartValueDiffBuilder<IParameterStartValue>
   {
      public ParameterStartValueDiffBuilder(IObjectComparer objectComparer, EntityDiffBuilder entityDiffBuilder, WithValueOriginComparison<IParameterStartValue> valueOriginComparison)
         : base(objectComparer, entityDiffBuilder, valueOriginComparison)
      {
      }
   }
}