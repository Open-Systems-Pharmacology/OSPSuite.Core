using NUnit.Framework;
using OSPSuite.Assets.Extensions;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Extensions;

namespace OSPSuite.Core.Domain
{
   public class When_creating_a_path_from_a_string : StaticContextSpecification
   {
      [Observation]
      public void should_return_the_expected_path  ()
      {
         "A|B|C".ToPathArray().ShouldOnlyContainInOrder("A","B","C");
      }

      [Observation]
      public void should_return_the_expected_path_for_a_single_entry()
      {
         "A".ToPathArray().ShouldOnlyContainInOrder("A");
      }
   }

   public class When_checking_if_a_string_is_not_empty : StaticContextSpecification
   {
      [Observation]
      public void should_return_true_if_the_string_is_not_empty()
      {
         "A|B|C".StringIsNotEmpty().ShouldBeTrue();
      }

      [Observation]
      public void should_return_false_if_the_string_is_empty()
      {
         "".StringIsNotEmpty().ShouldBeFalse();
         "    ".StringIsNotEmpty().ShouldBeFalse();
      }
   }

   public class When_converting_a_path_to_UNC_path : StaticContextSpecification
   {
      [Observation]
      [TestCase(null, null)]
      [TestCase("", "")]
      [TestCase("C\\A\\B", "C/A/B")]
      [TestCase("C/A/B", "C/A/B")]
      [TestCase("C\\\\A\\B", "C//A/B")]
      public void should_return_the_expected_converted_path(string originalPath, string convertedPath)
      {
         originalPath.ToUNCPath().ShouldBeEqualTo(convertedPath);
      }
   }

   public class Splitting_a_string_with_upper_case : StaticContextSpecification
   {
      [TestCase("ScaleDenominator", "Scale Denominator")]
      [TestCase("scale", "scale")]
      [TestCase("", "")]
      [TestCase(null, null)]
      [TestCase("ABC", "A B C")]
      public void should_return_the_expected_string(string input, string result)
      {
         input.SplitToUpperCase().ShouldBeEqualTo(result);
      }
   }

   public class When_returning_the_plural_of_a_string_ : StaticContextSpecification
   {
      [TestCase("Dog", "Dogs")]
      [TestCase("Analysis", "Analyses")]
      [TestCase("SimulationAnalysis", "SimulationAnalyses")]
      public void should_return_the_expected_string(string input, string result)
      {
         input.Pluralize().ShouldBeEqualTo(result);
      }
   }


   public class Striping_unit_from_a_given_string : StaticContextSpecification
   {
      [TestCase("ScaleDenominator", "ScaleDenominator")]
      [TestCase("scale []", "scale")]
      [TestCase("", "")]
      [TestCase(null, null)]
      [TestCase("ABC [HELLO]", "ABC")]
      [TestCase("ABC [HELLO] CDE", "ABC [HELLO] CDE")]
      public void should_return_the_expected_string(string input, string result)
      {
         input.StripUnit().ShouldBeEqualTo(result);
      }
   }

   public class When_returning_a_string_in_quotes : StaticContextSpecification
   {
      [Observation]
      public void should_return_the_expected_string()
      {
         "text".InQuotes().ShouldBeEqualTo("\"text\"");
      }
   }
}	