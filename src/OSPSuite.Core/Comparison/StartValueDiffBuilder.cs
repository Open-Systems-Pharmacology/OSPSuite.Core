using OSPSuite.Core.Domain.Builder;

namespace OSPSuite.Core.Comparison
{
   internal abstract class StartValueDiffBuilder<T> : DiffBuilder<T> where T : class, IStartValue
   {
      private readonly IObjectComparer _objectComparer;
      private readonly EntityDiffBuilder _entityDiffBuilder;

      protected StartValueDiffBuilder(IObjectComparer objectComparer, EntityDiffBuilder entityDiffBuilder)
      {
         _objectComparer = objectComparer;
         _entityDiffBuilder = entityDiffBuilder;
      }


      protected void CompareStartValue(IComparison<T> comparison)
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
      public MoleculeStartValueDiffBuilder(IObjectComparer objectComparer, EntityDiffBuilder entityDiffBuilder)
         : base(objectComparer, entityDiffBuilder)
      {
      }

      public override void Compare(IComparison<IMoleculeStartValue> comparison)
      {
         CompareStartValue(comparison);
         CompareValues(x => x.IsPresent, x => x.IsPresent, comparison);
         CompareValues(x => x.ScaleDivisor, x => x.ScaleDivisor, comparison);
         CompareValues(x => x.NegativeValuesAllowed, x => x.NegativeValuesAllowed, comparison);
      }
   }

   internal class ParameterStartValueDiffBuilder : StartValueDiffBuilder<IParameterStartValue>
   {
      public ParameterStartValueDiffBuilder(IObjectComparer objectComparer, EntityDiffBuilder entityDiffBuilder)
         : base(objectComparer, entityDiffBuilder)
      {
      }

      public override void Compare(IComparison<IParameterStartValue> comparison)
      {
         CompareStartValue(comparison);
      }
   }
}