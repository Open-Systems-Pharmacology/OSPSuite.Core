using System;
using System.Collections.Generic;
using System.Linq;
using OSPSuite.Assets;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Mappers;
using OSPSuite.Core.Services;
using OSPSuite.Utility.Events;
using OSPSuite.Utility.Extensions;

namespace OSPSuite.Core.Domain.Services
{
   public interface IModelConstructor
   {
      CreationResult CreateModelFrom(SimulationConfiguration simulationConfiguration, string modelName);
   }

   internal class ModelConstructor : IModelConstructor
   {
      private readonly IMoleculePropertiesContainerTask _moleculePropertiesContainerTask;
      private readonly IMoleculeBuilderToMoleculeAmountMapper _moleculeMapper;
      private readonly IObjectBaseFactory _objectBaseFactory;
      private readonly IObserverBuilderTask _observerBuilderTask;
      private readonly IReactionCreator _reactionCreator;
      private readonly IReferencesResolver _referencesResolver;
      private readonly IKeywordReplacerTask _keywordReplacerTask;
      private readonly ITransportCreator _transportCreator;
      private readonly IProgressManager _progressManager;
      private readonly IFormulaTask _formulaTask;
      private readonly ICalculationMethodTask _calculationMethodTask;
      private readonly ISimulationConfigurationValidator _simulationConfigurationValidator;
      private readonly IParameterBuilderToParameterMapper _parameterMapper;
      private readonly IQuantityValuesUpdater _quantityValuesUpdater;
      private readonly IModelValidatorFactory _modelValidatorFactory;
      private readonly IModelCircularReferenceChecker _circularReferenceChecker;
      private readonly ISpatialStructureMerger _spatialStructureMerger;
      private readonly IEventBuilderTask _eventBuilderTask;

      public ModelConstructor(
         IObjectBaseFactory objectBaseFactory,
         IObserverBuilderTask observerBuilderTask,
         IReactionCreator reactionCreator,
         IMoleculePropertiesContainerTask moleculePropertiesContainerTask,
         IMoleculeBuilderToMoleculeAmountMapper moleculeMapper,
         IReferencesResolver referencesResolver,
         IKeywordReplacerTask keywordReplacerTask,
         ITransportCreator transportCreator,
         IProgressManager progressManager,
         IFormulaTask formulaTask,
         ICalculationMethodTask calculationMethodTask,
         ISimulationConfigurationValidator simulationConfigurationValidator,
         IParameterBuilderToParameterMapper parameterMapper,
         IQuantityValuesUpdater quantityValuesUpdater,
         IModelValidatorFactory modelValidatorFactory,
         IModelCircularReferenceChecker circularReferenceChecker,
         ISpatialStructureMerger spatialStructureMerger,
         IEventBuilderTask eventBuilderTask)
      {
         _objectBaseFactory = objectBaseFactory;
         _simulationConfigurationValidator = simulationConfigurationValidator;
         _parameterMapper = parameterMapper;
         _quantityValuesUpdater = quantityValuesUpdater;
         _modelValidatorFactory = modelValidatorFactory;
         _circularReferenceChecker = circularReferenceChecker;
         _spatialStructureMerger = spatialStructureMerger;
         _observerBuilderTask = observerBuilderTask;
         _reactionCreator = reactionCreator;
         _moleculePropertiesContainerTask = moleculePropertiesContainerTask;
         _moleculeMapper = moleculeMapper;
         _referencesResolver = referencesResolver;
         _keywordReplacerTask = keywordReplacerTask;
         _transportCreator = transportCreator;
         _progressManager = progressManager;
         _formulaTask = formulaTask;
         _calculationMethodTask = calculationMethodTask;
         _eventBuilderTask = eventBuilderTask;
      }

      public CreationResult CreateModelFrom(SimulationConfiguration simulationConfiguration, string modelName)
      {
         var model = _objectBaseFactory.Create<IModel>().WithName(modelName);
         var simulationBuilder = new SimulationBuilder(simulationConfiguration);
         var modelConfiguration = new ModelConfiguration(model, simulationConfiguration, simulationBuilder);
         var creationResult = buildProcess(modelConfiguration,
            //One function per process step
            checkSimulationConfiguration,
            createModelStructure,
            validateModelName,
            createProcesses,
            createObserversAndEvents,
            setQuantitiesValues);

         if (creationResult.State == ValidationState.Invalid)
            return creationResult;

         //replace all keywords define in the model structure once all build processes have been executed
         _keywordReplacerTask.ReplaceIn(modelConfiguration);

         //This needs to be done before we validate the model to ensure that all references can be found
         _formulaTask.ExpandDynamicReferencesIn(model);

         creationResult.Add(validateModel(modelConfiguration));

         if (creationResult.State == ValidationState.Invalid)
            return creationResult;

         finalizeModel(model);

         return creationResult;
      }

