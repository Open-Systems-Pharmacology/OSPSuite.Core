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

      //Contains a temp  cache of builder and their corresponding building blocks
      private readonly Cache<string, BuilderSource> _builderSources = new Cache<string, BuilderSource>(x => x.Builder.Id, x => null);

      //This will be saved in the simulation at the end of the construction process
      internal ObjectSources ObjectSources { get; } = new ObjectSources();

      public SimulationBuilder(SimulationConfiguration simulationConfiguration)
      {
         _simulationConfiguration = simulationConfiguration;
         performMerge();
      }

      public bool CreateAllProcessRateParameters => _simulationConfiguration.CreateAllProcessRateParameters;

      public IEntity BuilderFor(IEntity modelObject) => ObjectSources.SourceFor(modelObject)?.Source;

      internal void AddObjectSource(EntitySource entitySource)
      {
         ObjectSources.Add(entitySource);
      }

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
         cacheBuilders(x => x.Reactions, _reactions);
         cacheBuilders(x => x.Molecules, _molecules);
         cacheMoleculeDependentBuilders(x => x.PassiveTransports, _passiveTransports);
         cacheMoleculeDependentBuilders(x => x.Observers, _observers);
         cacheParameterValueBuilders(x => x.SelectedParameterValues, _parameterValues);
         cacheInitialConditions();
         cacheEntities();
      }

      private void cacheEntities()
      {
         cacheContainers(_reactions);
         cacheContainers(_molecules);
         cacheContainers(_passiveTransports);

         //also add individual if any to source
         AddToBuilderSource(_simulationConfiguration.Individual);
         _simulationConfiguration.ExpressionProfiles.Each(AddToBuilderSource);
      }

      private void cacheContainers(IEnumerable<IContainer> containers)
      {
         containers.Each(container =>
         {
            var containerSource = BuilderSourceFor(container);
            //this should never happen since we just created it
            if (containerSource == null)
            {
               Console.WriteLine($"Cannot find container source for {container.EntityPath()}");
               return;
            }

            var allEntities = container.GetAllChildren<IEntity>();
            allEntities.Each(entity => AddToBuilderSource(entity, containerSource.BuildingBlock));
         });
      }

      private void cacheInitialConditions()
      {
         var expressionProfileInitialConditionsCache = new PathAndValueEntityCache<InitialCondition>();
         var initialConditionsFromConfigurationsCache = new PathAndValueEntityCache<InitialCondition>();

         cacheParameterValueBuilders(x => x.SelectedInitialConditions, initialConditionsFromConfigurationsCache);
         var expressionProfileInitialConditions = allInitialConditionsFromExpressionProfileSources();
         expressionProfileInitialConditionsCache.AddRange(expressionProfileInitialConditions.Select(x => x.InitialCondition));
         addToBuilderSource(expressionProfileInitialConditions);

         // Concat order is important so that the values from expression profiles are overwritten if duplicated
         _initialConditions.AddRange(expressionProfileInitialConditionsCache.Concat(initialConditionsFromConfigurationsCache));
      }

      private void cacheMoleculeDependentBuilders<T>(Func<Module, IBuildingBlock<T>> propAccess, ObjectBaseCache<T> cache) where T : class, IMoleculeDependentBuilder
      {
         var builders = cacheBuilders(propAccess, cache);
         cacheMoleculeLists(builders, cache);
      }

      private IReadOnlyList<T> cacheBuilders<T>(Func<Module, IBuildingBlock<T>> propAccess, ObjectBaseCache<T> cache) where T : class, IBuilder, IEntity
      {
         var builderSources = allBuilderSources(propAccess);
         var builders = builderSources.Select(x => x.Builder).ToList();
         cache.AddRange(builders);
         addToBuilderSource(builderSources);
         return builders;
      }

      private void cacheParameterValueBuilders<T>(Func<ModuleConfiguration, IBuildingBlock<T>> propAccess, PathAndValueEntityCache<T> cache) where T : PathAndValueEntity
      {
         var builderSources = allParameterValueBuilderSources(propAccess);
         var builders = builderSources.Select(x => x.Builder).ToList();
         cache.AddRange(builders);
         addToBuilderSource(builderSources);
      }

      public void AddToBuilderSource<TBuilder>(PathAndValueEntityBuildingBlock<TBuilder> buildingBlock) where TBuilder : PathAndValueEntity =>
         buildingBlock?.Each(x => AddToBuilderSource(x, buildingBlock));

      public void AddToBuilderSource<TBuilder>(IBuildingBlock<TBuilder> buildingBlock) where TBuilder : IBuilder, IContainer =>
         buildingBlock.SelectMany(builder => builder.GetAllChildrenAndSelf<IEntity>()).Each(x => AddToBuilderSource(x, buildingBlock));

      public void AddToBuilderSource(IEntity builder, IBuildingBlock buildingBlock)
      {
         _builderSources[builder.Id] = new BuilderSource(builder, buildingBlock);
      }

      private void addToBuilderSource<T>(IEnumerable<(T Builder, IBuildingBlock BuildingBlock)> builderSources) where T : class, IBuilder, IEntity =>
         builderSources.Each(x => AddToBuilderSource(x.Builder, x.BuildingBlock));

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

      internal class BuilderSource
      {
         public IEntity Builder { get; }
         public IBuildingBlock BuildingBlock { get; }

         public BuilderSource(IEntity builder, IBuildingBlock buildingBlock)
         {
            Builder = builder;
            BuildingBlock = buildingBlock;
         }

         public override string ToString()
         {
            return $"BuilderSource: {Builder.EntityPath()} in {BuildingBlock.Name}";
         }
      }

      internal BuilderSource BuilderSourceFor(IEntity sourceEntity) => _builderSources[sourceEntity.Id];

      private IReadOnlyList<(T Builder, IBuildingBlock BuildingBlock)> allBuilderSources<T>(Func<Module, IBuildingBlock<T>> propAccess) where T : IBuilder =>
         _simulationConfiguration.ModuleConfigurations
            .Select(x => propAccess(x.Module))
            .Where(x => x != null)
            .SelectMany(x => x.Select(builder => (builder, (IBuildingBlock) x)))
            .ToList();

      private IReadOnlyList<(T Builder, IBuildingBlock BuildingBlock)> allParameterValueBuilderSources<T>(Func<ModuleConfiguration, IBuildingBlock<T>> propAccess) where T : PathAndValueEntity =>
         _simulationConfiguration.ModuleConfigurations
            .Select(propAccess)
            .Where(x => x != null)
            .SelectMany(x => x.Select(builder => (builder, (IBuildingBlock) x)))
            .ToList();

      private IReadOnlyList<(InitialCondition InitialCondition, IBuildingBlock BuildingBlock)> allInitialConditionsFromExpressionProfileSources() =>
         _simulationConfiguration.ExpressionProfiles
            .Select(x => (BuildingBlock: x, x.InitialConditions))
            //null because these conditions do not belong in a module
            .SelectMany(x => x.InitialConditions.Select(ic => (ic, (IBuildingBlock) x.BuildingBlock)))
            .ToList();
   }
}