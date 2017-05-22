using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using OSPSuite.Assets;
using OSPSuite.Utility;
using OSPSuite.Utility.Collections;
using OSPSuite.Utility.Extensions;
using OSPSuite.Core.Domain.Mappers;
using OSPSuite.Core.Domain.ParameterIdentifications;
using OSPSuite.Core.Domain.ParameterIdentifications.Algorithms;

namespace OSPSuite.Core.Domain.Services.ParameterIdentifications
{
   public class ParameterIdentificationRunStatusEventArgs : EventArgs
   {
      public ParameterIdentificationRunState State { get; }

      public ParameterIdentificationRunStatusEventArgs(ParameterIdentificationRunState state)
      {
         State = state;
      }
   }

   public interface IParameterIdentificationRun
   {
      void InitializeWith(IParameterIdentifcationRunInitializer runInitializer);
      ParameterIdentificationRunResult Run(CancellationToken cancellationToken);
      event EventHandler<ParameterIdentificationRunStatusEventArgs> RunStatusChanged;
      void Clear();
      OptimizationRunResult BestResult { get; }
      ParameterIdentificationRunResult RunResult { get; }
      IReadOnlyList<float> TotalErrorHistory { get; }
      IReadOnlyCollection<IdentificationParameterHistory> ParametersHistory { get; }
   }

   public class ParameterIdentificationRun : IParameterIdentificationRun
   {
      private readonly IResidualCalculatorFactory _residualCalculatorFactory;
      private readonly ITimeGridUpdater _timeGridUpdater;
      private readonly ISimModelBatchFactory _simModelBatchFactory;
      private readonly ISimulationToModelCoreSimulationMapper _modelCoreSimulationMapper;
      private readonly IParameterIdentificationAlgorithmToOptmizationAlgorithmMapper _optimizationAlgorithmMapper;
      private readonly IOutputSelectionUpdater _outputSelectionUpdater;
      private readonly ICoreUserSettings _coreUserSettings;
      private readonly IJacobianMatrixCalculator _jacobianMatrixCalculator;
      private readonly Cache<ISimulation, ISimModelBatch> _allSimModelBatches = new Cache<ISimulation, ISimModelBatch>();
      private readonly List<float> _totalErrorHistory = new List<float>();
      private readonly Cache<string, IdentificationParameterHistory> _parametersHistory = new Cache<string, IdentificationParameterHistory>(x => x.Name);
      public ParameterIdentificationRunResult RunResult { get; } = new ParameterIdentificationRunResult();
      public IReadOnlyList<float> TotalErrorHistory => _totalErrorHistory;
      public IReadOnlyCollection<IdentificationParameterHistory> ParametersHistory => _parametersHistory;

      public event EventHandler<ParameterIdentificationRunStatusEventArgs> RunStatusChanged = delegate { };

      private ParameterIdentification _parameterIdentification;
      private IResidualCalculator _residualCalculator;
      private IOptimizationAlgorithm _optimizationAlgorithm;
      private CancellationToken _cancellationToken;
      private IParameterIdentifcationRunInitializer _runInitializer;
      private List<IdentificationParameter> _variableParameters;
      private List<IdentificationParameter> _fixedParameters;

      public OptimizationRunResult BestResult => RunResult.BestResult;

      public ParameterIdentificationRun(IResidualCalculatorFactory residualCalculatorFactory, ITimeGridUpdater timeGridUpdater,
         ISimModelBatchFactory simModelBatchFactory, ISimulationToModelCoreSimulationMapper modelCoreSimulationMapper,
         IParameterIdentificationAlgorithmToOptmizationAlgorithmMapper optimizationAlgorithmMapper, IOutputSelectionUpdater outputSelectionUpdater,
         ICoreUserSettings coreUserSettings, IJacobianMatrixCalculator jacobianMatrixCalculator)
      {
         _residualCalculatorFactory = residualCalculatorFactory;
         _timeGridUpdater = timeGridUpdater;
         _simModelBatchFactory = simModelBatchFactory;
         _modelCoreSimulationMapper = modelCoreSimulationMapper;
         _optimizationAlgorithmMapper = optimizationAlgorithmMapper;
         _outputSelectionUpdater = outputSelectionUpdater;
         _coreUserSettings = coreUserSettings;
         _jacobianMatrixCalculator = jacobianMatrixCalculator;
      }

