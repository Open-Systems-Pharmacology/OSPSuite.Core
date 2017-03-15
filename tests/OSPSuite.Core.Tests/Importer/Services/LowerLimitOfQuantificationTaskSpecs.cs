using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Importer;
using OSPSuite.Presentation.Services.Importer;

namespace OSPSuite.Importer.Services
{
   public abstract class concern_for_LowerLimitOfQuantificationTask : ContextSpecification<ILowerLimitOfQuantificationTask>
   {
      protected override void Context()
      {
         sut = new LowerLimitOfQuantificationTask();
      }
   }

   public class When_attaching_the_lloq_value_to_a_column : concern_for_LowerLimitOfQuantificationTask
   {
      [Observation]
      public void when_attaching_the_property_to_the_column()
      {
         var importDataColumn = new ImportDataColumn();
         sut.AttachLLOQ(importDataColumn, 1.0F);
         importDataColumn.ColumnContainsValidLLOQ().ShouldBeTrue();
         importDataColumn.LLOQProperty().ShouldBeEqualTo(1.0F);
      }
   }

   public class When_interpreting_the_lloq_value : concern_for_LowerLimitOfQuantificationTask
   {
      [Observation]
      public void should_return_nan_when_both_values_for_lloq_are_nan()
      {
         sut.GetInterpretationOfLLOQ("", new ImportDataColumn()).ShouldBeEqualTo(float.NaN);
      }

      [Observation]
      public void should_find_the_column_value_if_string_value_is_not_valid()
      {
         var stringValue = $">{99}";
         var column = new ImportDataColumn();
         column.ExtendedProperties[Constants.LLOQ] = 1.0F;
         sut.GetInterpretationOfLLOQ(stringValue, column).ShouldBeEqualTo(0.5F);
      }

      [Observation]
      public void should_find_the_higher_of_the_two_values_when_two_limits_are_encountered_in_a_column()
      {
         var stringValue = $"<{0.5}";
         var column = new ImportDataColumn();
         column.ExtendedProperties[Constants.LLOQ] = 1.0F;
         sut.GetInterpretationOfLLOQ(stringValue, column).ShouldBeEqualTo(0.5F);
      }

      [Observation]
      public void easy_case_should_indicate_a_match_can_be_found_should_match()
      {
         var stringValue = $"<{0.5}";
         sut.IsLLOQ(stringValue).ShouldBeTrue();
         sut.ParseLLOQ(stringValue).ShouldBeEqualTo(0.5F);
         var column = new ImportDataColumn();
         sut.GetInterpretationOfLLOQ(stringValue, column).ShouldBeEqualTo(0.25F);
      }
   }

   public class When_finding_matches_in_a_string : concern_for_LowerLimitOfQuantificationTask
   {
      [Observation]
      public void easy_case_has_a_space_between_the_less_than_sign_and_the_number_should_match()
      {
         var stringValue = $"< {0.5}";
         sut.IsLLOQ(stringValue).ShouldBeTrue();
         sut.ParseLLOQ(stringValue).ShouldBeEqualTo(0.5F);
      }

      [Observation]
      public void easy_case_has_the_leading_0_dropped_from_the_decimal_should_match()
      {
         var stringValue = $"< {0.5}";
         stringValue = stringValue.Replace("0", "");
         sut.IsLLOQ(stringValue).ShouldBeTrue();
         sut.ParseLLOQ(stringValue).ShouldBeEqualTo(0.5F);
      }

      [Observation]
      public void easy_case_has_a_letter_leading_0_dropped_from_the_decimal_should_not_match()
      {
         var stringValue = $"<{0.5}R";
         sut.IsLLOQ(stringValue).ShouldBeFalse();
         sut.ParseLLOQ(stringValue).ShouldBeEqualTo(float.NaN);
      }

      [Observation]
      public void when_the_string_uses_comma_as_decimal_separator()
      {
         var stringValue = "<0,5";
         sut.IsLLOQ(stringValue).ShouldBeTrue();
         sut.ParseLLOQ(stringValue).ShouldBeEqualTo(0.5f);
      }

      [Observation]
      public void does_not_contain_the_less_than_sign()
      {
         var stringValue = $"{0.5}";
         sut.IsLLOQ(stringValue).ShouldBeFalse();
         sut.ParseLLOQ(stringValue).ShouldBeEqualTo(float.NaN);
      }

      [Observation]
      public void has_leading_and_trailing_whitespace_should_match()
      {
         var stringValue = $"  < {0.5}\t";
         sut.IsLLOQ(stringValue).ShouldBeTrue();
         sut.ParseLLOQ(stringValue).ShouldBeEqualTo(0.5F);
      }
   }
}
