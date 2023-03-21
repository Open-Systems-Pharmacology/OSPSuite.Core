using System;
using System.Collections.Generic;
using System.Linq;
using OSPSuite.Assets;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Descriptors;
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
      private readonly IContainerBuilderToContainerMapper _containerMapper;
      private readonly IMoleculePropertiesContainerTask _moleculePropertiesContainerTask;
      private readonly IMoleculeBuilderToMoleculeAmountMapper _moleculeMapper;
      private readonly IObjectBaseFactory _objectBaseFactory;
      private readonly IObserverBuilderTask _observerBuilderTask;
      private readonly IReactionCreator _reactionCreator;
      private readonly INeighborhoodCollectionToContainerMapper _neighborhoodsMapper;
      private readonly IReferencesResolver _referencesResolver;
      private readonly IEventBuilderTask _eventBuilderTask;
      private readonly IKeywordReplacerTask _keywordReplacerTask;
      private readonly ITransportCreator _transportCreator;
      private readonly IProgressManager _progressManager;
      private readonly IFormulaTask _formulaTask;
      private readonly ICalculationMethodTask _calculationMethodTask;
      private readonly ISimulationConfigurationValidator _simulationConfigurationValidator;
      private readonly IParameterBuilderToParameterMapper _parameterMapper;
      private readonly IQuantityValuesUpdater _quantityValuesUpdater;
      private readonly IModelValidatorFactory _modelValidatorFactory;
      private readonly ICircularReferenceChecker _circularReferenceChecker;

      public ModelConstructor(
         IObjectBaseFactory objectBaseFactory,
         IObserverBuilderTask observerBuilderTask,
         IReactionCreator reactionCreator,
         IMoleculePropertiesContainerTask moleculePropertiesContainerTask,
         IContainerBuilderToContainerMapper containerMapper,
         INeighborhoodCollectionToContainerMapper neighborhoodsMapper,
         IMoleculeBuilderToMoleculeAmountMapper moleculeMapper,
         IReferencesResolver referencesResolver,
         IEventBuilderTask eventBuilderTask,
         IKeywordReplacerTask keywordReplacerTask,
         ITransportCreator transportCreator,
         IProgressManager progressManager,
         IFormulaTask formulaTask,
         ICalculationMethodTask calculationMethodTask,
         ISimulationConfigurationValidator simulationConfigurationValidator,
         IParameterBuilderToParameterMapper parameterMapper,
         IQuantityValuesUpdater quantityValuesUpdater,
         IModelValidatorFactory modelValidatorFactory,
         ICircularReferenceChecker circularReferenceChecker)
      {
         _objectBaseFactory = objectBaseFactory;
         _simulationConfigurationValidator = simulationConfigurationValidator;
         _parameterMapper = parameterMapper;
         _quantityValuesUpdater = quantityValuesUpdater;
         _modelValidatorFactory = modelValidatorFactory;
         _circularReferenceChecker = circularReferenceChecker;
         _observerBuilderTask = observerBuilderTask;
         _reactionCreator = reactionCreator;
         _moleculePropertiesContainerTask = moleculePropertiesContainerTask;
         _containerMapper = containerMapper;
         _neighborhoodsMapper = neighborhoodsMapper;
         _moleculeMapper = moleculeMapper;
         _referencesResolver = referencesResolver;
         _eventBuilderTask = eventBuilderTask;
         _keywordReplacerTask = keywordReplacerTask;
         _transportCreator = transportCreator;
         _progressManager = progressManager;
         _formulaTask = formulaTask;
         _calculationMethodTask = calculationMethodTask;
      }

      public CreationResult CreateModelFrom(SimulationConfiguration simulationConfiguration, string modelName)
      {
         try
         {
            var model = _objectBaseFactory.Create<IModel>().WithName(modelName);

            var creationResult = buildProcess(model, simulationConfiguration,
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
            _keywordReplacerTask.ReplaceIn(model.Root);

            //This needs to be done before we validate the model to ensure that all references can be found
            _formulaTask.ExpandNeighborhoodReferencesIn(model);

            creationResult.Add(validateModel(model, simulationConfiguration));

            if (creationResult.State == ValidationState.Invalid)
               return creationResult;

            finalizeModel(model);

            return creationResult;
         }
         finally
         {
            simulationConfiguration.ClearCache();
         }
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

      private ValidationResult validateModel(IModel model, SimulationConfiguration simulationConfiguration)
      {
         if (!simulationConfiguration.ShouldValidate)
            return new ValidationResult();

         var modelValidation = validate<ValidatorForQuantities>(model, simulationConfiguration);
         if (!simulationConfiguration.PerformCircularReferenceCheck)
            return new ValidationResult(modelValidation.Messages);

         var circularReferenceValidation = checkCircularReferences(model, simulationConfiguration);
         return new ValidationResult(modelValidation.Messages.Union(circularReferenceValidation.Messages));
      }

      private CreationResult buildProcess(IModel model, SimulationConfiguration simulationConfiguration,
         params Func<IModel, SimulationConfiguration, ValidationResult>[] steps)
      {
         var result = new CreationResult(model);
         IProgressUpdater progress = null;

         try
         {
            if (simulationConfiguration.ShowProgress)
            {
               progress = _progressManager.Create();
               progress.Initialize(steps.Length, Messages.CreatingModel);
            }

            foreach (var step in steps)
            {
               //call each build process with the model and the simulationConfiguration 
               result.Add(step(model, simulationConfiguration));

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

      private ValidationResult checkSimulationConfiguration(IModel model, SimulationConfiguration simulationConfiguration)
      {
         if (!simulationConfiguration.ShouldValidate)
            return new ValidationResult();

         return _simulationConfigurationValidator.Validate(simulationConfiguration);
      }

      private ValidationResult createModelStructure(IModel model, SimulationConfiguration simulationConfiguration)
      {
         // copy spatial structure with neighborhoods 
         copySpatialStructure(model, simulationConfiguration);

         // add molecules with IsPresent=true
         var moleculeMessages = createMoleculeAmounts(model.Root, simulationConfiguration);

         // create local molecule properties container in the spatial structure
         addLocalParametersToMolecule(model, simulationConfiguration);

         // create global molecule properties container in the spatial structure
         createGlobalMoleculeContainers(model, simulationConfiguration);

         // create calculation methods dependent formula and parameters
         createMoleculeCalculationMethodsFormula(model, simulationConfiguration);

         // replace all keywords define in the model structure
         _keywordReplacerTask.ReplaceIn(model.Root);

         return new ValidationResult(moleculeMessages);
      }

      private ValidationResult validateModelName(IModel model, SimulationConfiguration simulationConfiguration)
      {
         return validate<ModelNameValidator>(model, simulationConfiguration);
      }

      private void createMoleculeCalculationMethodsFormula(IModel model, SimulationConfiguration simulationConfiguration)
      {
         _calculationMethodTask.MergeCalculationMethodInModel(model, simulationConfiguration);
      }

      private ValidationResult createProcesses(IModel model, SimulationConfiguration simulationConfiguration)
      {
         _transportCreator.CreateActiveTransport(model, simulationConfiguration);
         var reactionMessages = createReactions(model, simulationConfiguration);

         createPassiveTransports(model, simulationConfiguration);

         var validationResult = validate<ValidatorForReactionsAndTransports>(model, simulationConfiguration);
         return new ValidationResult(reactionMessages.Union(validationResult.Messages));
      }

      private ValidationResult createObserversAndEvents(IModel model, SimulationConfiguration simulationConfiguration)
      {
         _eventBuilderTask.CreateEvents(model, simulationConfiguration);

         // Observers needs to be created last as they might reference parameters defined in the event builder
         _observerBuilderTask.CreateObservers(model, simulationConfiguration);

         return validate<ValidatorForObserversAndEvents>(model, simulationConfiguration);
      }

      private ValidationResult validate<TValidationVisitor>(IModel model, SimulationConfiguration simulationConfiguration)
         where TValidationVisitor : IModelValidator
      {
         if (!simulationConfiguration.ShouldValidate)
            return new ValidationResult();

         return _modelValidatorFactory.Create<TValidationVisitor>().Validate(model, simulationConfiguration);
      }

      private void copySpatialStructure(IModel model, SimulationConfiguration simulationConfiguration)
      {
         // Create Root Container for the model with the name of the model
         model.Root = _objectBaseFactory.Create<IContainer>()
            .WithName(model.Name)
            .WithMode(ContainerMode.Logical)
            .WithContainerType(ContainerType.Simulation);

         model.Root.AddTag(new Tag(Constants.ROOT_CONTAINER_TAG));

         //Add each container defined in the spatial structure and direct child of the root container
         foreach (var topContainer in simulationConfiguration.SpatialStructure.TopContainers)
         {
            model.Root.Add(_containerMapper.MapFrom(topContainer, simulationConfiguration));
         }

         // Add the neighborhoods
         model.Neighborhoods = _neighborhoodsMapper.MapFrom(model, simulationConfiguration);
      }

      private ValidationResult checkCircularReferences(IModel model, SimulationConfiguration simulationConfiguration)
      {
         if (!simulationConfiguration.ShouldValidate)
            return new ValidationResult();

         return _circularReferenceChecker.CheckCircularReferencesIn(model, simulationConfiguration);
      }

      private ValidationResult setQuantitiesValues(IModel model, SimulationConfiguration simulationConfiguration)
      {
         _quantityValuesUpdater.UpdateQuantitiesValues(model, simulationConfiguration);
         return new ValidationResult();
      }

      private void createPassiveTransports(IModel model, SimulationConfiguration simulationConfiguration)
      {
         simulationConfiguration.PassiveTransports?.Each(t => _transportCreator.CreatePassiveTransport(model, t, simulationConfiguration));
      }

      private IEnumerable<ValidationMessage> createReactions(IModel model, SimulationConfiguration simulationConfiguration)
      {
         var messages = simulationConfiguration.Reactions?
            .Where(r => !_reactionCreator.CreateReaction(r, model, simulationConfiguration))
            .Select(
               r => new ValidationMessage(NotificationType.Warning, Validation.WarningNoReactionCreated(r.Name), r, simulationConfiguration.Reactions))
            .ToList();

         model.Root.GetAllContainersAndSelf<IContainer>(x => x.ContainerType == ContainerType.Reaction)
            .Each(x => _keywordReplacerTask.ReplaceInReactionContainer(x, model.Root));

         return messages;
      }

      private void createGlobalMoleculeContainers(IModel model, SimulationConfiguration simulationConfiguration)
      {
         simulationConfiguration.AllPresentMolecules()
            .Each(m => _moleculePropertiesContainerTask.CreateGlobalMoleculeContainerFor(model.Root, m, simulationConfiguration));
      }

      private void addLocalParametersToMolecule(IModel model, SimulationConfiguration simulationConfiguration)
      {
         // retrieve all molecules container defined int the spatial structure
         var allMoleculePropertiesContainer = model.Root.GetAllChildren<IContainer>(x => x.IsNamed(Constants.MOLECULE_PROPERTIES)).ToList();

         var allPresentMolecules = simulationConfiguration.AllPresentXenobioticFloatingMoleculeNames();
         var allEndogenous = simulationConfiguration.AllPresentEndogenousStationaryMoleculeNames();

         foreach (var moleculePropertiesContainer in allMoleculePropertiesContainer)
         {
            addLocalStructureMoleculeParametersToMoleculeAmount(allPresentMolecules, moleculePropertiesContainer, simulationConfiguration, model,
               x => !isEndogenousParameter(x));
            addLocalStructureMoleculeParametersToMoleculeAmount(allEndogenous, moleculePropertiesContainer, simulationConfiguration, model,
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
         IContainer moleculePropertiesContainerTemplate,
         SimulationConfiguration simulationConfiguration, IModel model, Func<IParameter, bool> query)
      {
         foreach (var moleculeName in moleculeNames)
         {
            // check if molecule amount already exists
            var moleculeAmount = moleculePropertiesContainerTemplate.ParentContainer.GetSingleChildByName<IMoleculeAmount>(moleculeName);
            if (moleculeAmount == null)
               continue;


            moleculePropertiesContainerTemplate.GetChildren<IParameter>().Where(query)
               .Each(parameter => moleculeAmount.Add(_parameterMapper.MapFrom(parameter, simulationConfiguration)));

            _keywordReplacerTask.ReplaceIn(moleculeAmount, model.Root, moleculeName);
         }
      }

      private IEnumerable<ValidationMessage> createMoleculeAmounts(IContainer root, SimulationConfiguration simulationConfiguration)
      {
         var molecules = simulationConfiguration.Molecules;
         var presentMolecules = allPresentMoleculesInContainers(root, simulationConfiguration).ToList();

         var moleculesWithPhysicalContainers = presentMolecules.Where(containerIsPhysical);
         moleculesWithPhysicalContainers.Each(x => addMoleculeToContainer(simulationConfiguration, x.Container, molecules[x.MoleculeStartValue.MoleculeName]));

         return new MoleculeBuildingBlockValidator().Validate(molecules).Messages.Concat(createValidationMessagesForPresentMolecules(presentMolecules));
      }

      private static bool containerIsPhysical(StartValueAndContainer startValueAndContainer)
      {
         return startValueAndContainer.Container != null && startValueAndContainer.Container.Mode == ContainerMode.Physical;
      }

      private void addMoleculeToContainer(SimulationConfiguration simulationConfiguration, IContainer container, IMoleculeBuilder moleculeBuilder)
      {
         container.Add(_moleculeMapper.MapFrom(moleculeBuilder, container, simulationConfiguration));
      }

      private IEnumerable<ValidationMessage> createValidationMessagesForPresentMolecules(List<StartValueAndContainer> presentMolecules)
      {
         var moleculesWithoutPhysicalContainers = presentMolecules.Where(x => !containerIsPhysical(x));
         return moleculesWithoutPhysicalContainers.Select(x =>
            x.Container == null
               ? createValidationMessageForNullContainer(x)
               : createValidationMessageForMoleculesWithNonPhysicalContainer(x));
      }

      private ValidationMessage createValidationMessageForMoleculesWithNonPhysicalContainer(StartValueAndContainer startValueAndContainer)
      {
         var moleculeStartValue = startValueAndContainer.MoleculeStartValue;
         var message = Validation.StartValueDefinedForNonPhysicalContainer(moleculeStartValue.MoleculeName, moleculeStartValue.ContainerPath.PathAsString);
         return buildValidationMessage(moleculeStartValue, message);
      }

      private static ValidationMessage buildValidationMessage(MoleculeStartValue moleculeStartValue,
         string validationDescription)
      {
         return new ValidationMessage(NotificationType.Warning, validationDescription, moleculeStartValue, moleculeStartValue.BuildingBlock);
      }

      private ValidationMessage createValidationMessageForNullContainer(StartValueAndContainer startValueAndContainer)
      {
         var moleculeStartValue = startValueAndContainer.MoleculeStartValue;
         var message = Validation.StartValueDefinedForContainerThatCannotBeResolved(moleculeStartValue.MoleculeName, moleculeStartValue.ContainerPath.PathAsString);
         return buildValidationMessage(moleculeStartValue, message);
      }

      private static IEnumerable<StartValueAndContainer> allPresentMoleculesInContainers(IContainer root, SimulationConfiguration simulationConfiguration)
      {
         return from moleculeStartValue in simulationConfiguration.AllPresentMoleculeValues()
            let container = moleculeStartValue.ContainerPath.Resolve<IContainer>(root)
            select new StartValueAndContainer
            {
               MoleculeStartValue = moleculeStartValue,
               Container = container
            };
      }

      private class StartValueAndContainer
      {
         public MoleculeStartValue MoleculeStartValue { get; set; }
         public IContainer Container { get; set; }
      }
   }
}