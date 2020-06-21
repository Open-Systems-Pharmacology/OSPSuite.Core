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
         sut = new SensitivityAnalysis {Simulation = _oldSimulation};
      }
   }

   public class When_swapping_simulations_for_a_sensitivity_analysis : concern_for_SensitivityAnalysis
   {
      private ISimulation _newSimulation;

      protected override void Context()
      {
         base.Context();
         _newSimulation = A.Fake<ISimulation>();
         var sensitivityParameter = new SensitivityParameter {ParameterSelection = new ParameterSelection(_oldSimulation, A.Fake<QuantitySelection>())};
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
   } }