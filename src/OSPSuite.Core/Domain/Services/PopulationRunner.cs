using System;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Extensions;
using OSPSuite.Core.Serialization.SimModel.Services;
using OSPSuite.SimModel;
using OSPSuite.Utility.Extensions;

namespace OSPSuite.Core.Domain.Services
{
   public class PopulationRunner : SimModelManagerBase, IPopulationRunner
   {
      private readonly IEntitiesInSimulationRetriever _entitiesInSimulationRetriever;
      public event EventHandler<MultipleSimulationsProgressEventArgs> SimulationProgress = delegate { };

      private PopulationRunResults _populationRunResults;
      private PopulationDataSplitter _populationDataSplitter;
      private CancellationTokenSource _cancellationTokenSource;
      private int _numberOfSimulationsToRun;
      private int _numberOfProcessedSimulations;
      private string _simulationName;
      private PathCache<IParameter> _parameterCache;

      public PopulationRunner(
         ISimModelExporter simModelExporter, 
         ISimModelSimulationFactory simModelSimulationFactory, 
         IEntitiesInSimulationRetriever entitiesInSimulationRetriever) : base(simModelExporter, simModelSimulationFactory)
      {
         _entitiesInSimulationRetriever = entitiesInSimulationRetriever;
      }

      public async Task<PopulationRunResults> RunPopulationAsync(IModelCoreSimulation simulation, RunOptions runOptions, DataTable populationData, DataTable agingData = null, DataTable initialValues = null)
      {
         try
         {
            var numberOfCoresToUse = runOptions.NumberOfCoresToUse;
            if (numberOfCoresToUse < 1)
               numberOfCoresToUse = 1;

            _populationDataSplitter = new PopulationDataSplitter(numberOfCoresToUse, populationData, agingData, initialValues);
            _cancellationTokenSource = new CancellationTokenSource();
            _populationRunResults = new PopulationRunResults();

            _numberOfSimulationsToRun = _populationDataSplitter.NumberOfIndividuals;
            _numberOfProcessedSimulations = 0;

            _simulationName = simulation.Name;
            _parameterCache = _entitiesInSimulationRetriever.ParametersFrom(simulation);

            //create simmodel-XML
            var simulationExport = await CreateSimulationExportAsync(simulation, SimModelExportMode.Optimized);

            //Starts one task per core
            var tasks = Enumerable.Range(0, numberOfCoresToUse)
               .Select(coreIndex => runSimulation(coreIndex, simulationExport, _cancellationTokenSource.Token)).ToList();

            await Task.WhenAll(tasks);
            //all tasks are completed. Can return results

            _populationRunResults.SynchronizeResults();

            return _populationRunResults;
         }
         finally
         {
            _populationRunResults = null;
            _populationDataSplitter = null;
            _parameterCache = null;
            RaiseTerminated(this, EventArgs.Empty);
         }
      }

      private Task runSimulation(int coreIndex, string simulationExport, CancellationToken cancellationToken)
      {
         return Task.Run(() =>
         {
            using (var sim = createAndFinalizeSimulation(simulationExport, cancellationToken))
            {
               simulate(sim, coreIndex, cancellationToken);
            }
         }, cancellationToken);
      }

      /// <summary>
      ///    Perform single simulation run
      /// </summary>
      /// <param name="simulation">SimModel simulation (loaded and finalized)</param>
      /// <param name="coreIndex">0..NumberOfCores-1</param>
      /// <param name="cancellationToken">Token used to cancel the action if required</param>
      private void simulate(Simulation simulation, int coreIndex, CancellationToken cancellationToken)
      {
         var allIndividuals = _populationDataSplitter.GetIndividualIdsFor(coreIndex);

         var variableParameters = simulation.VariableParameters.ToList();
         var variableSpecies = simulation.VariableSpecies.ToList();

         foreach (var individualId in allIndividuals)
         {
            cancellationToken.ThrowIfCancellationRequested();

            //get row indices for the simulations on current core
            _populationDataSplitter.UpdateParametersAndSpeciesValuesForIndividual(individualId, variableParameters, variableSpecies, _parameterCache);


            //set new parameter values into SimModel
            simulation.SetParameterValues();

            //set new initial values into SimModel
            simulation.SetSpeciesValues();

            try
            {
               simulation.RunSimulation();
               _populationRunResults.Add(individualResultsFrom(simulation, individualId));
            }
            catch (Exception ex)
            {
               _populationRunResults.AddFailure(individualId, ex.FullMessage());
            }
            finally
            {
               _populationRunResults.AddWarnings(individualId, WarningsFrom(simulation));

               //Could lead to a wrong progress if two threads are accessing the value at the same time
               SimulationProgress(this, new MultipleSimulationsProgressEventArgs(++_numberOfProcessedSimulations, _numberOfSimulationsToRun));
            }
         }
      }

      /// <summary>
      ///    Get Results from SimModel
      /// </summary>
      /// <param name="simulation">SimModel simulation</param>
      /// <param name="individualId">Individual id</param>
      private IndividualResults individualResultsFrom(Simulation simulation, int individualId)
      {
         var results = new IndividualResults {IndividualId = individualId};
         var simulationTimes = simulation.SimulationTimes;
         var simulationTimesLength = simulationTimes.Length;

         foreach (var result in simulation.AllValues)
         {
            //Add quantity name and remove simulation name
            var quantityPath = result.Path.ToObjectPath();
            quantityPath.Remove(_simulationName);
            results.Add(quantityValuesFor(quantityPath.ToString(), result, simulationTimesLength));
         }

         results.Time = quantityValuesFor(Constants.TIME, simulation.SimulationTimes);
         return results;
      }

      private QuantityValues quantityValuesFor(string quantityPath, VariableValues quantityValues, int expectedLength)
      {
         //this is required since SimModel is only returning array of length one for constant 
         double[] values = quantityValues.Values;
         if (quantityValues.IsConstant)
         {
            double defaultValue = values.Length == 1 ? values[0] : double.NaN;
            values = new double[expectedLength].InitializeWith(defaultValue);
         }

         return quantityValuesFor(quantityPath, values);
      }

      private QuantityValues quantityValuesFor(string quantityPath, double[] values)
      {
         return new QuantityValues
         {
            QuantityPath = quantityPath,
            Values = values.ToFloatArray()
         };
      }

      private Simulation createAndFinalizeSimulation(string simulationExport, CancellationToken cancellationToken)
      {
         cancellationToken.ThrowIfCancellationRequested();
         var simulation = CreateSimulation(simulationExport);
         setVariableParameters(simulation);
         setVariableInitialValues(simulation);
         FinalizeSimulation(simulation);
         return simulation;
      }

      /// <summary>
      ///    Set parameters which will be varied into SimModel
      /// </summary>
      /// <param name="simulation">SimModel simulation</param>
      private void setVariableParameters(Simulation simulation)
      {
         SetVariableParameters(simulation, _populationDataSplitter.ParameterPathsToBeVaried(), calculateSensitivities: false);
      }

      /// <summary>
      ///    Set variable initial values which will be varied into SimModel
      /// </summary>
      /// <param name="simulation">SimModel simulation</param>
      private void setVariableInitialValues(Simulation simulation)
      {
         SetVariableMolecules(simulation, _populationDataSplitter.InitialValuesPathsToBeVaried());
      }

      public void StopSimulation() => _cancellationTokenSource.Cancel();
   }
}