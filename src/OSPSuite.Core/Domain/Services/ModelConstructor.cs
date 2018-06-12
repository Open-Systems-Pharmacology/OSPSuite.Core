using System;
using System.Collections.Generic;
using System.Linq;
using OSPSuite.Assets;
using OSPSuite.Utility.Events;
using OSPSuite.Utility.Extensions;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Descriptors;
using OSPSuite.Core.Domain.Mappers;
using OSPSuite.Core.Services;

namespace OSPSuite.Core.Domain.Services
{
   public interface IModelConstructor
   {
      CreationResult CreateModelFrom(IBuildConfiguration buildConfiguration, string modelName);
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
      private readonly IBuildConfigurationValidator _buildConfigurationValidator;
      private readonly IParameterBuilderToParameterMapper _parameterMapper;
      private readonly IQuantityValuesUpdater _quantityValuesUpdater;
      private readonly IModelValidatorFactory _modelValidatorFactory;
      private readonly ICircularReferenceChecker _circularReferenceChecker;

      public ModelConstructor(IObjectBaseFactory objectBaseFactory, IObserverBuilderTask observerBuilderTask,
         IReactionCreator reactionCreator, IMoleculePropertiesContainerTask moleculePropertiesContainerTask,
         IContainerBuilderToContainerMapper containerMapper, INeighborhoodCollectionToContainerMapper neighborhoodsMapper,
         IMoleculeBuilderToMoleculeAmountMapper moleculeMapper, IReferencesResolver referencesResolver,
         IEventBuilderTask eventBuilderTask, IKeywordReplacerTask keywordReplacerTask,
         ITransportCreator transportCreator, IProgressManager progressManager, IFormulaTask formulaTask, ICalculationMethodTask calculationMethodTask,
         IBuildConfigurationValidator buildConfigurationValidator, IParameterBuilderToParameterMapper parameterMapper,
         IQuantityValuesUpdater quantityValuesUpdater, IModelValidatorFactory modelValidatorFactory, ICircularReferenceChecker circularReferenceChecker)
      {
         _objectBaseFactory = objectBaseFactory;
         _buildConfigurationValidator = buildConfigurationValidator;
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

      public CreationResult CreateModelFrom(IBuildConfiguration buildConfiguration, string modelName)
      {
         try
         {
            var model = _objectBaseFactory.Create<IModel>().WithName(modelName);

            var creationResult = buildProcess(model, buildConfiguration,
               //One function per process step
               checkBuildConfiguration,
               createModelStructure,
               createProcesses,
               createObserversAndEvents,
               setQuantitiesValues);

            if (creationResult.State == ValidationState.Invalid)
               return creationResult;

            //replace all keywords define in the model structure once all build processes have been executed
            _keywordReplacerTask.ReplaceIn(model.Root);

            creationResult.Add(validateModel(model, buildConfiguration));

            if (creationResult.State == ValidationState.Invalid)
               return creationResult;

            finalizeModel(model);

            return creationResult;
         }
         finally
         {
            buildConfiguration.ClearCache();
         }
      }

      private void finalizeModel(IModel model)
      {
         _formulaTask.CheckFormulaOriginIn(model);

         _formulaTask.ExpandDynamicFormulaIn(model);

         //now we should be able to resolve all references
         _referencesResolver.ResolveReferencesIn(model);
      }

      private ValidationResult validateModel(IModel model, IBuildConfiguration buildConfiguration)
      {
         if (!buildConfiguration.ShouldValidate)
            return new ValidationResult();

         var modelValidation = validate<ValidatorForQuantities>(model, buildConfiguration);
         if (!buildConfiguration.PerformCircularReferenceCheck)
            return new ValidationResult(modelValidation.Messages);

         var circularReferenceValidation = checkCircularReferences(model, buildConfiguration);
         return new ValidationResult(modelValidation.Messages.Union(circularReferenceValidation.Messages));
      }

      private CreationResult buildProcess(IModel model, IBuildConfiguration buildConfiguration, params Func<IModel, IBuildConfiguration, ValidationResult>[] steps)
      {
         var result = new CreationResult(model);
         IProgressUpdater progress = null;

         try
         {
            if (buildConfiguration.ShowProgress)
            {
               progress = _progressManager.Create();
               progress.Initialize(steps.Length, Messages.CreatingModel);
            }

            foreach (var step in steps)
            {
               //call each build process with the model and the buildconfiguration 
               result.Add(step(model, buildConfiguration));

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

      private ValidationResult checkBuildConfiguration(IModel model, IBuildConfiguration buildConfiguration)
      {
         if (!buildConfiguration.ShouldValidate)
            return new ValidationResult();

         return _buildConfigurationValidator.Validate(buildConfiguration);
      }

      private ValidationResult createModelStructure(IModel model, IBuildConfiguration buildConfiguration)
      {
         // copy spatial structure with neighborhoods 
         copySpatialStructure(model, buildConfiguration);

         // add molecules with IsPresent=true
         var moleculeMessages = createMoleculeAmounts(model.Root, buildConfiguration);

         // create local molecule properties container in the spatial structure
         addLocalParametersToMolecule(model, buildConfiguration);

         // create global molecule properties container in the spatial structure
         createGlobalMoleculeContainers(model, buildConfiguration);

         // create calculation methods dependent formula and parameters
         createMoleculeCalculationMethodsFormula(model, buildConfiguration);

         // replace all keywords define in the model structure
         _keywordReplacerTask.ReplaceIn(model.Root);


         return new ValidationResult(moleculeMessages);
      }

      private void createMoleculeCalculationMethodsFormula(IModel model, IBuildConfiguration buildConfiguration)
      {
         _calculationMethodTask.MergeCalculationMethodInModel(model, buildConfiguration);
      }

      private ValidationResult createProcesses(IModel model, IBuildConfiguration buildConfiguration)
      {
         _transportCreator.CreateActiveTransport(model, buildConfiguration);
         var reactionMessages = createReactions(model, buildConfiguration);

         createPassiveTransports(model, buildConfiguration);

         var validationResult = validate<ValidatorForReactionsAndTransports>(model, buildConfiguration);
         return new ValidationResult(reactionMessages.Union(validationResult.Messages));
      }

      private ValidationResult createObserversAndEvents(IModel model, IBuildConfiguration buildConfiguration)
      {
         _eventBuilderTask.CreateEvents(buildConfiguration, model);

         // Observers needs to be created last as they might reference parameters defined in the event builder
         _observerBuilderTask.CreateObservers(buildConfiguration, model);

         return validate<ValidatorForObserversAndEvents>(model, buildConfiguration);
      }

      private ValidationResult validate<TValidationVisitor>(IModel model, IBuildConfiguration buildConfiguration) where TValidationVisitor : IModelValidator
      {
         if (!buildConfiguration.ShouldValidate)
            return new ValidationResult();

         return _modelValidatorFactory.Create<TValidationVisitor>().Validate(model, buildConfiguration);
      }

      private void copySpatialStructure(IModel model, IBuildConfiguration buildConfiguration)
      {
         // Create Root Container for the model with the name of the model
         model.Root = _objectBaseFactory.Create<IContainer>()
            .WithName(model.Name)
            .WithMode(ContainerMode.Logical)
            .WithContainerType(ContainerType.Simulation);

         model.Root.AddTag(new Tag(Constants.ROOT_CONTAINER_TAG));

         //Add each container defined in the spatial strucutre and direct child of the root container
         foreach (var topContainer in buildConfiguration.SpatialStructure.TopContainers)
         {
            model.Root.Add(_containerMapper.MapFrom(topContainer, buildConfiguration));
         }

         // Add the neibghborhoods
         model.Neighborhoods = _neighborhoodsMapper.MapFrom(model, buildConfiguration);
      }

      private ValidationResult checkCircularReferences(IModel model, IBuildConfiguration buildConfiguration)
      {
         if (!buildConfiguration.ShouldValidate)
            return new ValidationResult();

         return _circularReferenceChecker.CheckCircularReferencesIn(model, buildConfiguration);
      }

      private ValidationResult setQuantitiesValues(IModel model, IBuildConfiguration buildConfiguration)
      {
         _quantityValuesUpdater.UpdateQuantitiesValues(model, buildConfiguration);
         return new ValidationResult();
      }

      private void createPassiveTransports(IModel model, IBuildConfiguration buildConfiguration)
      {
         buildConfiguration.PassiveTransports.Each(t => _transportCreator.CreatePassiveTransport(model, t, buildConfiguration));
      }

      private IEnumerable<ValidationMessage> createReactions(IModel model, IBuildConfiguration buildConfiguration)
      {
         var messages = buildConfiguration.Reactions
            .Where(r => !_reactionCreator.CreateReaction(r, model, buildConfiguration))
            .Select(r => new ValidationMessage(NotificationType.Warning, Validation.WarningNoReactionCreated(r.Name), r, buildConfiguration.Reactions))
            .ToList();

         model.Root.GetAllContainersAndSelf<IContainer>(x => x.ContainerType == ContainerType.Reaction)
            .Each(x => _keywordReplacerTask.ReplaceInReactionContainer(x, model.Root));

         return messages;
      }

      private void createGlobalMoleculeContainers(IModel model, IBuildConfiguration buildConfiguration)
      {
         buildConfiguration.AllPresentMolecules().Each(m => _moleculePropertiesContainerTask.CreateGlobalMoleculeContainerFor(model.Root, m, buildConfiguration));
      }

      private void addLocalParametersToMolecule(IModel model, IBuildConfiguration buildConfiguration)
      {
         // retrieve all molecules container defined int the spatial structure
         var allMoleculePropertiesContainer = model.Root.GetAllChildren<IContainer>(x => x.IsNamed(Constants.MOLECULE_PROPERTIES)).ToList();

         var allPresentMolecules = buildConfiguration.AllPresentXenobioticFloatingMoleculeNames().ToList();
         var allEndogenous = buildConfiguration.AllPresentEndogenousStationaryMoleculeNames().ToList();

         foreach (var moleculePropertiesContainer in allMoleculePropertiesContainer)
         {
            addLocalStructureMoleculeParametersToMoleculeAmount(allPresentMolecules, moleculePropertiesContainer, buildConfiguration, model, x => !isEdogenousParameter(x));
            addLocalStructureMoleculeParametersToMoleculeAmount(allEndogenous, moleculePropertiesContainer, buildConfiguration, model, isEdogenousParameter);

            // remove the molecule properties container only used as template
            moleculePropertiesContainer.ParentContainer.RemoveChild(moleculePropertiesContainer);
         }
      }

      private static bool isEdogenousParameter(IParameter parameter)
      {
         return parameter.NameIsOneOf(Constants.ONTOGENY_FACTOR, Constants.HALF_LIFE, Constants.DEGRADATION_COEFF);
      }

      private void addLocalStructureMoleculeParametersToMoleculeAmount(IEnumerable<string> moleculeNames, IContainer moleculePropertiesContainerTemplate,
         IBuildConfiguration buildConfiguration, IModel model, Func<IParameter, bool> query)
      {
         foreach (var moleculeName in moleculeNames)
         {
            // check if molecule amount already exists
            var moleculeAmount = moleculePropertiesContainerTemplate.ParentContainer.GetSingleChildByName<IMoleculeAmount>(moleculeName);
            if (moleculeAmount == null)
               continue;


            moleculePropertiesContainerTemplate.GetChildren<IParameter>().Where(query)
               .Each(parameter => moleculeAmount.Add(_parameterMapper.MapFrom(parameter, buildConfiguration)));

            _keywordReplacerTask.ReplaceIn(moleculeAmount, model.Root, moleculeName);
         }
      }

      private IEnumerable<ValidationMessage> createMoleculeAmounts(IContainer root, IBuildConfiguration buildConfiguration)
      {
         var molecules = buildConfiguration.Molecules;
         var presentMolecules = allPresentMoleculesInContainers(root, buildConfiguration).ToList();

         var moleculesWithPhysicalContainers = presentMolecules.Where(containerIsPhysical);
         moleculesWithPhysicalContainers.Each(pm => addMoleculeToContainer(buildConfiguration, pm, molecules[pm.MoleculeStartValue.MoleculeName]));

         return new MoleculeBuildingBlockValidator().Validate(molecules).Messages.Concat(createValidationMessagesForPresentMolecules(presentMolecules, buildConfiguration.MoleculeStartValues));
      }

      private static bool containerIsPhysical(StartValueAndContainer startValueAndContainer)
      {
         return startValueAndContainer.Container != null && startValueAndContainer.Container.Mode == ContainerMode.Physical;
      }

      private void addMoleculeToContainer(IBuildConfiguration buildConfiguration, StartValueAndContainer startValueAndContainer, IMoleculeBuilder moleculeBuilder)
      {
         startValueAndContainer.Container.Add(_moleculeMapper.MapFrom(moleculeBuilder, buildConfiguration));
      }

      private IEnumerable<ValidationMessage> createValidationMessagesForPresentMolecules(List<StartValueAndContainer> presentMolecules, IBuildingBlock buildingBlock)
      {
         var moleculesWithoutPhysicalContainers = presentMolecules.Where(x => !containerIsPhysical(x));
         return moleculesWithoutPhysicalContainers.Select(x => x.Container == null ? createValidationMessageForNullContainer(x, buildingBlock) : createValidationMessageForMoleculesWithNonPhysicalContainer(x, buildingBlock));
      }

      private ValidationMessage createValidationMessageForMoleculesWithNonPhysicalContainer(StartValueAndContainer startValueAndContainer, IBuildingBlock buildingBlock)
      {
         var moleculeStartValue = startValueAndContainer.MoleculeStartValue;
         return buildValidationMessage(buildingBlock, moleculeStartValue, Validation.StartValueDefinedForNonPhysicalContainer(moleculeStartValue.MoleculeName, moleculeStartValue.ContainerPath.PathAsString));
      }

      private static ValidationMessage buildValidationMessage(IBuildingBlock buildingBlock, IMoleculeStartValue moleculeStartValue, string validationDescription)
      {
         return new ValidationMessage(NotificationType.Warning, validationDescription, moleculeStartValue, buildingBlock);
      }

      private ValidationMessage createValidationMessageForNullContainer(StartValueAndContainer startValueAndContainer, IBuildingBlock buildingBlock)
      {
         var moleculeStartValue = startValueAndContainer.MoleculeStartValue;
         return buildValidationMessage(buildingBlock, moleculeStartValue, Validation.StartValueDefinedForContainerThatCannotBeResolved(moleculeStartValue.MoleculeName, moleculeStartValue.ContainerPath.PathAsString));
      }

      private static IEnumerable<StartValueAndContainer> allPresentMoleculesInContainers(IContainer root, IBuildConfiguration buildConfiguration)
      {
         return from moleculeStartValue in buildConfiguration.AllPresentMoleculeValues()
            let container = moleculeStartValue.ContainerPath.Resolve<IContainer>(root)
            select new StartValueAndContainer
            {
               MoleculeStartValue = moleculeStartValue,
               Container = container
            };
      }

      private class StartValueAndContainer
      {
         public IMoleculeStartValue MoleculeStartValue { get; set; }
         public IContainer Container { get; set; }
      }
   }
}