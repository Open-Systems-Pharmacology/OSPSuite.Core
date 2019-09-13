using FakeItEasy;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Utility.Container;

namespace OSPSuite.R.Services
{
   public abstract class concern_for_SimulationResultsTask : ContextForIntegration<ISimulationResultsTask>
   {
      protected override void Context()
      {
         sut = IoC.Resolve<ISimulationResultsTask>();
      }
   }

   public class When_importing_simulation_results_from_a_valid_results_file : concern_for_SimulationResultsTask
   {
      [Observation]
      public void should_return_the_expected_simulation_results()
      {
         
      }
   }
}