using System.Collections.Generic;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using FakeItEasy;
using OSPSuite.Presentation.Presenters.Charts;
using OSPSuite.Presentation.Services.Charts;
using OSPSuite.Presentation.Views.Charts;

namespace OSPSuite.Presentation
{
   public abstract class concern_for_ChartExportSettingsPresenter : ContextSpecification<ChartExportSettingsPresenter>
   {
      protected IFontsTask _fontsTask;

      protected override void Context()
      {
         _fontsTask = A.Fake<IFontsTask>();
         sut = new ChartExportSettingsPresenter(A.Fake<IChartExportSettingsView>(), _fontsTask);
      }
   }

   public class When_retrieving_the_list_of_available_fonts : concern_for_ChartExportSettingsPresenter
   {
      private IEnumerable<string> _result;

      protected override void Context()
      {
         base.Context();
         A.CallTo(() => _fontsTask.ChartFontFamilyNames).Returns(new List<string> { "AnotherFont" });
      }

      protected override void Because()
      {
         _result = sut.AllFontFamilyNames;
      }

      [Observation]
      public void should_contains_all_fonts_families_define_for_charts()
      {
         _result.ShouldOnlyContainInOrder("AnotherFont");
      }
   }

  
}