using System;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;

namespace OSPSuite.Core.Comparison
{
   public abstract class BuildingBlockDiffBuilder<T> : DiffBuilder<IBuildingBlock<T>> where T : class, IObjectBase
   {
      private readonly ObjectBaseDiffBuilder _objectBaseDiffBuilder;
      private readonly EnumerableComparer _enumerableComparer;
      private readonly Func<T, object> _equalityProperty;
      protected Func<T, string> _presentObjectDetailsFunc;

      protected BuildingBlockDiffBuilder(ObjectBaseDiffBuilder objectBaseDiffBuilder,
         EnumerableComparer enumerableComparer, Func<T, object> equalityProperty)
      {
         _objectBaseDiffBuilder = objectBaseDiffBuilder;
         _equalityProperty = equalityProperty;
         _enumerableComparer = enumerableComparer;
      }

      protected BuildingBlockDiffBuilder(ObjectBaseDiffBuilder objectBaseDiffBuilder, EnumerableComparer enumerableComparer)
         : this(objectBaseDiffBuilder, enumerableComparer, item => item.Name)
      {
      }

      public override void Compare(IComparison<IBuildingBlock<T>> comparison)
      {
         _objectBaseDiffBuilder.Compare(comparison);
         _enumerableComparer.CompareEnumerables(comparison, x => x, _equalityProperty, _presentObjectDetailsFunc);
      }
   }

   public class MoleculeBuildingBlockDiffBuilder : BuildingBlockDiffBuilder<IMoleculeBuilder>
   {
      public MoleculeBuildingBlockDiffBuilder(ObjectBaseDiffBuilder objectBaseDiffBuilder, EnumerableComparer enumerableComparer) : base(objectBaseDiffBuilder, enumerableComparer)
      {
      }
   }

   public class SpatialStructureDiffBuilder : BuildingBlockDiffBuilder<IContainer>
   {
      public SpatialStructureDiffBuilder(ObjectBaseDiffBuilder objectBaseDiffBuilder, EnumerableComparer enumerableComparer)
         : base(objectBaseDiffBuilder, enumerableComparer)
      {
      }
   }

   public class ReactionBuildingBlockDiffBuilder : BuildingBlockDiffBuilder<IReactionBuilder>
   {
      public ReactionBuildingBlockDiffBuilder(ObjectBaseDiffBuilder objectBaseDiffBuilder, EnumerableComparer enumerableComparer) : base(objectBaseDiffBuilder, enumerableComparer)
      {
      }
   }

   public class PassiveTransportBuildingBlockDiffBuilder : BuildingBlockDiffBuilder<ITransportBuilder>
   {
      public PassiveTransportBuildingBlockDiffBuilder(ObjectBaseDiffBuilder objectBaseDiffBuilder, EnumerableComparer enumerableComparer)
         : base(objectBaseDiffBuilder, enumerableComparer)
      {
      }
   }

   public class EventGroupBuildingBlocksDiffBuilder : BuildingBlockDiffBuilder<IEventGroupBuilder>
   {
      public EventGroupBuildingBlocksDiffBuilder(ObjectBaseDiffBuilder objectBaseDiffBuilder, EnumerableComparer enumerableComparer)
         : base(objectBaseDiffBuilder, enumerableComparer)
      {
      }
   }

   public class ObserverBuildingBlocksDiffBuilder : BuildingBlockDiffBuilder<IObserverBuilder>
   {
      public ObserverBuildingBlocksDiffBuilder(ObjectBaseDiffBuilder objectBaseDiffBuilder, EnumerableComparer enumerableComparer)
         : base(objectBaseDiffBuilder, enumerableComparer)
      {
      }
   }

   public abstract class StartValueBuildingBlockDiffBuilder<TStartValue> : BuildingBlockDiffBuilder<TStartValue> where TStartValue : class, IStartValue
   {
      private readonly UnitFormatter _unitFormatter = new UnitFormatter();

      protected StartValueBuildingBlockDiffBuilder(ObjectBaseDiffBuilder objectBaseDiffBuilder, EnumerableComparer enumerableComparer) :
         base(objectBaseDiffBuilder, enumerableComparer, x => x.Path)
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

         var value = startValue.ConvertToDisplayUnit(startValue.StartValue);
         return _unitFormatter.Format(value, startValue.DisplayUnit);
      }
   }

   public class MoleculeStartValueBuildingBlockDiffBuilder : StartValueBuildingBlockDiffBuilder<IMoleculeStartValue>
   {
      public MoleculeStartValueBuildingBlockDiffBuilder(ObjectBaseDiffBuilder objectBaseDiffBuilder, EnumerableComparer enumerableComparer) : base(objectBaseDiffBuilder, enumerableComparer)
      {
      }
   }

   public class ParameterStartValueBuildingBlockDiffBuilder : StartValueBuildingBlockDiffBuilder<IParameterStartValue>
   {
      public ParameterStartValueBuildingBlockDiffBuilder(ObjectBaseDiffBuilder objectBaseDiffBuilder, EnumerableComparer enumerableComparer) : base(objectBaseDiffBuilder, enumerableComparer)
      {
      }
   }
}