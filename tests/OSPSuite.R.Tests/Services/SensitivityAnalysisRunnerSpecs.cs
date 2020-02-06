using System.Linq;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.SensitivityAnalyses;
using OSPSuite.R.Domain;
using OSPSuite.Utility.Container;
using SensitivityAnalysis = OSPSuite.R.Domain.SensitivityAnalysis;

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
         _result.AllPKParameterSensitivities.Select(x=>x.ParameterName).Distinct().ShouldOnlyContain("Organism-Liver-Volume");
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