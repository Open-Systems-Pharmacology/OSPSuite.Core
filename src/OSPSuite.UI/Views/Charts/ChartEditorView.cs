using System;
using System.IO;
using System.Windows.Forms;
using DevExpress.Utils;
using DevExpress.XtraBars;
using DevExpress.XtraEditors;
using DevExpress.XtraLayout.Utils;
using OSPSuite.Assets;
using OSPSuite.Presentation.MenuAndBars;
using OSPSuite.Presentation.Presenters.Charts;
using OSPSuite.Presentation.Views;
using OSPSuite.Presentation.Views.Charts;
using OSPSuite.UI.Controls;
using OSPSuite.UI.Extensions;
using OSPSuite.UI.Mappers;
using OSPSuite.UI.Services;
using OSPSuite.Utility.Extensions;

namespace OSPSuite.UI.Views.Charts
{
   public partial class ChartEditorView : BaseUserControl, IChartEditorView
   {
      private const int CHECKEDIT_WIDTH = 160;
      private readonly IMenuBarItemToBarItemMapper _barItemMapper;
      private IChartEditorPresenter _presenter;
      private readonly SvgImageCollection _allImages;

      public ChartEditorView(IMenuBarItemToBarItemMapper barItemMapper, IImageListRetriever imageListRetriever)
      {
         _barItemMapper = barItemMapper;
         InitializeComponent();
         //to avoid grouping state being lost
         layoutControl.UseLocalBindingContext = true;
         layoutControl.AllowCustomization = false;
         _allImages = imageListRetriever.AllImages16x16;
         _barManager.Images = _allImages;
         _barManager.MainMenu = null;
         _barManager.TransparentEditors = true;
         _barMenu.OptionsBar.AllowQuickCustomization = false;
      }

      public override void InitializeResources()
      {
         base.InitializeResources();
         layoutColorGrouping.Text = Captions.CurvesColorGrouping;
         layoutChartExportOptions.Text = Captions.ChartExportOptions;
         layoutChartOptions.Text = Captions.ChartOptions;
         layoutCurveAndChartSettings.Text = Captions.CurveAndAxisSettings;

         btnApplyChartUpdates.Text = Captions.ApplyUpdates;
         chkAutoUpdateCharts.Text = Captions.AutoUpdateChart;
         chkAutoUpdateCharts.SuperTip = new SuperToolTip().WithText(ToolTips.EnableOrDisableAutomaticUpdateOfTheChartForEachEdit);
         chkUsedIn.Text = Captions.UseSelected;
         chkUsedIn.Properties.AllowGrayed = true;
         chkUsedIn.SuperTip = new SuperToolTip().WithText(ToolTips.UseSelectedCurvesToolTip);
         chkLinkSimulationObserved.Text = Captions.LinkDataToSimulations;
         chkLinkSimulationObserved.SuperTip = new SuperToolTip().WithText(ToolTips.LinkSimulationObservedToolTip);

         chkUsedIn.EditValueChanged += (o, e) => OnEvent(() => changeUsed(o));
         chkLinkSimulationObserved.EditValueChanged += (o, e) => OnEvent(() => changeLinkSimulationToData(o));
         chkAutoUpdateCharts.EditValueChanged += (o, e) => OnEvent(() => changeAutoUpdateCharts(o));
         btnApplyChartUpdates.Click += (o, e) => OnEvent(() => _presenter.UpdateChartDisplay());

         layoutControlItemUsedIn.AdjustSize(CHECKEDIT_WIDTH, layoutControlItemUsedIn.Height, layoutControl);
         layoutControlItemLink.AdjustSize(CHECKEDIT_WIDTH, layoutControlItemLink.Height, layoutControl);
         layoutControlItemAutoUpdateCharts.AdjustSize(CHECKEDIT_WIDTH, layoutControlItemAutoUpdateCharts.Height, layoutControl);
         layoutControlItemApplyButton.AdjustButtonSize();

         layoutControlItemLink.Visibility = LayoutVisibility.Never;
      }

      private void changeUsed(object sender)
      {
         var checkEdit = sender as CheckEdit;
         if (checkEdit == null) return;

         // Do not update selection for indeterminate state
         if (checkEdit.CheckState != CheckState.Indeterminate)
            _presenter.UpdateUsedForSelection(checkEdit.Checked);
      }

