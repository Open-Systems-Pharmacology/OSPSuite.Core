using System.Windows.Forms;
using OSPSuite.DataBinding;
using OSPSuite.DataBinding.DevExpress;
using DevExpress.XtraLayout.Utils;
using OSPSuite.Assets;
using OSPSuite.Core.Chart;
using OSPSuite.Presentation.Extensions;
using OSPSuite.Presentation.Presenters.Charts;
using OSPSuite.Presentation.Views;
using OSPSuite.Presentation.Views.Charts;

namespace OSPSuite.UI.Views.Charts
{
   public partial class SingleCurveSettingsView : BaseModalView, ISingleCurveSettingsView
   {
      private readonly ScreenBinder<Curve> _screenBinder;

      public SingleCurveSettingsView()
      {
         InitializeComponent();
      }
      public SingleCurveSettingsView(IShell shell) : base(shell)
      {
         InitializeComponent();
         _screenBinder = new ScreenBinder<Curve>();
      }

      public override void InitializeBinding()
      {
         base.InitializeBinding();

         _screenBinder.Bind(x => x.Name)
            .To(nameTextBox);

         _screenBinder.Bind(x => x.yAxisType)
            .To(yAxisTypeComboBox)
            .WithValues(yAxisTypeComboBox.GetValidValues());

         _screenBinder.Bind(x => x.Color)
            .To(colorEdit);

         _screenBinder.Bind(x => x.LineStyle)
            .To(lineStyleComboBox)
            .WithValues(lineStyleComboBox.GetValidValues());

         _screenBinder.Bind(x => x.Symbol)
            .To(symbolComboBox)
            .WithValues(symbolComboBox.GetValidValues());

         _screenBinder.Bind(x => x.LineThickness)
            .To(lineThicknessComboBox)
            .WithValues(lineThicknessComboBox.GetValidValues());

         _screenBinder.Bind(x => x.Visible)
            .To(visibleCheckEdit);

         _screenBinder.Bind(x => x.VisibleInLegend)
            .To(visibleInLegendCheckEdit);

      }

      public override void InitializeResources()
      {
         base.InitializeResources();
         nameControlItem.Text = Captions.Name.FormatForLabel();
         yAxisTypeControlItem.Text = Captions.YAxisType.FormatForLabel();
         colorControlItem.Text = Captions.Color.FormatForLabel();
         lineStyleControlItem.Text = Captions.LineStyle.FormatForLabel();
         symbolControlItem.Text = Captions.Symbol.FormatForLabel();
         lineThicknessControlItem.Text = Captions.LineThickness.FormatForLabel();
         visibleControlItem.Text = Captions.Visible.FormatForLabel();
         visibleInLegendControlItem.Text = Captions.VisibleInLegend.FormatForLabel();
         yAxisTypeControlItem.Visibility = LayoutVisibility.Never;
         FormBorderStyle = FormBorderStyle.SizableToolWindow;
         Caption = Captions.CurveSettings;
      }

      public void AttachPresenter(ISingleCurveSettingsPresenter presenter)
      {
      }

      public void BindTo(Curve curve)
      {
         _screenBinder.BindToSource(curve);
      }
   }
}
