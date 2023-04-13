using System;
using System.Collections.Generic;
using System.Linq;
using OSPSuite.Utility.Collections;

namespace OSPSuite.Core.Domain.Builder
{
   public class SimulationBuilder
   {
      private readonly SimulationConfiguration _simulationConfiguration;

      private readonly ObjectBaseCache<TransportBuilder> _passiveTransports = new ObjectBaseCache<TransportBuilder>();
      private readonly ObjectBaseCache<ReactionBuilder> _reactions = new ObjectBaseCache<ReactionBuilder>();
      private readonly ObjectBaseCache<EventGroupBuilder> _eventGroups = new ObjectBaseCache<EventGroupBuilder>();
      private readonly ObjectBaseCache<ObserverBuilder> _observers = new ObjectBaseCache<ObserverBuilder>();
      private readonly ObjectBaseCache<MoleculeBuilder> _molecules = new ObjectBaseCache<MoleculeBuilder>();
      private readonly StartValueCache<ParameterStartValue> _parameterStartValues = new StartValueCache<ParameterStartValue>();
      private readonly StartValueCache<MoleculeStartValue> _moleculeStartValues = new StartValueCache<MoleculeStartValue>();

      private readonly Cache<IObjectBase, IObjectBase> _builderCache = new Cache<IObjectBase, IObjectBase>(onMissingKey: x => null);

      public SimulationBuilder(SimulationConfiguration simulationConfiguration)
      {
         _simulationConfiguration = simulationConfiguration;
         performMerge();
      }

      internal IObjectBase BuilderFor(IObjectBase modelObject) => _builderCache[modelObject];

      internal void AddBuilderReference(IObjectBase modelObject, IObjectBase builder)
      {
         _builderCache[modelObject] = builder;
      }

      private IReadOnlyList<T> all<T>(Func<Module, T> propAccess) where T : IBuildingBlock =>
         _simulationConfiguration.ModuleConfigurations.Select(x => propAccess(x.Module)).Where(x => x != null).ToList();

      private IEnumerable<T> allBuilder<T>(Func<Module, IBuildingBlock<T>> propAccess) where T : IBuilder =>
         all(propAccess).SelectMany(x => x);

      private IEnumerable<T> allStartValueBuilder<T>(Func<ModuleConfiguration, IBuildingBlock<T>> propAccess) where T : IStartValue =>
         _simulationConfiguration.ModuleConfigurations.Select(propAccess).Where(x => x != null).SelectMany(x => x);

      internal IEnumerable<MoleculeBuilder> AllPresentMolecules()
      {
         var moleculeNames = _moleculeStartValues
            .Where(moleculeStartValue => moleculeStartValue.IsPresent)
            .Select(moleculeStartValue => moleculeStartValue.MoleculeName)
            .Distinct();


         return moleculeNames.Select(x => _molecules[x]).Where(m => m != null);
      }

      internal IEnumerable<MoleculeStartValue> AllPresentMoleculeValues() =>
         AllPresentMoleculeValuesFor(_molecules.Select(x => x.Name));

      internal IEnumerable<MoleculeStartValue> AllPresentMoleculeValuesFor(IEnumerable<string> moleculeNames)
      {
         return _moleculeStartValues
            .Where(msv => moleculeNames.Contains(msv.MoleculeName))
            .Where(msv => msv.IsPresent);
      }

      internal IEnumerable<MoleculeBuilder> AllFloatingMolecules() => Molecules.Where(x => x.IsFloating);

      internal IReadOnlyList<string> AllPresentMoleculeNames() => AllPresentMoleculeNames(x => true);

      //Uses toArray so that the marshaling to R works out of the box (array vs list)
      internal IReadOnlyList<string> AllPresentMoleculeNames(Func<MoleculeBuilder, bool> query) =>
         AllPresentMolecules().Where(query).Select(x => x.Name).ToArray();

      internal IReadOnlyList<string> AllPresentFloatingMoleculeNames() =>
         AllPresentMoleculeNames(m => m.IsFloating);

      internal IReadOnlyList<string> AllPresentStationaryMoleculeNames() =>
         AllPresentMoleculeNames(m => !m.IsFloating);

      internal IReadOnlyList<string> AllPresentXenobioticFloatingMoleculeNames() =>
         AllPresentMoleculeNames(m => m.IsFloating && m.IsXenobiotic);

      internal IReadOnlyList<string> AllPresentEndogenousStationaryMoleculeNames() =>
         AllPresentMoleculeNames(m => !m.IsFloating && !m.IsXenobiotic);

      internal IReadOnlyList<string> AllPresentEndogenousMoleculeNames() => AllPresentMoleculeNames(m => !m.IsXenobiotic);

      private void performMerge()
      {
         _passiveTransports.AddRange(allBuilder(x => x.PassiveTransports));
         _reactions.AddRange(allBuilder(x => x.Reactions));
         _eventGroups.AddRange(allBuilder(x => x.EventGroups));
         _observers.AddRange(allBuilder(x => x.Observers));
         _molecules.AddRange(allBuilder(x => x.Molecules));
         _parameterStartValues.AddRange(allStartValueBuilder(x => x.SelectedParameterStartValues));
         _moleculeStartValues.AddRange(allStartValueBuilder(x => x.SelectedMoleculeStartValues));
      }

      internal IReadOnlyList<SpatialStructure> SpatialStructures => all(x => x.SpatialStructure);
      internal IReadOnlyCollection<TransportBuilder> PassiveTransports => _passiveTransports;
      internal IReadOnlyCollection<ReactionBuilder> Reactions => _reactions;
      internal IReadOnlyCollection<EventGroupBuilder> EventGroups => _eventGroups;
      internal IReadOnlyCollection<ObserverBuilder> Observers => _observers;
      internal IReadOnlyCollection<MoleculeBuilder> Molecules => _molecules;
      internal IReadOnlyCollection<ParameterStartValue> ParameterStartValues => _parameterStartValues;
      internal IReadOnlyCollection<MoleculeStartValue> MoleculeStartValues => _moleculeStartValues;

      internal MoleculeBuilder MoleculeByName(string name) => _molecules[name];
   }
}