using System;
using System.Collections.Generic;
using System.Linq;
using OSPSuite.Core.Serialization.SimModel.Services;
using OSPSuite.Utility.Collections;
using OSPSuite.Utility.Extensions;
using OSPSuite.Utility.Visitor;

namespace OSPSuite.Core.Domain.Builder
{
   public class SimulationConfiguration : IVisitable<IVisitor>
   {
      private readonly List<ExpressionProfileBuildingBlock> _expressionProfiles = new List<ExpressionProfileBuildingBlock>();
      private readonly List<ModuleConfiguration> _moduleConfigurations = new List<ModuleConfiguration>();
      private readonly List<ICoreCalculationMethod> _allCalculationMethods = new List<ICoreCalculationMethod>();

      //Temporary objects used for model construction only
      private readonly Cache<IObjectBase, IObjectBase> _builderCache = new Cache<IObjectBase, IObjectBase>(onMissingKey: x => null);
      private readonly ObjectBaseCache<ITransportBuilder> _passiveTransports = new ObjectBaseCache<ITransportBuilder>();
      private readonly ObjectBaseCache<IReactionBuilder> _reactions = new ObjectBaseCache<IReactionBuilder>();
      private readonly ObjectBaseCache<IEventGroupBuilder> _eventGroups = new ObjectBaseCache<IEventGroupBuilder>();
      private readonly ObjectBaseCache<IObserverBuilder> _observers = new ObjectBaseCache<IObserverBuilder>();
      private readonly ObjectBaseCache<IMoleculeBuilder> _molecules = new ObjectBaseCache<IMoleculeBuilder>();
      private readonly StartValueCache<ParameterStartValue> _parameterStartValues = new StartValueCache<ParameterStartValue>();
      private readonly StartValueCache<MoleculeStartValue> _moleculeStartValues = new StartValueCache<MoleculeStartValue>();

      public SimModelExportMode SimModelExportMode { get; set; } = SimModelExportMode.Full;

      public bool ShouldValidate { get; set; } = true;
      public bool ShowProgress { get; set; } = true;
      public bool PerformCircularReferenceCheck { get; set; } = true;

      public virtual IndividualBuildingBlock Individual { get; set; }
      public virtual SimulationSettings SimulationSettings { get; set; }

      public virtual IReadOnlyList<ExpressionProfileBuildingBlock> ExpressionProfiles => _expressionProfiles;
      public virtual IReadOnlyList<ICoreCalculationMethod> AllCalculationMethods => _allCalculationMethods;
      public virtual IReadOnlyList<ModuleConfiguration> ModuleConfigurations => _moduleConfigurations;

      public virtual void AddExpressionProfile(ExpressionProfileBuildingBlock expressionProfile) => _expressionProfiles.Add(expressionProfile);

      public virtual void AddModuleConfiguration(ModuleConfiguration moduleConfiguration) => _moduleConfigurations.Add(moduleConfiguration);

      public virtual void AddCalculationMethod(ICoreCalculationMethod calculationMethodToAdd) => _allCalculationMethods.Add(calculationMethodToAdd);

      public virtual IReadOnlyList<ISpatialStructure> SpatialStructures => all(x => x.SpatialStructure);
      public virtual IReadOnlyCollection<ITransportBuilder> PassiveTransports => _passiveTransports;
      public virtual IReadOnlyCollection<IReactionBuilder> Reactions => _reactions;
      public virtual IReadOnlyCollection<IEventGroupBuilder> EventGroups => _eventGroups;
      public virtual IReadOnlyCollection<IObserverBuilder> Observers => _observers;
      public virtual IReadOnlyCollection<IMoleculeBuilder> Molecules => _molecules;
      public virtual IReadOnlyCollection<ParameterStartValue> ParameterStartValues => _parameterStartValues;
      public virtual IReadOnlyCollection<MoleculeStartValue> MoleculeStartValues => _moleculeStartValues;

      public IMoleculeBuilder MoleculeByName(string name) => _molecules[name];

      private IReadOnlyList<T> all<T>(Func<Module, T> propAccess) where T : IBuildingBlock =>
         _moduleConfigurations.Select(x => propAccess(x.Module)).ToList();

