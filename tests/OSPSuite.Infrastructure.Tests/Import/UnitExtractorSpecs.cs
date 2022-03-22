using NUnit.Framework;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Infrastructure.Import.Services;

namespace OSPSuite.Infrastructure.Import
{
   public abstract class concern_for_UnitExtractor : StaticContextSpecification
   {
   }

   public class When_testing_different_combination_of_text : concern_for_UnitExtractor
   {
      [TestCase("Value", "Value", "")]
      [TestCase("Value [unit]", "Value", "unit")]
      [TestCase("Value [unit] ", "Value", "unit")]
      [TestCase("Value [raw] 1 [unit]", "Value [raw] 1", "unit")]
      [TestCase("Value [raw] [unit] ", "Value [raw]", "unit")]
      [TestCase("Value [raw] 1", "Value [raw] 1", "")]
      [TestCase("[Value] [unit]", "[Value]", "unit")]
      public void should_return_the_expected_value(string text, string name, string unit)
      {
         var res = UnitExtractor.ExtractNameAndUnit(text);
         res.name.ShouldBeEqualTo(name);
         res.unit.ShouldBeEqualTo(unit);
      }
   }
}