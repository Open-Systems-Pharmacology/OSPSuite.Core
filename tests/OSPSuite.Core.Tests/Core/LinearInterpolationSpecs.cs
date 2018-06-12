using System.Collections.Generic;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain;
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
         _knownSamples = new List<Sample> {new Sample(10, 9), new Sample(2, 1)};
      }

      [Observation]
      public void should_return_the_linear_interpolated_value_between_this_samples()
      {
         sut.Interpolate(_knownSamples, 8).ShouldBeEqualTo(7);
      }
   }

   public class When_interpolating_a_value_smaller_than_all_existing_samples : concern_for_LinearInterpolation
   {
      private IList<Sample> _knownSamples;

      protected override void Context()
      {
         base.Context();
         //y=x-1
         _knownSamples = new List<Sample> {new Sample(2, 1), new Sample(10, 9)};
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
         _knownSamples = new List<Sample> {new Sample(2, 1), new Sample(10, 9)};
      }

      [Observation]
      public void should_return_the_greatest_value_of_the_existing_samples()
      {
         sut.Interpolate(_knownSamples, 15).ShouldBeEqualTo(9);
      }
   }

   public class When_interpolating_a_value_with_only_one_sample : concern_for_LinearInterpolation
   {
      private IList<Sample> _knownSamples;

      protected override void Context()
      {
         base.Context();
         //y=x-1
         _knownSamples = new List<Sample> {new Sample(2, 1)};
      }

      [Observation]
      public void should_return_the_value_of_the_sample()
      {
         sut.Interpolate(_knownSamples, 15).ShouldBeEqualTo(1);
      }
   }

   public class When_interpolating_diescrete_values : concern_for_LinearInterpolation
   {
      private IList<Sample<IParameter>> _knownSamples;
      private IParameter _parameter1;
      private IParameter _parameter2;
      private IParameter _parameter3;

      protected override void Context()
      {
         base.Context();
         _parameter1 = new Parameter().WithName("P1");
         _parameter2 = new Parameter().WithName("P2");
         _parameter3 = new Parameter().WithName("P3");
         _knownSamples = new List<Sample<IParameter>>
         {
            new Sample<IParameter>(3, _parameter3),
            new Sample<IParameter>(1, _parameter1),
            new Sample<IParameter>(2, _parameter2),
         };
      }

      [Observation]
      public void should_return_the_discrete_value_that_is_closed_to_the_value_to_interpolate()
      {
         sut.Interpolate(_knownSamples, 1).ShouldBeEqualTo(_parameter1);
         sut.Interpolate(_knownSamples, 2).ShouldBeEqualTo(_parameter2);
         sut.Interpolate(_knownSamples, 3).ShouldBeEqualTo(_parameter3);
         sut.Interpolate(_knownSamples, 1.2).ShouldBeEqualTo(_parameter1);
         sut.Interpolate(_knownSamples, 2.8).ShouldBeEqualTo(_parameter3);
         sut.Interpolate(_knownSamples, 4).ShouldBeEqualTo(_parameter3);
         sut.Interpolate(_knownSamples, 0).ShouldBeEqualTo(_parameter1);
      }
   }
}