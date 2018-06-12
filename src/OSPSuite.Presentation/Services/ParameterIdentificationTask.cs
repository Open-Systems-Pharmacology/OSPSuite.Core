using System;
using System.Collections.Generic;
using System.Linq;
using OSPSuite.Assets;
using OSPSuite.Core.Commands;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Domain.ParameterIdentifications;
using OSPSuite.Core.Domain.Repositories;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Domain.Services.ParameterIdentifications;
using OSPSuite.Core.Events;
using OSPSuite.Core.Extensions;
using OSPSuite.Core.Services;
using OSPSuite.Presentation.Core;
using OSPSuite.Presentation.Presenters;
using OSPSuite.Utility.Extensions;

namespace OSPSuite.Presentation.Services
{
   public class ParameterIdentificationTask : IParameterIdentificationTask
   {
      private readonly IParameterIdentificationFactory _parameterIdentificationFactory;
      private readonly IWithIdRepository _withIdRepository;
      private readonly IEntitiesInSimulationRetriever _entitiesInSimulationRetriever;
      private readonly IObservedDataRepository _observedDataRepository;
      private readonly IEntityPathResolver _entityPathResolver;
      private readonly IIdentificationParameterFactory _identificationParameterFactory;
      private readonly IOSPSuiteExecutionContext _executionContext;
      private readonly IFavoriteRepository _favoriteRepository;
      private readonly IParameterIdentificationSimulationSwapValidator _simulationSwapValidator;
      private readonly IApplicationController _applicationController;
      private readonly IParameterIdentificationSimulationSwapCorrector _parameterIdentificationSimulationSwapCorrector;
      private readonly IDialogCreator _dialogCreator;
      private readonly ISimulationSelector _simulationSelector;
      private readonly IHeavyWorkManager _heavyWorkManager;
      private readonly IParameterAnalysableParameterSelector _parameterSelector;

      public ParameterIdentificationTask(
         IParameterIdentificationFactory parameterIdentificationFactory,
         IWithIdRepository withIdRepository,
         IEntitiesInSimulationRetriever entitiesInSimulationRetriever,
         IObservedDataRepository observedDataRepository,
         IEntityPathResolver entityPathResolver,
         IIdentificationParameterFactory identificationParameterFactory,
         IOSPSuiteExecutionContext executionContext,
         IFavoriteRepository favoriteRepository,
         IParameterIdentificationSimulationSwapValidator simulationSwapValidator,
         IApplicationController applicationController,
         IParameterIdentificationSimulationSwapCorrector parameterIdentificationSimulationSwapCorrector,
         IDialogCreator dialogCreator,
         ISimulationSelector simulationSelector,
         IHeavyWorkManager heavyWorkManager,
         IParameterAnalysableParameterSelector parameterSelector)
      {
         _parameterIdentificationFactory = parameterIdentificationFactory;
         _withIdRepository = withIdRepository;
         _entitiesInSimulationRetriever = entitiesInSimulationRetriever;
         _observedDataRepository = observedDataRepository;
         _entityPathResolver = entityPathResolver;
         _identificationParameterFactory = identificationParameterFactory;
         _executionContext = executionContext;
         _favoriteRepository = favoriteRepository;
         _simulationSwapValidator = simulationSwapValidator;
         _applicationController = applicationController;
         _parameterIdentificationSimulationSwapCorrector = parameterIdentificationSimulationSwapCorrector;
         _dialogCreator = dialogCreator;
         _simulationSelector = simulationSelector;
         _heavyWorkManager = heavyWorkManager;
         _parameterSelector = parameterSelector;
      }

      public void AddToProject(ParameterIdentification parameterIdentification)
      {
         addParameterIdentificationToProject(parameterIdentification, _executionContext);
      }

      public void SwapSimulations(ParameterIdentification parameterIdentification, ISimulation oldSimulation, ISimulation newSimulation)
      {
         _executionContext.Load(newSimulation);

         if (!shouldSwap(parameterIdentification, oldSimulation, newSimulation))
            return;

         _parameterIdentificationSimulationSwapCorrector.CorrectParameterIdentification(parameterIdentification, oldSimulation, newSimulation);

         parameterIdentification.SwapSimulations(oldSimulation, newSimulation);
         _executionContext.PublishEvent(new SimulationReplacedInParameterAnalyzableEvent(parameterIdentification, oldSimulation, newSimulation));
      }

