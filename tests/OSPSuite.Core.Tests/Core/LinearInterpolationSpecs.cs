using System.Collections.Generic;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Maths.Interpolations;

namespace OSPSuite.Core
{
    public abstract class concern_for_LinearInterpolation : ContextSpecification<IInterpolation>
    {
        protected override void Context()
        {
            sut = new LinearInterpolation();
        }
    }

    public class When_interpolating_a_value_defined_between_existing_samples : concern_for_LinearInterpolation
    {
        private IList<Sample> _knownSamples;

        protected override void Context()
        {
            base.Context();
            //y=x-1
            _knownSamples = new List<Sample> { new Sample(10, 9), new Sample(2, 1) };
        }

        [Observation]
        public void should_return_the_linear_interpolated_value_between_this_samples()
        {
            sut.Interpolate(_knownSamples,8).ShouldBeEqualTo(7);
        }
    }
  
    public class When_interpolating_a_value_smaller_than_all_existing_samples : concern_for_LinearInterpolation
    {
        private IList<Sample> _knownSamples;

        protected override void Context()
        {
            base.Context();
            //y=x-1
            _knownSamples = new List<Sample> { new Sample(2, 1), new Sample(10, 9) };
        }

        [Observation]
        public void should_return_the_smallest_value_of_the_existing_samples()
        {
            sut.Interpolate(_knownSamples, 1).ShouldBeEqualTo(1);
        }
    }
    public class When_interpolating_a_value_greater_than_all_existing_samples : concern_for_LinearInterpolation
    {
        private IList<Sample> _knownSamples;

        protected override void Context()
        {
            base.Context();
            //y=x-1
            _knownSamples = new List<Sample> { new Sample(2, 1), new Sample(10, 9) };
        }

        [Observation]
        public void should_return_the_greatest_value_of_the_existing_samples()
        {
            sut.Interpolate(_knownSamples,15).ShouldBeEqualTo(9);
        }
    }

   public class When_interpolating_a_value_with_only_one_sample : concern_for_LinearInterpolation
    {
        private IList<Sample> _knownSamples;

        protected override void Context()
        {
            base.Context();
            //y=x-1
            _knownSamples = new List<Sample> { new Sample(2, 1) };
        }

        [Observation]
        public void should_return_the_value_of_the_sample()
        {
            sut.Interpolate(_knownSamples, 15).ShouldBeEqualTo(1);
        }
    }
}