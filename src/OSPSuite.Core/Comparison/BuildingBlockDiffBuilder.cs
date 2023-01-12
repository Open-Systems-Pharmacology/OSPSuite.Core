using System;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;

namespace OSPSuite.Core.Comparison
{
   public abstract class BuildingBlockDiffBuilder<TBuildingBlock, TBuilder> : DiffBuilder<TBuildingBlock> where TBuildingBlock : class, IBuildingBlock<TBuilder> where TBuilder : class, IObjectBase
   {
      private readonly ObjectBaseDiffBuilder _objectBaseDiffBuilder;
      private readonly EnumerableComparer _enumerableComparer;
      private readonly Func<TBuilder, object> _equalityProperty;
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
         _enumerableComparer.CompareEnumerables(comparison, x => x, _equalityProperty, _presentObjectDetailsFunc);
      }
   }

   public class MoleculeBuildingBlockDiffBuilder : BuildingBlockDiffBuilder<IMoleculeBuildingBlock, IMoleculeBuilder>
   {
      public MoleculeBuildingBlockDiffBuilder(ObjectBaseDiffBuilder objectBaseDiffBuilder, EnumerableComparer enumerableComparer) : base(objectBaseDiffBuilder, enumerableComparer)
      {
      }
   }

   public class SpatialStructureDiffBuilder : BuildingBlockDiffBuilder<ISpatialStructure, IContainer>
   {
      public SpatialStructureDiffBuilder(ObjectBaseDiffBuilder objectBaseDiffBuilder, EnumerableComparer enumerableComparer)
         : base(objectBaseDiffBuilder, enumerableComparer)
      {
      }
   }

   public class ReactionBuildingBlockDiffBuilder : BuildingBlockDiffBuilder<IReactionBuildingBlock, IReactionBuilder>
   {
      public ReactionBuildingBlockDiffBuilder(ObjectBaseDiffBuilder objectBaseDiffBuilder, EnumerableComparer enumerableComparer) : base(objectBaseDiffBuilder, enumerableComparer)
      {
      }
   }

   public class PassiveTransportBuildingBlockDiffBuilder : BuildingBlockDiffBuilder<IPassiveTransportBuildingBlock, ITransportBuilder>
   {
      public PassiveTransportBuildingBlockDiffBuilder(ObjectBaseDiffBuilder objectBaseDiffBuilder, EnumerableComparer enumerableComparer)
         : base(objectBaseDiffBuilder, enumerableComparer)
      {
      }
   }

   public class EventGroupBuildingBlocksDiffBuilder : BuildingBlockDiffBuilder<IEventGroupBuildingBlock, IEventGroupBuilder>
   {
      public EventGroupBuildingBlocksDiffBuilder(ObjectBaseDiffBuilder objectBaseDiffBuilder, EnumerableComparer enumerableComparer)
         : base(objectBaseDiffBuilder, enumerableComparer)
      {
      }
   }

   public class ObserverBuildingBlocksDiffBuilder : BuildingBlockDiffBuilder<IObserverBuildingBlock, IObserverBuilder>
   {
      public ObserverBuildingBlocksDiffBuilder(ObjectBaseDiffBuilder objectBaseDiffBuilder, EnumerableComparer enumerableComparer)
         : base(objectBaseDiffBuilder, enumerableComparer)
      {
      }
   }

   public abstract class StartValueBuildingBlockDiffBuilder<TBuildingBlock, TStartValue> : PathAndAndValueEntityBuildingBlockDiffBuilder<TBuildingBlock, TStartValue> where TStartValue : StartValueBase, IStartValue where TBuildingBlock : class, IBuildingBlock<TStartValue>
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

   public class MoleculeStartValueBuildingBlockDiffBuilder : StartValueBuildingBlockDiffBuilder<IMoleculeStartValuesBuildingBlock, MoleculeStartValue>
   {
      public MoleculeStartValueBuildingBlockDiffBuilder(ObjectBaseDiffBuilder objectBaseDiffBuilder, EnumerableComparer enumerableComparer) : base(objectBaseDiffBuilder, enumerableComparer)
      {
      }
   }

   public class ParameterStartValueBuildingBlockDiffBuilder : StartValueBuildingBlockDiffBuilder<IParameterStartValuesBuildingBlock, ParameterStartValue>
   {
      public ParameterStartValueBuildingBlockDiffBuilder(ObjectBaseDiffBuilder objectBaseDiffBuilder, EnumerableComparer enumerableComparer) : base(objectBaseDiffBuilder, enumerableComparer)
      {
      }
   }
}