using System.Drawing;
using OSPSuite.Assets;
using OSPSuite.Core.Chart;
using OSPSuite.DataBinding;
using OSPSuite.DataBinding.DevExpress;
using OSPSuite.Presentation.Extensions;
using OSPSuite.Presentation.Formatters;
using OSPSuite.Presentation.Presenters.Charts;
using OSPSuite.Presentation.Views.Charts;
using OSPSuite.Utility.Format;

namespace OSPSuite.UI.Views.Charts
{
   public partial class CurveMultiItemEditorView : BaseModalView, ICurveMultiItemEditorView
   {
      private ICurveMultiItemEditorPresenter _presenter;
      private readonly IFormatter<bool?> _boolFormatter = new BooleanYesNoFormatter();
      private readonly IFormatter<LineStyles?> _lineStylesFormatter = new LineStylesFormatter();
      private readonly IFormatter<Symbols?> _symbolsFormatter = new SymbolsFormatter();
      private readonly ScreenBinder<SelectedCurveValues> _screenBinder = new ScreenBinder<SelectedCurveValues> {BindingMode = BindingMode.TwoWay};

      public CurveMultiItemEditorView()
      {
         InitializeComponent();
      }

      public override void InitializeResources()
      {
         base.InitializeResources();

         colorLayoutControlItem.Text = Captions.Chart.CurveOptions.Color.FormatForLabel();
         styleLayoutControlItem.Text = Captions.Chart.CurveOptions.LineStyle.FormatForLabel();
         symbolLayoutControlItem.Text = Captions.Chart.CurveOptions.Symbol.FormatForLabel();
         visibleLayoutControlItem.Text = Captions.Chart.CurveOptions.Visible.FormatForLabel();
         inLegendLayoutControlItem.Text = Captions.Chart.CurveOptions.VisibleInLegend.FormatForLabel();
      }

      public override void InitializeBinding()
      {
         base.InitializeBinding();

         _screenBinder.Bind(x => x.Color)
            .To(colorPickEdit1)
            .OnValueUpdated += colorChanged;

         _screenBinder.Bind(x => x.Style)
            .To(styleComboBoxEdit)
            .WithValues(_presenter.AllLineStyles)
            .WithFormat(_lineStylesFormatter);


         _screenBinder.Bind(x => x.Symbol)
            .To(symbolComboBoxEdit)
            .WithValues(_presenter.AllSymbols)
            .WithFormat(_symbolsFormatter);
         ;

         _screenBinder.Bind(x => x.Visible)
            .To(visibleComboBoxEdit)
            .WithValues(_presenter.AllBooleanOptions)
            .WithFormat(_boolFormatter);

         _screenBinder.Bind(x => x.VisibleInLegend)
            .To(inLegendComboBoxEdit)
            .WithValues(_presenter.AllBooleanOptions)
            .WithFormat(_boolFormatter);
      }

      private void colorChanged(SelectedCurveValues o, Color color)
      {
         _presenter.SetColorChangedFlag();
      }

      public void AttachPresenter(ICurveMultiItemEditorPresenter presenter)
      {
         _presenter = presenter;
      }

      public void BindTo(SelectedCurveValues selectedCurveValues)
      {
         _screenBinder.BindToSource(selectedCurveValues);
      }
   }
}