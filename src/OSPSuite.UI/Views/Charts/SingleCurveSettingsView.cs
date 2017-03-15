using OSPSuite.DataBinding;
using OSPSuite.DataBinding.DevExpress;
using DevExpress.XtraLayout.Utils;
using OSPSuite.Assets;
using OSPSuite.Core.Chart;
using OSPSuite.Presentation.Extensions;
using OSPSuite.Presentation.Presenters.Charts;
using OSPSuite.Presentation.Views.Charts;
using OSPSuite.UI.Controls;

namespace OSPSuite.UI.Views.Charts
{
   public partial class SingleCurveSettingsView : BaseUserControl, ISingleCurveSettingsView
   {
      private ScreenBinder<ICurve> _curveScreenBinder;

      public SingleCurveSettingsView()
      {
         InitializeComponent();
      }

      public override void InitializeBinding()
      {
         base.InitializeBinding();
         _curveScreenBinder = new ScreenBinder<ICurve>();

         _curveScreenBinder.Bind(x => x.Name).To(nameTextBox);

         yAxisTypeControlItem.Visibility = LayoutVisibility.Never;
         _curveScreenBinder.Bind(x => x.yAxisType).To(yAxisTypeComboBox).WithValues(yAxisTypeComboBox.GetValidValues());
         _curveScreenBinder.Bind(x => x.Color).To(colorEdit);
         _curveScreenBinder.Bind(x => x.LineStyle).To(lineStyleComboBox).WithValues(lineStyleComboBox.GetValidValues());
         _curveScreenBinder.Bind(x => x.Symbol).To(symbolComboBox).WithValues(symbolComboBox.GetValidValues());
         _curveScreenBinder.Bind(x => x.LineThickness).To(lineThicknessComboBox).WithValues(lineThicknessComboBox.GetValidValues());
         _curveScreenBinder.Bind(x => x.Visible).To(visibleCheckEdit);
         _curveScreenBinder.Bind(x => x.VisibleInLegend).To(visibleInLegendCheckEdit);
      }

      public override void InitializeResources()
      {
         base.InitializeResources();
         setControlItemCaptions();
      }

      private void setControlItemCaptions()
      {
         nameControlItem.Text = Captions.Name.FormatForLabel();
         yAxisTypeControlItem.Text = Captions.YAxisType.FormatForLabel();
         colorControlItem.Text = Captions.Color.FormatForLabel();
         lineStyleControlItem.Text = Captions.LineStyle.FormatForLabel();
         symbolControlItem.Text = Captions.Symbol.FormatForLabel();
         lineThicknessControlItem.Text = Captions.LineThickness.FormatForLabel();
         visibleControlItem.Text = Captions.Visible.FormatForLabel();
         visibleInLegendControlItem.Text = Captions.VisibleInLegend.FormatForLabel();
      }

      public void AttachPresenter(ISingleCurveSettingsPresenter presenter)
      {
      }

      public void BindTo(ICurve curve)
      {
         _curveScreenBinder.BindToSource(curve);
      }
   }
}