      private void finalizeModel(IModel model)
      {
         _formulaTask.CheckFormulaOriginIn(model);

         _formulaTask.ExpandDynamicFormulaIn(model);

         //now we should be able to resolve all references
         _referencesResolver.ResolveReferencesIn(model);

         //This should be done after reference were resolved to ensure that we do not remove formula parameter that could not be evaluated
         removeUndefinedLocalMoleculeParametersIn(model);

         //now that we have removed potential nan parameters, let's make sure that no formula was actually using them
         _referencesResolver.ResolveReferencesIn(model);
      }

      private void removeUndefinedLocalMoleculeParametersIn(IModel model)
      {
         var allNaNParametersFromMolecules = model.Root.GetAllChildren<IContainer>()
            .Where(c => c.ContainerType == ContainerType.Molecule)
            .SelectMany(c => c.AllParameters(p => double.IsNaN(p.Value)))
            .Where(x => x.BuildMode == ParameterBuildMode.Local)
            .ToList();

         allNaNParametersFromMolecules.Each(x => x.ParentContainer.RemoveChild(x));
      }

      private ValidationResult validateModel(ModelConfiguration modelConfiguration)
      {
         if (!modelConfiguration.ShouldValidate)
            return new ValidationResult();

         //Validation needs to happen at the very end of the process
         var reactionAndTransportValidation = validate<ValidatorForReactionsAndTransports>(modelConfiguration);
         var observersAndEventsValidation = validate<ValidatorForObserversAndEvents>(modelConfiguration);
         var modelValidation = validate<ValidatorForQuantities>(modelConfiguration);

         var validation = new ValidationResult(reactionAndTransportValidation, observersAndEventsValidation, modelValidation);
         if (!modelConfiguration.SimulationConfiguration.PerformCircularReferenceCheck)
            return validation;

         var circularReferenceValidation = checkCircularReferences(modelConfiguration);
         return new ValidationResult(validation, circularReferenceValidation);
      }

      private CreationResult buildProcess(ModelConfiguration modelConfiguration,
         params Func<ModelConfiguration, ValidationResult>[] steps)
      {
         var result = new CreationResult(modelConfiguration.Model, modelConfiguration.SimulationBuilder);
         IProgressUpdater progress = null;

         try
         {
            if (modelConfiguration.SimulationConfiguration.ShowProgress)
            {
               progress = _progressManager.Create();
               progress.Initialize(steps.Length, Messages.CreatingModel);
            }

            foreach (var step in steps)
            {
               //call each build process with the model and the simulationConfiguration 
               result.Add(step(modelConfiguration));

               progress?.IncrementProgress();

               //if the result has become invalid, stop the build process
               if (result.IsInvalid)
                  break;
            }
         }
         finally
         {
            progress?.Dispose();
         }

         return result;
      }

      private ValidationResult checkSimulationConfiguration(ModelConfiguration modelConfiguration)
      {
         if (!modelConfiguration.ShouldValidate)
            return new ValidationResult();

         return _simulationConfigurationValidator.Validate(modelConfiguration.SimulationConfiguration);
      }

      private ValidationResult createModelStructure(ModelConfiguration modelConfiguration)
      {
         // copy spatial structure with neighborhoods 
         var spatialStructureValidation = copySpatialStructure(modelConfiguration);

         // add molecules with IsPresent=true
         var moleculeAmountValidation = createMoleculeAmounts(modelConfiguration);

         // create local molecule properties container in the spatial structure
         addLocalParametersToMolecule(modelConfiguration);

         // create global molecule properties container in the spatial structure
         createGlobalMoleculeContainers(modelConfiguration);

         // create calculation methods dependent formula and parameters
         createMoleculeCalculationMethodsFormula(modelConfiguration);

         var validation = new ValidationResult(spatialStructureValidation, moleculeAmountValidation);

         if (validation.ValidationState != ValidationState.Invalid)
         {
            // replace all keywords define in the model structure
            _keywordReplacerTask.ReplaceIn(modelConfiguration);
         }

         return validation;
      }

      private ValidationResult validateModelName(ModelConfiguration modelConfiguration)
      {
         return validate<ModelNameValidator>(modelConfiguration);
      }

      private void createMoleculeCalculationMethodsFormula(ModelConfiguration modelConfiguration)
      {
         _calculationMethodTask.MergeCalculationMethodInModel(modelConfiguration);
      }

      private ValidationResult createProcesses(ModelConfiguration modelConfiguration)
      {
         _transportCreator.CreateActiveTransport(modelConfiguration);
         var reactionMessages = createReactions(modelConfiguration);

         createPassiveTransports(modelConfiguration);

         return new ValidationResult(reactionMessages);
      }

