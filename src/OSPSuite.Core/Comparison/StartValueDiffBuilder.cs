using OSPSuite.Assets;
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
         _objectComparer.Compare(comparison.DimensionComparison());

         CompareValues(x => x.ContainerPath, x => x.ContainerPath, comparison);

         // if the distribution types are different then we cannot compare values since the value
         // of a distributed PathAndValueEntity is not fully calculated until  it is turned into
         // a parameter
         if(!comparison.Object1.DistributionType.Equals(comparison.Object2.DistributionType))
            CompareValues(x => x.DistributionType, x => x.DistributionType, comparison, 
               areEqual: (type1, type2) => Equals(type2, type1), 
               formatter: (entity, type) => type == null ? Captions.NotDistributed : type.ToString());
         else
         {
            // Always Compare Value and Formula, independent of settings as these are two different properties of a start value
            CompareNullableDoubleValues(x => x.Value, x => x.Value, comparison, x => x.DisplayUnit);
            _objectComparer.Compare(comparison.FormulaComparison());
         }
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

   internal class InitialConditionDiffBuilder : StartValueDiffBuilder<InitialCondition>
   {
      public InitialConditionDiffBuilder(IObjectComparer objectComparer, EntityDiffBuilder entityDiffBuilder, WithValueOriginComparison<InitialCondition> valueOriginComparison)
         : base(objectComparer, entityDiffBuilder, valueOriginComparison)
      {
      }

      protected override void ComparePathAndEntityValues(IComparison<InitialCondition> comparison)
      {
         base.ComparePathAndEntityValues(comparison);
         CompareValues(x => x.IsPresent, x => x.IsPresent, comparison);
         CompareValues(x => x.ScaleDivisor, x => x.ScaleDivisor, comparison);
         CompareValues(x => x.NegativeValuesAllowed, x => x.NegativeValuesAllowed, comparison);
      }
   }

   internal class ParameterValueDiffBuilder : StartValueDiffBuilder<ParameterValue>
   {
      public ParameterValueDiffBuilder(IObjectComparer objectComparer, EntityDiffBuilder entityDiffBuilder, WithValueOriginComparison<ParameterValue> valueOriginComparison)
         : base(objectComparer, entityDiffBuilder, valueOriginComparison)
      {
      }
   }
}