      private void changeAutoUpdateCharts(object sender)
      {
         var checkEdit = sender as CheckEdit;
         if (checkEdit == null) return;

         _presenter.UpdateAutoUpdateChartMode(autoMode: checkEdit.Checked);
         btnApplyChartUpdates.Enabled = !checkEdit.Checked;
      }

      private void changeLinkSimulationToData(object sender)
      {
         var checkEdit = sender as CheckEdit;
         if (checkEdit == null) return;

         _presenter.UpdateLinkSimulationToDataSelection(checkEdit.Checked);
      }

      protected override void SetVisibleCore(bool value)
      {
         base.SetVisibleCore(value);
         panelAxisOptions.Refresh();
         panelChartSettings.Refresh();
         panelCurveOptions.Refresh();
         panelDataBrowser.Refresh();
         panelCurveColorGrouping.Refresh();
      }

      protected override void OnVisibleChanged(EventArgs e)
      {
         base.OnVisibleChanged(e);
         if (!Visible) return;

         panelAxisOptions.Refresh();
         panelCurveOptions.Refresh();
      }

      public void AddButton(IMenuBarItem menuBarItem)
      {
         var button = _barItemMapper.MapFrom(_barManager, menuBarItem);
         //required to set the image in the top menu
         if (button.ImageIndex >= 0)
         {
            button.ImageOptions.SvgImage = _allImages[button.ImageIndex];
            button.PaintStyle = BarItemPaintStyle.CaptionGlyph;
         }

         _barMenu.AddItem(button);
      }

      public string SaveLayoutToString()
      {
         var streamMainView = new MemoryStream();
         layoutControl.SaveLayoutToStream(streamMainView);
         return streamToString(streamMainView);
      }

      public void LoadLayoutFromString(string layoutString)
      {
         if (!string.IsNullOrEmpty(layoutString))
            layoutControl.RestoreLayoutFromStream(streamFromString(layoutString));
      }

      public void ShowCustomizationForm()
      {
         layoutControl.ShowCustomizationForm();
      }

      private MemoryStream streamFromString(string stringToConvert)
      {
         return new MemoryStream(stringToConvert.ToByteArray());
      }

      private string streamToString(MemoryStream streamToConvert)
      {
         return streamToConvert.ToArray().ToByteString();
      }

      public void ClearButtons()
      {
         _barManager.Items.Clear();
         _barMenu.ItemLinks.Clear();
         _barMenu.LinksPersistInfo.Clear();
      }

      public void AddUsedInMenuItemCheckBox() => layoutControlItemUsedIn.Visibility = LayoutVisibility.Always;

      public void AddLinkSimulationObservedMenuItemCheckBox() => layoutControlItemLink.Visibility = LayoutVisibility.Always;

      public void SetlinkSimDataMenuItemVisisbility(bool isVisible) => layoutControlItemLink.Visibility = isVisible ? LayoutVisibility.Always : LayoutVisibility.Never;

      public void SetSelectAllCheckBox(bool? checkedState)
      {
         if (checkedState.HasValue)
            chkUsedIn.CheckState = checkedState.Value ? CheckState.Checked : CheckState.Unchecked;
         else
            chkUsedIn.CheckState = CheckState.Indeterminate;
      }

      public void SetAutoUpdateModeCheckBox(bool? checkedState) => chkAutoUpdateCharts.EditValue = checkedState;

      public void AttachPresenter(IChartEditorPresenter presenter) => _presenter = presenter;

      public void SetDataBrowserView(IView view) => panelDataBrowser.FillWith(view);

      public void SetCurveSettingsView(IView view) => panelCurveOptions.FillWith(view);

      public void SetAxisSettingsView(IView view) => panelAxisOptions.FillWith(view);

      public void SetChartSettingsView(IView view) => panelChartSettings.FillWith(view);

      public void SetCurveColorGroupingView(IView view) => panelCurveColorGrouping.FillWith(view);

      public void SetChartExportSettingsView(IView view) => panelChartExportSettings.FillWith(view);
   }
}