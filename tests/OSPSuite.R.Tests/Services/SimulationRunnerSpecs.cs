using System.Data;
using System.Linq;
using FakeItEasy;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Domain.Populations;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Helpers;

namespace OSPSuite.R.Services
{
   public abstract class concern_for_SimulationRunner : ContextSpecification<ISimulationRunner>
   {
      protected ISimModelManager _simModelManager;
      protected ISimulationResultsCreator _simulationResultsCreator;
      protected ISimulationPersistableUpdater _simulationPersitableUpdater;
      protected IPopulationRunner _populationRunner;
      protected IPopulationTask _populationTask;

      protected override void Context()
      {
         _simModelManager = A.Fake<ISimModelManager>();
         _simulationPersitableUpdater = A.Fake<ISimulationPersistableUpdater>();
         _populationRunner = A.Fake<IPopulationRunner>();
         _populationTask = A.Fake<IPopulationTask>();
         _simulationResultsCreator = new SimulationResultsCreator();
         sut = new SimulationRunner(_simModelManager, _populationRunner, _simulationResultsCreator, _simulationPersitableUpdater, _populationTask);
      }
   }

   public class When_running_a_simulation : concern_for_SimulationRunner
   {
      private IModelCoreSimulation _simulation;
      private SimulationResults _results;
      private SimulationRunResults _simulationRunResults;

      protected override void Context()
      {
         base.Context();
         _simulationRunResults = new SimulationRunResults(true, Enumerable.Empty<SolverWarning>(), DomainHelperForSpecs.IndividualSimulationDataRepositoryFor("Sim"));
         _simulation = new ModelCoreSimulation();
         A.CallTo(_simModelManager).WithReturnType<SimulationRunResults>().Returns(_simulationRunResults);
      }

      protected override void Because()
      {
         _results = sut.RunSimulation(_simulation);
      }

      [Observation]
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

   public class When_returning_a_population_simulation : concern_for_SimulationRunner
   {
      private IModelCoreSimulation _simulation;
      private IndividualValuesCache _population;
      private DataTable _populationData;
      private SimulationResults _results;

      protected override void Context()
      {
         base.Context();
         _simulation = new ModelCoreSimulation();
         _population = new IndividualValuesCache();
         _populationData = new DataTable();
         A.CallTo(() => _populationTask.PopulationTableFrom(_population)).Returns(_populationData);
      }

      protected override void Because()
      {
         _results = sut.RunSimulation(_simulation, _population);
      }

      [Observation]
      public void should_update_the_persistable_flag_in_the_simulation_based_on_the_simulation_settings()
      {
         A.CallTo(() => _simulationPersitableUpdater.UpdateSimulationPersistable(_simulation)).MustHaveHappened();
      }

      [Observation]
      public void should_run_the_simulation_using_the_population_data()
      {
         A.CallTo(() => _populationRunner.RunPopulationAsync(_simulation, _populationData, null, null)).MustHaveHappened();
      }
   }
}