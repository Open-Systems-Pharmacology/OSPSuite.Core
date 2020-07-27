using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using DevExpress.Utils;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
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
         Configuration,
         ColumnName
      }

      public static string GetName(ColumnName name)
      {
         return name.ToString();
      }
   }

   public partial class ColumnMappingControl : BaseUserControl, IColumnMappingControl
   {
      private IEnumerable<DataFormatParameter> _mappings;
      private readonly IImageListRetriever _imageListRetriever;
      private IReadOnlyList<MetaDataCategory> _metaDataCategories;
      private IReadOnlyList<ColumnInfo> _columnInfos;
      private DataImporterSettings _dataImporterSettings;
      private readonly IImporterTask _importerTask;
      private readonly Dictionary<string, ImageComboBoxEdit> _editorsForEditing = new Dictionary<string, ImageComboBoxEdit>();

      private enum Buttons
      {
         MetaData = 2,
         UnitInformation
      }

      public ColumnMappingControl(IImageListRetriever imageListRetriever, IImporterTask importerTask)
      {
         _importerTask = importerTask;
         _imageListRetriever = imageListRetriever;
         InitializeComponent();
         uxGridView.OptionsView.ShowGroupPanel = false;
         uxGridView.OptionsMenu.EnableColumnMenu = false;
         uxGridView.CellValueChanged += (s, e) => validateMapping();
         uxGridView.OptionsView.ShowButtonMode = ShowButtonModeEnum.ShowOnlyInEditor;
         uxGridView.OptionsBehavior.EditorShowMode = EditorShowMode.MouseUp;

         uxGridView.CustomRowCellEdit += onCustomRowCellEdit;
         uxGridView.CustomRowCellEditForEditing += onCustomRowCellEditForEditing;
         uxGridView.MouseDown += onMouseDown;
         uxGrid.ToolTipController = new ToolTipController();
         uxGrid.ToolTipController.GetActiveObjectInfo += onGetActiveObjectInfo;
      }

      public void SetSettings(
         IReadOnlyList<MetaDataCategory> metaDataCategories,
         IReadOnlyList<ColumnInfo> columnInfos,
         DataImporterSettings dataImporterSettings)
      {
         _metaDataCategories = metaDataCategories;
         _columnInfos = columnInfos;
         _dataImporterSettings = dataImporterSettings;
      }

      public void AttachPresenter(IColumnMappingPresenter presenter)
      {
      }

      public void SetMappingSource(IEnumerable<DataFormatParameter> mappings)
      {
         _mappings = mappings;
         uxGrid.DataSource = mappings;

         configureColumn(ColumnMapping.GetName(ColumnMapping.ColumnName.ColumnName), true, false);
         configureColumn(ColumnMapping.GetName(ColumnMapping.ColumnName.Configuration), true, true);
      }

      private void validateMapping()
      {
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
         var mappingRow = _mappings.ElementAt(hitInfo.RowHandle);
         e.Info = new ToolTipControlInfo(mappingRow, string.Empty)
         {
            SuperTip = generateToolTipControlInfo(mappingRow),
            ToolTipType = ToolTipType.SuperTip
         };
         uxGrid.ToolTipController.ShowHint(e.Info);
      }

      private SuperToolTip generateToolTipControlInfo(DataFormatParameter parameter)
      {
         var superToolTip = new SuperToolTip();
         switch (parameter)
         {
            case MappingDataFormatParameter mp:
               superToolTip.Items.AddTitle("Mapping:");
               superToolTip.Items.Add(
                  $"The column {mp.ColumnName} will be mapped into {mp?.MappedColumn.Name} with units as {mp.MappedColumn.Unit}");
               break;
            case GroupByDataFormatParameter gp:
               superToolTip.Items.AddTitle("Group by:");
               superToolTip.Items.Add($"The column {gp.ColumnName} will be used for grouping by");
               break;
            case MetaDataFormatParameter mp:
               superToolTip.Items.AddTitle("Meta data:");
               superToolTip.Items.Add($"The column {mp.ColumnName} will be used as meta data to extract the following data: {mp.MetaDataId}");
               break;
            default:
               throw new Exception($"{parameter.GetType()} is not currently been handled");
         }

         return superToolTip;
      }

      private void onCustomRowCellEdit(object sender, CustomRowCellEditEventArgs e)
      {
         /*var view = sender as GridView;
         if (view == null) return;
         if (!e.Column.OptionsColumn.AllowEdit) return;

         //var sourceGridColumn = view.Columns[ColumnMapping.GetName(ColumnMapping.ColumnName.Type)];
         var mappingRow = _mappings.ElementAt(e.RowHandle);

         var editorObject = new ImageComboBoxEdit();
         Controls.Add(editorObject);
         var editor = editorObject.Properties;

         editor.AutoComplete = true;
         editor.AllowNullInput = DefaultBoolean.True;
         editor.CloseUpKey = new KeyShortcut(Keys.Enter);
         fillComboBoxItems(editorObject, mappingRow);
         refreshButtons(editorObject, mappingRow);

         e.RepositoryItem = editorObject.Properties;*/
      }

      private void fillComboBoxItems(ImageComboBoxEdit editor, DataFormatParameter mappingRow)
      {
         editor.Properties.Items.Clear();
         editor.Properties.SmallImages = _imageListRetriever.AllImages16x16;
         editor.Properties.NullText = Captions.Importer.NoneEditorNullText;
         editor.Properties.Items.Add(new ImageComboBoxItem(editor.Properties.NullText));

         //GroupBy
         editor.Properties.Items.Add(new ImageComboBoxItem(mappingRow)
            {ImageIndex = _importerTask.GetImageIndex(new GroupByDataFormatParameter(mappingRow.ColumnName), null)});

         //Mappings
         foreach (var info in _columnInfos)
         {
            if (!_mappings.Any(m =>
               m is MappingDataFormatParameter && (m as MappingDataFormatParameter)?.MappedColumn.Name.ToString() == info.DisplayName))
            {
               var col = new Column
               {
                  Name = Utility.EnumHelper.ParseValue<Column.ColumnNames>(info.DisplayName)
               };
               var imageIndex = _importerTask.GetImageIndex(new MappingDataFormatParameter(info.DisplayName, col), _mappings);
               var item = new ImageComboBoxItem(mappingRow) {ImageIndex = imageIndex, Description = info.DisplayName};
               editor.Properties.Items.Add(item);
            }
         }

         //MetaData
         foreach (var category in _metaDataCategories)
         {
            if (!_mappings.Any(m => m is MetaDataFormatParameter && (m as MetaDataFormatParameter).MetaDataId == category.DisplayName))
            {
               var imageIndex = _importerTask.GetImageIndex(new MetaDataFormatParameter(category.DisplayName, category.DisplayName), _mappings);
               var item = new ImageComboBoxItem(mappingRow) {ImageIndex = imageIndex, Description = category.DisplayName};
               editor.Properties.Items.Add(item);
            }
         }

         editor.Properties.KeyDown += clearSelectionOnDeleteForComboBoxEdit;
         if (editor.Properties.Items.Count != 0) return;
         editor.Properties.NullText = Captions.Importer.NothingSelectableEditorNullText;
         editor.Enabled = false;
      }

      private void refreshButtons(ButtonEdit editor, DataFormatParameter mappingRow)
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

         createButtons(editor, mappingRow);
      }

      private void onEditorButtonClick(object sender, ButtonPressedEventArgs e)
      {
         //throw new NotImplementedException();
      }

      private void createButtons(ButtonEdit editor, DataFormatParameter mappingRow)
      {
         createDeleteButton(editor);
         editor.ButtonClick += onEditorButtonClick;
         if (!String.IsNullOrEmpty(mappingRow.ColumnName))
         {
            // create meta data button
            createMetaDataButton(editor, true);

            // create unit information button
            createUnitInformationButton(editor, false);
         }
      }

      private void onMouseDown(object sender, MouseEventArgs e)
      {
         //throw new NotImplementedException();
      }

      private void onCustomRowCellEditForEditing(object sender, CustomRowCellEditEventArgs e)
      {
         //throw new NotImplementedException();
      }

      private static void clearSelectionOnDeleteForComboBoxEdit(object sender, KeyEventArgs e)
      {
         var comboBoxEdit = sender as ImageComboBoxEdit;
         var gridControl = comboBoxEdit?.Parent as UxGridControl;
         var view = gridControl?.FocusedView as ColumnView;
         if (view == null) return;

         if (e.KeyCode == Keys.Delete)
            view.ActiveEditor.EditValue = null;
      }

      private void onEditorEditValueChanged(object sender, EventArgs e)
      {
         //throw new NotImplementedException();
      }

      private static void createMetaDataButton(ButtonEdit editor, bool visible)
      {
         var metaDataTip = new SuperToolTip();
         metaDataTip.Items.Add(
            "Here you can enter meta data which will be used for all created import data table columns. CRTL-M");
         var metaDataButton = new EditorButton(ButtonPredefines.Glyph, ApplicationIcons.MetaData.ToImage(),
                                               metaDataTip)
         {
            Shortcut = new KeyShortcut(Keys.Control | Keys.M),
            Caption = "Metadata",
            Visible = visible
         };
         editor.Properties.Buttons.Add(metaDataButton);
      }

      private static void createDeleteButton(ButtonEdit editor)
      {
         var deleteButton = new EditorButton(ButtonPredefines.Delete)
         {
            Shortcut = new KeyShortcut(Keys.Control | Keys.D),
            Caption = "Delete",
            Enabled = true
         };
         editor.Properties.Buttons.Add(deleteButton);
      }

      private static void createUnitInformationButton(ButtonEdit editor, bool visible)
      {
         var unitInformationTip = new SuperToolTip();
         unitInformationTip.Items.Add(
            "Here you can enter unit information which will be used for all created import data table columns. CRTL-I");
         var unitInformationButton = new EditorButton(ButtonPredefines.Glyph,
                                                      ApplicationIcons.UnitInformation.ToImage(),
                                                      unitInformationTip)
         {
            Shortcut = new KeyShortcut(Keys.Control | Keys.I),
            Caption = "Unit Information",
            Enabled = visible
         };
         editor.Properties.Buttons.Add(unitInformationButton);
      }
   }
}