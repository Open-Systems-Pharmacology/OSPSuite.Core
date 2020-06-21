using System;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain.ParameterIdentifications;

namespace OSPSuite.Core.Domain
{
   public abstract class concern_for_ParameterIdentificationRunResult : ContextSpecification<ParameterIdentificationRunResult>
   {
      protected override void Context()
      {
         sut = new ParameterIdentificationRunResult();
      }
   }

   public class When_retrieving_the_single_line_description_of_a_run_result_without_description : concern_for_ParameterIdentificationRunResult
   {
      protected override void Context()
      {
         base.Context();
         sut.Index = 5;
      }

      [Observation]
      public void should_return_the_run_index()
      {
         sut.SingleLineDescription.ShouldBeEqualTo("5");
      }
   }

   public class When_retrieving_the_single_line_description_of_a_run_result_with_a_description_on_multiple_line : concern_for_ParameterIdentificationRunResult
   {
      protected override void Context()
      {
         base.Context();
         sut.Index = 5;
         sut.Description = $"A{Environment.NewLine}B";
      }

      [Observation]
      public void should_replace_the_multiple_line_with_a_separator_and_add_the_run_index_at_the_front()
      {
         sut.SingleLineDescription.ShouldBeEqualTo("5 - A / B");
      }
   }
}