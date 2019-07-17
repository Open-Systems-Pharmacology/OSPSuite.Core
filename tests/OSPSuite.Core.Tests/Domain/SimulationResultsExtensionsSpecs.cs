using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain.Data;

namespace OSPSuite.Core.Domain
{
   public abstract class concern_for_SimulationResultsExtensions : StaticContextSpecification
   {
     
   }

   public class When_checking_if_a_simulation_result_is_null : concern_for_SimulationResultsExtensions
   {
      [Observation]
      public void should_return_null_if_the_simulation_result_is_null()
      {
         SimulationResults res = null;
         res.IsNull().ShouldBeTrue();
      }

      [Observation]
      public void should_return_null_if_the_simulation_result_is_a_null_simulation_results()
      {
         SimulationResults res = new NullSimulationResults();
         res.IsNull().ShouldBeTrue();
      }

      [Observation]
      public void should_return_false_if_the_simulaton_is_defined()
      {
         var res = new SimulationResults();
         res.IsNull().ShouldBeFalse();

      }
   }
}	