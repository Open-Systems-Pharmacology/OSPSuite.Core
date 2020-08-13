using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using DevExpress.Utils;
using DevExpress.Utils.Extensions;
using DevExpress.Utils.Menu;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraGrid.Menu;
using DevExpress.XtraGrid.Views.Base;
using DevExpress.XtraGrid.Views.Grid;
using OSPSuite.Assets;
using OSPSuite.Core.Importer;
using OSPSuite.Presentation.Importer.Core;
using OSPSuite.Presentation.Importer.Core.DataFormat;
using OSPSuite.Presentation.Importer.Presenters;
using OSPSuite.UI.Controls;
using OSPSuite.UI.Services;

namespace OSPSuite.Presentation.Importer.Views
{
   static class ColumnMapping
   {
      public enum ColumnName
      {
         ColumnName,
         Description,
         Source
      }

      public static string GetName(ColumnName name)
      {
         return name.ToString();
      }
   }

   public partial class ColumnMappingControl : BaseUserControl, IColumnMappingControl
   {
      private readonly IImageListRetriever _imageListRetriever;
      private readonly Dictionary<int, ImageComboBoxEdit> _editorsForEditing = new Dictionary<int, ImageComboBoxEdit>();
      private IColumnMappingPresenter _presenter;
      private IReadOnlyList<ColumnInfo> _columnInfos;
      private IDataFormat _format;

      public ColumnMappingControl(IImageListRetriever imageListRetriever)
      {
         _imageListRetriever = imageListRetriever;
         InitializeComponent();
         uxGridView.OptionsView.ShowGroupPanel = false;
         uxGridView.OptionsMenu.EnableColumnMenu = false;
         //uxGridView.CellValueChanged += (s, e) => validateMapping();

         uxGridView.CustomRowCellEdit += onCustomRowCellEdit;
         uxGridView.CustomRowCellEditForEditing += onCustomRowCellEditForEditing;
         uxGridView.OptionsView.ShowButtonMode = ShowButtonModeEnum.ShowOnlyInEditor;
         uxGridView.OptionsBehavior.EditorShowMode = EditorShowMode.MouseUp;
         uxGridView.MouseDown += onMouseDown;
         uxGrid.ToolTipController = new ToolTipController();
         uxGrid.ToolTipController.GetActiveObjectInfo += onGetActiveObjectInfo;
      }

      public void AttachPresenter(IColumnMappingPresenter presenter)
      {
         _presenter = presenter;
      }

      public void SetSettings(IReadOnlyList<ColumnInfo> columnInfos, IDataFormat format)
      {
         _columnInfos = columnInfos;
         _format = format;
      }

      public void SetMappingSource(IReadOnlyList<ColumnMappingViewModel> mappings)
      {
         uxGrid.DataSource = mappings;

         configureColumn(ColumnMapping.GetName(ColumnMapping.ColumnName.ColumnName), true, false);
         configureColumn(ColumnMapping.GetName(ColumnMapping.ColumnName.Description), true, true);
         configureColumn(ColumnMapping.GetName(ColumnMapping.ColumnName.Source), false, false);
      }

      private void configureColumn(string columnName, bool visible, bool allowEdit = false)
      {
         var col = uxGridView.Columns.ColumnByFieldName(columnName);
         if (visible)
         {
            col.OptionsColumn.AllowEdit = allowEdit;
            col.OptionsColumn.AllowGroup = DefaultBoolean.False;
            col.OptionsColumn.AllowShowHide = false;
            col.OptionsColumn.AllowSort = DefaultBoolean.False;
            col.OptionsFilter.AllowFilter = false;
         }
         else
            col.Visible = false;
      }

      private void onGetActiveObjectInfo(object sender, ToolTipControllerGetActiveObjectInfoEventArgs e)
      {
         if (sender != uxGrid.ToolTipController) return;
         var view = uxGrid.GetViewAt(e.ControlMousePosition) as GridView;
         if (view == null) return;
         var hitInfo = view.CalcHitInfo(e.ControlMousePosition);

         if (!hitInfo.InRow) return;
         e.Info = new ToolTipControlInfo(hitInfo.RowHandle, string.Empty)
         {
            SuperTip = generateToolTipControlInfo(hitInfo.RowHandle),
            ToolTipType = ToolTipType.SuperTip
         };
         uxGrid.ToolTipController.ShowHint(e.Info);
      }

