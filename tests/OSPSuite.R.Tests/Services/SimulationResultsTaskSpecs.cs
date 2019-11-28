using System;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Utility;
using OSPSuite.Utility.Container;

namespace OSPSuite.R.Services
{
   public abstract class concern_for_SimulationResultsTask : ContextForIntegration<ISimulationResultsTask>
   {
      protected IModelCoreSimulation _simulation;
      protected ISimulationPersister _simulationPersister;

      protected override void Context()
      {
         sut = IoC.Resolve<ISimulationResultsTask>();
         _simulationPersister = IoC.Resolve<ISimulationPersister>();
         var simulationFile = HelperForSpecs.DataFile("S1.pkml");
         _simulation = _simulationPersister.LoadSimulation(simulationFile);
      }
   }

   public class When_importing_simulation_results_from_a_valid_results_file : concern_for_SimulationResultsTask
   {
      private string _resultsFile;
      private SimulationResults _results;

      protected override void Context()
      {
         base.Context();
         _resultsFile = HelperForSpecs.DataFile("res_10.csv");
      }

      protected override void Because()
      {
         _results = sut.ImportResultsFromCSV(_simulation, _resultsFile);
      }

      [Observation]
      public void should_return_the_expected_simulation_results()
      {
         _results.Count.ShouldBeEqualTo(10);
      }
   }

   public class When_importing_simulation_results_from_an_invalid_results_file : concern_for_SimulationResultsTask
   {
      private string _junkFile;

      protected override void Context()
      {
         base.Context();
         _junkFile = HelperForSpecs.DataFile("pop_10.csv");
      }

      [Observation]
      public void should_throw_an_exception()
      {
         The.Action(() => sut.ImportResultsFromCSV(_simulation, _junkFile)).ShouldThrowAn<Exception>();
      }
   }

   public class When_exporting_simulation_results_to_file : concern_for_SimulationResultsTask
   {
      private SimulationResults _results;
      private string _csvFile;

      protected override void Context()
      {
         base.Context();
         var simulationRunner = IoC.Resolve<ISimulationRunner>();
         _results = simulationRunner.Run(_simulation);
         _csvFile = FileHelper.GenerateTemporaryFileName();
      }

      protected override void Because()
      {
         sut.ExportResultsToCSV(_results, _simulation, _csvFile);
      }

      [Observation]
      public void should_have_crated_a_file_with_the_exported_results()
      {
         FileHelper.FileExists(_csvFile).ShouldBeTrue();
      }

      public override void Cleanup()
      {
         base.Cleanup();
         FileHelper.DeleteFile(_csvFile);
      }
   }


   public class When_exporting_simulation_results_to_file_for_a_simulation_without_outputs : concern_for_SimulationResultsTask
   {
      private SimulationResults _results;
      private string _csvFile;

      protected override void Context()
      {
         base.Context();
         var simulationFile = HelperForSpecs.DataFile("Simple.pkml");
         _simulation = _simulationPersister.LoadSimulation(simulationFile);

         var simulationRunner = IoC.Resolve<ISimulationRunner>();
         _results = simulationRunner.Run(_simulation);
         _csvFile = FileHelper.GenerateTemporaryFileName();
      }

      protected override void Because()
      {
         sut.ExportResultsToCSV(_results, _simulation, _csvFile);
      }

      [Observation]
      public void should_have_crated_a_file_with_the_exported_results()
      {
         FileHelper.FileExists(_csvFile).ShouldBeTrue();
      }

      public override void Cleanup()
      {
         base.Cleanup();
         FileHelper.DeleteFile(_csvFile);
      }
   }
}