      private bool shouldSwap(ParameterIdentification parameterIdentification, ISimulation oldSimulation, ISimulation newSimulation)
      {
         var validationResult = _simulationSwapValidator.ValidateSwap(parameterIdentification, oldSimulation, newSimulation);

         if (validationResult.ValidationState == ValidationState.Valid)
            return true;

         using (var validationMessagesPresenter = _applicationController.Start<IValidationMessagesPresenter>())
         {
            return validationMessagesPresenter.Display(validationResult);
         }
      }

      private void addParameterIdentificationToProject(ParameterIdentification parameterIdentification, IOSPSuiteExecutionContext executionContext)
      {
         var project = executionContext.Project;
         project.AddParameterIdentification(parameterIdentification);
         parameterIdentification.IsLoaded = true;
         executionContext.Register(parameterIdentification);
         executionContext.PublishEvent(new ParameterIdentificationCreatedEvent(parameterIdentification));
      }

      public ParameterIdentification CreateParameterIdentificationBasedOn(IEnumerable<IParameter> parameters)
      {
         return createParameterIdentificationBasedOn(parameterIdentification => { AddParametersTo(parameterIdentification, parameters); });
      }

      public ParameterIdentification CreateParameterIdentificationBasedOn(IEnumerable<ISimulation> simulations)
      {
         var simulationList = simulations.ToList();
         return createParameterIdentificationBasedOn(parameterIdentification =>
         {
            AddSimulationsTo(parameterIdentification, simulationList);
            addFavoritesParametersTo(parameterIdentification, simulationList);
         });
      }

      private void addFavoritesParametersTo(ParameterIdentification parameterIdentification, IReadOnlyList<ISimulation> simulations)
      {
         simulations.Each(x => addFavoriteParametersTo(parameterIdentification, x));
      }

      private void addFavoriteParametersTo(ParameterIdentification parameterIdentification, ISimulation simulation)
      {
         var allParameters = _entitiesInSimulationRetriever.ParametersFrom(simulation);
         var parametersToAdd = _favoriteRepository.All()
            .Select(path => allParameters[path])
            .Where(p => p != null);

         AddParametersTo(parameterIdentification, parametersToAdd);
      }

      private ParameterIdentification createParameterIdentificationBasedOn(Action<ParameterIdentification> configureParameterIdentificationAction)
      {
         var parameterIdentification = _parameterIdentificationFactory.Create();
         configureParameterIdentificationAction(parameterIdentification);
         AddToProject(parameterIdentification);
         return parameterIdentification;
      }

      public void AddParametersTo(ParameterIdentification parameterIdentification, IEnumerable<IParameter> parameters)
      {
         parameters.Each(x => AddParameterTo(parameterIdentification, x));
      }

      public void AddSimulationsTo(ParameterIdentification parameterIdentification, IEnumerable<ISimulation> simulations)
      {
         simulations.Each(x => AddSimulationTo(parameterIdentification, x));
      }

      public void AddParameterTo(ParameterIdentification parameterIdentification, IParameter parameter)
      {
         if (!_parameterSelector.CanUseParameter(parameter))
            return;

         var simulation = simulationContaining(parameter);
         if (simulation == null)
            return;

         AddSimulationTo(parameterIdentification, simulation);

         var parameterSelection = new ParameterSelection(simulation, _entityPathResolver.PathFor(parameter));
         addParameterSelectionToParameterIdentification(parameterIdentification, parameterSelection);
      }

      private void addParameterSelectionToParameterIdentification(ParameterIdentification parameterIdentification, ParameterSelection parameterSelection)
      {
         var identificationParameter = parameterIdentification.IdentificationParameterByLinkedPath(parameterSelection.Path);
         if (identificationParameter != null)
            identificationParameter.AddLinkedParameter(parameterSelection);
         else
            parameterIdentification.AddIdentificationParameter(_identificationParameterFactory.CreateFor(parameterSelection, parameterIdentification));
      }

      public void AddSimulationTo(ParameterIdentification parameterIdentification, ISimulation simulation)
      {
         if (parameterIdentification.UsesSimulation(simulation))
            return;

         _executionContext.Load(simulation);

         parameterIdentification.AddSimulation(simulation);
         addOutputMappingsFor(simulation, parameterIdentification);
      }

