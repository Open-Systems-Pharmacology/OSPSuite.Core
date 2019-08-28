//TODO SIMMODEL



//using System;
//using System.Data;
//using System.Linq;
//using System.Threading;
//using System.Threading.Tasks;
//using OSPSuite.Core.Domain.Data;
//using OSPSuite.Core.Domain.Services;
//using OSPSuite.Core.Extensions;
//using OSPSuite.Core.Serialization.SimModel.Services;
//using OSPSuite.Utility.Extensions;
//
//namespace OSPSuite.Core.Domain
//{
//   public class PopulationRunner : SimModelManagerBase, IPopulationRunner
//   {
//      private readonly IObjectPathFactory _objectPathFactory;
//      public event EventHandler<PopulationSimulationProgressEventArgs> SimulationProgress = delegate { };
//
//      private PopulationRunResults _populationRunResults;
//      private PopulationDataSplitter _populationDataSplitter;
//      private CancellationTokenSource _cancelationTokenSource;
//      private int _numberOfSimulationsToRun;
//      private int _numberOfProcessedSimulations;
//      private string _simulationName;
//      public int NumberOfCoresToUse { get; set; }
//
//      public PopulationRunner(ISimModelExporter simModelExporter, ISimModelSimulationFactory simModelSimulationFactory, IObjectPathFactory objectPathFactory) : base(simModelExporter, simModelSimulationFactory)
//      {
//         _objectPathFactory = objectPathFactory;
//         NumberOfCoresToUse = 1;
//      }
//
//      public async Task<PopulationRunResults> RunPopulationAsync(IModelCoreSimulation simulation, DataTable populationData, DataTable agingData = null, DataTable initialValues = null)
//      {
//         try
//         {
//            if (NumberOfCoresToUse < 1)
//               NumberOfCoresToUse = 1;
//
//            agingData = agingData ?? undefinedAgingData();
//            initialValues = initialValues ?? undefinedInitialValues();
//            _populationDataSplitter = new PopulationDataSplitter(populationData, agingData, initialValues, NumberOfCoresToUse);
//            _cancelationTokenSource = new CancellationTokenSource();
//            _populationRunResults = new PopulationRunResults();
//
//            _numberOfSimulationsToRun = _populationDataSplitter.NumberOfIndividuals;
//            _numberOfProcessedSimulations = 0;
//
//            _simulationName = simulation.Name;
//            //create simmodel-XML
//            string simulationExport = await CreateSimulationExportAsync(simulation, SimModelExportMode.Optimized);
//
//            //Starts one task per core
//            var tasks = Enumerable.Range(0, NumberOfCoresToUse)
//               .Select(coreIndex => runSimulation(coreIndex, simulationExport, _cancelationTokenSource.Token)).ToList();
//
//            await Task.WhenAll(tasks);
//            //all tasks are completed. Can return results
//
//            _populationRunResults.SynchronizeResults();
//
//            return _populationRunResults;
//         }
//         finally
//         {
//            _populationRunResults = null;
//            _populationDataSplitter = null;
//            RaiseTerminated(this, EventArgs.Empty);
//         }
//      }
//
//      private DataTable undefinedAgingData()
//      {
//         var table = new DataTable();
//         table.AddColumn<int>(Constants.Population.INDIVIDUAL_ID_COLUMN);
//         table.AddColumn<string>(Constants.Population.PARAMETER_PATH_COLUMN);
//         return table;
//      }
//
//      private DataTable undefinedInitialValues()
//      {
//         var table = new DataTable();
//         table.AddColumn<int>(Constants.Population.INDIVIDUAL_ID_COLUMN);
//         return table;
//      }
//
//      private Task runSimulation(int coreIndex, string simulationExport, CancellationToken cancellationToken)
//      {
//         return Task.Run(() =>
//         {
//            var sim = createAndFinalizeSimulation(simulationExport, cancellationToken);
//            simulate(sim, coreIndex, cancellationToken);
//         }, cancellationToken);
//      }
//
//      /// <summary>
//      ///    Perform single simulation run
//      /// </summary>
//      /// <param name="simulation">SimModel simulation (loaded and finalized)</param>
//      /// <param name="coreIndex">0..NumberOfCores-1</param>
//      /// <param name="cancellationToken">Token used to cancel the action if required</param>
//      private void simulate(ISimulation simulation, int coreIndex, CancellationToken cancellationToken)
//      {
//         var allIndividuals = _populationDataSplitter.GetIndividualIdsFor(coreIndex);
//
//         var variableParameters = simulation.VariableParameters;
//         var initialValues = simulation.VariableSpecies;
//
//         foreach (var individualId in allIndividuals)
//         {
//            cancellationToken.ThrowIfCancellationRequested();
//
//            //get row indices for the simulations on current core
//            _populationDataSplitter.UpdateParametersAndInitialValuesForIndividual(individualId, variableParameters, initialValues);
//
//            //set new parameter values into SimModel
//            simulation.SetParameterValues(variableParameters);
//
//            //set new initial values into SimModel
//            simulation.SetSpeciesProperties(initialValues);
//
//            try
//            {
//               simulation.RunSimulation();
//               _populationRunResults.Add(individualResultsFrom(simulation, individualId));
//            }
//            catch (Exception ex)
//            {
//               _populationRunResults.AddFailure(individualId, ex.FullMessage());
//            }
//            finally
//            {
//               _populationRunResults.AddWarnings(individualId, WarningsFrom(simulation.SolverWarnings));
//
//               //Could lead to a wrong progress if two threads are accessing the value at the same time
//               SimulationProgress(this, new PopulationSimulationProgressEventArgs(++_numberOfProcessedSimulations, _numberOfSimulationsToRun));
//            }
//         }
//      }
//
//      /// <summary>
//      ///    Get Results from SimModel
//      /// </summary>
//      /// <param name="simulation">SimModel simulation</param>
//      /// <param name="individualId">Individual id</param>
//      private IndividualResults individualResultsFrom(ISimulation simulation, int individualId)
//      {
//         var results = new IndividualResults {IndividualId = individualId};
//         var simulationTimes = simulation.SimulationTimes;
//         var simulationTimesLength = simulationTimes.Length;
//
//         foreach (var result in simulation.AllValues)
//         {
//            //Add quantity name and remove simulation name
//            var quantityPath = _objectPathFactory.CreateObjectPathFrom(result.Path.ToPathArray());
//            quantityPath.Remove(_simulationName);
//            results.Add(quantityValuesFor(quantityPath.ToString(), result, simulationTimesLength));
//         }
//
//         results.Time = quantityValuesFor(Constants.TIME, simulation.SimulationTimes);
//         return results;
//      }
//
//      private QuantityValues quantityValuesFor(string quantityPath, IValues quantitValues, int expectedLength)
//      {
//         //this is required since SimModel is only returning array of length one for constant 
//         double[] values = quantitValues.Values;
//         if (quantitValues.IsConstant)
//         {
//            double defaultValue = values.Length == 1 ? values[0] : double.NaN;
//            values = new double[expectedLength].InitializeWith(defaultValue);
//         }
//
//         return quantityValuesFor(quantityPath, values);
//      }
//
//      private QuantityValues quantityValuesFor(string quantityPath, double[] values)
//      {
//         return new QuantityValues
//         {
//            QuantityPath = quantityPath,
//            Values = values.ToFloatArray()
//         };
//      }
//
//      private ISimulation createAndFinalizeSimulation(string simulationExport, CancellationToken cancellationToken)
//      {
//         cancellationToken.ThrowIfCancellationRequested();
//         var simulation = CreateSimulation(simulationExport);
//         setVariableParameters(simulation);
//         setVariableInitialValues(simulation);
//         FinalizeSimulation(simulation);
//         return simulation;
//      }
//
//      /// <summary>
//      ///    Set parameters which will be varied into SimModel
//      /// </summary>
//      /// <param name="simulation">SimModel simulation</param>
//      private void setVariableParameters(ISimulation simulation)
//      {
//         var parameterPathsToBeVaried = _populationDataSplitter.ParameterPathsToBeVaried();
//         var allParameters = simulation.ParameterProperties;
//         var parametersToBeVaried = allParameters.Where(p => parameterPathsToBeVaried.Contains(p.Path));
//         simulation.VariableParameters = parametersToBeVaried.ToList();
//      }
//
//      /// <summary>
//      ///    Set variable initial values which will be varied into SimModel
//      /// </summary>
//      /// <param name="simulation">SimModel simulation</param>
//      private void setVariableInitialValues(ISimulation simulation)
//      {
//         var initialValuesPathsToBeVaried = _populationDataSplitter.InitialValuesPathsToBeVaried();
//         var allInitialValues = simulation.SpeciesProperties;
//         var initialValuesToBeVaried = allInitialValues.Where(p => initialValuesPathsToBeVaried.Contains(p.Path));
//         simulation.VariableSpecies = initialValuesToBeVaried.ToList();
//      }
//
//      public void StopSimulation()
//      {
//         _cancelationTokenSource.Cancel();
//      }
//   }
//}