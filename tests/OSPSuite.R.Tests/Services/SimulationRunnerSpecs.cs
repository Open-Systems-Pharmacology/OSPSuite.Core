using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FakeItEasy;
using NUnit.Framework;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Domain.Populations;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Helpers;
using OSPSuite.R.Domain;
using OSPSuite.SimModel;
using OSPSuite.Utility.Events;
using OSPSuite.Utility.Exceptions;
using RSimulation = OSPSuite.R.Domain.Simulation;
using SimulationRunOptions = OSPSuite.R.Domain.SimulationRunOptions;

namespace OSPSuite.R.Services
{
   public abstract class concern_for_SimulationRunner : ContextSpecification<ISimulationRunner>
   {
      protected ISimModelManager _simModelManager;
      protected ISimulationResultsCreator _simulationResultsCreator;
      protected ISimulationPersistableUpdater _simulationPersitableUpdater;
      protected IPopulationRunner _populationRunner;
      protected IPopulationTask _populationTask;
      protected IProgressManager _progressManager;
      protected IConcurrencyManager _concurrencyManager;
      protected ISimulationRunner _individualRunner;

      protected override void Context()
      {
         _simModelManager = A.Fake<ISimModelManager>();
         _simulationPersitableUpdater = A.Fake<ISimulationPersistableUpdater>();
         _populationRunner = A.Fake<IPopulationRunner>();
         _populationTask = A.Fake<IPopulationTask>();
         _progressManager = A.Fake<IProgressManager>();
         _concurrencyManager = new ConcurrencyManager(A.Fake<IObjectTypeResolver>());
         _individualRunner = A.Fake<ISimulationRunner>();
         A.CallTo(() => _progressManager.Create()).Returns(A.Fake<IProgressUpdater>());
         _simulationResultsCreator = new SimulationResultsCreator();
         sut = new SimulationRunner(_simModelManager, _populationRunner, _simulationResultsCreator, _simulationPersitableUpdater, _populationTask,
            _progressManager, _concurrencyManager, () => _individualRunner);
      }

      protected static RSimulation PopulationSimulationWithId(string id, params int[] individualIds) =>
         new RSimulation(new ModelCoreSimulation { Id = id })
         {
            IndividualValuesCache = new IndividualValuesCache(new ParameterValuesCache(), new CovariateValuesCache(), individualIds.ToList())
         };
   }

   public class When_running_a_simulation : concern_for_SimulationRunner
   {
      private IModelCoreSimulation _simulation;
      private SimulationResults _results;
      private SimulationRunResults _simulationRunResults;

      protected override void Context()
      {
         base.Context();
         _simulationRunResults = new SimulationRunResults(Enumerable.Empty<SolverWarning>(),
            DomainHelperForSpecs.IndividualSimulationDataRepositoryFor("Sim"));
         _simulation = new ModelCoreSimulation();
         addConfigurationWithSolverSettings();
         A.CallTo(_simModelManager).WithReturnType<Task<SimulationRunResults>>().Returns(_simulationRunResults);
      }
      private void addConfigurationWithSolverSettings()
      {
         _simulation.Configuration = new SimulationConfiguration();
         _simulation.Configuration.SimulationSettings = new SimulationSettings();
         _simulation.Configuration.SimulationSettings.Solver = new SolverSettings();
         var parameter = new Parameter
         {
            Name = Constants.Parameters.CHECK_FOR_NEGATIVE_VALUES,
            Value = 1,
            GroupName = Constants.Groups.SOLVER_SETTINGS,
            BuildingBlockType = PKSimBuildingBlockType.Simulation,
            CanBeVaried = false,
            CanBeVariedInPopulation = false,
            Visible = true,
            Editable = true,
            IsDefault = true
         };
         _simulation.Configuration.SimulationSettings.Solver.Add(parameter);
      }

      protected override void Because()
      {
         _results = sut.Run(new SimulationRunArgs { Simulation = _simulation });
      }

      [Test]
      public void should_update_the_persistable_flag_in_the_simulation_based_on_the_simulation_settings()
      {
         A.CallTo(() => _simulationPersitableUpdater.UpdateSimulationPersistable(_simulation)).MustHaveHappened();
      }

