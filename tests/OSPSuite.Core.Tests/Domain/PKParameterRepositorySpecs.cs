using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain.PKAnalyses;

namespace OSPSuite.Core.Domain
{
   public abstract class concern_for_PKParameterRepository : ContextSpecification<IPKParameterRepository>
   {
      protected override void Context()
      {
         sut = new PKParameterRepository();
      }
   }

   public class When_retrieving_the_display_name_for_a_parameter_name : concern_for_PKParameterRepository
   {
      protected override void Context()
      {
         base.Context();
         sut.Add(new PKParameter{Name = "auc",DisplayName = "AUC"});
      }

      [Observation]
      public void should_return_the_display_name_of_the_parameter_if_the_parameter_was_defined()
      {
         sut.DisplayNameFor("auc").ShouldBeEqualTo("AUC");
      }

      [Observation]
      public void should_return_the_input_name_if_no_pk_parameter_is_defined_with_the_given_name()
      {
         sut.DisplayNameFor("toto").ShouldBeEqualTo("toto");
      }
   }

   public class When_retrieving_the_description_for_a_parameter: concern_for_PKParameterRepository
   {
      protected override void Context()
      {
         base.Context();
         sut.Add(new PKParameter { Name = "auc", Description = "AUC DESCRIPTION" });
      }
      [Observation]
      public void should_return_the_expected_description_if_the_parameter_is_defined()
      {
         sut.DescriptionFor("auc").ShouldBeEqualTo("AUC DESCRIPTION");
      }

      [Observation]
      public void should_return_an_empty_string_if_the_parameter_is_not_defined()
      {
         sut.DescriptionFor("toto").ShouldBeEqualTo(string.Empty);
      }
   }
}	