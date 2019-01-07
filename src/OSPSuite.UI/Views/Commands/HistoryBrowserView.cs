using System;
using System.Globalization;
using System.Linq;
using OSPSuite.Utility.Collections;
using OSPSuite.Utility.Extensions;
using DevExpress.Utils;
using DevExpress.XtraBars;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraTreeList;
using DevExpress.XtraTreeList.Columns;
using DevExpress.XtraTreeList.Nodes;
using OSPSuite.Assets;
using OSPSuite.Presentation.DTO.Commands;
using OSPSuite.Presentation.Presenters.Commands;
using OSPSuite.Presentation.Views.Commands;
using OSPSuite.UI.Services;

namespace OSPSuite.UI.Views.Commands
{
   public partial class HistoryBrowserView : XtraUserControl, IHistoryBrowserView
   {
      private readonly IHistoryBrowserConfiguration _historyBrowserConfiguration;
      private IHistoryBrowserPresenter _presenter;
      private readonly ICache<ColumnProperties, TreeListColumn> _columnCache = new Cache<ColumnProperties, TreeListColumn>();

      public HistoryBrowserView(IImageListRetriever imageListRetriever, IHistoryBrowserConfiguration historyBrowserConfiguration)
      {
         _historyBrowserConfiguration = historyBrowserConfiguration;
         InitializeComponent();
         _barManager.Images = imageListRetriever.AllImages16x16;
         initializeResources();
         initializeBinding();
         historyLayoutControl.AllowCustomization = false;
      }

      private void initializeResources()
      {
         historyTreeList.OptionsView.ShowPreview = true;
         historyTreeList.OptionsView.ShowIndicator = false;
         historyTreeList.PreviewFieldName = Captions.Commands.Comment;
         historyTreeList.BestFitVisibleOnly = true;
         initializeButton(btnUndo, ApplicationIcons.Undo, Captions.Commands.Undo, () => _presenter.Undo(), ToolTips.Commands.Undo);
         initializeButton(btnAddLabel, ApplicationIcons.LabelAdd, Captions.Commands.AddLabel, () => _presenter.AddLabel(), ToolTips.Commands.AddLabel);
         initializeButton(btnEditComment, ApplicationIcons.Edit, Captions.Commands.EditComment, () => _presenter.EditCommentFor(historyItemIdFrom(historyTreeList.FocusedNode)), ToolTips.Commands.EditComment);
         initializeButton(btnClearHistory, ApplicationIcons.ClearHistory, Captions.Commands.ClearHistory, () => _presenter.ClearHistory(), ToolTips.ClearHistory);
         lblRollBack.Caption = Captions.Commands.Rollback;
         tbRollBackState.Caption = Captions.Commands.Rollback;
         tbRollBackState.EditValue = 0;
         tbRollBackStateEditor.ButtonClick += (o, e) => _presenter.RollBack(tbRollBackState.EditValue.ConvertedTo<int>());
         tbRollBackStateEditor.Buttons[0].Kind = ButtonPredefines.Glyph;
         tbRollBackStateEditor.Buttons[0].Image = ApplicationIcons.Run.ToImage(IconSizes.Size16x16);
         tbRollBackStateEditor.Buttons[0].ToolTip = tbRollBackState.Caption;
         tbRollBackStateEditor.TextEditStyle = TextEditStyles.DisableTextEditor;
         _barManager.AllowCustomization = false;
         _barManager.AllowQuickCustomization = true;
      }

      private void initializeBinding()
      {
         historyTreeList.OptionsSelection.EnableAppearanceFocusedCell = false;
         historyTreeList.NodeCellStyle += historyTreeListNodeCellStyle;
         historyTreeList.ToolTipController = new ToolTipController();
         historyTreeList.ToolTipController.GetActiveObjectInfo += (o, e) => onGetActiveObjectInfo(e);
      }

      private void onGetActiveObjectInfo(ToolTipControllerGetActiveObjectInfoEventArgs e)
      {
         if (e.SelectedControl != historyTreeList) return;
         if (historyTreeList.DataSource == null) return;

         //Get the view's element information that resides at the current position
         var hi = historyTreeList.CalcHitInfo(e.ControlMousePosition);
         if (hi == null) return;
         if (hi.Node == null) return;
         if (hi.Column == null) return;
         if (hi.Column.FieldName != HistoryColumns.Description.Name) return;
         var historyItem = historyItemFrom(hi.Node);
         if (historyItem == null) return;

         //An object that uniquely identifies a row cell
         var info = new ToolTipControlInfo(historyItem, string.Empty);
         info.SuperTip = new SuperToolTip();
         info.SuperTip.Appearance.TextOptions.HotkeyPrefix = HKeyPrefix.None;
         addItemToToolTip(info.SuperTip, historyItem.Description);
         addItemToToolTip(info.SuperTip, historyItem.ExtendedDescription);

         //at least two elements to display (there is always one element because of the title)
         if (info.SuperTip.Items.Count == 1) return;
         e.Info = info;
      }

