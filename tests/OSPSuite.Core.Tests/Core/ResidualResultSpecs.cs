using System;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Domain.ParameterIdentifications;
using OSPSuite.Helpers;

namespace OSPSuite.Core
{
   public abstract class concern_for_ResidualsResult : ContextSpecification<ResidualsResult>
   {
      protected DataRepository _observedData;

      protected override void Context()
      {
         _observedData = DomainHelperForSpecs.ObservedData();

         sut = new ResidualsResult();
      }
   }

   public class When_computing_the_total_error_for_a_run_where_no_error_occured : concern_for_ResidualsResult
   {
      protected override void Because()
      {
         sut.AddOutputResiduals("A", _observedData, new[] { new Residual(1f, 2f, 1), new Residual(1.5f, 3f, 1) });
         sut.AddOutputResiduals("B", _observedData, new[] { new Residual(1f, 4f, 1), new Residual(1.5f, 5f, 1) });
      }

      [Observation]
      public void should_return_the_expected_value()
      {
         sut.TotalError.ShouldBeEqualTo(Math.Sqrt(54));
      }
   }

   public class When_computing_the_total_error_for_a_run_where_an_exception_occured : concern_for_ResidualsResult
   {
      protected override void Because()
      {
         sut.AddOutputResiduals("A", _observedData, new[] { new Residual(1f, 2f, 1), new Residual(1.5f, 3f, 1) });
         sut.ExceptionOccured = true;
      }

      [Observation]
      public void should_return_infinity()
      {
         double.IsInfinity(sut.TotalError).ShouldBeTrue();
      }
   }
}