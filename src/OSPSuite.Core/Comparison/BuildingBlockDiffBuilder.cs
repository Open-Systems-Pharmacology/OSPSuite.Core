using System;
using System.Collections.Generic;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;

namespace OSPSuite.Core.Comparison
{
   public abstract class BuildingBlockDiffBuilder<TBuildingBlock, TBuilder> : DiffBuilder<TBuildingBlock> where TBuildingBlock : class, IBuildingBlock<TBuilder> where TBuilder : class, IBuilder
   {
      protected readonly ObjectBaseDiffBuilder _objectBaseDiffBuilder;
      protected readonly EnumerableComparer _enumerableComparer;
      protected readonly Func<TBuilder, object> _equalityProperty;
      protected Func<TBuilder, string> _presentObjectDetailsFunc;

      protected BuildingBlockDiffBuilder(ObjectBaseDiffBuilder objectBaseDiffBuilder,
         EnumerableComparer enumerableComparer, Func<TBuilder, object> equalityProperty)
      {
         _objectBaseDiffBuilder = objectBaseDiffBuilder;
         _equalityProperty = equalityProperty;
         _enumerableComparer = enumerableComparer;
      }

      protected BuildingBlockDiffBuilder(ObjectBaseDiffBuilder objectBaseDiffBuilder, EnumerableComparer enumerableComparer)
         : this(objectBaseDiffBuilder, enumerableComparer, item => item.Name)
      {
      }

      public override void Compare(IComparison<TBuildingBlock> comparison)
      {
         _objectBaseDiffBuilder.Compare(comparison);
         _enumerableComparer.CompareEnumerables(comparison, GetBuilders, _equalityProperty, _presentObjectDetailsFunc);
      }

      protected virtual IEnumerable<TBuilder> GetBuilders(TBuildingBlock buildingBlock)
      {
         return buildingBlock;
      }
   }

   public class MoleculeBuildingBlockDiffBuilder : BuildingBlockDiffBuilder<MoleculeBuildingBlock, MoleculeBuilder>
   {
      public MoleculeBuildingBlockDiffBuilder(ObjectBaseDiffBuilder objectBaseDiffBuilder, EnumerableComparer enumerableComparer) : base(objectBaseDiffBuilder, enumerableComparer)
      {
      }
   }

   public class SpatialStructureDiffBuilder : DiffBuilder<SpatialStructure>
   {
      private readonly ObjectBaseDiffBuilder _objectBaseDiffBuilder;
      private readonly EnumerableComparer _enumerableComparer;

      public SpatialStructureDiffBuilder(ObjectBaseDiffBuilder objectBaseDiffBuilder, EnumerableComparer enumerableComparer)
      {
         _objectBaseDiffBuilder = objectBaseDiffBuilder;
         _enumerableComparer = enumerableComparer;
      }

      public override void Compare(IComparison<SpatialStructure> comparison)
      {
         _objectBaseDiffBuilder.Compare(comparison);
         _enumerableComparer.CompareEnumerables(comparison, x => x, x => x.Name);
      }
   }

   public class ReactionBuildingBlockDiffBuilder : BuildingBlockDiffBuilder<ReactionBuildingBlock, ReactionBuilder>
   {
      public ReactionBuildingBlockDiffBuilder(ObjectBaseDiffBuilder objectBaseDiffBuilder, EnumerableComparer enumerableComparer) : base(objectBaseDiffBuilder, enumerableComparer)
      {
      }
   }

   public class PassiveTransportBuildingBlockDiffBuilder : BuildingBlockDiffBuilder<PassiveTransportBuildingBlock, TransportBuilder>
   {
      public PassiveTransportBuildingBlockDiffBuilder(ObjectBaseDiffBuilder objectBaseDiffBuilder, EnumerableComparer enumerableComparer)
         : base(objectBaseDiffBuilder, enumerableComparer)
      {
      }
   }

   public class EventGroupBuildingBlocksDiffBuilder : BuildingBlockDiffBuilder<EventGroupBuildingBlock, EventGroupBuilder>
   {
      public EventGroupBuildingBlocksDiffBuilder(ObjectBaseDiffBuilder objectBaseDiffBuilder, EnumerableComparer enumerableComparer)
         : base(objectBaseDiffBuilder, enumerableComparer)
      {
      }
   }

   public class ObserverBuildingBlocksDiffBuilder : BuildingBlockDiffBuilder<ObserverBuildingBlock, ObserverBuilder>
   {
      public ObserverBuildingBlocksDiffBuilder(ObjectBaseDiffBuilder objectBaseDiffBuilder, EnumerableComparer enumerableComparer)
         : base(objectBaseDiffBuilder, enumerableComparer)
      {
      }
   }

   public abstract class StartValueBuildingBlockDiffBuilder<TBuildingBlock, TStartValue> : PathAndValueEntityBuildingBlockDiffBuilder<TBuildingBlock, TStartValue> where TStartValue : PathAndValueEntity where TBuildingBlock : class, IBuildingBlock<TStartValue>
   {
      private readonly UnitFormatter _unitFormatter = new UnitFormatter();

      protected StartValueBuildingBlockDiffBuilder(ObjectBaseDiffBuilder objectBaseDiffBuilder, EnumerableComparer enumerableComparer) :
         base(objectBaseDiffBuilder, enumerableComparer)
      {
         _presentObjectDetailsFunc = startValueDisplayFor;
      }

      private string startValueDisplayFor(TStartValue startValue)
      {
         if (startValue.Formula != null)
         {
            var name = startValue.Formula.Name;
            var display = startValue.Formula.ToString();
            return string.Equals(name, display) ? name : $"{name} ({display})";
         }

         var value = startValue.ConvertToDisplayUnit(startValue.Value);
         return _unitFormatter.Format(value, startValue.DisplayUnit);
      }
   }

   public class InitialConditionsBuildingBlockDiffBuilder : StartValueBuildingBlockDiffBuilder<InitialConditionsBuildingBlock, InitialCondition>
   {
      public InitialConditionsBuildingBlockDiffBuilder(ObjectBaseDiffBuilder objectBaseDiffBuilder, EnumerableComparer enumerableComparer) : base(objectBaseDiffBuilder, enumerableComparer)
      {
      }
   }

   public class ParameterValueBuildingBlockDiffBuilder : StartValueBuildingBlockDiffBuilder<ParameterValuesBuildingBlock, ParameterValue>
   {
      public ParameterValueBuildingBlockDiffBuilder(ObjectBaseDiffBuilder objectBaseDiffBuilder, EnumerableComparer enumerableComparer) : base(objectBaseDiffBuilder, enumerableComparer)
      {
      }
   }
}