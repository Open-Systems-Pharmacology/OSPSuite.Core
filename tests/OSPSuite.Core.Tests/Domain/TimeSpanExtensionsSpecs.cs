using System;
using NUnit.Framework;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Extensions;

namespace OSPSuite.Core.Domain
{
   public class When_retrieving_the_formatted_time  : StaticContextSpecification
   {
      [Observation]
      [TestCase(0, 0, 10, 40, 28, "10m:40s:28ms" )]
      [TestCase(0, 1, 10, 40, 28, "01h:10m:40s:28ms" )]
      [TestCase(1, 0, 10, 40, 28, "01d:00h:10m:40s:28ms" )]
      public void should_return_the_expected_value(int days,int hour, int min, int sec, int ms, string display)
      {
         var timeSpan = new TimeSpan(days, hour, min, sec, ms);
         timeSpan.ToDisplay().ShouldBeEqualTo(display);
      }
   }
}	