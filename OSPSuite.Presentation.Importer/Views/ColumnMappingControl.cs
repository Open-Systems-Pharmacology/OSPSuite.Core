using System;
using OSPSuite.UI.Controls;
using OSPSuite.Presentation.Importer.Presenters;
using System.Collections.Generic;
using OSPSuite.Presentation.Importer.Core.DataFormat;
using DevExpress.Utils;
using DevExpress.XtraGrid.Views.Base;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;
using System.Linq;
using DevExpress.XtraEditors;
using OSPSuite.UI.Services;
using OSPSuite.Assets;
using DevExpress.XtraEditors.Controls;
using System.Windows.Forms;
using OSPSuite.Core.Importer;
using Org.BouncyCastle.Asn1.Cms;

namespace OSPSuite.Presentation.Importer.Views
{
   public partial class ColumnMappingControl : BaseUserControl, IColumnMappingControl
   {
      private IEnumerable<DataFormatParameter> mappings;
      private readonly IImageListRetriever imageListRetriever;
      private IReadOnlyList<MetaDataCategory> metaDataCategories;
      private IReadOnlyList<ColumnInfo> columnInfos;
      private DataImporterSettings dataImporterSettings;
      private readonly IImporterTask importerTask;

      public ColumnMappingControl(IImageListRetriever imageListRetriever, IImporterTask importerTask)
      {
         this.importerTask = importerTask;
         this.imageListRetriever = imageListRetriever;
         InitializeComponent();
         uxGridView.OptionsView.ShowGroupPanel = false;
         uxGridView.OptionsMenu.EnableColumnMenu = false;
         uxGridView.CellValueChanged += (s, e) => ValidateMapping();
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
         this.metaDataCategories = metaDataCategories;
         this.columnInfos = columnInfos;
         this.dataImporterSettings = dataImporterSettings;
      }

      public void AttachPresenter(IColumnMappingPresenter presenter) { }

      public void SetMappingSource(IEnumerable<DataFormatParameter> mappings)
      {
         this.mappings = mappings;
         uxGrid.DataSource = mappings;

         configureColumn("ColumnName", true, false);
         configureColumn("Type", true, true);
      }

      private void ValidateMapping()
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
         GridHitInfo hitInfo = view.CalcHitInfo(e.ControlMousePosition);

         if (!hitInfo.InRow) return;
         var mappingRow = mappings.ElementAt(hitInfo.RowHandle);
         e.Info = new ToolTipControlInfo(mappingRow, String.Empty);

         e.Info.SuperTip = GenerateToolTipControlInfo(mappingRow);
         e.Info.ToolTipType = ToolTipType.SuperTip;
         uxGrid.ToolTipController.ShowHint(e.Info);
      }

      private SuperToolTip GenerateToolTipControlInfo(DataFormatParameter parameter)
      {
         var superToolTip = new SuperToolTip();
         switch (parameter.Type)
         {
            case DataFormatParameterType.MAPPING:
               var mapping = parameter as MappingDataFormatParameter;
               superToolTip.Items.AddTitle("Mapping:");
               superToolTip.Items.Add($"The column {parameter.ColumnName} will be mapped into {mapping.MappedColumn.Name} with units as {mapping.MappedColumn.Unit}");
               break;
            case DataFormatParameterType.GROUP_BY:
               superToolTip.Items.AddTitle("Group by:");
               superToolTip.Items.Add($"The column {parameter.ColumnName} will be used for grouping by");
               break;
            case DataFormatParameterType.META_DATA:
               superToolTip.Items.AddTitle("Meta data:");
               superToolTip.Items.Add($"The column {parameter.ColumnName} will be used as meta data");
               break;
         }
         return superToolTip;
      }

      void onCustomRowCellEdit(object sender, CustomRowCellEditEventArgs e)
      {
         var view = sender as GridView;
         if (view == null) return;
         if (!e.Column.OptionsColumn.AllowEdit) return;

         var sourceGridColumn = view.Columns["Type"];
         var mappingRow = mappings.ElementAt(e.RowHandle);

         ImageComboBoxEdit editorObject = new ImageComboBoxEdit();
         Controls.Add(editorObject);
         var editor = editorObject.Properties;

         editor.AutoComplete = true;
         editor.AllowNullInput = DefaultBoolean.True;
         editor.CloseUpKey = new KeyShortcut(Keys.Enter);
         fillComboBoxItems(editorObject, mappingRow);
         refreshButtons(editorObject, mappingRow);

         e.RepositoryItem = editorObject.Properties;
      }

      private void fillComboBoxItems(ImageComboBoxEdit editor, DataFormatParameter mappingRow)
      {
         editor.Properties.Items.Clear();
         editor.Properties.SmallImages = imageListRetriever.AllImages16x16;
         editor.Properties.NullText = Captions.Importer.NoneEditorNullText;
         editor.Properties.Items.Add(new ImageComboBoxItem(editor.Properties.NullText));

         //GroupBy
         editor.Properties.Items.Add(new ImageComboBoxItem("Group by") { ImageIndex = importerTask.GetImageIndex(new GroupByDataFormatParameter(""), null) });

         //Mappings
         foreach (var info in columnInfos)
         {
            if (mappings.Where(m => m.Type == DataFormatParameterType.MAPPING && (m as MappingDataFormatParameter).MappedColumn.Name.ToString() == info.DisplayName).Count() == 0)
            {
               Core.Column.ColumnNames columnName;
               Enum.TryParse(info.DisplayName, out columnName);
               Core.Column col = new Core.Column();
               col.Name = columnName;
               var imageIndex = importerTask.GetImageIndex(new MappingDataFormatParameter(info.DisplayName, col), mappings);
               var item = new ImageComboBoxItem(mappingRow.ColumnName) { ImageIndex = imageIndex, Description = info.DisplayName };
               editor.Properties.Items.Add(item);
            }
         }

         //MetaData
         foreach (var category in metaDataCategories)
         {
            if (mappings.Where(m => m.Type == DataFormatParameterType.META_DATA && m.ColumnName == category.DisplayName).Count() == 0)
            {
               var imageIndex = importerTask.GetImageIndex(new MetaDataFormatParameter(category.DisplayName), mappings);
               var item = new ImageComboBoxItem(mappingRow.ColumnName) { ImageIndex = imageIndex, Description = category.DisplayName };
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
         //throw new NotImplementedException();
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
   }
}
