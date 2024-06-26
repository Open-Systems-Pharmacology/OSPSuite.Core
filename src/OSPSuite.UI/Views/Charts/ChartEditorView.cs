﻿using System;
using System.IO;
using OSPSuite.Utility.Extensions;
using DevExpress.Utils;
using DevExpress.XtraBars;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Repository;
using OSPSuite.Assets;
using OSPSuite.Presentation.MenuAndBars;
using OSPSuite.Presentation.Presenters.Charts;
using OSPSuite.Presentation.Views;
using OSPSuite.Presentation.Views.Charts;
using OSPSuite.UI.Controls;
using OSPSuite.UI.Extensions;
using OSPSuite.UI.Mappers;
using OSPSuite.UI.Services;

namespace OSPSuite.UI.Views.Charts
{
   public partial class ChartEditorView : BaseUserControl, IChartEditorView
   {
      private readonly IMenuBarItemToBarItemMapper _barItemMapper;
      private IChartEditorPresenter _presenter;
      private readonly SvgImageCollection _allImages;
      private readonly BarEditItem _barEditItemForUsedIn;
      private readonly BarEditItem _barEditItemLinkSimulationObserved;

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
         var repositoryItemCheckEditForUsedIn = new RepositoryItemCheckEdit
         {
            UseParentBackground = true, 
            Caption = string.Empty,
            GlyphAlignment = HorzAlignment.Near,
            AutoWidth = false
         };

         var repositoryItemCheckEditLinkSimulationAndObserved = CreateLinkSimulationAndObservedRepositoryItem();

         _barEditItemForUsedIn = new BarEditItem(_barManager)
         {
            Edit = repositoryItemCheckEditForUsedIn,
            Alignment = BarItemLinkAlignment.Right,
            AutoFillWidth = false,
            Width = 20, 
            Caption = Captions.UseSelected,
            CaptionAlignment = HorzAlignment.Near,
            PaintStyle = BarItemPaintStyle.Caption
         };

         _barEditItemForUsedIn.SuperTip = new SuperToolTip().WithText(ToolTips.UseSelectedCurvesToolTip);

         _barEditItemLinkSimulationObserved = new BarEditItem(_barManager)
         {
            Edit = repositoryItemCheckEditLinkSimulationAndObserved,
            Alignment = BarItemLinkAlignment.Right,
            AutoFillWidth = false,
            Width = 20,
            Caption = Captions.LinkDataToSimulations,
            CaptionAlignment = HorzAlignment.Near,
            PaintStyle = BarItemPaintStyle.Caption
         };

         _barEditItemLinkSimulationObserved.SuperTip = new SuperToolTip().WithText(ToolTips.LinkSimulationObservedToolTip);

         repositoryItemCheckEditForUsedIn.EditValueChanged += (o, e) => OnEvent(() => changeUsed(o));

         repositoryItemCheckEditLinkSimulationAndObserved.EditValueChanged += (o, e) => OnEvent(() => changeLinkSimulationToData(o));

         repositoryItemCheckEditForUsedIn.ValueGrayed = null;

         _barEditItemLinkSimulationObserved.EditValue = false;
         _barEditItemLinkSimulationObserved.Visibility = BarItemVisibility.Never;
      }

      private static RepositoryItemCheckEdit CreateLinkSimulationAndObservedRepositoryItem()
      {
         var repositoryItemCheckEditLinkSimulationAndObserved = new RepositoryItemCheckEdit
         {
            UseParentBackground = true,
            Caption = string.Empty,
            GlyphAlignment = HorzAlignment.Near,
            AutoWidth = false
         };
         return repositoryItemCheckEditLinkSimulationAndObserved;
      }

      private void changeUsed(object sender)
      {
         var checkEdit = sender as CheckEdit;
         if (checkEdit == null) return;

         _presenter.UpdateUsedForSelection(checkEdit.Checked);
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

      public void AddUsedInMenuItemCheckBox()
      {
         _barMenu.AddItem(_barEditItemForUsedIn);
      }

      public void AddLinkSimulationObservedMenuItemCheckBox()
      {
         _barMenu.AddItem(_barEditItemLinkSimulationObserved);
      }

      public void SetlinkSimDataMenuItemVisisbility(bool isVisible)
      {
         _barEditItemLinkSimulationObserved.Visibility = isVisible ? BarItemVisibility.Always : BarItemVisibility.Never;
      }

      public void SetSelectAllCheckBox(bool? checkedState)
      {
         _barEditItemForUsedIn.EditValue = checkedState;
      }

      public void AttachPresenter(IChartEditorPresenter presenter)
      {
         _presenter = presenter;
      }

      public void SetDataBrowserView(IView view)
      {
         panelDataBrowser.FillWith(view);
      }

      public void SetCurveSettingsView(IView view)
      {
         panelCurveOptions.FillWith(view);
      }

      public void SetAxisSettingsView(IView view)
      {
         panelAxisOptions.FillWith(view);
      }

      public void SetChartSettingsView(IView view)
      {
         panelChartSettings.FillWith(view);
      }

      public void SetCurveColorGroupingView(IView view)
      {
         panelCurveColorGrouping.FillWith(view);
      }

      public void SetChartExportSettingsView(IView view)
      {
         panelChartExportSettings.FillWith(view);
      }

      public override void InitializeResources()
      {
         base.InitializeResources();
         layoutColorGrouping.Text = Captions.CurvesColorGrouping;
         layoutChartExportOptions.Text = Captions.ChartExportOptions;
         layoutChartOptions.Text = Captions.ChartOptions;
         layoutCurveAndChartSettings.Text = Captions.CurveAndAxisSettings;
      }
   }
}