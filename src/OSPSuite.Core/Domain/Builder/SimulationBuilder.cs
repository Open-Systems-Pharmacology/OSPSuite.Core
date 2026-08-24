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
      private readonly ObjectBaseCache<EventGroupBuilder> _eventGroups = new ObjectBaseCache<EventGroupBuilder>();

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
         _reactions.AddRange(mergeBuilders(x => x.Reactions, mergeReactions));
         _eventGroups.AddRange(mergeBuilders(x => x.EventGroups, mergeEvents));
         _molecules.AddRange(mergeBuilders(x => x.Molecules, mergeMolecules));
         _passiveTransports.AddRange(mergeBuilders(x => x.PassiveTransports, mergeTransports));
         _observers.AddRange(mergeBuilders(x => x.Observers, mergeObservers));
         mergeParameterValueBuilders(x => x.SelectedParameterValues, _parameterValues);
         mergeInitialConditions();
         cacheEntities();
      }

      private IReadOnlyList<T> mergeBuilders<T>(Func<Module, IBuildingBlock<T>> propAccess, Action<T, BuilderSource<T>> extendStrategyAction) where T : class, IBuilder, IEntity
      {
         var analyzedMerges = analyzeBuilderMerges(propAccess);
         var results = new List<T>();
         foreach (var mergeInfo in analyzedMerges)
         {
            var (baseBuilder, buildingBlock) = mergeInfo.BaseBuilder;

            if (mergeInfo.RequiresBaseClone)
            {
               baseBuilder = cloneBuilder(baseBuilder);
               foreach (var sourceBuilderThatExtend in mergeInfo.BuildersThatExtend)
               {
                  //Clone the source builder if needed to prevent cross-contamination during sequential merges
                  //When merging multiple builders, earlier merged builders could be affected by later merges through shared references
                  var finalBuilderThatExtend = mergeInfo.RequiresExtensionClone ? cloneBuilder(sourceBuilderThatExtend.Builder) : sourceBuilderThatExtend.Builder;
                  var finalSourceBuilderThatExtend = new BuilderSource<T>(finalBuilderThatExtend, sourceBuilderThatExtend.BuildingBlock);
                  tryExtendContainers(baseBuilder, finalSourceBuilderThatExtend);
                  extendStrategyAction(baseBuilder, finalSourceBuilderThatExtend);
               }
            }

            results.Add(baseBuilder);
            AddToBuilderSource(baseBuilder, buildingBlock);
         }

         return results;
      }

      private T cloneBuilder<T>(T builder) where T : class, IBuilder
      {
         //we need to make sure we keep the Id the same to ensure to keep tracking the same
         return _cloneManager.CloneAndKeepId(builder);
      }

      private void tryExtendContainers<T>(T target, BuilderSource<T> builderSource) where T : IEntity
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

      private void mergeEvents(EventGroupBuilder targetBuilder, BuilderSource<EventGroupBuilder> source)
      {
         var sourceBuilder = source.Builder;
         targetBuilder.EventGroupType = sourceBuilder.EventGroupType;

         mergeDescriptorCriteria(targetBuilder.SourceCriteria, sourceBuilder.SourceCriteria);

         mergeEventGroupProperties(targetBuilder, sourceBuilder);
      }

      /// <summary>
      ///    Merges the given event group builders, and all equally named builders defined under them, property wise.
      ///    This is required because values such as <see cref="ApplicationBuilder.MoleculeName" />, the start condition of an
      ///    <see cref="EventBuilder" /> or the kinetic of a <see cref="TransportBuilder" /> are properties and not child
      ///    entities and are therefore not covered by the generic container merge
      /// </summary>
      private void mergeEventGroupProperties(EventGroupBuilder targetBuilder, EventGroupBuilder sourceBuilder)
      {
         if (targetBuilder is ApplicationBuilder targetApplication && sourceBuilder is ApplicationBuilder sourceApplication)
            mergeApplication(targetApplication, sourceApplication);

         mergeSameNamedChildren<EventBuilder>(targetBuilder, sourceBuilder, mergeEvent);
         mergeSameNamedChildren<TransportBuilder>(targetBuilder, sourceBuilder, mergeTransportProperties);
         mergeSameNamedChildren<EventGroupBuilder>(targetBuilder, sourceBuilder, mergeEventGroupProperties);
      }

      private void mergeSameNamedChildren<T>(IContainer targetContainer, IContainer sourceContainer, Action<T, T> mergeAction) where T : class, IEntity
      {
         var targetChildren = targetContainer.GetChildren<T>().ToList();
         foreach (var sourceChild in sourceContainer.GetChildren<T>().ToList())
         {
            //The container merge already placed every source child in the target, so this is only null when the target
            //uses that name for another type of entity. A child that was added as is exists in the target as the same
            //instance and there is nothing left to merge
            var targetChild = targetChildren.FirstOrDefault(x => string.Equals(x.Name, sourceChild.Name));
            if (targetChild == null || ReferenceEquals(targetChild, sourceChild))
               continue;

            mergeAction(targetChild, sourceChild);
         }
      }

      private void mergeEvent(EventBuilder targetEvent, EventBuilder sourceEvent)
      {
         targetEvent.OneTime = sourceEvent.OneTime;
         //no need to clone the formula. it's either use as is or already a clone
         targetEvent.Formula = sourceEvent.Formula;
      }

      private void mergeApplication(ApplicationBuilder targetApplication, ApplicationBuilder sourceApplication)
      {
         //Application molecules only define where the administered molecule is applied. They are meaningless for another
         //molecule, so the ones inherited from the base builder are dropped when the administered molecule changes
         if (!string.Equals(targetApplication.MoleculeName, sourceApplication.MoleculeName))
            removeApplicationMoleculesNotDefinedIn(targetApplication, sourceApplication);

         targetApplication.MoleculeName = sourceApplication.MoleculeName;
      }

      private void removeApplicationMoleculesNotDefinedIn(ApplicationBuilder targetApplication, ApplicationBuilder sourceApplication)
      {
         //the container merge added the application molecules of the source to the target by name
         var sourceMoleculeNames = sourceApplication.Molecules.Select(x => x.Name).ToList();
         targetApplication.Molecules
            .Where(x => !sourceMoleculeNames.Contains(x.Name))
            .ToList()
            .Each(targetApplication.RemoveMolecule);
      }

      private void mergeReactions(ReactionBuilder targetReaction, BuilderSource<ReactionBuilder> sourceReaction)
      {
         var incoming = sourceReaction.Builder;
         tryExtendContainers(targetReaction, sourceReaction);
         targetReaction.Formula = incoming.Formula;
         targetReaction.CreateProcessRateParameter = incoming.CreateProcessRateParameter;
         targetReaction.ProcessRateParameterPersistable = incoming.ProcessRateParameterPersistable;

         upsertPartners(targetReaction, incoming, isEduct: true);
         upsertPartners(targetReaction, incoming, isEduct: false);

         var mods = new HashSet<string>(targetReaction.ModifierNames, StringComparer.OrdinalIgnoreCase);
         foreach (var modifierName in incoming.ModifierNames)
         {
            if (mods.Add(modifierName))
               targetReaction.AddModifier(modifierName);
         }

         targetReaction.Icon = incoming.Icon ?? targetReaction.Icon;
         targetReaction.Description = string.IsNullOrEmpty(incoming.Description) ? targetReaction.Description : incoming.Description;
         targetReaction.Dimension = incoming.Dimension ?? targetReaction.Dimension;

         if (incoming.ContainerCriteria != null)
            targetReaction.ContainerCriteria = incoming.ContainerCriteria;
      }

      private static void upsertPartners(ReactionBuilder target, ReactionBuilder incoming, bool isEduct)
      {
         if (isEduct)
         {
            foreach (var src in incoming.Educts)
            {
               var existing = target.EductBy(src.MoleculeName);
               if (existing != null)
                  target.RemoveEduct(existing);

               target.AddEduct(src.Clone());
            }
         }
         else
         {
            foreach (var src in incoming.Products)
            {
               var existing = target.ProductBy(src.MoleculeName);
               if (existing != null)
                  target.RemoveProduct(existing);

               target.AddProduct(src.Clone());
            }
         }
      }

      private void mergeTransports(TransportBuilder target, BuilderSource<TransportBuilder> builderSource)
      {
         var source = builderSource.Builder;
         mergeMoleculeLists(target, source);
         mergeTransportProperties(target, source);
      }

      private void mergeTransportProperties(TransportBuilder target, TransportBuilder source)
      {
         mergeDescriptorCriteria(target.SourceCriteria, source.SourceCriteria);
         mergeDescriptorCriteria(target.TargetCriteria, source.TargetCriteria);
         target.CreateProcessRateParameter = source.CreateProcessRateParameter;
         target.ProcessRateParameterPersistable = source.ProcessRateParameterPersistable;
         //no need to clone the formula. it's either use as is or already a clone
         target.Formula = source.Formula;
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
         target.Formula = source.Builder.Formula;
      }

      private void mergeMolecules(MoleculeBuilder target, BuilderSource<MoleculeBuilder> source)
      {
         var incoming = source.Builder;
         target.DefaultStartFormula = incoming.DefaultStartFormula;
         target.Dimension = incoming.Dimension;
         target.DisplayUnit = incoming.DisplayUnit;
         target.IsFloating = incoming.IsFloating;
         target.IsXenobiotic = incoming.IsXenobiotic;
         target.QuantityType = incoming.QuantityType;
         target.Icon = incoming.Icon;

         // calculation methods are replaced
         target.ClearUsedCalculationMethods();
         incoming.UsedCalculationMethods.Each(x => target.AddUsedCalculationMethod(x.Clone()));

         mergeSameNamedChildren<TransporterMoleculeContainer>(target, incoming, mergeTransporterMoleculeContainer);
      }

      //TransportName and the properties of the nested active transport realizations are not covered by the generic container merge
      private void mergeTransporterMoleculeContainer(TransporterMoleculeContainer targetTransporter, TransporterMoleculeContainer sourceTransporter)
      {
         targetTransporter.TransportName = sourceTransporter.TransportName;
         mergeSameNamedChildren<TransportBuilder>(targetTransporter, sourceTransporter, mergeTransportProperties);
      }

      private void cacheEntities()
      {
         cacheContainers(_reactions);
         cacheContainers(_molecules);
         cacheContainers(_passiveTransports);
         cacheContainers(_eventGroups);

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

      internal IReadOnlyCollection<TransportBuilder> PassiveTransports => _passiveTransports;
      internal IReadOnlyCollection<ReactionBuilder> Reactions => _reactions;
      internal IReadOnlyCollection<ObserverBuilder> Observers => _observers;
      internal IReadOnlyCollection<MoleculeBuilder> Molecules => _molecules;
      internal IReadOnlyCollection<ParameterValue> ParameterValues => _parameterValues;
      internal IReadOnlyCollection<InitialCondition> InitialConditions => _initialConditions;
      internal IReadOnlyCollection<EventGroupBuilder> EventGroups => _eventGroups;

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

         /// <summary>
         ///    List of builders to EXTEND on top of the base builder
         /// </summary>
         public IReadOnlyList<BuilderSource<T>> BuildersThatExtend { get; }

         /// <summary>
         ///    Indicates that the base builder needs to be cloned before merging
         /// </summary>
         public bool RequiresBaseClone => BuildersThatExtend.Count > 0;

         /// <summary>
         ///    It is required to also clone each builder being merged if we have 2 or more builders to merge.
         ///    This prevents cross-contamination between builders during sequential merges.
         ///    Example: When merging B1, then B2 into finalBuilder:
         ///    - Merge B1: B1's children/references are transferred to finalBuilder
         ///    - Merge B2: B2's merge operations might modify children that came from B1
         ///    - If B1 wasn't cloned, these modifications propagate back to the original building block through shared references
         ///    With only 1 builder to merge, there are no subsequent merges to cause side effects, so cloning is not needed.
         /// </summary>
         public bool RequiresExtensionClone => BuildersThatExtend.Count >= 2;

         public BuilderMergeInfo(BuilderSource<T> baseBuilder) : this(baseBuilder, Enumerable.Empty<BuilderSource<T>>())
         {
         }

         public BuilderMergeInfo(BuilderSource<T> baseBuilder, IEnumerable<BuilderSource<T>> buildersToMerge)
         {
            BaseBuilder = baseBuilder;
            BuildersThatExtend = buildersToMerge.ToList();
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
            .SelectMany(x => x.Select(builder => (builder, (IBuildingBlock)x)))
            .ToList();

      private IReadOnlyList<(InitialCondition InitialCondition, IBuildingBlock BuildingBlock)> allInitialConditionsFromExpressionProfileSources() =>
         _simulationConfiguration.ExpressionProfiles
            .Select(x => (BuildingBlock: x, x.InitialConditions))
            //null because these conditions do not belong in a module
            .SelectMany(x => x.InitialConditions.Select(ic => (ic, (IBuildingBlock)x.BuildingBlock)))
            .ToList();
   }
}