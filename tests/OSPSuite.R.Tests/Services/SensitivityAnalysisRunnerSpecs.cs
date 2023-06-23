using System.Linq;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.PKAnalyses;
using OSPSuite.Core.Domain.SensitivityAnalyses;
using OSPSuite.R.Domain;
using SensitivityAnalysis = OSPSuite.R.Domain.SensitivityAnalysis;
using SensitivityAnalysisRunOptions = OSPSuite.R.Domain.SensitivityAnalysisRunOptions;

namespace OSPSuite.R.Services
{
   public abstract class concern_for_SensitivityAnalysisRunner : ContextForIntegration<ISensitivityAnalysisRunner>
   {
      protected SensitivityAnalysisRunResult _result;

      protected override void Context()
      {
         sut = Api.GetSensitivityAnalysisRunner();
      }
   }

   public class When_running_a_sensitivity_analysis_for_a_simulation_with_predefined_parameters : concern_for_SensitivityAnalysisRunner
   {
      private SensitivityAnalysis _sensitivityAnalysis;
      private Simulation _simulation;

      public override void GlobalContext()
      {
         base.GlobalContext();
         var simulationFile = HelperForSpecs.DataFile("S1.pkml");
         var simulationPersister = Api.GetSimulationPersister();
         _simulation = simulationPersister.LoadSimulation(simulationFile);
         _sensitivityAnalysis = new SensitivityAnalysis(_simulation) {NumberOfSteps = 2, VariationRange = 0.2};
         var containerTask = Api.GetContainerTask();
         var liverVolumes = containerTask.AllParametersMatching(_simulation, "Organism|Liver|Volume");
         _sensitivityAnalysis.AddParameterPaths(liverVolumes.Select(x => x.ConsolidatedPath()));
      }

      protected override void Because()
      {
         _result = sut.Run(_sensitivityAnalysis);
      }

      [Observation]
      public void should_run_the_simulation_as_expected()
      {
         _result.AllPKParameterSensitivities.Select(x => x.ParameterName).Distinct().ShouldOnlyContain("Liver-Volume");
      }

      [Observation]
      public void should_not_calculate_output_sensitivities()
      {
         _result.AllOutputParameterSensitivities.ShouldBeEmpty();
      }
   }

   public class When_running_a_sensitivity_analysis_for_a_simulation_with_predefined_parameters_and_the_simulation_results_should_also_be_exported : concern_for_SensitivityAnalysisRunner
   {
      private SensitivityAnalysis _sensitivityAnalysis;
      private Simulation _simulation;
      private SensitivityAnalysisRunOptions _runOptions;

      public override void GlobalContext()
      {
         base.GlobalContext();
         var simulationFile = HelperForSpecs.DataFile("S1.pkml");
         var simulationPersister = Api.GetSimulationPersister();
         _simulation = simulationPersister.LoadSimulation(simulationFile);
         _sensitivityAnalysis = new SensitivityAnalysis(_simulation) {NumberOfSteps = 2, VariationRange = 0.2};
         var containerTask = Api.GetContainerTask();
         var liverVolumes = containerTask.AllParametersMatching(_simulation, "Organism|Liver|Volume");
         var parameterPaths = liverVolumes.Select(x => x.ConsolidatedPath()).ToArray();
         _sensitivityAnalysis.AddParameterPaths(parameterPaths);
         _runOptions = new SensitivityAnalysisRunOptions {ReturnOutputValues = true};
      }

      protected override void Because()
      {
         _result = sut.Run(_sensitivityAnalysis, _runOptions);
      }

      [Observation]
      public void should_run_the_simulation_as_expected()
      {
         _result.AllPKParameterSensitivities.Select(x => x.ParameterName).Distinct().ShouldOnlyContain("Liver-Volume");
      }

      [Observation]
      public void should_have_calculated_output_sensitivities()
      {
         _result.AllOutputParameterSensitivities.ShouldNotBeEmpty();
         foreach (var outputPath in _result.AllQuantityPaths)
         {
            var outputParameterSensitivity = _result.AllOutputParameterSensitivitiesBySensitivityParameterName(outputPath, "Liver-Volume");
            outputParameterSensitivity.Count().ShouldBeEqualTo(4);
         }
      }
   }

   public class When_running_a_sensitivity_analysis_for_a_simulation_with_predefined_parameters_and_dynamic_pk_parameters : concern_for_SensitivityAnalysisRunner
   {
      private SensitivityAnalysis _sensitivityAnalysis;
      private Simulation _simulation;
      private UserDefinedPKParameter _userDefinedPKParameter;
      private UserDefinedPKParameter _userDefinedCLParameter;

      public override void GlobalContext()
      {
         base.GlobalContext();
         var simulationFile = HelperForSpecs.DataFile("S1.pkml");
         var simulationPersister = Api.GetSimulationPersister();
         var pkParametersTask = Api.GetPKParameterTask();
         _simulation = simulationPersister.LoadSimulation(simulationFile);
         _sensitivityAnalysis = new SensitivityAnalysis(_simulation) {NumberOfSteps = 2, VariationRange = 0.2};

         //Should calculate CMax/100
         _userDefinedPKParameter = new UserDefinedPKParameter {Name = "Toto", NormalizationFactor = 100, StandardPKParameter = StandardPKParameter.C_max};
         pkParametersTask.AddUserDefinedPKParameter(_userDefinedPKParameter);

         _userDefinedCLParameter = new UserDefinedPKParameter {Name = "MyCL", StandardPKParameter = StandardPKParameter.CL};
         pkParametersTask.AddUserDefinedPKParameter(_userDefinedCLParameter);

         var containerTask = Api.GetContainerTask();
         var liverVolumes = containerTask.AllParametersMatching(_simulation, "Organism|Liver|Volume");
         _sensitivityAnalysis.AddParameterPaths(liverVolumes.Select(x => x.ConsolidatedPath()));
      }

      protected override void Because()
      {
         _result = sut.Run(_sensitivityAnalysis);
      }

      [Observation]
      public void should_run_the_simulation_as_expected_and_return_results_for_dynamic_parameters()
      {
         var allPKParameterSensitivities = _result.AllPKParameterSensitivities.Where(x => x.PKParameterName == _userDefinedPKParameter.Name);
         allPKParameterSensitivities.Any().ShouldBeTrue();

         var allPKParameterCLSensitivities = _result.AllPKParameterSensitivities.Where(x => x.PKParameterName == _userDefinedCLParameter.Name);
         allPKParameterCLSensitivities.Any().ShouldBeTrue();
      }
   }

   public class When_running_a_sensitivity_analysis_for_a_simulation_without_predefined_parameters : concern_for_SensitivityAnalysisRunner
   {
      private SensitivityAnalysis _sensitivityAnalysis;
      private Simulation _simulation;

      public override void GlobalContext()
      {
         base.GlobalContext();
         var simulationFile = HelperForSpecs.DataFile("simple.pkml");
         var simulationPersister = Api.GetSimulationPersister();
         _simulation = simulationPersister.LoadSimulation(simulationFile);
         _sensitivityAnalysis = new SensitivityAnalysis(_simulation) {NumberOfSteps = 1, VariationRange = 0.2};
      }

      protected override void Because()
      {
         _result = sut.Run(_sensitivityAnalysis);
      }

      [Observation]
      public void should_run_the_simulation_as_expected()
      {
         var pkAnalysis = _result.AllPKParameterSensitivitiesFor("C_max", "Organism|Liver|A", 1);
         pkAnalysis.Count.ShouldBeGreaterThan(0);
      }
   }
}