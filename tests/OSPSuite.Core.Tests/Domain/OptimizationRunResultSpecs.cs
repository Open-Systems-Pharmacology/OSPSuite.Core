using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain.ParameterIdentifications;

namespace OSPSuite.Core.Domain
{
   public abstract class concern_for_OptimizationRunResult : ContextSpecification<OptimizationRunResult>
   {
      protected override void Context()
      {
         sut = new OptimizationRunResult();
      }
   }

   public class When_creating_an_optimization_run_results : concern_for_OptimizationRunResult
   {
      [Observation]
      public void should_have_a_default_total_error_of_infinity()
      {
         double.IsPositiveInfinity(sut.TotalError).ShouldBeTrue();
      }

      [Observation]
      public void should_have_an_empty_list_of_residuals()
      {
         sut.AllResiduals.Count.ShouldBeEqualTo(0);
      }

      [Observation]
      public void should_have_an_empty_list_of_output_residuals()
      {
         sut.AllOutputResiduals.Count.ShouldBeEqualTo(0);
      }
   }
}