      private void addOutputMappingsFor(ISimulation simulation, ParameterIdentification parameterIdentification)
      {
         foreach (var keyValue in _entitiesInSimulationRetriever.OutputsFrom(simulation).KeyValues)
         {
            var output = keyValue.Value;
            var outputPath = keyValue.Key;
            observedDataInSimulationMatchingOutputs(outputPath, simulation).Each(obs => addOutputMapping(output, outputPath, obs, simulation, parameterIdentification));
         }
      }

      private void addOutputMapping(IQuantity output, string outputPath, DataRepository observedData, ISimulation simulation, ParameterIdentification parameterIdentification)
      {
         if (parameterIdentification.UsesObservedData(observedData))
            return;

         parameterIdentification.AddOutputMapping(new OutputMapping
         {
            OutputSelection = new SimulationQuantitySelection(simulation, new QuantitySelection(outputPath, output.QuantityType)),
            WeightedObservedData = new WeightedObservedData(observedData),
            Scaling = DefaultScalingFor(output)
         });
      }

      public Scalings DefaultScalingFor(IQuantity output)
      {
         return output.IsFraction() ? Scalings.Linear : Scalings.Log;
      }

      private IEnumerable<DataRepository> observedDataInSimulationMatchingOutputs(string outputPath, ISimulation simulation)
      {
         return AllObservedDataUsedBy(simulation).Where(obs => observedDataMatchesOutput(obs, outputPath));
      }

      private bool observedDataMatchesOutput(DataRepository observedData, string outputPath)
      {
         var organ = observedData.ExtendedPropertyValueFor(ObservedData.ORGAN);
         var compartment = observedData.ExtendedPropertyValueFor(ObservedData.COMPARTMENT);
         var molecule = observedData.ExtendedPropertyValueFor(ObservedData.MOLECULE);

         if (organ == null || compartment == null || molecule == null)
            return false;

         return outputPath.Contains(organ) && outputPath.Contains(compartment) && outputPath.Contains(molecule);
      }

      private ISimulation simulationContaining(IParameter parameter)
      {
         return _withIdRepository.Get<ISimulation>(parameter.Origin.SimulationId);
      }

      public ParameterIdentification CreateParameterIdentification()
      {
         return CreateParameterIdentificationBasedOn(Enumerable.Empty<IParameter>());
      }

      public IEnumerable<DataRepository> AllObservedDataUsedBy(ISimulation simulation) => _observedDataRepository.AllObservedDataUsedBy(simulation);

      public IEnumerable<ParameterIdentification> ParameterIdentificationsUsingObservedData(DataRepository observedData)
      {
         return allParameterIdentifications().Where(parameterIdentification => parameterIdentification.OutputMappingsUsingDataRepository(observedData).Any());
      }

      private IEnumerable<ParameterIdentification> allParameterIdentifications()
      {
         return _executionContext.Project.AllParameterIdentifications;
      }

      public bool Delete(IReadOnlyList<ParameterIdentification> parameterIdentifications)
      {
         var res = _dialogCreator.MessageBoxYesNo(Captions.ParameterIdentification.ReallyDeleteParameterIdentifications(parameterIdentifications.AllNames()));
         if (res == ViewResult.No)
            return false;

         parameterIdentifications.Each(delete);
         return true;
      }

      public bool SimulationCanBeUsedForIdentification(ISimulation simulation)
      {
         return _simulationSelector.SimulationCanBeUsedForIdentification(simulation);
      }

      public ParameterIdentification Clone(ParameterIdentification parameterIdentification)
      {
         loadParameterIdentification(parameterIdentification);

         using (var clonePresenter = _applicationController.Start<ICloneObjectBasePresenter<ParameterIdentification>>())
         {
            return clonePresenter.CreateCloneFor(parameterIdentification);
         }
      }

      private void loadParameterIdentification(ParameterIdentification parameterIdentification)
      {
         _heavyWorkManager.Start(() => _executionContext.Load(parameterIdentification));
      }

      private void delete(ParameterIdentification parameterIdentification)
      {
         _applicationController.Close(parameterIdentification);

         _executionContext.Project.RemoveParameterIdentification(parameterIdentification);

         _executionContext.Unregister(parameterIdentification);
         _executionContext.PublishEvent(new ParameterIdentificationDeletedEvent(parameterIdentification));
      }
   }
}