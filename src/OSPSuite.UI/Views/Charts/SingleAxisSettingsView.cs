using System.Windows.Forms;
using OSPSuite.DataBinding;
using OSPSuite.DataBinding.DevExpress;
using DevExpress.XtraLayout.Utils;
using OSPSuite.Assets;
using OSPSuite.Core.Chart;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Presentation.Extensions;
using OSPSuite.Presentation.Presenters.Charts;
using OSPSuite.Presentation.Views;
using OSPSuite.Presentation.Views.Charts;
using OSPSuite.UI.Services;

namespace OSPSuite.UI.Views.Charts
{
   public partial class SingleAxisSettingsView : BaseModalView, ISingleAxisSettingsView
   {
      private readonly ScreenBinder<Axis> _screenBinder;
      private ISingleAxisSettingsPresenter _presenter;
      private ComboBoxEditElementBinder<Axis, IDimension> _dimensionBinder;
      private ComboBoxEditElementBinder<Axis, string> _unitBinder;
      private readonly IToolTipCreator _toolTipCreator;

      public SingleAxisSettingsView(IShell shell, IToolTipCreator toolTipCreator) : base(shell)
      {
         InitializeComponent();
         _toolTipCreator = toolTipCreator;
         _screenBinder = new ScreenBinder<Axis>();
      }

      public override void InitializeBinding()
      {
         base.InitializeBinding();

         _screenBinder.Bind(x => x.AxisType)
            .To(axisTypeTextBox);

         _screenBinder.Bind(x => x.NumberMode)
            .To(numberRepresentationComboBox)
            .WithValues(numberRepresentationComboBox.GetValidValues());

         _screenBinder.Bind(x => x.Caption)
            .To(captionTextBox);

         _dimensionBinder = _screenBinder.Bind(x => x.Dimension)
            .To(dimensionComboBox);
         _dimensionBinder.Changed += updateUnits;

         _unitBinder = _screenBinder.Bind(x => x.UnitName)
            .To(unitComboBox);

         _screenBinder.Bind(x => x.Scaling)
            .To(scalingComboBox)
            .WithValues(scalingComboBox.GetValidValues());

         _screenBinder.Bind(x => x.Min)
            .To(minTextBox);

         _screenBinder.Bind(x => x.Max)
            .To(maxTextBox);

         _screenBinder.Bind(x => x.GridLines)
            .To(gridLinesCheckEdit);

         _screenBinder.Bind(x => x.DefaultColor)
            .To(defaultColorColorEdit);

         _screenBinder.Bind(x => x.DefaultLineStyle)
            .To(defaultLineSytleComboBox)
            .WithValues(defaultLineSytleComboBox.GetValidValues());
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
         FormBorderStyle = FormBorderStyle.SizableToolWindow;
         Caption =  Captions.AxisSettings;
      }

      private void updateUnits()
      {
         _unitBinder.WithValues(_presenter.AllUnitsForDimension()).Refresh();
      }

      public void AttachPresenter(ISingleAxisSettingsPresenter presenter)
      {
         _presenter = presenter;
      }

      public void BindTo(Axis axis)
      {
         _dimensionBinder.WithValues(_presenter.AllDimensionsForEditor(axis.Dimension));
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
