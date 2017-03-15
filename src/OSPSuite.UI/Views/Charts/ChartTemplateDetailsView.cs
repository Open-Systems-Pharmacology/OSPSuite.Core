using OSPSuite.Assets;
using OSPSuite.Presentation.Presenters.Charts;
using OSPSuite.Presentation.Views;
using OSPSuite.Presentation.Views.Charts;
using OSPSuite.UI.Controls;
using OSPSuite.UI.Extensions;

namespace OSPSuite.UI.Views.Charts
{
   public partial class ChartTemplateDetailsView : BaseUserControl, IChartTemplateDetailsView
   {
      private IChartTemplateDetailsPresenter _presenter;

      public ChartTemplateDetailsView()
      {
         InitializeComponent();
      }

      public void AttachPresenter(IChartTemplateDetailsPresenter presenter)
      {
         _presenter = presenter;
      }

      public void SetChartSettingsView(IView view)
      {
         panelChartSettings.FillWith(view);
      }

      public void SetCurveTemplateView(IView view)
      {
         panelCurves.FillWith(view);
      }

      public void SetAxisSettingsView(IView view)
      {
         panelAxes.FillWith(view);
      }

      public void SetChartExportSettingsView(IView view)
      {
         panelChartExportSettings.FillWith(view);
      }

      public override void InitializeResources()
      {
         base.InitializeResources();
         layoutGroupCurvesAndAxis.Text = Captions.CurveAndAxisSettings;
         layoutGroupChartSettings.Text = Captions.ChartSettings;
         layoutGroupChartExportSettings.Text = Captions.ChartExportSettings;
      }
   }
}
