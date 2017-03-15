using System;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Maths.Random;
using OSPSuite.Core.Maths.Statistics;

namespace OSPSuite.Core
{
   public abstract class concern_for_LogNormalDistribution : ContextSpecification<LogNormalDistribution>
   {
      protected override void Context()
      {
         sut = new LogNormalDistribution(2,0);
      }
   }  

   public class When_generating_a_log_normal_value_when_deviation_is_0 : concern_for_LogNormalDistribution
   {
      [Observation]
      public void should_retun_the_exponent_of_the_mean()
      {
         sut.RandomDeviate(new RandomGenerator(),0,double.PositiveInfinity).ShouldBeEqualTo(Math.Exp(2));
      }
   }
   
   public class When_generating_a_value_outside_of_the_range : concern_for_LogNormalDistribution
   {
      protected override void Context()
      {
         sut = new LogNormalDistribution(Math.Log(22), 0.001);
      }

      [Observation]
      public void should_throw_an_exception()
      {
         The.Action(()=>sut.RandomDeviate(new RandomGenerator(), 0, 14)).ShouldThrowAn<LimitsNotInLogNormalDistributionFunctionRangeException>();
      }
   }
}	