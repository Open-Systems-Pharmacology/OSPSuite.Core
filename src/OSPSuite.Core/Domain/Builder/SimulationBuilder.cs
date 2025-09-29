using System;
using System.Collections.Generic;
using System.Linq;
using OSPSuite.Core.Domain.Descriptors;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Utility.Collections;
using OSPSuite.Utility.Extensions;

namespace OSPSuite.Core.Domain.Builder
{
   public class SimulationBuilder
   {
      private readonly ICloneManagerForModel _cloneManager;
      private readonly IContainerMergeTask _containerMergeTask;
      private SimulationConfiguration _simulationConfiguration;

      private readonly CacheByName<TransportBuilder> _passiveTransports = new CacheByName<TransportBuilder>();
      private readonly CacheByName<ReactionBuilder> _reactions = new CacheByName<ReactionBuilder>();
      private readonly CacheByName<ObserverBuilder> _observers = new CacheByName<ObserverBuilder>();
      private readonly CacheByName<MoleculeBuilder> _molecules = new CacheByName<MoleculeBuilder>();
      private readonly PathAndValueEntityCache<ParameterValue> _parameterValues = new PathAndValueEntityCache<ParameterValue>();
      private readonly PathAndValueEntityCache<InitialCondition> _initialConditions = new PathAndValueEntityCache<InitialCondition>();

      //Contains a temp  cache of builder and their corresponding building blocks
      private readonly Cache<string, BuilderSource> _builderSources = new Cache<string, BuilderSource>(x => x.Builder.Id, x => null);

      //Cache of entity source by id and not by path. It is required because the path is not available at time of construction in the entity
      private readonly Cache<string, SimulationEntitySource> _entitySources = new Cache<string, SimulationEntitySource>(onMissingKey: x => null);

      public SimulationBuilder(ICloneManagerForModel cloneManager, IContainerMergeTask containerMergeTask)
      {
         _cloneManager = cloneManager;
         _containerMergeTask = containerMergeTask;
      }

      protected internal void PerformMerge(SimulationConfiguration simulationConfiguration)
      {
         _simulationConfiguration = simulationConfiguration;
         performMerge();
      }

      public virtual bool CreateAllProcessRateParameters => _simulationConfiguration.CreateAllProcessRateParameters;

      public IEntity BuilderFor(IEntity modelObject) => SimulationEntitySourceFor(modelObject)?.Source;

      internal SimulationEntitySource SimulationEntitySourceFor(IEntity entity) => _entitySources[entity.Id];

      internal void AddSimulationEntitySource(string entityId, SimulationEntitySource simulationEntitySource)
      {
         _entitySources[entityId] = simulationEntitySource;
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
         mergeBuilders(x => x.Reactions, _reactions, mergeReactions);
         mergeBuilders(x => x.Molecules, _molecules, mergeMolecules);
         mergeBuilders(x => x.PassiveTransports, _passiveTransports, mergeTransports);
         mergeBuilders(x => x.Observers, _observers, mergeObservers);
         mergeParameterValueBuilders(x => x.SelectedParameterValues, _parameterValues);
         mergeInitialConditions();
         cacheEntities();
      }

      private void mergeBuilders<T>(Func<Module, IBuildingBlock<T>> propAccess, CacheByName<T> cache, Action<T, BuilderSource<T>> mergeStrategyAction) where T : class, IBuilder, IEntity
      {
         var analyzedMerges = analyzeBuilderMerges(propAccess);

         foreach (var mergeInfo in analyzedMerges)
         {
            var (finalBuilder, buildingBlock) = mergeInfo.BaseBuilder;

            if (mergeInfo.RequiresClone)
            {
               finalBuilder = cloneBuilder(finalBuilder);
               foreach (var (sourceBuilder, sourceBuildingBlock) in mergeInfo.BuildersToMerge)
               {
                  //Clone the source builder if needed to prevent cross-contamination during sequential merges
                  //When merging multiple builders, earlier merged builders could be affected by later merges through shared references
                  var sourceBuilderToMerge = mergeInfo.RequiresMergeClone ? cloneBuilder(sourceBuilder) : sourceBuilder;
                  var mergedBuilderSource = new BuilderSource<T>(sourceBuilderToMerge, sourceBuildingBlock);
                  tryMergeContainers(finalBuilder, mergedBuilderSource);
                  mergeStrategyAction(finalBuilder, mergedBuilderSource);
               }
            }

            cache.Add(finalBuilder);
            AddToBuilderSource(finalBuilder, buildingBlock);
         }
      }

      private T cloneBuilder<T>(T builder) where T : class, IBuilder
      {
         //we will need to make sure we keep the Id the same somehow to ensure proper tracking of entities. TBD
         return _cloneManager.Clone(builder);
      }

      private void tryMergeContainers<T>(T target, BuilderSource<T> builderSource) where T : IEntity
      {
         var targetContainer = target as IContainer;
         var sourceContainer = builderSource.Builder as IContainer;
         if (targetContainer == null || sourceContainer == null)
            return;

         mergeContainers(targetContainer, new BuilderSource<IContainer>(sourceContainer, builderSource.BuildingBlock));
      }