      private SuperToolTip generateToolTipControlInfo(int index)
      {
         var superToolTip = new SuperToolTip();
         var tooltip = _presenter.ToolTipDescriptionFor(index);
         superToolTip.Items.AddTitle(tooltip.Title);
         superToolTip.Items.Add(tooltip.Description);
         return superToolTip;
      }

      private void onCustomRowCellEdit(object sender, CustomRowCellEditEventArgs e)
      {
         if (sender == null) return;
         if (!e.Column.OptionsColumn.AllowEdit) return;

         var editorObject = new ImageComboBoxEdit();
         fillComboBoxItems(editorObject, _presenter.GetAvailableOptionsFor(e.RowHandle));

         if (editorObject.Properties.Items.Count == 0) return;
         e.RepositoryItem = editorObject.Properties;
      }

      private void onCustomRowCellEditForEditing(object sender, CustomRowCellEditEventArgs e)
      {
         var view = sender as GridView;
         if (view == null) return;
         if (!e.Column.OptionsColumn.AllowEdit) return;

         var editorObject = _editorsForEditing.GetOrAdd(e.RowHandle, (index) =>
         {
            var editorObj = new ImageComboBoxEdit();
            editorObj.EditValueChanged += onEditorEditValueChanged;
            Controls.Add(editorObj);
            var editor = editorObj.Properties;

            editor.AutoComplete = true;
            editor.AllowNullInput = DefaultBoolean.True;
            editor.CloseUpKey = new KeyShortcut(Keys.Enter);

            return editorObj;
         });
         // create individual list of values
         fillComboBoxItems(editorObject, _presenter.GetAvailableOptionsFor(e.RowHandle));
         refreshButtons(editorObject);

         if (editorObject.Properties.Items.Count == 0) return;
         e.RepositoryItem = editorObject.Properties;
      }

      private void fillComboBoxItems(ImageComboBoxEdit editor, IEnumerable<ColumnMappingOption> options)
      {
         editor.Properties.Items.Clear();
         editor.Properties.SmallImages = _imageListRetriever.AllImages16x16;
         editor.Properties.NullText = Captions.Importer.NoneEditorNullText;
         foreach (var option in options)
         {
            editor.Properties.Items.Add(new ImageComboBoxItem(option.Description)
            {
               Description = option.Label,
               ImageIndex = option.IconIndex
            });
         }

         editor.Properties.KeyDown += clearSelectionOnDeleteForComboBoxEdit;
         editor.SelectedIndex = 0;
         if (editor.Properties.Items.Count != 0) return;
         editor.Properties.NullText = Captions.Importer.NothingSelectableEditorNullText;
         editor.Enabled = false;
      }

      private void refreshButtons(ButtonEdit editor)
      {
         for (var i = editor.Properties.Buttons.Count - 1; i >= 0; i--)
         {
            var button = editor.Properties.Buttons[i];

            if (button.Kind == ButtonPredefines.Delete)
            {
               editor.Properties.Buttons.RemoveAt(i);
               editor.ButtonClick -= onEditorButtonClick;
            }

            if (button.Kind == ButtonPredefines.Glyph)
               editor.Properties.Buttons.RemoveAt(i);
         }

         createButtons(editor);
      }

      private void onEditorButtonClick(object sender, ButtonPressedEventArgs e)
      {
         var editor = sender as ImageComboBoxEdit;
         if (editor == null) return;
         if (editor.EditValue == null) return;

         if (e.Button.Caption == MenuNames.DeleteSubMenu)
         {
            editor.EditValue = ColumnMappingFormatter.Ignored();
            _presenter.ClearActiveRow();
            return;
         }
         else //unit information button
         {
            //open edit view
            //TODO: This should go in the presenter
            var icon = (ParentForm == null) ? ApplicationIcons.EmptyIcon : ParentForm.Icon;
            var activeRow = (_presenter.ActiveRow().Source as MappingDataFormatParameter).MappedColumn;
            var frm = new SetUnitView(activeRow, _columnInfos.First(i => i.DisplayName == activeRow.Name.ToString()).DimensionInfos.Select(d => d.Dimension), _format) { StartPosition = FormStartPosition.CenterParent, Icon = icon };
            frm.OnCopyUnitInfo += onCopyUnitInfo;

            uxGrid.BeginUpdate();
            if (frm.ShowDialog() == DialogResult.OK)
            {
            }
            uxGrid.EndUpdate();
         }
      }

