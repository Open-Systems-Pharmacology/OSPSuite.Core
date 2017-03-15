using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using FakeItEasy;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.SensitivityAnalyses;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Domain.Services.SensitivityAnalyses;

namespace OSPSuite.Core
{
   public abstract class concern_for_SensitivityAnalysisSimulationSwapCorrector : ContextSpecification<SensitivityAnalysisSimulationSwapCorrector>
   {
      protected SensitivityAnalysis _sensitivityAnalysis;
      protected ISimulation _oldSimulation;
      protected ISimulation _newSimulation;
      protected ISimulationQuantitySelectionFinder _simulationQuantitySelectionFinder;
      protected SensitivityParameter _validSensitivityParameter;
      protected SensitivityParameter _invalidSensitivityParameter;

      protected override void Context()
      {
         _oldSimulation = A.Fake<ISimulation>();
         _sensitivityAnalysis = new SensitivityAnalysis { Simulation = _oldSimulation };
         _newSimulation = A.Fake<ISimulation>();

         _simulationQuantitySelectionFinder = A.Fake<ISimulationQuantitySelectionFinder>();
         sut = new SensitivityAnalysisSimulationSwapCorrector(_simulationQuantitySelectionFinder);

         _validSensitivityParameter = new SensitivityParameter { ParameterSelection = new ParameterSelection(_oldSimulation, "valid") };
         _sensitivityAnalysis.AddSensitivityParameter(_validSensitivityParameter);
         _invalidSensitivityParameter = new SensitivityParameter { ParameterSelection = new ParameterSelection(_oldSimulation, "invalid") };
         _sensitivityAnalysis.AddSensitivityParameter(_invalidSensitivityParameter);

         A.CallTo(() => _simulationQuantitySelectionFinder.SimulationHasSelection(_invalidSensitivityParameter.ParameterSelection, _newSimulation)).Returns(false);
         A.CallTo(() => _simulationQuantitySelectionFinder.SimulationHasSelection(_validSensitivityParameter.ParameterSelection, _newSimulation)).Returns(true);
      }
   }

   public class When_correcting_a_sensitivity_analysis_after_swap : concern_for_SensitivityAnalysisSimulationSwapCorrector
   {
      protected override void Because()
      {
         sut.CorrectSensitivityAnalysis(_sensitivityAnalysis, _oldSimulation, _newSimulation);
      }

      [Observation]
      public void the_missing_parameter_selections_are_removed_from_the_sensitivity_analysis()
      {
         _sensitivityAnalysis.AllSensitivityParameters.ShouldContain(_validSensitivityParameter);
         _sensitivityAnalysis.AllSensitivityParameters.ShouldNotContain(_invalidSensitivityParameter);
      }
   }
}
