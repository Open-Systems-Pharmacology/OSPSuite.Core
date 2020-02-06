using System.Collections.Generic;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain.SensitivityAnalyses;
using OSPSuite.R.Domain;
using OSPSuite.Utility;
using OSPSuite.Utility.Container;
using SensitivityAnalysis = OSPSuite.R.Domain.SensitivityAnalysis;

namespace OSPSuite.R.Services
{
   public abstract class concern_for_SensitivityAnalysisTask : ContextForIntegration<ISensitivityAnalysisTask>
   {
      private ISensitivityAnalysisRunner _sensitivityAnalysisRunner;
      protected Simulation _simulation;
      protected SensitivityAnalysis _sensitivityAnalysis;
      protected SensitivityAnalysisRunResult _sensitivityAnalysisRunResult;
      protected IReadOnlyList<PKParameterSensitivity> _allPKParameterSensitivities;

      public override void GlobalContext()
      {
         base.GlobalContext();
         var simulationFile = HelperForSpecs.DataFile("S1.pkml");
         var simulationPersister = Api.GetSimulationPersister();
         _simulation = simulationPersister.LoadSimulation(simulationFile);
         _sensitivityAnalysis = new SensitivityAnalysis(_simulation) {NumberOfSteps = 2, VariationRange = 0.2};
         var containerTask = Api.GetContainerTask();
         var liverVolumes = containerTask.AllParametersMatching(_simulation, "Organism|Liver|Volume");
         _sensitivityAnalysis.AddParameters(liverVolumes);
         _sensitivityAnalysisRunner = Api.GetSensitivityAnalysisRunner();

         _sensitivityAnalysisRunResult = _sensitivityAnalysisRunner.Run(_sensitivityAnalysis);
         _allPKParameterSensitivities = _sensitivityAnalysisRunResult.AllPKParameterSensitivities;
      }

      protected override void Context()
      {
         sut = Api.GetSensitivityAnalysisTask();
      }
   }

   public class When_exporting_the_result_of_a_sensitivity_analysis_for_a_simulation : concern_for_SensitivityAnalysisTask
   {
      private string _csvFile;

      protected override void Context()
      {
         base.Context();
         _csvFile = FileHelper.GenerateTemporaryFileName();
      }

      protected override void Because()
      {
         sut.ExportResultsToCSV(_sensitivityAnalysisRunResult, _simulation, _csvFile);
      }

      [Observation]
      public void should_have_exported_a_file_with_the_sensitivity_results()
      {
         FileHelper.FileExists(_csvFile).ShouldBeTrue();
      }

      public override void Cleanup()
      {
         base.Cleanup();
         FileHelper.DeleteFile(_csvFile);
      }
   }

   public class When_import_the_result_of_a_sensitivity_analysis_for_a_simulation : concern_for_SensitivityAnalysisTask
   {
      private string _csvFile;
      private SensitivityAnalysisRunResult _importedResults;

      protected override void Context()
      {
         base.Context();
         _csvFile = FileHelper.GenerateTemporaryFileName();
      }

      protected override void Because()
      {
         sut.ExportResultsToCSV(_sensitivityAnalysisRunResult, _simulation, _csvFile);
         _importedResults = sut.ImportResultsFromCSV(_simulation, _csvFile);
      }

      [Observation]
      public void should_have_exported_a_file_with_the_sensitivity_results()
      {
         _importedResults.AllPKParameterSensitivities.Count.ShouldBeEqualTo(_allPKParameterSensitivities.Count);
         for (int i = 0; i < _allPKParameterSensitivities.Count; i++)
         {
            _importedResults.AllPKParameterSensitivities[i].PKParameterName.ShouldBeEqualTo(_allPKParameterSensitivities[i].PKParameterName);
            _importedResults.AllPKParameterSensitivities[i].ParameterName.ShouldBeEqualTo(_allPKParameterSensitivities[i].ParameterName);
            _importedResults.AllPKParameterSensitivities[i].QuantityPath.ShouldBeEqualTo(_allPKParameterSensitivities[i].QuantityPath);
            _importedResults.AllPKParameterSensitivities[i].Value.ShouldBeEqualTo(_allPKParameterSensitivities[i].Value, 0.01);
         }
      }

      public override void Cleanup()
      {
         base.Cleanup();
         FileHelper.DeleteFile(_csvFile);
      }
   }
}