      private IEnumerable<T> allBuilder<T>(Func<Module, IBuildingBlock<T>> propAccess) where T : IBuilder =>
         _moduleConfigurations.SelectMany(x => propAccess(x.Module));

      private IEnumerable<T> allStartValueBuilder<T>(Func<ModuleConfiguration, IBuildingBlock<T>> propAccess) where T : IStartValue =>
         _moduleConfigurations.Select(propAccess).Where(x => x != null).SelectMany(x => x);

      public virtual IEnumerable<IMoleculeBuilder> AllPresentMolecules()
      {
         var moleculeNames = _moleculeStartValues
            .Where(moleculeStartValue => moleculeStartValue.IsPresent)
            .Select(moleculeStartValue => moleculeStartValue.MoleculeName)
            .Distinct();


         return moleculeNames.Select(x => _molecules[x]).Where(m => m != null);
      }

      public virtual IEnumerable<MoleculeStartValue> AllPresentMoleculeValues() =>
         AllPresentMoleculeValuesFor(_molecules.Select(x => x.Name));

      public virtual IEnumerable<MoleculeStartValue> AllPresentMoleculeValuesFor(IEnumerable<string> moleculeNames)
      {
         return _moleculeStartValues
            .Where(msv => moleculeNames.Contains(msv.MoleculeName))
            .Where(msv => msv.IsPresent);
      }

      public virtual IEnumerable<IMoleculeBuilder> AllFloatingMolecules() => Molecules.Where(x => x.IsFloating);

      public virtual IReadOnlyList<string> AllPresentMoleculeNames() => AllPresentMoleculeNames(x => true);

      //Uses toArray so that the marshaling to R works out of the box (array vs list)
      public virtual IReadOnlyList<string> AllPresentMoleculeNames(Func<IMoleculeBuilder, bool> query) =>
         AllPresentMolecules().Where(query).Select(x => x.Name).ToArray();

      public virtual IReadOnlyList<string> AllPresentFloatingMoleculeNames() =>
         AllPresentMoleculeNames(m => m.IsFloating);

      public virtual IReadOnlyList<string> AllPresentStationaryMoleculeNames() =>
         AllPresentMoleculeNames(m => !m.IsFloating);

      public virtual IReadOnlyList<string> AllPresentXenobioticFloatingMoleculeNames() =>
         AllPresentMoleculeNames(m => m.IsFloating && m.IsXenobiotic);

      public virtual IReadOnlyList<string> AllPresentEndogenousStationaryMoleculeNames() =>
         AllPresentMoleculeNames(m => !m.IsFloating && !m.IsXenobiotic);

      public virtual IReadOnlyList<string> AllPresentEndogenousMoleculeNames() => AllPresentMoleculeNames(m => !m.IsXenobiotic);

      public virtual IObjectBase BuilderFor(IObjectBase modelObject) => _builderCache[modelObject];

      public virtual void AddBuilderReference(IObjectBase modelObject, IObjectBase builder)
      {
         _builderCache[modelObject] = builder;
      }

      //TODO clone? Update from?

      public virtual void AcceptVisitor(IVisitor visitor)
      {
         ModuleConfigurations.Each(x => x.AcceptVisitor(visitor));
         Individual?.AcceptVisitor(visitor);
         _expressionProfiles.Each(x => x.AcceptVisitor(visitor));
      }

      //Internal because this should not be called outside of core.
      internal virtual void Freeze()
      {
         _passiveTransports.AddRange(allBuilder(x => x.PassiveTransports));
         _reactions.AddRange(allBuilder(x => x.Reactions));
         _eventGroups.AddRange(allBuilder(x => x.EventGroups));
         _observers.AddRange(allBuilder(x => x.Observers));
         _molecules.AddRange(allBuilder(x => x.Molecules));
         _parameterStartValues.AddRange(allStartValueBuilder(x => x.SelectedParameterStartValues));
         _moleculeStartValues.AddRange(allStartValueBuilder(x => x.SelectedMoleculeStartValues));
      }

      public virtual void ClearCache()
      {
         _builderCache.Clear();
         _passiveTransports.Clear();
         _reactions.Clear();
         _eventGroups.Clear();
         _observers.Clear();
         _molecules.Clear();
         _parameterStartValues.Clear();
         _moleculeStartValues.Clear();
      }
   }
}