      public void InitializeWith(IParameterIdentifcationRunInitializer runInitializer)
      {
         _runInitializer = runInitializer;
      }

      private void initialize()
      {
         _parameterIdentification = _runInitializer.InitializeRun().Result;
         RunResult.Description = _parameterIdentification.Description;
         _optimizationAlgorithm = _optimizationAlgorithmMapper.MapFrom(_parameterIdentification.AlgorithmProperties);
         _residualCalculator = _residualCalculatorFactory.CreateFor(_parameterIdentification.Configuration);
         _parameterIdentification.AllSimulations.Each(s => _allSimModelBatches.Add(s, createSimModelBatch(s)));
         initializeParameterHistoryCache();
         _variableParameters = _parameterIdentification.AllVariableIdentificationParameters.ToList();
         _fixedParameters = _parameterIdentification.AllFixedIdentificationParameters.ToList();
      }

      private void initializeParameterHistoryCache()
      {
         _parameterIdentification.AllIdentificationParameters.Each(x => _parametersHistory.Add(new IdentificationParameterHistory(x)));
      }

      private ISimModelBatch createSimModelBatch(ISimulation simulation)
      {
         var simModelBatch = _simModelBatchFactory.Create();
         var modelCoreSimulation = _modelCoreSimulationMapper.MapFrom(simulation, shouldCloneModel:true);
         _timeGridUpdater.UpdateSimulationTimeGrid(modelCoreSimulation, _parameterIdentification.Configuration.RemoveLLOQMode, _parameterIdentification.AllDataRepositoryMappedFor(simulation));
         _outputSelectionUpdater.UpdateOutputsIn(modelCoreSimulation, _parameterIdentification.AllOutputsMappedFor(simulation));
         simModelBatch.InitializeWith(modelCoreSimulation, _parameterIdentification.PathOfOptimizedParameterBelongingTo(simulation), simulationResultsName: Captions.ParameterIdentification.SimulationResultsForRun(RunResult.Index));
         return simModelBatch;
      }

      public virtual ParameterIdentificationRunResult Run(CancellationToken cancellationToken)
      {
         _cancellationToken = cancellationToken;
         var begin = SystemTime.UtcNow();
         try
         {
            RunResult.Status = RunStatus.Running;
            initialize();

            var variableParameterConstraints = retrieveVariableParameterConstraints();
            RunResult.Properties = _optimizationAlgorithm.Optimize(variableParameterConstraints, performRun);
            calculateJacobian();
            RunResult.Status = RunStatus.RanToCompletion;
            return RunResult;
         }
         catch (OperationCanceledException)
         {
            RunResult.Status = RunStatus.Canceled;
            return RunResult;
         }
         catch (Exception e)
         {
            RunResult.Status = RunStatus.Faulted;
            RunResult.Message = e.FullMessage();
            return RunResult;
         }
         finally
         {
            var end = SystemTime.UtcNow();
            var timeSpent = end - begin;
            RunResult.Duration = timeSpent;
            raiseRunStatusChanged(BestResult);
         }
      }

      public void Clear()
      {
         _allSimModelBatches.Each(x => x.Clear());
         _allSimModelBatches.Clear();
         _variableParameters?.Clear();
         _fixedParameters?.Clear();
         _parameterIdentification = null;
         _runInitializer = null;
      }

