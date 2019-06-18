using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using FakeItEasy;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Domain.Mappers;
using OSPSuite.Core.Domain.ParameterIdentifications;
using OSPSuite.Core.Domain.ParameterIdentifications.Algorithms;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Domain.Services.ParameterIdentifications;
using OSPSuite.Core.Extensions;
using OSPSuite.Helpers;
using OSPSuite.Utility.Extensions;

namespace OSPSuite.Core
{
   public abstract class concern_for_ParameterIdentificationRun : ContextSpecification<ParameterIdentificationRun>
   {
      protected ISimulationToModelCoreSimulationMapper _modelCoreSimulationMapper;
      protected IResidualCalculatorFactory _residualCalculatorFactory;
      protected ITimeGridUpdater _timeGridUpdater;
      protected ISimModelBatchFactory _simModelBatchFactory;
      protected ParameterIdentification _parameterIdentification;
      protected ISimulation _simulation;
      protected IModelCoreSimulation _modelCoreSimulation;
      protected OutputMapping _outputMapping;
      protected ISimModelBatch _simModelBatch;
      protected IdentificationParameter _identificationParameter;
      protected IResidualCalculator _residualCalculator;
      private IParameterIdentificationAlgorithmToOptmizationAlgorithmMapper _optimizationAlgorithmMapper;
      protected IOptimizationAlgorithm _algorithm;
      protected ParameterSelection _parameterSelection1;
      protected ParameterSelection _parameterSelection2;
      protected IParameter _parameter1;
      protected IParameter _parameter2;
      protected IOutputSelectionUpdater _outputSelectionUpdater;
      private ICoreUserSettings _coreUserSettings;
      protected CancellationTokenSource _cancellationTokenSource;
      protected CancellationToken _cancellationToken;
      protected IJacobianMatrixCalculator _jacobianMatrixCalculator;
      private IParameterIdentifcationRunInitializer _runInitializer;

