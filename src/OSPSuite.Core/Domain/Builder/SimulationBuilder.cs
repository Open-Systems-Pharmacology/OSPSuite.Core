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
      private readonly ObjectBaseCache<ObserverBuilder> _observers = new ObjectBaseCache<ObserverBuilder>();
      private readonly ObjectBaseCache<MoleculeBuilder> _molecules = new ObjectBaseCache<MoleculeBuilder>();
      private readonly PathAndValueEntityCache<ParameterValue> _parameterValues = new PathAndValueEntityCache<ParameterValue>();
      private readonly PathAndValueEntityCache<InitialCondition> _initialConditions = new PathAndValueEntityCache<InitialCondition>();
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
         cacheMoleculeDependentBuilders(x => x.Observers, _observers);
         _molecules.AddRange(allBuilder(x => x.Molecules));
         _parameterValues.AddRange(allStartValueBuilder(x => x.SelectedParameterValues));

         // Concat order is important so that the values from expression profiles are overwritten if duplicated
         _initialConditions.AddRange(_simulationConfiguration.ExpressionProfiles.SelectMany(x => x.InitialConditions)
            .Concat(allStartValueBuilder(x => x.SelectedInitialConditions)));
      }

      private void cacheMoleculeDependentBuilders<T>(Func<Module, IBuildingBlock<T>> propAccess, ObjectBaseCache<T> cache) where T : class, IMoleculeDependentBuilder
      {
         var builders = allBuilder(propAccess).ToList();
         cache.AddRange(builders);
         cacheMoleculeLists(builders, cache);
      }

      private void cacheMoleculeLists<T>(IReadOnlyList<T> allBuilders, ObjectBaseCache<T> builderCache) where T : class, IMoleculeDependentBuilder
      {
         builderCache.Each(builderUsedInSimulation => combineMoleculeLists(allBuilders.Where(x => x.IsNamed(builderUsedInSimulation.Name)), builderUsedInSimulation));
      }

      private void combineMoleculeLists(IEnumerable<IMoleculeDependentBuilder> builders, IMoleculeDependentBuilder builderUsedInSimulation)
      {
         _moleculeListCache[builderUsedInSimulation] = builderUsedInSimulation.MoleculeList.Clone();
         builders.Each(x => addMolecules(x, _moleculeListCache[builderUsedInSimulation]));
      }

      private void addMolecules(IMoleculeDependentBuilder builder, MoleculeList moleculeList)
      {
         builder.MoleculeList.MoleculeNames.Each(moleculeList.AddMoleculeName);
         builder.MoleculeList.MoleculeNamesToExclude.Each(moleculeList.AddMoleculeNameToExclude);
      }

      internal IReadOnlyList<(SpatialStructure spatialStructure, MergeBehavior mergeBehavior)> SpatialStructureAndMergeBehaviors =>
         _simulationConfiguration.ModuleConfigurations.Where(x => x.Module.SpatialStructure != null).Select(x => (x.Module.SpatialStructure, x.MergeBehavior)).ToList();
      internal IReadOnlyList<(EventGroupBuildingBlock eventGroupBuildingBlock, MergeBehavior mergeBehavior)> EventGroupAndMergeBehaviors =>
         _simulationConfiguration.ModuleConfigurations.Where(x => x.Module.EventGroups != null).Select(x => (x.Module.EventGroups, x.MergeBehavior)).ToList();

      internal IReadOnlyCollection<TransportBuilder> PassiveTransports => _passiveTransports;
      internal IReadOnlyCollection<ReactionBuilder> Reactions => _reactions;
      internal IReadOnlyCollection<ObserverBuilder> Observers => _observers;
      internal IReadOnlyCollection<MoleculeBuilder> Molecules => _molecules;
      internal IReadOnlyCollection<ParameterValue> ParameterValues => _parameterValues;
      internal IReadOnlyCollection<InitialCondition> InitialConditions => _initialConditions;
      internal MoleculeList MoleculeListFor(IMoleculeDependentBuilder builder) => _moleculeListCache[builder];

      internal MoleculeBuilder MoleculeByName(string name) => _molecules[name];
   }
}