      private ValidationResult createObserversAndEvents(ModelConfiguration modelConfiguration)
      {
         _eventBuilderTask.MergeEventGroupsContainer(modelConfiguration);

         // Observers needs to be created last as they might reference parameters defined in the event builder
         _observerBuilderTask.CreateObservers(modelConfiguration);

         return new ValidationResult();
      }

      private ValidationResult validate<TValidationVisitor>(ModelConfiguration modelConfiguration)
         where TValidationVisitor : IModelValidator
      {
         if (!modelConfiguration.ShouldValidate)
            return new ValidationResult();

         return _modelValidatorFactory.Create<TValidationVisitor>().Validate(modelConfiguration);
      }

      private ValidationResult copySpatialStructure(ModelConfiguration modelConfiguration)
      {
         var model = modelConfiguration.Model;
         model.Root = _spatialStructureMerger.MergeContainerStructure(modelConfiguration);
         //we update the replacement context first when the root container is created so that we can replace keywords from the root container
         modelConfiguration.UpdateReplacementContext();

         model.Neighborhoods = _spatialStructureMerger.MergeNeighborhoods(modelConfiguration);

         return validate<SpatialStructureValidator>(modelConfiguration);
      }

      private ValidationResult checkCircularReferences(ModelConfiguration modelConfiguration)
      {
         if (!modelConfiguration.ShouldValidate)
            return new ValidationResult();

         return _circularReferenceChecker.CheckCircularReferencesIn(modelConfiguration);
      }

      private ValidationResult setQuantitiesValues(ModelConfiguration modelConfiguration)
      {
         return _quantityValuesUpdater.UpdateQuantitiesValues(modelConfiguration);
      }

      private void createPassiveTransports(ModelConfiguration modelConfiguration)
      {
         modelConfiguration.SimulationBuilder.PassiveTransports.Each(t => _transportCreator.CreatePassiveTransport(t, modelConfiguration));
      }

      private IEnumerable<ValidationMessage> createReactions(ModelConfiguration modelConfiguration)
      {
         var (model, simulationConfiguration, replacementContext) = modelConfiguration;
         var messages = simulationConfiguration.Reactions
            .Where(x => !_reactionCreator.CreateReaction(x, modelConfiguration))
            .Select(
               x => new ValidationMessage(NotificationType.Warning, Validation.WarningNoReactionCreated(x.Name), x, x.BuildingBlock))
            .ToList();

         model.Root.GetAllContainersAndSelf<IContainer>(x => x.ContainerType == ContainerType.Reaction)
            .Each(x => _keywordReplacerTask.ReplaceInReactionContainer(x, replacementContext));

         return messages;
      }

      private void createGlobalMoleculeContainers(ModelConfiguration modelConfiguration)
      {
         var (model, simulationConfiguration) = modelConfiguration;

         void createGlobalContainer(MoleculeBuilder moleculeBuilder) =>
            _moleculePropertiesContainerTask.CreateGlobalMoleculeContainerFor(moleculeBuilder, modelConfiguration);

         simulationConfiguration.AllPresentMolecules().Each(createGlobalContainer);

         //once the global properties have been created, we can remove the global container only used as template
         var globalMoleculeContainer = model.Root.Container(Constants.MOLECULE_PROPERTIES);
         if (globalMoleculeContainer != null)
            model.Root.RemoveChild(globalMoleculeContainer);
      }

      private void addLocalParametersToMolecule(ModelConfiguration modelConfiguration)
      {
         var (model, simulationConfiguration) = modelConfiguration;
         // retrieve all molecules container defined in the spatial structure
         // We filter our the global molecule container that is created under root. Only interested in LOCAL Containers
         var allMoleculePropertiesContainer = model.Root.GetAllChildren<IContainer>(x => x.ParentContainer != model.Root && x.IsNamed(Constants.MOLECULE_PROPERTIES)).ToList();

         var allPresentMolecules = simulationConfiguration.AllPresentXenobioticFloatingMoleculeNames();
         var allEndogenous = simulationConfiguration.AllPresentEndogenousStationaryMoleculeNames();

         foreach (var moleculePropertiesContainer in allMoleculePropertiesContainer)
         {
            addLocalStructureMoleculeParametersToMoleculeAmount(allPresentMolecules, moleculePropertiesContainer, modelConfiguration,
               x => !isEndogenousParameter(x));

            addLocalStructureMoleculeParametersToMoleculeAmount(allEndogenous, moleculePropertiesContainer, modelConfiguration,
               isEndogenousParameter);

            // remove the molecule properties container only used as template
            moleculePropertiesContainer.ParentContainer.RemoveChild(moleculePropertiesContainer);
         }
      }

