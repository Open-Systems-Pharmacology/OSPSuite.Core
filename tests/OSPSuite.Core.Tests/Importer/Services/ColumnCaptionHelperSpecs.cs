using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using NUnit.Framework;
using OSPSuite.Core.Importer.Mappers;

namespace OSPSuite.Importer.Services
{
   public abstract class concern_for_ColumnCaptionHelper : ContextSpecification<IColumnCaptionHelper>
   {
      protected string _result;

      protected override void Context()
      {
         sut = new ColumnCaptionHelper();
      }
   }

   public class when_getting_units_with_excess_whitespace : concern_for_ColumnCaptionHelper
   {
      private const string _columnCaptionString = " Column Caption [ms] ";

      protected override void Because()
      {
         _result = sut.GetUnit(_columnCaptionString);
      }

      [Observation]
      public void caption_string_should_have_excess_whitespace_trimmed()
      {
         _result.ShouldBeEqualTo("ms");
      }
   }

   public class when_trimming_caption_with_excess_whitespace : concern_for_ColumnCaptionHelper
   {
      private const string _columnCaptionString = " Column Caption ";

      protected override void Because()
      {
         _result = sut.TrimUnits(_columnCaptionString);
      }

      [Observation]
      public void caption_string_should_have_excess_whitespace_trimmed()
      {
         _result.ShouldBeEqualTo("Column Caption");
      }
   }


   public class when_getting_units_without_units_in_caption : concern_for_ColumnCaptionHelper
   {
      private const string _columnCaptionString = " Column Caption ";

      protected override void Because()
      {
         _result = sut.GetUnit(_columnCaptionString);
      }

      [Observation]
      public void caption_string_should_have_excess_whitespace_trimmed()
      {
         _result.ShouldBeEqualTo(string.Empty);
      }
   }

   public class when_trimming_caption_without_units : concern_for_ColumnCaptionHelper
   {
      private const string _columnCaptionString = "Column Caption";

      protected override void Because()
      {
         _result = sut.TrimUnits(_columnCaptionString);
      }

      [Observation]
      public void caption_string_should_not_be_modified()
      {
         _result.ShouldBeEqualTo(_columnCaptionString);
      }
   }

   public class when_getting_units_with_units : concern_for_ColumnCaptionHelper
   {
      private const string _columnCaptionString = " Column Caption[ms]";

      protected override void Because()
      {
         _result = sut.GetUnit(_columnCaptionString);
      }

      [Observation]
      public void caption_string_should_have_excess_whitespace_trimmed()
      {
         _result.ShouldBeEqualTo("ms");
      }
   }

   public class when_trimming_caption_with_units : concern_for_ColumnCaptionHelper
   {
      private const string _columnCaptionString = " Column[Caption]";

      protected override void Because()
      {
         _result = sut.TrimUnits(_columnCaptionString);
      }

      [Observation]
      public void caption_string_should_not_contain_units_and_no_padding()
      {
         _result.ShouldBeEqualTo("Column");
      }
   }

   public class when_getting_units_with_delimiters_backward : concern_for_ColumnCaptionHelper
   {
      private const string _columnCaptionString = " Column Caption ]ms[ ";

      protected override void Because()
      {
         _result = sut.GetUnit(_columnCaptionString);
      }

      [Observation]
      public void caption_string_should_have_excess_whitespace_trimmed()
      {
         _result.ShouldBeEqualTo(string.Empty);
      }
   }

   public class when_trimming_caption_braces_backward : concern_for_ColumnCaptionHelper
   {
      private const string _columnCaptionString = "Column ]Caption[";

      protected override void Because()
      {
         _result = sut.TrimUnits(_columnCaptionString);
      }

      [Observation]
      public void caption_string_should_not_be_modified()
      {
         _result.ShouldBeEqualTo(_columnCaptionString);
      }
      
   }

   public class when_column_names_are_compared : concern_for_ColumnCaptionHelper
   {
      [TestCase("Dose", "Dose", true)]
      [TestCase("Dose", "dose", true)]
      [TestCase("Dose", "dOse", true)]
      [TestCase("Dose [mg]", "Dose [g]", true)]
      [TestCase("dose [g]", "DOsE [G]", true)]
      [TestCase("dose", "DOse [l]", true)]
      [TestCase("dose [g]", "DOse", true)]
      [TestCase("Dosen", "Dose", false)]
      [TestCase("Dosen [g]", "Dose [g]", false)]
      [TestCase("Dosen [ml]", "Dose", false)]
      [TestCase("Dosen", "Dose [g]", false)]
      [TestCase("item", "Item", true)]
      [TestCase("iteM", "Item [g]", true)]
      [TestCase("Item[]", "item[]", true)]
      [TestCase(" Item []", " item[]", true)]
      public void should_compare_as_expected(string leftString, string rightString, bool result)
      {
         sut.IsEquivalent(leftString, rightString).ShouldBeEqualTo(result);
      }


   }
}