      void onCopyUnitInfo(object sender, SetUnitView.CopyUnitInfoEventArgs e)
      {
         /*foreach (var cm in Mapping)
         {
            if (cm.Target != _contextMappingRow.Target) continue;

            var tableColumn = getTableColumn(cm);

            if (tableColumn.Dimensions == null) continue;
            tableColumn.ActiveDimension = DimensionHelper.FindDimension(tableColumn.Dimensions, e.Dimension.Name);
            DimensionHelper.TakeOverInputParameters(e.Dimension, tableColumn.ActiveDimension);
            cm.InputParameters = tableColumn.ActiveDimension.InputParameters;
            tableColumn.ActiveUnit = tableColumn.ActiveDimension.FindUnit(e.Unit.Name);
            tableColumn.IsUnitExplicitlySet = true;
            cm.SelectedUnit = tableColumn.ActiveUnit;
            cm.IsUnitExplicitlySet = tableColumn.IsUnitExplicitlySet;
         }
         _grid.MainView?.RefreshData();*/
      }

      private void createButtons(ButtonEdit editor)
      {
         editor.ButtonClick += onEditorButtonClick;
         var buttonsConfiguration = _presenter.ButtonsConfigurationForActiveRow();
         if (buttonsConfiguration.ShowButtons)
         {
            createDeleteButton(editor);
            createUnitInformationButton(editor, buttonsConfiguration.UnitActive);
         }
      }

      private void onMouseDown(object sender, MouseEventArgs mouseEventArgs)
      {
         if (mouseEventArgs.Button != MouseButtons.Right) return;
         var mv = sender as GridView;
         if (mv == null) return;

         var menu = new GridViewColumnMenu(mv);
         menu.Items.Clear();
         menu.Items.Add(new DXMenuItem(Captions.Importer.ResetMapping, onCreateAutoMappingClick));
         menu.Items.Add(new DXMenuItem(Captions.Importer.ClearMapping, onClearMappingClick));
         menu.Show(mouseEventArgs.Location);
      }

      private static void clearSelectionOnDeleteForComboBoxEdit(object sender, KeyEventArgs e)
      {
         var comboBoxEdit = sender as ImageComboBoxEdit;
         var gridControl = comboBoxEdit?.Parent as UxGridControl;
         var view = gridControl?.FocusedView as ColumnView;
         if (view == null) return;

         if (e.KeyCode == Keys.Delete)
            view.ActiveEditor.EditValue = Captions.Importer.NoneEditorNullText;
      }

      private void onEditorEditValueChanged(object sender, EventArgs e)
      {
         var editor = sender as ImageComboBoxEdit;
         if (editor == null) return;

         uxGrid.MainView.PostEditor();

         if (editor.EditValue != null && editor.EditValue.ToString() == editor.Properties.NullText)
            editor.EditValue = Captions.Importer.NoneEditorNullText;

         _presenter.SetDescriptionForActiveRow(editor.EditValue?.ToString());
         refreshButtons(editor);
      }

      private static void createDeleteButton(ButtonEdit editor)
      {
         var deleteButton = new EditorButton(ButtonPredefines.Delete)
         {
            Shortcut = new KeyShortcut(Keys.Control | Keys.D),
            Caption = MenuNames.DeleteSubMenu,
            Enabled = true
         };
         editor.Properties.Buttons.Add(deleteButton);
      }

      private static void createUnitInformationButton(ButtonEdit editor, bool visible)
      {
         var unitInformationTip = new SuperToolTip();
         unitInformationTip.Items.Add(Captions.UnitInformationDescription);
         var unitInformationButton = new EditorButton(ButtonPredefines.Glyph,
                                                      ApplicationIcons.UnitInformation.ToImage(),
                                                      unitInformationTip)
         {
            Shortcut = new KeyShortcut(Keys.Control | Keys.I),
            Caption = Captions.UnitInformationCaption,
            Enabled = visible
         };
         editor.Properties.Buttons.Add(unitInformationButton);
      }

      private void onCreateAutoMappingClick(object sender, EventArgs eventArgs)
      {
         uxGridView.BeginUpdate();
         _presenter.ResetMapping();
         uxGridView.EndUpdate();
      }

      private void onClearMappingClick(object sender, EventArgs eventArgs)
      {
         uxGridView.BeginUpdate();
         _presenter.ClearMapping();
         uxGridView.EndUpdate();
      }
   }
}