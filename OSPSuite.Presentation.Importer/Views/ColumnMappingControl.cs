using System;
using System.Collections.Generic;
using System.Windows.Forms;
using DevExpress.Utils;
using DevExpress.Utils.Extensions;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraGrid.Views.Base;
using DevExpress.XtraGrid.Views.Grid;
using OSPSuite.Assets;
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
            /*if (String.IsNullOrEmpty(mappingRow.Target)) return;
            var tableColumn = getTableColumn(mappingRow);
            if (tableColumn?.Dimensions == null) return;
            var sourceColumn = _sourceTable.Columns[mappingRow.SourceColumn];
            var unit = _importerTask.GetUnit(sourceColumn);
            if (!String.IsNullOrEmpty(unit) && tableColumn.SupportsUnit(unit) && !tableColumn.IsUnitExplicitlySet)
            {
               _importerTask.SetColumnUnit(tableColumn, unit, true);
               mappingRow.IsUnitExplicitlySet = true;
            }
            else
            {
               if (mappingRow.SelectedUnit.Name != null)
                  if (tableColumn.SupportsUnit(mappingRow.SelectedUnit.Name))
                     _importerTask.SetColumnUnit(tableColumn, mappingRow.SelectedUnit.Name, mappingRow.IsUnitExplicitlySet);
            }
            if (mappingRow.InputParameters != null && tableColumn.ActiveDimension.InputParameters != null)
               tableColumn.ActiveDimension.InputParameters = new List<InputParameter>(mappingRow.InputParameters);
            if (mappingRow.MetaData != null)
               tableColumn.MetaData = mappingRow.MetaData.Copy();

            //open edit view
            var icon = (ParentForm == null) ? ApplicationIcons.EmptyIcon : ParentForm.Icon;
            var frm = new SetUnitView(tableColumn, _importerTask) { StartPosition = FormStartPosition.CenterParent, Icon = icon };
            frm.OnCopyUnitInfo += onCopyUnitInfo;
            if (frm.ShowDialog() == DialogResult.OK)
            {
               if (tableColumn.ActiveDimension.InputParameters != null)
                  mappingRow.InputParameters = new List<InputParameter>(tableColumn.ActiveDimension.InputParameters);

               mappingRow.SelectedUnit = tableColumn.ActiveUnit;
               mappingRow.IsUnitExplicitlySet = tableColumn.IsUnitExplicitlySet;

               var item = editor.Properties.Items.GetItem(mappingRow.Target);
               item.ImageIndex = _importerTask.GetImageIndex(tableColumn);
            }
            */
         }
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

      private void onMouseDown(object sender, MouseEventArgs e)
      {
         //TODO add context menu
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
   }
}