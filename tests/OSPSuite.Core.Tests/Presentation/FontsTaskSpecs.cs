using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Presentation.Services.Charts;

namespace OSPSuite.Presentation
{
   public abstract class concern_for_FontsTask : ContextSpecification<IFontsTask>
   {
      protected List<string> _systemFonts;
      protected string _sansSerifFont;

      protected override void Context()
      {
         base.Context();
         sut = new FontsTask(() => _systemFonts, () => _sansSerifFont);
      }
   }

   public class When_getting_available_font_family_names_and_fonts_intersect_with_expected_fonts_but_does_not_include_default_system_font : concern_for_FontsTask
   {
      private IEnumerable<string> _result;

      protected override void Context()
      {
         _systemFonts = new List<string> {"Arial", "Helvetica", "Times New Roman"};
         _sansSerifFont = "AnotherFont";
         base.Context();
      }

      protected override void Because()
      {
         _result = sut.ChartFontFamilyNames;
      }

      [Observation]
      public void should_contain_all_expected_names()
      {
         _result.ShouldOnlyContainInOrder("Arial", "Helvetica", "Times New Roman", "AnotherFont");
      }
   }

   public class When_getting_available_font_family_names_and_fonts_intersect_with_expected_fonts_and_includes_default_system_font : concern_for_FontsTask
   {
      private IEnumerable<string> _result;

      protected override void Context()
      {
         _systemFonts = new List<string> {"Arial", "Helvetica", "Times New Roman"};
         _sansSerifFont = "Arial";
         base.Context();
      }

      protected override void Because()
      {
         _result = sut.ChartFontFamilyNames;
      }

      [Observation]
      public void should_contain_all_expected_names()
      {
         _result.ShouldOnlyContainInOrder("Arial", "Helvetica", "Times New Roman");
      }
   }

   public class When_retrieving_the_system_font_family_names : concern_for_FontsTask
   {
      protected override void Context()
      {
         base.Context();
         sut = new FontsTask();
      }

      [Observation]
      public void should_reutrn_the_available_fonts()
      {
         sut.SystemFontFamilyNames.ShouldBeEqualTo(FontFamily.Families.Select(x => x.Name));
      }
   }

   public class When_retrieving_the_default_sans_serif_font_name : concern_for_FontsTask
   {
      protected override void Context()
      {
         base.Context();
         sut = new FontsTask();
      }

      [Observation]
      public void should_reutrn_the_available_fonts()
      {
         sut.DefaultSansSerifFontName.ShouldBeEqualTo(FontFamily.GenericSansSerif.Name);
      }
   }
}