      private void initializeButton(BarButtonItem barButtonItem, ApplicationIcon icon, string caption, Action clickAction, string toolTipText)
      {
         barButtonItem.Caption = caption;
         barButtonItem.ImageIndex = ApplicationIcons.IconIndex(icon);
         barButtonItem.PaintStyle = BarItemPaintStyle.CaptionGlyph;
         barButtonItem.ButtonStyle = BarButtonStyle.Check;
         barButtonItem.ItemClick += (o, e) => clickAction();
         var toolTip = new SuperToolTip();
         toolTip.Setup(new SuperToolTipSetupArgs {Contents = {Text = toolTipText}});
         barButtonItem.SuperTip = toolTip;
      }

      private void addItemToToolTip(SuperToolTip superTip, string itemToAdd)
      {
         if (string.IsNullOrEmpty(itemToAdd)) return;
         superTip.Items.Add(itemToAdd);
      }

      private IHistoryItemDTO historyItemFrom(TreeListNode node)
      {
         var id = historyItemIdFrom(node);
         return _presenter.HistoryItemFrom(id);
      }

      private string historyItemIdFrom(TreeListNode node)
      {
         if (node == null) return string.Empty;
         var idValue = node.GetValue(HistoryColumns.Id.Name);
         return idValue != null ? idValue.ToString() : string.Empty;
      }

      public void AttachPresenter(IHistoryBrowserPresenter presenter)
      {
         _presenter = presenter;
      }

      public void BindTo(IHistoryItemDTOList historyItems)
      {
         historyTreeList.DataSource = historyItems;
         RefreshView();
      }

      public void Unbind()
      {
         BindTo(null);
      }

      public void AddColumn(ColumnProperties columnToAdd)
      {
         _columnCache.Add(columnToAdd, addColumn(columnToAdd));
      }

      private TreeListColumn addColumn(ColumnProperties columnProperties)
      {
         var col = historyTreeList.Columns.Add();
         col.Caption = columnProperties.Caption;
         col.OptionsColumn.AllowEdit = !columnProperties.IsReadOnly;

         //visible index can only be set when all columns have been added!
         if (!columnProperties.IsVisible)
            col.VisibleIndex = -1;
         return col;
      }

      public void RefreshView()
      {
         historyTreeList.RefreshDataSource();
         historyTreeList.ForceInitialize();
      }

      public void Clear()
      {
         historyTreeList.Columns.Clear();
      }

      public bool EnableFiltering
      {
         set
         {
            historyTreeList.OptionsBehavior.EnableFiltering = value;
            historyTreeList.OptionsFilter.AllowFilterEditor = true;
            historyTreeList.OptionsFilter.FilterMode = FilterMode.Smart;
            historyTreeList.OptionsFilter.ShowAllValuesInFilterPopup = true;
         }
      }

      public bool EnableAutoFilterRow
      {
         set { historyTreeList.OptionsView.ShowAutoFilterRow = value; }
      }

      public void UpdateColumnPosition()
      {
         foreach (var columnProperty in _columnCache.Keys.Where(x => x.IsVisible).OrderBy(x => x.Position))
         {
            var treeColumn = _columnCache[columnProperty];
            treeColumn.VisibleIndex = columnProperty.Position;
         }
      }

      public void BestFitColumns()
      {
         historyTreeList.BestFitColumns();
      }

      private void historyTreeListFocusedNodeChanged(object sender, FocusedNodeChangedEventArgs e)
      {
         if (e.Node == null) return;
         var historyItem = historyItemIdFrom(e.Node);
         tbRollBackState.EditValue = _presenter.StateFor(historyItem).ToString(CultureInfo.InvariantCulture);
         tbRollBackState.Enabled = _presenter.CanRollBackTo(historyItem);
         tbRollBackStateEditor.Enabled = _presenter.CanRollBackTo(historyItem);
      }

      private void historyTreeListNodeCellStyle(object sender, GetCustomNodeCellStyleEventArgs e)
      {
         if (e.Node == null) return;
         var historyItemId = historyItemIdFrom(e.Node);

         if (_presenter.IsLabel(historyItemId))
            e.Appearance.BackColor = _historyBrowserConfiguration.LabelColor;
         else if (!_presenter.CanRollBackTo(historyItemId))
            e.Appearance.BackColor = _historyBrowserConfiguration.NotReversibleColor;
      }
   }
}