      private static bool isEndogenousParameter(IParameter parameter)
      {
         return parameter.NameIsOneOf(Constants.ONTOGENY_FACTOR, Constants.HALF_LIFE, Constants.DEGRADATION_COEFF);
      }

      private void addLocalStructureMoleculeParametersToMoleculeAmount(IEnumerable<string> moleculeNames,
         IContainer moleculePropertiesContainerTemplate, ModelConfiguration modelConfiguration, Func<IParameter, bool> query)
      {
         var (model, simulationConfiguration, replacementContext) = modelConfiguration;
         foreach (var moleculeName in moleculeNames)
         {
            // check if molecule amount already exists
            var moleculeAmount = moleculePropertiesContainerTemplate.ParentContainer.GetSingleChildByName<MoleculeAmount>(moleculeName);
            if (moleculeAmount == null)
               continue;


            moleculePropertiesContainerTemplate.GetChildren<IParameter>().Where(query)
               .Each(parameter => moleculeAmount.Add(_parameterMapper.MapFrom(parameter, simulationConfiguration)));

            _keywordReplacerTask.ReplaceIn(moleculeAmount, moleculeName, replacementContext);
         }
      }

      private ValidationResult createMoleculeAmounts(ModelConfiguration modelConfiguration)
      {
         var (_, simulationBuilder) = modelConfiguration;
         var presentMolecules = allPresentMoleculesInContainers(modelConfiguration).ToList();

         var moleculesWithPhysicalContainers = presentMolecules.Where(containerIsPhysical);
         moleculesWithPhysicalContainers.Each(x => { addMoleculeToContainer(simulationBuilder, x.Container, simulationBuilder.MoleculeByName(x.InitialCondition.MoleculeName)); });

         return new MoleculeBuildingBlockValidator().Validate(simulationBuilder.Molecules)
            .AddMessagesFrom(createValidationMessagesForPresentMolecules(presentMolecules));
      }

      private static bool containerIsPhysical(InitialConditionAndContainer initialConditionAndContainer)
      {
         return initialConditionAndContainer.Container != null && initialConditionAndContainer.Container.Mode == ContainerMode.Physical;
      }

      private void addMoleculeToContainer(SimulationBuilder simulationBuilder, IContainer container, MoleculeBuilder moleculeBuilder)
      {
         container.Add(_moleculeMapper.MapFrom(moleculeBuilder, container, simulationBuilder));
      }

      private ValidationResult createValidationMessagesForPresentMolecules(List<InitialConditionAndContainer> presentMolecules)
      {
         var moleculesWithoutPhysicalContainers = presentMolecules.Where(x => !containerIsPhysical(x));
         var messages = moleculesWithoutPhysicalContainers.Select(x =>
            x.Container == null
               ? createValidationMessageForNullContainer(x)
               : createValidationMessageForMoleculesWithNonPhysicalContainer(x));

         return new ValidationResult(messages);
      }

      private ValidationMessage createValidationMessageForMoleculesWithNonPhysicalContainer(InitialConditionAndContainer initialConditionAndContainer)
      {
         var initialCondition = initialConditionAndContainer.InitialCondition;
         var message = Validation.StartValueDefinedForNonPhysicalContainer(initialCondition.MoleculeName, initialCondition.ContainerPath.PathAsString);
         return buildValidationMessage(initialCondition, message);
      }

      private static ValidationMessage buildValidationMessage(InitialCondition initialCondition, string validationDescription)
      {
         return new ValidationMessage(NotificationType.Warning, validationDescription, initialCondition, initialCondition.BuildingBlock);
      }

      private ValidationMessage createValidationMessageForNullContainer(InitialConditionAndContainer initialConditionAndContainer)
      {
         var initialCondition = initialConditionAndContainer.InitialCondition;
         var message = Validation.StartValueDefinedForContainerThatCannotBeResolved(initialCondition.MoleculeName, initialCondition.ContainerPath.PathAsString);
         return buildValidationMessage(initialCondition, message);
      }

      private static IEnumerable<InitialConditionAndContainer> allPresentMoleculesInContainers(ModelConfiguration modelConfiguration)
      {
         var (model, simulationConfiguration) = modelConfiguration;
         var root = model.Root;
         return from initialCondition in simulationConfiguration.AllPresentMoleculeValues()
            select new InitialConditionAndContainer
            {
               InitialCondition = initialCondition,
               Container = initialCondition.ContainerPath.Resolve<IContainer>(root)
            };
      }

      private class InitialConditionAndContainer
      {
         public InitialCondition InitialCondition { get; set; }
         public IContainer Container { get; set; }
      }
   }
}