      protected override void Context()
      {
         _modelCoreSimulationMapper = A.Fake<ISimulationToModelCoreSimulationMapper>();
         _residualCalculatorFactory = A.Fake<IResidualCalculatorFactory>();
         _timeGridUpdater = A.Fake<ITimeGridUpdater>();
         _simModelBatchFactory = A.Fake<ISimModelBatchFactory>();
         _optimizationAlgorithmMapper = A.Fake<IParameterIdentificationAlgorithmToOptmizationAlgorithmMapper>();
         _outputSelectionUpdater = A.Fake<IOutputSelectionUpdater>();
         _coreUserSettings = A.Fake<ICoreUserSettings>();
         _jacobianMatrixCalculator = A.Fake<IJacobianMatrixCalculator>();

         _coreUserSettings.MaximumNumberOfCoresToUse = 2;
         sut = new ParameterIdentificationRun(_residualCalculatorFactory, _timeGridUpdater, _simModelBatchFactory, _modelCoreSimulationMapper,
            _optimizationAlgorithmMapper, _outputSelectionUpdater, _coreUserSettings, _jacobianMatrixCalculator);

         _simulation = A.Fake<ISimulation>();
         _parameter1 = A.Fake<IParameter>();
         _parameter1.Dimension = DomainHelperForSpecs.ConcentrationDimensionForSpecs();
         _parameter1.Value = 15;
         _parameter2 = A.Fake<IParameter>();
         _parameter2.Value = 35;
         _parameter2.Dimension = DomainHelperForSpecs.ConcentrationDimensionForSpecs();

         _parameterIdentification = new ParameterIdentification();
         _parameterIdentification.Configuration.LLOQMode = LLOQModes.OnlyObservedData;
         _parameterIdentification.Configuration.RemoveLLOQMode = RemoveLLOQModes.NoTrailing;

         _parameterIdentification.AddSimulation(_simulation);

         _parameterSelection1 = ParameterSelectionFor(_parameter1, "ParameterPath1");
         _parameterSelection2 = ParameterSelectionFor(_parameter2, "ParameterPath2");

         _identificationParameter = DomainHelperForSpecs.IdentificationParameter("IdParam", min: 10, max: 20, startValue: 15);

         _identificationParameter.AddLinkedParameter(_parameterSelection1);
         _identificationParameter.AddLinkedParameter(_parameterSelection2);

         _modelCoreSimulation = A.Fake<IModelCoreSimulation>();

         A.CallTo(() => _modelCoreSimulationMapper.MapFrom(_simulation, true)).Returns(_modelCoreSimulation);

         _outputMapping = A.Fake<OutputMapping>();
         A.CallTo(() => _outputMapping.UsesSimulation(_simulation)).Returns(true);
         A.CallTo(() => _outputMapping.WeightedObservedData.ObservedData).Returns(DomainHelperForSpecs.ObservedData());
         _parameterIdentification.AddOutputMapping(_outputMapping);

         _simModelBatch = A.Fake<ISimModelBatch>();
         A.CallTo(() => _simModelBatchFactory.Create()).Returns(_simModelBatch);

         _parameterIdentification.AddIdentificationParameter(_identificationParameter);

         _residualCalculator = A.Fake<IResidualCalculator>();
         A.CallTo(_residualCalculatorFactory).WithReturnType<IResidualCalculator>().Returns(_residualCalculator);

         _algorithm = A.Fake<IOptimizationAlgorithm>();
         A.CallTo(() => _optimizationAlgorithmMapper.MapFrom(_parameterIdentification.AlgorithmProperties)).Returns(_algorithm);

         _cancellationTokenSource = new CancellationTokenSource();
         _cancellationToken = _cancellationTokenSource.Token;

         _runInitializer = A.Fake<IParameterIdentifcationRunInitializer>();
         A.CallTo(() => _runInitializer.InitializeRun()).ReturnsAsync(_parameterIdentification);

         PerformExtraInitializationSteps();
         sut.InitializeWith(_runInitializer);
      }

      protected virtual void PerformExtraInitializationSteps()
      {
      }

      protected ParameterSelection ParameterSelectionFor(IParameter parameter, string path)
      {
         var parameterSelection = A.Fake<ParameterSelection>();
         A.CallTo(() => parameterSelection.Path).Returns(path);
         A.CallTo(() => parameterSelection.Simulation).Returns(_simulation);
         A.CallTo(() => parameterSelection.Parameter).Returns(parameter);
         A.CallTo(() => parameterSelection.Dimension).Returns(parameter.Dimension);
         return parameterSelection;
      }
   }

   public class When_initializing_the_parameter_identification_run_with_a_parameter_identification : concern_for_ParameterIdentificationRun
   {
      private IReadOnlyList<string> _parameters;
      private List<DataRepository> _observedDataList;
   
      protected override void Context()
      {
         base.Context();
         A.CallTo(() => _simModelBatch.InitializeWith(_modelCoreSimulation, A<IReadOnlyList<string>>._, A<bool>._, A<string>._))
            .Invokes(x => _parameters = x.GetArgument<IReadOnlyList<string>>(1));

         A.CallTo(() => _timeGridUpdater.UpdateSimulationTimeGrid(_modelCoreSimulation, RemoveLLOQModes.NoTrailing, A<IEnumerable<DataRepository>>._))
            .Invokes(x => _observedDataList = x.GetArgument<IEnumerable<DataRepository>>(2).ToList());
      }

      protected override void Because()
      {
         sut.Run(_cancellationToken);
      }

      [Observation]
      public void should_initialize_a_new_instance_per_simulation_of_a_sim_model_batch_and_pass_it_all_variable_parmaeters()
      {
         _parameters.ShouldContain(_parameterSelection1.Path, _parameterSelection2.Path);
      }

