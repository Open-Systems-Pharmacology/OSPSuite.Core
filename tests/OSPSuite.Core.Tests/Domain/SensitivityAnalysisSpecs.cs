using System.Collections.Generic;
using System.Linq;
using FakeItEasy;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain.SensitivityAnalyses;

namespace OSPSuite.Core.Domain
{
   public abstract class concern_for_SensitivityAnalysis : ContextSpecification<SensitivityAnalysis>
   {
      protected ISimulation _oldSimulation;

      protected override void Context()
      {
         _oldSimulation = A.Fake<ISimulation>();
         sut = new SensitivityAnalysis { Simulation = _oldSimulation };
      }
   }

   public class When_swapping_simulations_for_a_sensitivity_analysis : concern_for_SensitivityAnalysis
   {
      private ISimulation _newSimulation;

      protected override void Context()
      {
         base.Context();
         _newSimulation = A.Fake<ISimulation>();
         var sensitivityParameter = new SensitivityParameter { ParameterSelection = new ParameterSelection(_oldSimulation, A.Fake<QuantitySelection>()) };
         sut.AddSensitivityParameter(sensitivityParameter);
      }

      protected override void Because()
      {
         sut.SwapSimulations(_oldSimulation, _newSimulation);
      }

      [Observation]
      public void the_simulation_referenced_by_the_analysis_should_be_updated()
      {
         sut.Simulation.ShouldBeEqualTo(_newSimulation);
      }

      [Observation]
      public void the_simulation_references_in_sensitivity_parameters_must_be_updated()
      {
         sut.AllSensitivityParameters.All(parameter => Equals(parameter.ParameterSelection.Simulation, _newSimulation)).ShouldBeTrue();
      }
   }


   public class When_retrieving_the_pk_parameter_sensitivty_analysis_covering_a_given_total_sensitivity : concern_for_SensitivityAnalysis
   {
      private IReadOnlyList<PKParameterSensitivity> _result;
      private string _pkParameterName;
      private string _outputPath;
      private PKParameterSensitivity _pk1;
      private PKParameterSensitivity _pk2;
      private PKParameterSensitivity _pk3;

      protected override void Context()
      {
         base.Context();
         _pkParameterName = "AUC";
         _outputPath = "Output";
         sut.Results = new SensitivityAnalysisRunResult();
         _pk1 = new PKParameterSensitivity {PKParameterName = _pkParameterName, QuantityPath = _outputPath, Value = 0.4};
         _pk2 = new PKParameterSensitivity {PKParameterName = _pkParameterName, QuantityPath = _outputPath, Value = 0.1};
         _pk3 = new PKParameterSensitivity {PKParameterName = _pkParameterName, QuantityPath = _outputPath, Value = -0.6};
         sut.Results.AddPKParameterSensitivity(_pk1);
         sut.Results.AddPKParameterSensitivity(_pk2);
         sut.Results.AddPKParameterSensitivity(_pk3);
      }

      protected override void Because()
      {
         _result = sut.AllPKParameterSensitivitiesFor(_pkParameterName, _outputPath, 0.7);
      }

      [Observation]
      public void should_return_the_expected_parameters()
      {
         _result.ShouldOnlyContainInOrder(_pk3,_pk1);
      }
   }
}
