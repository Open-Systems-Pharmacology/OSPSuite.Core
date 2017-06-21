using OSPSuite.DataBinding;
using OSPSuite.DataBinding.DevExpress;
using OSPSuite.Utility;
using DevExpress.XtraLayout.Utils;
using OSPSuite.Assets;
using OSPSuite.Core.Chart;
using OSPSuite.Core.Domain;
using OSPSuite.Presentation.Extensions;
using OSPSuite.Presentation.Presenters.Charts;
using OSPSuite.Presentation.Views.Charts;
using OSPSuite.UI.Controls;

namespace OSPSuite.UI.Views.Charts
{
   internal partial class ChartSettingsView : BaseUserControl, IChartSettingsView
   {
      private IChartSettingsPresenter _presenter;
      private readonly ScreenBinder<ChartSettings> _settingsBinder;
      private readonly ScreenBinder<IWithName> _nameBinder;
      private readonly ScreenBinder<IChart> _curveChartBinder;

      public ChartSettingsView()
      {
         InitializeComponent();
         _settingsBinder = new ScreenBinder<ChartSettings> { BindingMode = BindingMode.TwoWay };
         _nameBinder = new ScreenBinder<IWithName> { BindingMode = BindingMode.TwoWay };
         _curveChartBinder = new ScreenBinder<IChart> { BindingMode = BindingMode.TwoWay };
      }

      public override void InitializeResources()
      {
         base.InitializeResources();
         diagramBackgroundControlItem.Text = Captions.DiagramBackground.FormatForLabel();
         chartColorControlItem.Text = Captions.ChartColor.FormatForLabel();
         sideMarginsEnabledCheckEdit.Text = Captions.SideMarginsEnabled.FormatForLabel(checkCase: true, addColon: false);
         legendPositionControlItem.Text = Captions.LegendPosition.FormatForLabel();
         descriptionControlItem.Text = Captions.Description.FormatForLabel();
         titleControlItem.Text = Captions.Title.FormatForLabel();
         nameControlItem.Text = Captions.Name.FormatForLabel();
      }

      public void AttachPresenter(IChartSettingsPresenter presenter)
      {
         _presenter = presenter;
      }

      public void BindTo(IChart chart)
      {
         bindCommonProperties(chart);
         bindName(chart);
         _curveChartBinder.BindToSource(chart);

         Refresh();
      }

      public void DeleteBinding()
      {
         _settingsBinder.DeleteBinding();
         _nameBinder.DeleteBinding();
         _curveChartBinder.DeleteBinding();
      }

      public bool NameVisible
      {
         get => nameControlItem.Visible;
         set => nameControlItem.Visibility = LayoutVisibilityConvertor.FromBoolean(value);
      }

      public void BindTo(CurveChartTemplate chartTemplate)
      {
         bindCommonProperties(chartTemplate);
         bindName(chartTemplate);
         hideChartOnlyFields();
      }

      private void bindName(IWithName chartTemplate)
      {
         _nameBinder.BindToSource(chartTemplate);
      }

      private void hideChartOnlyFields()
      {
         descriptionControlItem.Visibility = LayoutVisibilityConvertor.FromBoolean(false);
         titleControlItem.Visibility = LayoutVisibilityConvertor.FromBoolean(false);
      }

      private void bindCommonProperties(IChartManagement withChartSettings)
      {
         _settingsBinder.BindToSource(withChartSettings.ChartSettings);
      }

      public override void InitializeBinding()
      {
         _nameBinder.Bind(c => c.Name)
            .To(nameTextBox)
            .OnValueUpdated += notifyChartSettingsChanged;

         _curveChartBinder.Bind(c => c.Title)
            .To(titleTextBox)
            .OnValueUpdated += notifyChartSettingsChanged;

         _curveChartBinder.Bind(c => c.Description)
            .To(descriptionTextBox)
            .OnValueUpdated += notifyChartSettingsChanged;

         _settingsBinder.Bind(c => c.SideMarginsEnabled)
            .To(sideMarginsEnabledCheckEdit)
            .OnValueUpdated += notifyChartSettingsChanged;

         _settingsBinder.Bind(c => c.LegendPosition)
            .To(legendPositionComboBoxEdit)
            .WithValues(EnumHelper.AllValuesFor<LegendPositions>())
            .OnValueUpdated += notifyChartSettingsChanged;

         _settingsBinder.Bind(c => c.BackColor)
            .To(backgroundColorColorEdit)
            .OnValueUpdated += notifyChartSettingsChanged;

         _settingsBinder.Bind(c => c.DiagramBackColor)
            .To(diagramBackgroundColorColorEdit)
            .OnValueUpdated += notifyChartSettingsChanged;

         RegisterValidationFor(_settingsBinder, statusChangedNotify:NotifyViewChanged);
         RegisterValidationFor(_curveChartBinder, statusChangedNotify: NotifyViewChanged);
         RegisterValidationFor(_nameBinder, statusChangedNotify: NotifyViewChanged);
      }

      private void notifyChartSettingsChanged<T>(object sender, T e)
      {
         _presenter.NotifyChartSettingsChanged();
      }
   }
}