      [Observation]
      public void should_update_the_time_grid_of_all_resulting_core_simulation_with_the_observed_data_used_by_the_simulation()
      {
         _observedDataList.ShouldOnlyContain(_outputMapping.WeightedObservedData.ObservedData);
      }

      [Observation]
      public void should_retrieve_a_new_instance_of_the_residual_calculator_based_on_the_parameter_identification_configuration()
      {
         A.CallTo(() => _residualCalculatorFactory.CreateFor(_parameterIdentification.Configuration)).MustHaveHappened();
      }
   }

   public class When_initializing_the_parameter_indentification_run_with_a_parameter_identification_with_multiple_simulations : concern_for_ParameterIdentificationRun
   {
      private OutputMapping _outputMapping2;
      private ISimulation _simulation2;
      private IModelCoreSimulation _modelCoreSimulation2;

      protected override void Context()
      {
         base.Context();

         _outputMapping2 = A.Fake<OutputMapping>();
         A.CallTo(() => _outputMapping2.UsesSimulation(_simulation2)).Returns(true);

         _simulation2 = A.Fake<ISimulation>();
         _parameterIdentification.AddSimulation(_simulation2);

         _parameterIdentification.AddOutputMapping(_outputMapping2);

         _modelCoreSimulation2 = A.Fake<IModelCoreSimulation>();
         A.CallTo(() => _modelCoreSimulationMapper.MapFrom(_simulation2, true)).Returns(_modelCoreSimulation2);
      }

      protected override void Because()
      {
         sut.Run(_cancellationToken);
      }

      [Observation]
      public void should_update_the_time_grid_of_all_resulting_core_simulations()
      {
         A.CallTo(() => _timeGridUpdater.UpdateSimulationTimeGrid(_modelCoreSimulation, RemoveLLOQModes.NoTrailing, A<IEnumerable<DataRepository>>._)).MustHaveHappened();
         A.CallTo(() => _timeGridUpdater.UpdateSimulationTimeGrid(_modelCoreSimulation2, RemoveLLOQModes.NoTrailing, A<IEnumerable<DataRepository>>._)).MustHaveHappened();
      }

      [Observation]
      public void should_update_the_output_selection_of_all_resulting_core_simulations()
      {
         A.CallTo(() => _outputSelectionUpdater.UpdateOutputsIn(_modelCoreSimulation, A<IEnumerable<QuantitySelection>>._)).MustHaveHappened();
         A.CallTo(() => _outputSelectionUpdater.UpdateOutputsIn(_modelCoreSimulation2, A<IEnumerable<QuantitySelection>>._)).MustHaveHappened();
      }

      [Observation]
      public void should_initialize_a_new_instance_per_simulation_of_a_sim_model_batch()
      {
         A.CallTo(() => _simModelBatchFactory.Create()).MustHaveHappenedTwiceExactly();
      }
   }

   public class When_starting_the_parameter_identification_run_configured_with_a_parameter_identification : concern_for_ParameterIdentificationRun
   {
      private OptimizationRunProperties _runProperties;
      private OptimizedParameterConstraint[] _constraints;
      private ParameterIdentificationRunResult _runResult;

      protected override void Context()
      {
         base.Context();
         _runProperties = new OptimizationRunProperties(10);
         A.CallTo(_algorithm).WithReturnType<OptimizationRunProperties>()
            .Invokes(x => { _constraints = x.GetArgument<OptimizedParameterConstraint[]>(0); })
            .Returns(_runProperties);
      }

      protected override void Because()
      {
         _runResult = sut.Run(_cancellationToken);
      }

      [Observation]
      public void should_start_the_optimization_with_the_expected_parameter_constraints()
      {
         _constraints.Length.ShouldBeEqualTo(1);
         var constraint = _constraints[0];
         constraint.Name.ShouldBeEqualTo(_identificationParameter.Name);
         constraint.Min.ShouldBeEqualTo(_identificationParameter.MinValueParameter.Value);
         constraint.Max.ShouldBeEqualTo(_identificationParameter.MaxValueParameter.Value);
         constraint.Value.ShouldBeEqualTo(_identificationParameter.StartValueParameter.Value);
      }