      [Observation]
      public void should_return_results_for_the_expected_outputs()
      {
         _results.AllIndividualResults.Count.ShouldBeEqualTo(1);
         _results.AllIndividualResults.ElementAt(0).AllValues.Count.ShouldBeEqualTo(1);
      }
   }

   public class When_running_a_population_simulation : concern_for_SimulationRunner
   {
      private IModelCoreSimulation _simulation;
      private IndividualValuesCache _population;
      private DataTable _populationData;
      private SimulationResults _results;
      private SimulationRunOptions _simulationRunOptions;

      protected override void Context()
      {
         base.Context();
         _simulation = new ModelCoreSimulation();
         _population = new IndividualValuesCache();
         _populationData = new DataTable();
         _simulationRunOptions = new SimulationRunOptions();
         A.CallTo(() => _populationTask.PopulationTableFrom(_population, _simulation)).Returns(_populationData);
      }

      protected override void Because()
      {
         _results = sut.Run(new SimulationRunArgs { Simulation = _simulation, Population = _population, SimulationRunOptions = _simulationRunOptions});
      }

      [Observation]
      public void should_update_the_persistable_flag_in_the_simulation_based_on_the_simulation_settings()
      {
         A.CallTo(() => _simulationPersitableUpdater.UpdateSimulationPersistable(_simulation)).MustHaveHappened();
      }

      [Observation]
      public void should_run_the_simulation_using_the_population_data()
      {
         A.CallTo(() => _populationRunner.RunPopulationAsync(_simulation, _simulationRunOptions, _populationData, null, null, CancellationToken.None)).MustHaveHappened();
      }
   }

   public class When_running_a_population_simulation_with_aging_data : concern_for_SimulationRunner
   {
      private IModelCoreSimulation _simulation;
      private IndividualValuesCache _population;
      private DataTable _populationData;
      private SimulationResults _results;
      private SimulationRunOptions _simulationRunOptions;
      private AgingData _agingData;

      protected override void Context()
      {
         base.Context();
         _simulation = new ModelCoreSimulation();
         _population = new IndividualValuesCache();
         _populationData = new DataTable();
         _simulationRunOptions = new SimulationRunOptions();
         _agingData = new AgingData
         {
            IndividualIds = new[] {0, 1},
            ParameterPaths = new[] {"Organism|Liver|Volume", "Organism|Liver|Volume"},
            Times = new[] {10, 20.0},
            Values = new[] {4.0, 5.0},
         };

         A.CallTo(() => _populationTask.PopulationTableFrom(_population, _simulation)).Returns(_populationData);
      }

      protected override void Because()
      {
         _results = sut.Run(new SimulationRunArgs { Simulation = _simulation, Population = _population, SimulationRunOptions = _simulationRunOptions, AgingData = _agingData});
      }

      [Observation]
      public void should_update_the_persistable_flag_in_the_simulation_based_on_the_simulation_settings()
      {
         A.CallTo(() => _simulationPersitableUpdater.UpdateSimulationPersistable(_simulation)).MustHaveHappened();
      }

      [Observation]
      public void should_run_the_simulation_using_the_population_data()
      {
         A.CallTo(() => _populationRunner.RunPopulationAsync(_simulation, _simulationRunOptions, _populationData, A<DataTable>._, null, CancellationToken.None))
            .MustHaveHappened();
      }
   }

   public class When_running_a_mixed_list_of_individual_and_population_simulations : concern_for_SimulationRunner
   {
      private RSimulation _individual;
      private RSimulation _population;
      private SimulationResults _individualResults;
      private SimulationResults _populationResults;
      private ConcurrencyManagerResult<SimulationResults>[] _results;

      protected override void Context()
      {
         base.Context();
         _individual = new RSimulation(new ModelCoreSimulation { Id = "individual" });
         _population = PopulationSimulationWithId("population", 0, 1);

         _individualResults = new SimulationResults();
         A.CallTo(() => _individualRunner.Run(A<SimulationRunArgs>._)).Returns(_individualResults);

         var populationRunResults = new PopulationRunResults();
         _populationResults = populationRunResults.Results;
         A.CallTo(() => _populationRunner.RunPopulationAsync(_population.CoreSimulation, A<RunOptions>._, A<DataTable>._, A<DataTable>._, A<DataTable>._, A<CancellationToken>._))
            .Returns(Task.FromResult(populationRunResults));
      }

