using OSPSuite.DataBinding;
using OSPSuite.DataBinding.DevExpress;
using DevExpress.XtraLayout.Utils;
using OSPSuite.Assets;
using OSPSuite.Core.Chart;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Presentation.Extensions;
using OSPSuite.Presentation.Presenters.Charts;
using OSPSuite.Presentation.Views.Charts;
using OSPSuite.UI.Controls;
using OSPSuite.UI.Services;

namespace OSPSuite.UI.Views.Charts
{
   public partial class SingleAxisSettingsView : BaseUserControl, ISingleAxisSettingsView
   {
      private ScreenBinder<IAxis> _screenBinder;
      private ISingleAxisSettingsPresenter _presenter;
      private ComboBoxEditElementBinder<IAxis, IDimension> _dimensionBinder;
      private ComboBoxEditElementBinder<IAxis, string> _unitBinder;
      private readonly IToolTipCreator _toolTipCreator;

      public SingleAxisSettingsView(IToolTipCreator toolTipCreator)
      {
         _toolTipCreator = toolTipCreator;
         InitializeComponent();
      }

      public override void InitializeBinding()
      {
         base.InitializeBinding();
         _screenBinder = new ScreenBinder<IAxis>();

         _screenBinder.Bind(x => x.AxisType).To(axisTypeTextBox);
         _screenBinder.Bind(x => x.NumberMode).To(numberRepresentationComboBox).WithValues(numberRepresentationComboBox.GetValidValues());
         _screenBinder.Bind(x => x.Caption).To(captionTextBox);
         _dimensionBinder = _screenBinder.Bind(x => x.Dimension).To(dimensionComboBox);
         _dimensionBinder.Changed += updateUnits;
         _unitBinder = _screenBinder.Bind(x => x.UnitName).To(unitComboBox);
         _screenBinder.Bind(x => x.Scaling).To(scalingComboBox).WithValues(scalingComboBox.GetValidValues());
         _screenBinder.Bind(x => x.Min).To(minTextBox);
         _screenBinder.Bind(x => x.Max).To(maxTextBox);
         _screenBinder.Bind(x => x.GridLines).To(gridLinesCheckEdit);
         _screenBinder.Bind(x => x.DefaultColor).To(defaultColorColorEdit);
         _screenBinder.Bind(x => x.DefaultLineStyle).To(defaultLineSytleComboBox).WithValues(defaultLineSytleComboBox.GetValidValues());
      }

      public override void InitializeResources()
      {
         base.InitializeResources();
         axisTypeLayoutControlItem.Text = Captions.AxisType.FormatForLabel();
         numberRepresentationLayoutControlItem.Text = Captions.NumberRepresentation.FormatForLabel();
         captionLayoutControlItem.Text = Captions.Caption.FormatForLabel();
         dimensionLayoutControlItem.Text = Captions.Dimension.FormatForLabel();
         unitLayoutControlItem.Text = Captions.Unit.FormatForLabel();
         scalingLayoutControlItem.Text = Captions.Scaling.FormatForLabel();
         minLayoutControlItem.Text = Captions.AxisMinimum.FormatForLabel();
         maxLayoutControlItem.Text = Captions.AxisMaximum.FormatForLabel();
         gridLinesLayoutControlItem.Text = Captions.GridLines.FormatForLabel();
         defaultColorLayoutControlItem.Text = Captions.DefaultColor.FormatForLabel();
         defaultLineStyleLayoutControlItem.Text = Captions.DefaultLineStyle.FormatForLabel();

         axisTypeTextBox.Enabled = false;

         defaultColorColorEdit.SuperTip = _toolTipCreator.CreateToolTip(ToolTips.DefaultCurveColor, ToolTips.DefaultCurveColorTitle);
         defaultLineSytleComboBox.SuperTip = _toolTipCreator.CreateToolTip(ToolTips.DefaultLineStyle, ToolTips.DefaultLineStyleTitle);
      }

      private void updateUnits()
      {
         _unitBinder.WithValues(_presenter.GetUnitsForDimension()).Refresh();
      }

      public void AttachPresenter(ISingleAxisSettingsPresenter presenter)
      {
         _presenter = presenter;
      }

      public void BindToSource(IAxis axis)
      {
         _dimensionBinder.WithValues(_presenter.GetDimensionsForEditor(axis.Dimension));
         updateUnits();
         _screenBinder.BindToSource(axis);
      }

      public void HideDefaultStyles()
      {
         defaultColorLayoutControlItem.Visibility = LayoutVisibilityConvertor.FromBoolean(false);
         defaultLineStyleLayoutControlItem.Visibility = LayoutVisibilityConvertor.FromBoolean(false);
      }
   }
}