      [Observation]
      public void should_return_the_result_of_the_optimization()
      {
         _runResult.Properties.ShouldBeEqualTo(_runProperties);
      }

      [Observation]
      public void should_update_the_status_of_the_run_result_to_completed()
      {
         _runResult.Status.ShouldBeEqualTo(RunStatus.RanToCompletion);
      }
   }

   public class When_the_parameter_identification_run_is_running_the_simulations : concern_for_ParameterIdentificationRun
   {
      private Func<IReadOnlyList<OptimizedParameterValue>, OptimizationRunResult> _objectiveFunction;

      protected override void Context()
      {
         base.Context();

         A.CallTo(_algorithm).WithReturnType<OptimizationRunProperties>()
            .Invokes(x => { _objectiveFunction = x.GetArgument<Func<IReadOnlyList<OptimizedParameterValue>, OptimizationRunResult>>(1); })
            .Returns(A.Fake<OptimizationRunProperties>());

         sut.Run(_cancellationToken);
      }

      protected override void Because()
      {
         _objectiveFunction(new[] {new OptimizedParameterValue("A", 100.0, 50d)});
      }

      [Observation]
      public void should_update_the_simulation_with_the_current_values()
      {
         A.CallTo(() => _simModelBatch.UpdateParameterValue(_parameterSelection1.Path, 100.0)).MustHaveHappened();
         A.CallTo(() => _simModelBatch.UpdateParameterValue(_parameterSelection2.Path, 100.0)).MustHaveHappened();
      }

      [Observation]
      public void should_run_the_simulation()
      {
         A.CallTo(() => _simModelBatch.RunSimulation()).MustHaveHappened();
      }
   }

   public class When_the_parameter_identification_run_is_running_multiple_simulations : concern_for_ParameterIdentificationRun
   {
      private Func<OptimizedParameterValue[], OptimizationRunResult> _objectiveFunction;
      private int _counter = 0;
      private ResidualsResult _runResult1;
      private ResidualsResult _runResult2;
      private ResidualsResult _runResult3;
      private ResidualsResult[] _runResults;
      private int _raiseCount;
      private ParameterIdentificationRunStatusEventArgs _lastArgs;

      protected override void Context()
      {
         base.Context();
         _runResult1 = A.Fake<ResidualsResult>();
         _runResult2 = A.Fake<ResidualsResult>();
         _runResult3 = A.Fake<ResidualsResult>();
         A.CallTo(() => _runResult1.TotalError).Returns(10);
         A.CallTo(() => _runResult2.TotalError).Returns(5);
         A.CallTo(() => _runResult3.TotalError).Returns(20);
         _runResults = new[] {_runResult1, _runResult2, _runResult3};

         A.CallTo(_algorithm).WithReturnType<OptimizationRunProperties>()
            .Invokes(x => { _objectiveFunction = x.GetArgument<Func<IReadOnlyList<OptimizedParameterValue>, OptimizationRunResult>>(1); })
            .Returns(A.Fake<OptimizationRunProperties>());


         A.CallTo(_residualCalculator).WithReturnType<ResidualsResult>()
            .ReturnsLazily(x => _runResults[_counter++]);

         sut.Run(_cancellationToken);

         sut.RunStatusChanged += (o, e) =>
         {
            _raiseCount++;
            _lastArgs = e;
         };
      }

      protected override void Because()
      {
         _counter = 0;
         //Call the objective function as many time as we have results
         _runResults.Each(x => { _objectiveFunction(new[] {new OptimizedParameterValue("P", 100.0, 50d)}); });
      }

      [Observation]
      public void should_save_the_best_result()
      {
         sut.BestResult.ResidualsResult.ShouldBeEqualTo(_runResult2);
      }