      protected override void Because()
      {
         _results = sut.RunSimulations(new SimulationRunOptions(), _individual, _population);
      }

      [Observation]
      public void should_return_one_result_per_simulation()
      {
         _results.Length.ShouldBeEqualTo(2);
      }

      [Observation]
      public void should_run_the_individual_through_a_freshly_resolved_runner()
      {
         var result = _results.Single(x => x.Id == "individual");
         result.Succeeded.ShouldBeTrue();
         result.Result.ShouldBeEqualTo(_individualResults);
      }

      [Observation]
      public void should_run_the_population_through_the_population_runner()
      {
         A.CallTo(() => _populationRunner.RunPopulationAsync(_population.CoreSimulation, A<RunOptions>._, A<DataTable>._, A<DataTable>._, A<DataTable>._, A<CancellationToken>._))
            .MustHaveHappened();
         var result = _results.Single(x => x.Id == "population");
         result.Succeeded.ShouldBeTrue();
         result.Result.ShouldBeEqualTo(_populationResults);
      }
   }

   public class When_running_a_population_simulation_with_aging_data_through_run_simulations : concern_for_SimulationRunner
   {
      private RSimulation _population;
      private ConcurrencyManagerResult<SimulationResults>[] _results;

      protected override void Context()
      {
         base.Context();
         _population = PopulationSimulationWithId("aging", 0, 1);
         _population.AgingData = new AgingData
         {
            IndividualIds = new[] { 0, 1 },
            ParameterPaths = new[] { "Organism|Liver|Volume", "Organism|Liver|Volume" },
            Times = new[] { 10.0, 20.0 },
            Values = new[] { 4.0, 5.0 },
         };
         A.CallTo(() => _populationRunner.RunPopulationAsync(_population.CoreSimulation, A<RunOptions>._, A<DataTable>._, A<DataTable>._, A<DataTable>._, A<CancellationToken>._))
            .Returns(Task.FromResult(new PopulationRunResults()));
      }

      protected override void Because()
      {
         _results = sut.RunSimulations(new SimulationRunOptions(), _population);
      }

      [Observation]
      public void should_forward_the_aging_data_to_the_population_runner()
      {
         A.CallTo(() => _populationRunner.RunPopulationAsync(_population.CoreSimulation, A<RunOptions>._, A<DataTable>._, A<DataTable>.That.Not.IsNull(), A<DataTable>._, A<CancellationToken>._))
            .MustHaveHappened();
      }
   }

   public class When_one_population_in_the_list_fails : concern_for_SimulationRunner
   {
      private RSimulation _goodPopulation;
      private RSimulation _badPopulation;
      private ConcurrencyManagerResult<SimulationResults>[] _results;

      protected override void Context()
      {
         base.Context();
         _goodPopulation = PopulationSimulationWithId("good", 0, 1);
         _badPopulation = PopulationSimulationWithId("bad", 0, 1);

         A.CallTo(() => _populationRunner.RunPopulationAsync(_goodPopulation.CoreSimulation, A<RunOptions>._, A<DataTable>._, A<DataTable>._, A<DataTable>._, A<CancellationToken>._))
            .Returns(Task.FromResult(new PopulationRunResults()));
         A.CallTo(() => _populationRunner.RunPopulationAsync(_badPopulation.CoreSimulation, A<RunOptions>._, A<DataTable>._, A<DataTable>._, A<DataTable>._, A<CancellationToken>._))
            .Throws(new OSPSuiteException("the population blew up"));
      }

      protected override void Because()
      {
         _results = sut.RunSimulations(new SimulationRunOptions(), _goodPopulation, _badPopulation);
      }

      [Observation]
      public void should_isolate_the_failure_and_still_complete_the_other_population()
      {
         _results.Single(x => x.Id == "good").Succeeded.ShouldBeTrue();
         _results.Single(x => x.Id == "bad").Succeeded.ShouldBeFalse();
      }
   }
}