      private void mergeContainers<T>(T target, BuilderSource<T> source) where T : IContainer
      {
         var (sourceBuilder, sourceBuildingBlock) = source;
         //Marking all entities in the source as coming from this source
         var allEntities = sourceBuilder.GetAllChildren<IEntity>();
         allEntities.Each(entity => AddToBuilderSource(entity, sourceBuildingBlock));

         //at this step, all entities in the target should already be marked as coming from their respective source
         _containerMergeTask.MergeContainers(target, sourceBuilder);
      }

      private void mergeReactions(ReactionBuilder target, BuilderSource<ReactionBuilder> source)
      {
         //TODO add reaction merge behavior
      }

      private void mergeTransports(TransportBuilder target, BuilderSource<TransportBuilder> builderSource)
      {
         var source = builderSource.Builder;
         mergeMoleculeLists(target, source);
         mergeDescriptorCriteria(target.SourceCriteria, source.SourceCriteria);
         mergeDescriptorCriteria(target.TargetCriteria, source.TargetCriteria);
         target.CreateProcessRateParameter = source.CreateProcessRateParameter;
         target.ProcessRateParameterPersistable = source.ProcessRateParameterPersistable;
         //TODO: do we need to clone?
         target.Formula = _cloneManager.Clone(source.Formula);
      }

      private void mergeDescriptorCriteria(DescriptorCriteria target, DescriptorCriteria source)
      {
         target.Operator = source.Operator;
         source.Each(t => target.Add(t.CloneCondition()));
      }

      private void mergeMoleculeLists(IMoleculeDependentBuilder target, IMoleculeDependentBuilder sourceToMerge)
      {
         var sourceMoleculeList = sourceToMerge.MoleculeList;
         var targetMoleculeList = target.MoleculeList;
         //copy property forAll from merged list
         targetMoleculeList.ForAll = sourceMoleculeList.ForAll;
         sourceMoleculeList.MoleculeNames.Each(m =>
         {
            targetMoleculeList.RemoveMoleculeNameToExclude(m);
            targetMoleculeList.AddMoleculeName(m);
         });

         sourceMoleculeList.MoleculeNamesToExclude.Each(m =>
         {
            targetMoleculeList.RemoveMoleculeName(m);
            targetMoleculeList.AddMoleculeNameToExclude(m);
         });
      }

      private void mergeObservers(ObserverBuilder target, BuilderSource<ObserverBuilder> source)
      {
         mergeMoleculeLists(target, source.Builder);
      }

