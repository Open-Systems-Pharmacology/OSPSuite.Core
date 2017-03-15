using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Presentation.Extensions;

namespace OSPSuite.Presentation
{
   public class Formatting_a_string_for_label_that_as_some_weird_case : StaticContextSpecification
   {
      private string _string;

      protected override void Context()
      {
         base.Context();
         _string = "A weIrd StrIng";
      }

      [Observation]
      public void should_have_only_the_first_letter_upper_case_if_the_check_case_is_set_to_true()
      {
         _string.FormatForLabel().ShouldBeEqualTo("A weird string:");
      }

      [Observation]
      public void should_not_change_the_case_if_the_check_case_flag_is_set_to_false()
      {
         _string.FormatForLabel(checkCase: false).ShouldBeEqualTo("A weIrd StrIng:");
      }
   }

   public class When_formatting_a_string_without_adding_a_colon : StaticContextSpecification
   {
      private string _string;

      protected override void Context()
      {
         base.Context();
         _string = "Label for a Checkedit";
      }

      [Observation]
      public void should_lower_all_case_but_first_and_not_end_with_colon()
      {
         _string.FormatForLabel(checkCase: true, addColon: false).ShouldBeEqualTo("Label for a checkedit");
      }
   }

   public class When_removing_the_html_tags_from_a_given_string : StaticContextSpecification
   {
      private string _stringWithHtml;

      protected override void Context()
      {
         base.Context();
         _stringWithHtml = "<b>this is a lovely</b> weather with <U>a lot of</U> people <i>around us</i>";
      }

      [Observation]
      public void should_return_the_expected_path()
      {
         _stringWithHtml.RemoveHtml().ShouldBeEqualTo("this is a lovely weather with a lot of people around us");
      }
   }
}