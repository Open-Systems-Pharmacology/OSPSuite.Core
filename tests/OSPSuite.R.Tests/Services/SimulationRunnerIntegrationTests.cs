using System.IO;
using System.Linq;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Domain.Populations;
using OSPSuite.R.Domain;
using OSPSuite.Utility;

namespace OSPSuite.R.Services
{
   public abstract class concern_for_SimulationRunnerIntegration : ContextForIntegration<ISimulationRunner>
   {
      protected string _populationFile;
      protected ISimulationPersister _simulationPersister;
      protected string _simulationFile;
      protected string _populationFileWithUnitInParameterName;
      protected Simulation _simulation;
      protected IPopulationTask _populationTask;

      public override void GlobalContext()
      {
         base.GlobalContext();
         _populationFile = HelperForSpecs.DataFile("pop_10.csv");
         _simulationFile = HelperForSpecs.DataFile("S1.pkml");
         _simulationPersister = Api.GetSimulationPersister();
         _populationTask = Api.GetPopulationTask();
         sut = Api.GetSimulationRunner();

         _simulation = _simulationPersister.LoadSimulation(_simulationFile);
      }
   }

   public class When_creating_a_simulation_with_a_file : concern_for_SimulationRunnerIntegration
   {
      [Observation]
      public void should_create_the_solver_settings_with_check_for_negative_values()
      {
         _simulation.Settings.Solver.CheckForNegativeValues.ShouldNotBeNull();
      }
   }

   public class
      When_performing_a_population_simulation_run_with_a_file_containing_only_a_subset_of_the_individual : concern_for_SimulationRunnerIntegration
   {
      private string _outputFolder;
      private string _subPopulationFile;
      private IndividualValuesCache _subPopulation;
      private SimulationResults _results;

      protected override void Context()
      {
         base.Context();
         var tmpFile = FileHelper.GenerateTemporaryFileName();
         _outputFolder = new FileInfo(tmpFile).DirectoryName;
         //Take the 3 out of 5 which would have indices 6 and 7 
         _subPopulationFile = _populationTask.SplitPopulation(_populationFile, 5, _outputFolder, "TestSplit")[3];
         _subPopulation = _populationTask.ImportPopulation(_subPopulationFile);
      }

      protected override void Because()
      {
         _results = sut.Run(new SimulationRunArgs { Simulation = _simulation, Population = _subPopulation });
      }

      [Observation]
      public void should_create_results_matching_the_individual_ids_in_the_population()
      {
         _results.AllIndividualIds().ShouldOnlyContain(6, 7);
      }
   }

   public class When_performing_a_population_simulation_run : concern_for_SimulationRunnerIntegration
   {
      private IndividualValuesCache _population;
      private SimulationResults _results;

      protected override void Context()
      {
         base.Context();
         _population = _populationTask.ImportPopulation(_populationFile);
      }

      protected override void Because()
      {
         _results = sut.Run(new SimulationRunArgs { Simulation = _simulation, Population = _population });
      }

      [Observation]
      public void should_create_results_matching_the_individual_ids_in_the_population()
      {
         _results.AllIndividualIds().ShouldOnlyContain(Enumerable.Range(0, _population.Count));
      }
   }

   public class When_performing_a_population_simulation_run_after_renaming : concern_for_SimulationRunnerIntegration
   {
      private IndividualValuesCache _population;
      private SimulationResults _results;
      private readonly string _newSimulationName = "New Name";

      protected override void Context()
      {
         base.Context();
         _population = _populationTask.ImportPopulation(_populationFile);
      }

      protected override void Because()
      {
         _simulation.Name = _newSimulationName;
         _results = sut.Run(new SimulationRunArgs { Simulation = _simulation, Population = _population });
      }

      [Observation]
      public void should_return_paths_with_new_simulation_name()
      {
         var paths = _results.AllQuantityPaths();
         paths.Any(x => x.ToString().Contains(_newSimulationName)).ShouldBeFalse();
      }
   }
}