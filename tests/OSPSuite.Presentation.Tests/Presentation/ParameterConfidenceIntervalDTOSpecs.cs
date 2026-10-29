using NUnit.Framework;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Presentation.DTO.ParameterIdentifications;

namespace OSPSuite.Presentation.Presentation
{
   public abstract class concern_for_ParameterConfidenceIntervalDTO : ContextSpecification<ParameterConfidenceIntervalDTO>
   {
      protected override void Context()
      {
         sut = new ParameterConfidenceIntervalDTO();
      }
   }

   public class When_computing_the_relative_confidence_interval_of_a_parameter : concern_for_ParameterConfidenceIntervalDTO
   {
      [Observation]
      [TestCase(10, 5, 50)]
      [TestCase(-10, 5, 50)]
      [TestCase(4, 1, 25)]
      public void should_return_the_confidence_interval_in_percent_of_the_absolute_point_estimate(double value, double confidenceInterval, double expectedRelativeConfidenceInterval)
      {
         sut.Value = value;
         sut.ConfidenceInterval = confidenceInterval;
         sut.RelativeConfidenceInterval.ShouldBeEqualTo(expectedRelativeConfidenceInterval, 1e-10);
      }

      [Observation]
      public void should_return_nan_if_the_point_estimate_is_zero()
      {
         sut.Value = 0;
         sut.ConfidenceInterval = 5;
         double.IsNaN(sut.RelativeConfidenceInterval).ShouldBeTrue();
      }
   }
}