      [Observation]
      public void should_save_the_error_history()
      {
         _lastArgs.State.ErrorHistory.ShouldBeEqualTo(new[] {_runResult1.TotalError, _runResult2.TotalError, _runResult2.TotalError}.ToFloatArray());
      }

      [Observation]
      public void should_raise_the_results_updated_event_for_each_iteration_and_save_the_best_result()
      {
         _raiseCount.ShouldBeEqualTo(_runResults.Length);
         _lastArgs.State.BestResult.TotalError.ShouldBeEqualTo(_runResult2.TotalError);
      }
   }

   public class When_the_parameter_identification_run_is_running_the_simulations_and_updating_an_identification_parameter_using_factor : concern_for_ParameterIdentificationRun
   {
      private Func<IReadOnlyList<OptimizedParameterValue>, OptimizationRunResult> _objectiveFunction;

      protected override void Context()
      {
         base.Context();
         _identificationParameter.UseAsFactor = true;
         A.CallTo(() => _algorithm.Optimize(A<OptimizedParameterConstraint[]>._, A<Func<IReadOnlyList<OptimizedParameterValue>, OptimizationRunResult>>._))
            .Invokes(x => { _objectiveFunction = x.GetArgument<Func<IReadOnlyList<OptimizedParameterValue>, OptimizationRunResult>>(1); })
            .Returns(new OptimizationRunProperties(5));

         sut.Run(_cancellationToken);
      }

      protected override void Because()
      {
         _objectiveFunction(new[] {new OptimizedParameterValue("P1", 5d, 5d)});
      }

      [Observation]
      public void should_update_the_simulation_with_the_current_values()
      {
         A.CallTo(() => _simModelBatch.UpdateParameterValue(_parameterSelection1.Path, 5 * _parameter1.Value)).MustHaveHappened();
         A.CallTo(() => _simModelBatch.UpdateParameterValue(_parameterSelection2.Path, 5 * _parameter2.Value)).MustHaveHappened();
      }

      [Observation]
      public void should_run_the_simulation()
      {
         A.CallTo(() => _simModelBatch.RunSimulation()).MustHaveHappened();
      }
   }

   public class When_the_parameter_identification_run_is_notified_that_a_run_was_canceled : concern_for_ParameterIdentificationRun
   {
      private ParameterIdentificationRunResult _result;

      protected override void Context()
      {
         base.Context();

         A.CallTo(() => _algorithm.Optimize(A<OptimizedParameterConstraint[]>._, A<Func<IReadOnlyList<OptimizedParameterValue>, OptimizationRunResult>>._))
            .Invokes(x =>
            {
               _cancellationTokenSource.Cancel();
            })
            .Throws(x=> new OperationCanceledException());
      }

      protected override void Because()
      {
         _result = sut.Run(_cancellationToken);
      }

      [Observation]
      public void should_set_the_run_status_of_the_underlying_run_result_to_canceled()
      {
         _result.Status.ShouldBeEqualTo(RunStatus.Canceled);
      }
   }

   public class When_the_parameter_identification_run_is_running_a_run_that_throws_an_exception : concern_for_ParameterIdentificationRun
   {
      private ParameterIdentificationRunResult _result;

      protected override void Context()
      {
         base.Context();

         A.CallTo(() => _algorithm.Optimize(A<OptimizedParameterConstraint[]>._, A<Func<IReadOnlyList<OptimizedParameterValue>, OptimizationRunResult>>._))
            .Throws(x => new ArithmeticException("Math Error"));
      }

      protected override void Because()
      {
         _result = sut.Run(_cancellationToken);
      }

      [Observation]
      public void should_set_the_run_status_of_the_underlying_run_result_to_faulted()
      {
         _result.Status.ShouldBeEqualTo(RunStatus.Faulted);
         _result.Message.ShouldBeEqualTo("Math Error");
      }
   }

