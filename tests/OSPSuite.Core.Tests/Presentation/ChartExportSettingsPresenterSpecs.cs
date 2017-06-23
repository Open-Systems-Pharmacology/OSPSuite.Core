using System.Collections.Generic;
using FakeItEasy;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Chart;
using OSPSuite.Presentation.Presenters.Charts;
using OSPSuite.Presentation.Services.Charts;
using OSPSuite.Presentation.Views.Charts;

namespace OSPSuite.Presentation
{
   public abstract class concern_for_ChartExportSettingsPresenter : ContextSpecification<ChartExportSettingsPresenter>
   {
      protected IFontsTask _fontsTask;
      protected IChartExportSettingsView _view;

      protected override void Context()
      {
         _fontsTask = A.Fake<IFontsTask>();
         _view= A.Fake<IChartExportSettingsView>();
         sut = new ChartExportSettingsPresenter(_view, _fontsTask);
      }
   }

   public class When_retrieving_the_list_of_available_fonts : concern_for_ChartExportSettingsPresenter
   {
      private IEnumerable<string> _result;

      protected override void Context()
      {
         base.Context();
         A.CallTo(() => _fontsTask.ChartFontFamilyNames).Returns(new List<string> {"AnotherFont"});
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

   public class Whyen_the_chart_export_settings_presenter_is_editing_an_chart_management_object : concern_for_ChartExportSettingsPresenter
   {
      private IChartManagement _chartManagement;

      protected override void Context()
      {
         base.Context();
         _chartManagement= A.Fake<IChartManagement>();
      }

      protected override void Because()
      {
         sut.Edit(_chartManagement);
      }

      [Observation]
      public void should_bind_the_objec_tto_the_view()
      {
         A.CallTo(() => _view.BindTo(_chartManagement)).MustHaveHappened();   
      }
   }

   public class When_the_chart_export_settings_presenter_is_notified_that_some_export_settings_were_changed : concern_for_ChartExportSettingsPresenter
   {
      private bool _exportSettingsChanged;

      protected override void Context()
      {
         base.Context();
         sut.ChartExportSettingsChanged += (o,e) => _exportSettingsChanged = true;
      }
      protected override void Because()
      {
         sut.NotifyChartExportSettingsChanged();
      }

      [Observation]
      public void should_notify_the_chart_export_settings()
      {
         _exportSettingsChanged.ShouldBeTrue();
      }
   }

   public class When_the_chart_export_settings_presenter_is_told_to_clear_its_content : concern_for_ChartExportSettingsPresenter
   {
      protected override void Because()
      {
         sut.Clear();
      }

      [Observation]
      public void should_delete_the_binding_in_the_view()
      {
         A.CallTo(() => _view.DeleteBinding()).MustHaveHappened();
      }
   }

   public class When_resetting_the_chart_export_settings_to_default : concern_for_ChartExportSettingsPresenter
   {
      private bool _chartExportSettingsChanged;
      private IChartManagement _chartManagement;

      protected override void Context()
      {
         base.Context();
         _chartManagement= A.Fake<IChartManagement>();
         A.CallTo(() => _chartManagement.FontAndSize).Returns(new ChartFontAndSizeSettings());
         _chartManagement.FontAndSize.ChartHeight = 500;
         sut.ChartExportSettingsChanged += (o,e) => _chartExportSettingsChanged = true;
         sut.Edit(_chartManagement);
      }

      protected override void Because()
      {
         sut.ResetValuesToDefault();
      }

      [Observation]
      public void should_reset_the_chart_export_settings_to_default()
      {
         _chartManagement.FontAndSize.ChartHeight.ShouldBeNull();
      }

      [Observation]
      public void should_also_notify_the_chart_export_settings_event()
      {
         _chartExportSettingsChanged.ShouldBeTrue();
      }
   }
}