      private void calculateJacobian()
      {
         if (!_parameterIdentification.Configuration.CalculateJacobian)
            return;

         RunResult.Status = RunStatus.CalculatingSensitivity;
         raiseRunStatusChanged(BestResult);

         _allSimModelBatches.Each(x => x.InitializeForSensitivity());
         updateValuesAndCalculate(RunResult.BestResult.Values);
         RunResult.JacobianMatrix = _jacobianMatrixCalculator.CalculateFor(_parameterIdentification, RunResult.BestResult, _allSimModelBatches);
      }

      private OptimizationRunResult performRun(IReadOnlyList<OptimizedParameterValue> values)
      {
         var optimizationRunResult = updateValuesAndCalculate(values);

         updateRunResult(optimizationRunResult);

         raiseRunStatusChanged(optimizationRunResult);
         return optimizationRunResult;
      }

      private OptimizationRunResult updateValuesAndCalculate(IReadOnlyList<OptimizedParameterValue> values)
      {
         updateParameterValues(values);

         var simulationResults = runSimulations();

         return new OptimizationRunResult
         {
            ResidualsResult = _residualCalculator.Calculate(simulationResults, _parameterIdentification.AllOutputMappings),
            Values = values,
            SimulationResults = simulationResults.Select(x => x.Results).ToList()
         };
      }

      private void raiseRunStatusChanged(OptimizationRunResult currentRunResult)
      {
         RunStatusChanged(this, new ParameterIdentificationRunStatusEventArgs(new ParameterIdentificationRunState(RunResult, currentRunResult, _totalErrorHistory, _parametersHistory)));
      }

      private void updateRunResult(OptimizationRunResult currentResult)
      {
         if (currentResult.TotalError < RunResult.BestResult.TotalError)
            RunResult.BestResult = currentResult;

         _totalErrorHistory.Add(Convert.ToSingle(RunResult.BestResult.TotalError));
      }

      private IReadOnlyList<SimulationRunResults> runSimulations()
      {
         var simulationResults = new ConcurrentBag<SimulationRunResults>();
         var parallelOptions = new ParallelOptions
         {
            CancellationToken = _cancellationToken,
            MaxDegreeOfParallelism = _parameterIdentification.IsSingleRun ? _coreUserSettings.MaximumNumberOfCoresToUse : 1
         };

         try
         {
            Parallel.ForEach(_allSimModelBatches, parallelOptions, simulationBatch =>
            {
               simulationResults.Add(simulationBatch.RunSimulation());
               parallelOptions.CancellationToken.ThrowIfCancellationRequested();
            });
         }
         catch (OperationCanceledException)
         {
            _allSimModelBatches.Each(s => s.StopSimulation());
            throw;
         }

         return simulationResults.ToList();
      }

      private void updateParameterValues(IReadOnlyList<OptimizedParameterValue> values)
      {
         if (_variableParameters.Count != values.Count)
            throw new ArgumentException(Error.IdentificationParametersAndValuesDoNotTheSameLength(_variableParameters.Count, values.Count));

         //Update variable parameters according to optimized parameter values
         _variableParameters.Each((x, i) => addParameterValue(x, values[i].Value));

         //Preserve fixed parameter values
         _fixedParameters.Each((x, i) => addParameterValue(x, x.StartValue));
      }

      private void addParameterValue(IdentificationParameter identificationParameter, double value)
      {
         foreach (var linkedParameter in identificationParameter.AllLinkedParameters)
         {
            var simModelBatch = _allSimModelBatches[linkedParameter.Simulation];
            var valueToUse = identificationParameter.UseAsFactor ? linkedParameter.Parameter.Value * value : value;
            simModelBatch.UpdateParameterValue(linkedParameter.Path, valueToUse);
         }

         _parametersHistory[identificationParameter.Name].AddValue(value);
      }

      private IReadOnlyList<OptimizedParameterConstraint> retrieveVariableParameterConstraints()
      {
         return _variableParameters.Select(x => new OptimizedParameterConstraint(x.Name, x.MinValueParameter.Value, x.MaxValueParameter.Value, x.StartValueParameter.Value, x.Scaling)).ToArray();
      }
   }
}