   public class When_the_parameter_identification_run_is_running_a_parameter_identification_where_the_jacobian_should_be_calculated : concern_for_ParameterIdentificationRun
   {
      private ParameterIdentificationRunResult _result;
      private JacobianMatrix _jacobianMatrix;

      protected override void Context()
      {
         base.Context();
         _jacobianMatrix = new JacobianMatrix(new[] {"A"});
         _parameterIdentification.Configuration.CalculateJacobian = true;
         A.CallTo(_jacobianMatrixCalculator).WithReturnType<JacobianMatrix>().Returns(_jacobianMatrix);
         A.CallTo(() => _algorithm.Optimize(A<OptimizedParameterConstraint[]>._, A<Func<IReadOnlyList<OptimizedParameterValue>, OptimizationRunResult>>._))
            .Invokes(x =>
            {
               var objectiveFunction = x.GetArgument<Func<IReadOnlyList<OptimizedParameterValue>, OptimizationRunResult>>(1);
               objectiveFunction(new[] {new OptimizedParameterValue("P1", 5d, 5d)});
            });
      }

      protected override void Because()
      {
         _result = sut.Run(_cancellationToken);
      }

      [Observation]
      public void should_calculate_the_jacobian_matrix_and_save_the_result_in_the_returned_run()
      {
         _result.JacobianMatrix.ShouldBeEqualTo(_jacobianMatrix);
      }

      [Observation]
      public void should_initialize_the_sim_model_batch_for_sensitivity()
      {
         A.CallTo(() => _simModelBatch.InitializeForSensitivity()).MustHaveHappened();
      }
   }

   public class When_the_parameter_identification_runner_is_running_a_parameter_identification_with_fixed_parameters : concern_for_ParameterIdentificationRun
   {
      private Func<IReadOnlyList<OptimizedParameterValue>, OptimizationRunResult> _objectiveFunction;
      private IReadOnlyList<OptimizedParameterConstraint> _constraints;
      private IParameter _parameter3;
      private ParameterSelection _parameterSelection3;
      private IdentificationParameter _fixedIdentificationParameter;

      protected override void Context()
      {
         base.Context();

         A.CallTo(_algorithm).WithReturnType<OptimizationRunProperties>()
            .Invokes(x =>
            {
               _constraints = x.GetArgument<IReadOnlyList<OptimizedParameterConstraint>>(0);
               _objectiveFunction = x.GetArgument<Func<IReadOnlyList<OptimizedParameterValue>, OptimizationRunResult>>(1);
            })
            .Returns(A.Fake<OptimizationRunProperties>());

         sut.Run(_cancellationToken);
      }

      protected override void PerformExtraInitializationSteps()
      {
         _parameter3 = A.Fake<IParameter>();
         _parameter3.Dimension = DomainHelperForSpecs.ConcentrationDimensionForSpecs();
         _parameter3.Value = 100;
         _parameterSelection3 = ParameterSelectionFor(_parameter3, "ParameterPath3");

         _fixedIdentificationParameter = DomainHelperForSpecs.IdentificationParameter("Fixed IdParam", min: 0, max: 200, startValue: 60, isFixed: true);
         _fixedIdentificationParameter.AddLinkedParameter(_parameterSelection3);
         _parameterIdentification.AddIdentificationParameter(_fixedIdentificationParameter);
      }

      protected override void Because()
      {
         _objectiveFunction(new[] {new OptimizedParameterValue("A", 100.0, 50d)});
      }

      [Observation]
      public void should_initialize_the_optimization_algorithm_with_all_non_fixed_parameters()
      {
         _constraints.Count.ShouldBeEqualTo(1);
         _constraints[0].Name.ShouldBeEqualTo(_identificationParameter.Name);
      }

      [Observation]
      public void should_initialize_the_batch_simulation_with_the_fixed_value()
      {
         A.CallTo(() => _simModelBatch.UpdateParameterValue(_parameterSelection3.Path, _fixedIdentificationParameter.StartValue)).MustHaveHappened();
      }
   }
}