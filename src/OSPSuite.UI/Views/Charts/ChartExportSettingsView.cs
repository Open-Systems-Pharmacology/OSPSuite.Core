using OSPSuite.Assets;
using OSPSuite.Core.Chart;
using OSPSuite.Core.Domain;
using OSPSuite.DataBinding;
using OSPSuite.DataBinding.DevExpress;
using OSPSuite.Presentation.Extensions;
using OSPSuite.Presentation.Presenters.Charts;
using OSPSuite.Presentation.Views.Charts;
using OSPSuite.UI.Controls;

namespace OSPSuite.UI.Views.Charts
{
   internal partial class ChartExportSettingsView : BaseUserControl, IChartExportSettingsView
   {
      private IChartExportSettingsPresenter _presenter;
      private readonly ScreenBinder<IChartManagement> _screenBinderForChartManagement;
      private readonly ScreenBinder<ChartFontAndSizeSettings> _screenBinderForExportSettings;
      private readonly ScreenBinder<ChartFonts> _screenBinderForFonts;

      public ChartExportSettingsView()
      {
         InitializeComponent();
         _screenBinderForExportSettings = new ScreenBinder<ChartFontAndSizeSettings> {BindingMode = BindingMode.TwoWay};
         _screenBinderForFonts = new ScreenBinder<ChartFonts> {BindingMode = BindingMode.TwoWay};
         _screenBinderForChartManagement = new ScreenBinder<IChartManagement> {BindingMode = BindingMode.TwoWay};
      }

      public void AttachPresenter(IChartExportSettingsPresenter presenter)
      {
         _presenter = presenter;
      }

      public void BindTo(IChartManagement chart)
      {
         _screenBinderForChartManagement.BindToSource(chart);
         _screenBinderForExportSettings.BindToSource(chart.FontAndSize);
         _screenBinderForFonts.BindToSource(chart.FontAndSize.Fonts);
         Refresh();
      }

      public void DeleteBinding()
      {
         _screenBinderForChartManagement.DeleteBinding();
         _screenBinderForExportSettings.DeleteBinding();
         _screenBinderForFonts.DeleteBinding();
      }

      public override void InitializeBinding()
      {
         _screenBinderForExportSettings.Bind(x => x.ChartWidth)
            .To(tbWidth)
            .OnValueUpdated += notifyChartSettingsChanged;

         _screenBinderForExportSettings.Bind(x => x.ChartHeight)
            .To(tbHeight)
            .OnValueUpdated += notifyChartSettingsChanged;

         _screenBinderForChartManagement.Bind(x => x.IncludeOriginData)
            .To(includeOriginDataInChartCheckEdit)
            .OnValueUpdated += notifyChartSettingsChanged;

         _screenBinderForFonts.Bind(x => x.FontFamilyName)
            .To(cbFontFamily)
            .WithValues(x => _presenter.AllFontFamilyNames)
            .OnValueUpdated += notifyChartSettingsChanged;

         _screenBinderForFonts.Bind(x => x.TitleSize)
            .To(cbFontSizeTitle)
            .WithValues(x => _presenter.AllFontSizes)
            .OnValueUpdated += notifyChartSettingsChanged;

         _screenBinderForFonts.Bind(x => x.DescriptionSize)
            .To(cbFontSizeDescription)
            .WithValues(x => _presenter.AllFontSizes)
            .OnValueUpdated += notifyChartSettingsChanged;

         _screenBinderForFonts.Bind(x => x.OriginSize)
            .To(cbFontSizeOrigin)
            .WithValues(x => _presenter.AllFontSizes)
            .OnValueUpdated += notifyChartSettingsChanged;

         _screenBinderForFonts.Bind(x => x.AxisSize)
            .To(cbFontSizeAxis)
            .WithValues(x => _presenter.AllFontSizes)
            .OnValueUpdated += notifyChartSettingsChanged;

         _screenBinderForFonts.Bind(x => x.WatermarkSize)
            .To(cbFontSizeWatermark)
            .WithValues(x => _presenter.AllFontSizes)
            .OnValueUpdated += notifyChartSettingsChanged;

         _screenBinderForFonts.Bind(x => x.LegendSize)
            .To(cbFontSizeLegend)
            .WithValues(x => _presenter.AllFontSizes)
            .OnValueUpdated += notifyChartSettingsChanged;

         _screenBinderForChartManagement.Bind(x => x.PreviewSettings)
            .To(cePreviewSettings)
            .OnValueUpdated += notifyChartSettingsChanged;

         btnResetValues.Click += (o, e) => _presenter.ResetValuesToDefault();

         RegisterValidationFor(_screenBinderForExportSettings, statusChangedNotify: NotifyViewChanged);
         RegisterValidationFor(_screenBinderForChartManagement, statusChangedNotify: NotifyViewChanged);
         RegisterValidationFor(_screenBinderForFonts, statusChangedNotify: NotifyViewChanged);
      }

      private void notifyChartSettingsChanged<T>(object sender, T e)
      {
         _presenter.NotifyChartExportSettingsChanged();
      }

      public override void InitializeResources()
      {
         cePreviewSettings.Text = Captions.Chart.FontAndSizeSettings.PreviewSettings;
         layoutItemWidth.Text = Constants.NameWithUnitFor(Captions.Chart.FontAndSizeSettings.Width, Captions.Chart.FontAndSizeSettings.Pixels).FormatForLabel();
         layoutItemHeight.Text = Constants.NameWithUnitFor(Captions.Chart.FontAndSizeSettings.Height, Captions.Chart.FontAndSizeSettings.Pixels).FormatForLabel();
         layoutItemFontSizeAxis.Text = Captions.Chart.FontAndSizeSettings.FontSizeAxis.FormatForLabel();
         layoutItemFontSizeLegend.Text = Captions.Chart.FontAndSizeSettings.FontSizeLegend.FormatForLabel();
         layoutItemFontSizeTitle.Text = Captions.Chart.FontAndSizeSettings.FontSizeTitle.FormatForLabel();
         layoutItemFontSizeDescription.Text = Captions.Chart.FontAndSizeSettings.FontSizeDescription.FormatForLabel();
         layoutItemFontSizeOrigin.Text = Captions.Chart.FontAndSizeSettings.FontSizeOrigin.FormatForLabel();
         layoutItemFontSizeWatermark.Text = Captions.Chart.FontAndSizeSettings.FontSizeWatermark.FormatForLabel();
         includeOriginDataInChartCheckEdit.Text = Captions.Chart.FontAndSizeSettings.IncludeOriginData.FormatForLabel(addColon: false);
         btnResetValues.Text = Captions.ResetToDefault;
      }
   }
}