      private void mergeMolecules(MoleculeBuilder target, BuilderSource<MoleculeBuilder> source)
      {
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

      private void mergeInitialConditions()
      {
         var expressionProfileInitialConditionsCache = new PathAndValueEntityCache<InitialCondition>();
         var initialConditionsFromConfigurationsCache = new PathAndValueEntityCache<InitialCondition>();

         mergeParameterValueBuilders(x => x.SelectedInitialConditions, initialConditionsFromConfigurationsCache);
         var expressionProfileInitialConditions = allInitialConditionsFromExpressionProfileSources();
         expressionProfileInitialConditionsCache.AddRange(expressionProfileInitialConditions.Select(x => x.InitialCondition));
         addToBuilderSource(expressionProfileInitialConditions);

         // Concat order is important so that the values from expression profiles are overwritten if duplicated
         _initialConditions.AddRange(expressionProfileInitialConditionsCache.Concat(initialConditionsFromConfigurationsCache));
      }

      private void mergeParameterValueBuilders<T>(Func<ModuleConfiguration, IBuildingBlock<T>> propAccess, PathAndValueEntityCache<T> cache) where T : PathAndValueEntity
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

      internal IReadOnlyList<(SpatialStructure spatialStructure, MergeBehavior mergeBehavior)> SpatialStructureAndMergeBehaviors =>
         buildingBlockAndMergeBehaviors(x => x.SpatialStructure);

      internal IReadOnlyList<(EventGroupBuildingBlock eventGroupBuildingBlock, MergeBehavior mergeBehavior)> EventGroupAndMergeBehaviors =>
         buildingBlockAndMergeBehaviors(x => x.EventGroups);

      internal IReadOnlyCollection<TransportBuilder> PassiveTransports => _passiveTransports;
      internal IReadOnlyCollection<ReactionBuilder> Reactions => _reactions;
      internal IReadOnlyCollection<ObserverBuilder> Observers => _observers;
      internal IReadOnlyCollection<MoleculeBuilder> Molecules => _molecules;
      internal IReadOnlyCollection<ParameterValue> ParameterValues => _parameterValues;
      internal IReadOnlyCollection<InitialCondition> InitialConditions => _initialConditions;

      private IReadOnlyList<(TBuildingBlock buildingBlock, MergeBehavior mergeBehavior)> buildingBlockAndMergeBehaviors<TBuildingBlock>(Func<Module, TBuildingBlock> propAccess) where TBuildingBlock : class, IBuildingBlock =>
         _simulationConfiguration.ModuleConfigurations
            .Where(x => propAccess(x.Module) != null)
            .Select(x => (propAccess(x.Module), x.MergeBehavior))
            .ToList();

      public virtual IReadOnlyCollection<SimulationEntitySource> EntitySources => _entitySources;

      internal MoleculeList MoleculeListFor(IMoleculeDependentBuilder builder) => builder.MoleculeList;

      internal MoleculeBuilder MoleculeByName(string name) => _molecules[name];

      internal class BuilderSource : BuilderSource<IEntity>
      {
         public BuilderSource(IEntity builder, IBuildingBlock buildingBlock) : base(builder, buildingBlock)
         {
         }
      }

      internal BuilderSource BuilderSourceFor(IEntity sourceEntity) => _builderSources[sourceEntity.Id];

      internal class BuilderSource<T> where T : IEntity
      {
         public T Builder { get; }
         public IBuildingBlock BuildingBlock { get; }

         public BuilderSource(T builder, IBuildingBlock buildingBlock)
         {
            Builder = builder;
            BuildingBlock = buildingBlock;
         }

         public void Deconstruct(out T builder, out IBuildingBlock buildingBlock)
         {
            builder = Builder;
            buildingBlock = BuildingBlock;
         }
      }

      internal class BuilderMergeInfo<T> where T : IBuilder
      {
         public BuilderSource<T> BaseBuilder { get; }
         public IReadOnlyList<BuilderSource<T>> BuildersToMerge { get; }

         /// <summary>
         ///    Indicates that the base builder needs to be cloned before merging
         /// </summary>
         public bool RequiresClone => BuildersToMerge.Count > 0;

         /// <summary>
         ///    It is required to also clone each builder being merged if we have 2 or more builders to merge.
         ///    This prevents cross-contamination between builders during sequential merges.
         ///    Example: When merging B1, then B2 into finalBuilder:
         ///    - Merge B1: B1's children/references are transferred to finalBuilder
         ///    - Merge B2: B2's merge operations might modify children that came from B1
         ///    - If B1 wasn't cloned, these modifications propagate back to the original building block through shared references
         ///    With only 1 builder to merge, there are no subsequent merges to cause side effects, so cloning is not needed.
         /// </summary>
         public bool RequiresMergeClone => BuildersToMerge.Count >= 2;

         public BuilderMergeInfo(BuilderSource<T> baseBuilder) : this(baseBuilder, Enumerable.Empty<BuilderSource<T>>())
         {
         }

         public BuilderMergeInfo(BuilderSource<T> baseBuilder, IEnumerable<BuilderSource<T>> buildersToMerge)
         {
            BaseBuilder = baseBuilder;
            BuildersToMerge = buildersToMerge.ToList();
         }
      }

      private IReadOnlyList<BuilderMergeInfo<T>> analyzeBuilderMerges<T>(Func<Module, IBuildingBlock<T>> propAccess) where T : IBuilder
      {
         var buildingBlocksAndMergeBehaviors = buildingBlockAndMergeBehaviors(propAccess);

         if (buildingBlocksAndMergeBehaviors.Count == 0)
            return new List<BuilderMergeInfo<T>>();

         var allBuildersWithBehaviors = buildingBlocksAndMergeBehaviors
            .SelectMany(x => x.buildingBlock.Select(builder => (builder, x.buildingBlock, x.mergeBehavior)))
            .ToList();

         var results = new List<BuilderMergeInfo<T>>();

         foreach (var group in allBuildersWithBehaviors.GroupBy(x => x.builder.Name))
         {
            var builders = group.ToList();

            //only one, we use it as is
            if (builders.Count == 1)
            {
               var builderSource = new BuilderSource<T>(builders[0].builder, builders[0].buildingBlock);
               results.Add(new BuilderMergeInfo<T>(builderSource));
               continue;
            }

            //last one is overwrite. We will use it as is also
            var (lastBuilder, lastBuildingBlock, mergeBehavior) = builders[builders.Count - 1];
            if (mergeBehavior == MergeBehavior.Overwrite)
            {
               var builderSource = new BuilderSource<T>(lastBuilder, lastBuildingBlock);
               results.Add(new BuilderMergeInfo<T>(builderSource));
               continue;
            }

            //We find the last one that has an overwrite before the extend sequence. This will be the base. We will clone it and merge everything on top of it
            //If no overwrite is found, baseIndex stays at 0 and we use the first builder as the base
            int baseIndex = 0;
            for (int i = builders.Count - 2; i >= 0; i--)
            {
               if (builders[i].mergeBehavior == MergeBehavior.Overwrite)
               {
                  baseIndex = i;
                  break;
               }
            }

            var baseBuilderSource = new BuilderSource<T>(builders[baseIndex].builder, builders[baseIndex].buildingBlock);
            var buildersToMerge = builders.Skip(baseIndex + 1).Select(x => new BuilderSource<T>(x.builder, x.buildingBlock));
            results.Add(new BuilderMergeInfo<T>(baseBuilderSource, buildersToMerge));
         }

         return results;
      }

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