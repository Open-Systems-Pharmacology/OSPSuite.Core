using System;
using System.Collections.Generic;
using System.Linq;
using OSPSuite.Utility.Collections;
using OSPSuite.Utility.Extensions;

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
      private readonly StartValueCache<ParameterValue> _parameterValues = new StartValueCache<ParameterValue>();
      private readonly StartValueCache<InitialCondition> _initialConditions = new StartValueCache<InitialCondition>();
      private readonly Cache<IMoleculeDependentBuilder, MoleculeList> _moleculeListCache = new Cache<IMoleculeDependentBuilder, MoleculeList>();

      private readonly Cache<IObjectBase, IObjectBase> _builderCache = new Cache<IObjectBase, IObjectBase>(onMissingKey: x => null);

      public SimulationBuilder(SimulationConfiguration simulationConfiguration)
      {
         _simulationConfiguration = simulationConfiguration;
         performMerge();
      }

      public IObjectBase BuilderFor(IObjectBase modelObject) => _builderCache[modelObject];

      internal void AddBuilderReference(IObjectBase modelObject, IObjectBase builder)
      {
         _builderCache[modelObject] = builder;
      }

      private IReadOnlyList<T> all<T>(Func<Module, T> propAccess) where T : IBuildingBlock =>
         _simulationConfiguration.ModuleConfigurations.Select(x => propAccess(x.Module)).Where(x => x != null).ToList();

      private IEnumerable<T> allBuilder<T>(Func<Module, IBuildingBlock<T>> propAccess) where T : IBuilder =>
         all(propAccess).SelectMany(x => x);

      private IEnumerable<T> allStartValueBuilder<T>(Func<ModuleConfiguration, IBuildingBlock<T>> propAccess) where T : PathAndValueEntity =>
         _simulationConfiguration.ModuleConfigurations.Select(propAccess).Where(x => x != null).SelectMany(x => x);

      internal IEnumerable<MoleculeBuilder> AllPresentMolecules()
      {
         var moleculeNames = _initialConditions
            .Where(initialCondition => initialCondition.IsPresent)
            .Select(initialCondition => initialCondition.MoleculeName)
            .Distinct();

         return moleculeNames.Select(x => _molecules[x]).Where(m => m != null);
      }

      internal IEnumerable<InitialCondition> AllPresentMoleculeValues() =>
         AllPresentMoleculeValuesFor(_molecules.Select(x => x.Name));

      internal IEnumerable<InitialCondition> AllPresentMoleculeValuesFor(IEnumerable<string> moleculeNames)
      {
         return _initialConditions
            .Where(initialCondition => moleculeNames.Contains(initialCondition.MoleculeName))
            .Where(initialCondition => initialCondition.IsPresent);
      }

      internal IEnumerable<MoleculeBuilder> AllFloatingMolecules() => Molecules.Where(x => x.IsFloating);

      public IReadOnlyList<string> AllPresentMoleculeNames() => AllPresentMoleculeNames(x => true);

      //Uses toArray so that the marshaling to R works out of the box (array vs list)
      public IReadOnlyList<string> AllPresentMoleculeNames(Func<MoleculeBuilder, bool> query) =>
         AllPresentMolecules().Where(query).Select(x => x.Name).ToArray();

      public IReadOnlyList<string> AllPresentFloatingMoleculeNames() =>
         AllPresentMoleculeNames(m => m.IsFloating);

      public IReadOnlyList<string> AllPresentStationaryMoleculeNames() =>
         AllPresentMoleculeNames(m => !m.IsFloating);

      public IReadOnlyList<string> AllPresentXenobioticFloatingMoleculeNames() =>
         AllPresentMoleculeNames(m => m.IsFloating && m.IsXenobiotic);

      public IReadOnlyList<string> AllPresentEndogenousStationaryMoleculeNames() =>
         AllPresentMoleculeNames(m => !m.IsFloating && !m.IsXenobiotic);

      public IReadOnlyList<string> AllPresentEndogenousMoleculeNames() => AllPresentMoleculeNames(m => !m.IsXenobiotic);

      private void performMerge()
      {
         cacheMoleculeDependentBuilders(x => x.PassiveTransports, _passiveTransports);

         _reactions.AddRange(allBuilder(x => x.Reactions));
         _eventGroups.AddRange(allBuilder(x => x.EventGroups));

         cacheMoleculeDependentBuilders(x => x.Observers, _observers);

         _molecules.AddRange(allBuilder(x => x.Molecules));
         _parameterValues.AddRange(allStartValueBuilder(x => x.SelectedParameterValues));
         _initialConditions.AddRange(allStartValueBuilder(x => x.SelectedInitialConditions)
            .Concat(_simulationConfiguration.ExpressionProfiles.SelectMany(x => x.InitialConditions)));
      }

      private void cacheMoleculeDependentBuilders<T>(Func<Module, IBuildingBlock<T>> propAccess, ObjectBaseCache<T> cache) where T : class, IMoleculeDependentBuilder
      {
         var builders = allBuilder(propAccess).ToList();
         cache.AddRange(builders);
         cacheMoleculeLists(builders, cache);
      }

      private void cacheMoleculeLists<T>(IReadOnlyList<T> allBuilders, ObjectBaseCache<T> builderCache) where T : class, IMoleculeDependentBuilder
      {
         builderCache.Each(builder => { combineMoleculeLists(allBuilders.Where(x => string.Equals(x.Name, builder.Name)).ToList(), builder); });
      }

      private void combineMoleculeLists(IReadOnlyList<IMoleculeDependentBuilder> builders, IMoleculeDependentBuilder lastBuilder)
      {
         _moleculeListCache[lastBuilder] = lastBuilder.MoleculeList.Clone();
         builders.Each(x => addMolecules(x, _moleculeListCache[lastBuilder]));
      }

      private void addMolecules(IMoleculeDependentBuilder builder, MoleculeList moleculeList)
      {
         builder.MoleculeList.MoleculeNames.Each(moleculeList.AddMoleculeName);
         builder.MoleculeList.MoleculeNamesToExclude.Each(moleculeList.AddMoleculeNameToExclude);
      }

      internal IReadOnlyList<SpatialStructure> SpatialStructures => all(x => x.SpatialStructure);
      internal IReadOnlyCollection<TransportBuilder> PassiveTransports => _passiveTransports;
      internal IReadOnlyCollection<ReactionBuilder> Reactions => _reactions;
      internal IReadOnlyCollection<EventGroupBuilder> EventGroups => _eventGroups;
      internal IReadOnlyCollection<ObserverBuilder> Observers => _observers;
      internal IReadOnlyCollection<MoleculeBuilder> Molecules => _molecules;
      internal IReadOnlyCollection<ParameterValue> ParameterValues => _parameterValues;
      internal IReadOnlyCollection<InitialCondition> InitialConditions => _initialConditions;
      internal MoleculeList MoleculeListFor(IMoleculeDependentBuilder builder) => _moleculeListCache[builder];

      internal MoleculeBuilder MoleculeByName(string name) => _molecules[name];
   }
}