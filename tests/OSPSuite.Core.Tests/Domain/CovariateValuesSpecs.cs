using System.Collections.Generic;
using FakeItEasy;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain.Populations;

namespace OSPSuite.Core.Domain
{
   public abstract class concern_for_CovariateValues : ContextSpecification<CovariateValues>
   {
      protected override void Context()
      {
         sut = new CovariateValues("Toto", new List<string>(){"V1", "V2"});
      }
   }

   public class When_retrieving_the_values_for_a_valid_individual_index : concern_for_CovariateValues
   {
      [Observation]
      public void should_return_the_expected_values()
      {
         sut.ValueAt(0).ShouldBeEqualTo("V1");
         sut.ValueAt(1).ShouldBeEqualTo("V2");
      }
   }
   public class When_retrieving_the_values_for_an_valid_individual_index : concern_for_CovariateValues
   {
      [Observation]
      public void should_return_unknown()
      {
         sut.ValueAt(-5).ShouldBeEqualTo(Constants.UNKNOWN);
         sut.ValueAt(2).ShouldBeEqualTo(Constants.UNKNOWN);
      }
   }



}