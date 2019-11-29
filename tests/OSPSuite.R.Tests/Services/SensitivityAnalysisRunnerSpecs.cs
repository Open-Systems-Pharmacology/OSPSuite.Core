using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.R.Domain;
using OSPSuite.Utility.Container;

namespace OSPSuite.R.Services
{
   public abstract class concern_for_SensitivityAnalysisRunner : ContextForIntegration<ISensitivityAnalysisRunner>
   {
      protected override void Context()
      {
         sut = IoC.Resolve<ISensitivityAnalysisRunner>();
      }
   }

   public class When_running_a_sensitivity_analysis_for_a_simulation_with_predefined_parameters : concern_for_SensitivityAnalysisRunner
   {
      private SensitivityAnalysis _sensitivityAnalysis;
      private Simulation _simulation;
      private Core.Domain.SensitivityAnalyses.SensitivityAnalysis _result;

      public override void GlobalContext()
      {
         base.GlobalContext();
         var simulationFile = HelperForSpecs.DataFile("S1.pkml");
         var simulationPersister = IoC.Resolve<ISimulationPersister>();
         _simulation = simulationPersister.LoadSimulation(simulationFile);
         _sensitivityAnalysis = new SensitivityAnalysis(_simulation) {NumberOfSteps = 2, VariationRange = 0.2};
         var containerTask = IoC.Resolve<IContainerTask>();
         var liverVolumes = containerTask.AllParametersMatching(_simulation, "Organism|Liver|Volume");
         _sensitivityAnalysis.AddParameters(liverVolumes);
      }

      protected override void Because()
      {
         _result = sut.Run(_sensitivityAnalysis);
      }

      [Observation]
      public void should_run_the_simulation_as_expected()
      {
         _result.AllSensitivityParameterPaths.ShouldOnlyContain("Organism|Liver|Volume");
      }
   }

   public class When_running_a_sensitivity_analysis_for_a_simulation_without_predefined_parameters : concern_for_SensitivityAnalysisRunner
   {
      private SensitivityAnalysis _sensitivityAnalysis;
      private Simulation _simulation;
      private Core.Domain.SensitivityAnalyses.SensitivityAnalysis _result;

      public override void GlobalContext()
      {
         base.GlobalContext();
         var simulationFile = HelperForSpecs.DataFile("simple.pkml");
         var simulationPersister = IoC.Resolve<ISimulationPersister>();
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
         _result.AllSensitivityParameterPaths.Count.ShouldBeGreaterThan(0);

         var pkAnalysis = _result.AllPKParameterSensitivitiesFor("AUC", "Simple|Organism|Liver|A|Concentration", 1);
      }
   }
}