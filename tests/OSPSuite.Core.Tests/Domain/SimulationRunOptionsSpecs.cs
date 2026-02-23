using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain;

namespace OSPSuite.Core.Tests.Domain
{
   public abstract class concern_for_SimulationRunOptions : ContextSpecification<SimulationRunOptions>
   {
      protected override void Context()
      {
         sut = new SimulationRunOptions();
      }
   }

   public class When_creating_simulation_run_options_with_default_values : concern_for_SimulationRunOptions
   {
      [Observation]
      public void should_enable_check_for_negative_values_by_default()
      {
         sut.CheckForNegativeValues.ShouldBeTrue();
      }

      [Observation]
      public void should_disable_auto_reduce_tolerances_by_default()
      {
         sut.AutoReduceTolerances.ShouldBeFalse();
      }
   }
}
