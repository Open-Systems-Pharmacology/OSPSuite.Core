using DevExpress.XtraLayout.Utils;
using OSPSuite.Assets;
using OSPSuite.Core.Domain;
using OSPSuite.Presentation.Presenters.Charts;
using OSPSuite.Presentation.Views;
using OSPSuite.Presentation.Views.Charts;
using OSPSuite.UI.Controls;
using OSPSuite.UI.Extensions;

namespace OSPSuite.UI.Views.Charts
{
   public partial class SimpleChartView : BaseUserControl, ISimpleChartView
   {
      private ISimpleChartPresenter _presenter;

      public SimpleChartView()
      {
         InitializeComponent();
      }

      public override void InitializeResources()
      {
         base.InitializeResources();
         logLinToggleSwitch.Properties.OnText = Captions.LogScale;
         logLinToggleSwitch.Properties.OffText = Captions.LinearScale;
         logLinToggleSwitch.Properties.AllowThumbAnimation = false;
         logLinToggleSwitch.EditValueChanged += (o, e) => OnEvent(() => radioGroupLogLinChanged(logLinToggleSwitch.EditValue));
      }

      private void radioGroupLogLinChanged(object editValue)
      {
         var isLogScaleSelected = (bool) editValue;
         _presenter.SetChartScale(isLogScaleSelected ? Scalings.Log : Scalings.Linear);
      }

      public void SetChartScale(Scalings scale)
      {
         logLinToggleSwitch.EditValue = (scale == Scalings.Log);
      }

      public void AttachPresenter(ISimpleChartPresenter presenter)
      {
         _presenter = presenter;
      }

      public void AddView(IView chartView)
      {
         chartPanel.FillWith(chartView);
      }

      public bool LogLinSelectionEnabled
      {
         set { layoutItemLogScaling.Visibility = LayoutVisibilityConvertor.FromBoolean(value); }
         get { return LayoutVisibilityConvertor.ToBoolean(layoutItemLogScaling.Visibility); }
      }
   }
}