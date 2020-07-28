using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using DevExpress.Utils;
using DevExpress.Utils.Extensions;
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
using OSPSuite.Utility;

namespace OSPSuite.Presentation.Importer.Views
{
   static class ColumnMapping
   {
      public enum ColumnName
      {
         Configuration,
         ColumnName,
         Source
      }

      public static string GetName(ColumnName name)
      {
         return name.ToString();
      }
   }

   class MappingClass
   {
      public string ColumnName { get => Source.ColumnName; }
      public string Configuration 
      {
         get => Source.Configuration.ToString()??"Group by";
         set
         {
            if (value.StartsWith("Group by"))
            {
               Source = new GroupByDataFormatParameter(ColumnName);
            }
            else if (value.StartsWith("Mapping ["))
            {
               Source = new MappingDataFormatParameter(Source.ColumnName, new Column() { Name = EnumHelper.ParseValue<Column.ColumnNames>(value.Substring(9, value.Length - 10)) });
            }
            else if (value.StartsWith("Meta data ["))
            {
               Source = new MetaDataFormatParameter(Source.ColumnName, value.Substring(11, value.Length - 12));
            }
         }
      }
      public DataFormatParameter Source { get; protected set; }

      public MappingClass(DataFormatParameter parameter)
      {
         Source = parameter;
      }
   }

   public partial class ColumnMappingControl : BaseUserControl, IColumnMappingControl
   {
      private IList<MappingClass> _mappings;
      private readonly IImageListRetriever _imageListRetriever;
      private IReadOnlyList<MetaDataCategory> _metaDataCategories;
      private IReadOnlyList<ColumnInfo> _columnInfos;
      private DataImporterSettings _dataImporterSettings;
      private readonly IImporterTask _importerTask;
      private readonly Dictionary<string, ImageComboBoxEdit> _editorsForEditing = new Dictionary<string, ImageComboBoxEdit>();
      private MappingClass _contextMappingRow;

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
         _mappings = mappings.Select(m => new MappingClass(m)).ToList();
         uxGrid.DataSource = _mappings;

         configureColumn(ColumnMapping.GetName(ColumnMapping.ColumnName.ColumnName), true, false);
         configureColumn(ColumnMapping.GetName(ColumnMapping.ColumnName.Configuration), true, true);
         configureColumn(ColumnMapping.GetName(ColumnMapping.ColumnName.Source), false, false);
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
            SuperTip = generateToolTipControlInfo(mappingRow.Source),
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
         var view = sender as GridView;
         if (view == null) return;
         if (!e.Column.OptionsColumn.AllowEdit) return;

         var mappingRow = _mappings.ElementAt(e.RowHandle);
         _contextMappingRow = mappingRow;

         var editorObject = new ImageComboBoxEdit();
         Controls.Add(editorObject);
         var editor = editorObject.Properties;

         editor.AutoComplete = true;
         editor.AllowNullInput = DefaultBoolean.True;
         editor.CloseUpKey = new KeyShortcut(Keys.Enter);
         fillComboBoxItems(editorObject, mappingRow);
         refreshButtons(editorObject, mappingRow);

         e.RepositoryItem = editorObject.Properties;
      }

      private void fillComboBoxItems(ImageComboBoxEdit editor, MappingClass mappingRow)
      {
         editor.Properties.Items.Clear();
         editor.Properties.SmallImages = _imageListRetriever.AllImages16x16;
         editor.Properties.NullText = Captions.Importer.NoneEditorNullText;
         switch (mappingRow.Source)
         {
            case MappingDataFormatParameter mp:
               var imageIndex = _importerTask.GetImageIndex(new MappingDataFormatParameter(mp.ColumnName, mp.MappedColumn), _mappings.Select(m => m.Source));
               editor.Properties.Items.Add(
                  new ImageComboBoxItem($"Mapping [{mp.MappedColumn.Name}]")
                  {
                     ImageIndex = imageIndex,
                     Description = mp.MappedColumn.Name.ToString()
                  });
               break;
            case MetaDataFormatParameter mp:
               imageIndex = _importerTask.GetImageIndex(new MetaDataFormatParameter(mp.ColumnName, mp.MetaDataId), _mappings.Select(m => m.Source));
               editor.Properties.Items.Add(
                  new ImageComboBoxItem($"Meta data [{mp.MetaDataId}]") 
                  { 
                     ImageIndex = imageIndex, 
                     Description = mp.MetaDataId 
                  });
               break;
            case GroupByDataFormatParameter gp:
               editor.Properties.Items.Add(
                  new ImageComboBoxItem("Group by")
                  {
                     ImageIndex = _importerTask.GetImageIndex(new GroupByDataFormatParameter(gp.ColumnName), _mappings.Select(m => m.Source))
                  });
               break;
         }
         editor.SelectedIndex = 0;
         editor.Properties.Items.Add(new ImageComboBoxItem(editor.Properties.NullText));

         //GroupBy
         editor.Properties.Items.Add( 
            new ImageComboBoxItem("Group by")
            { 
               ImageIndex = _importerTask.GetImageIndex(new GroupByDataFormatParameter(mappingRow.ColumnName), _mappings.Select(m => m.Source)) 
            });

         //Mappings
         foreach (var info in _columnInfos)
         {
            if (!_mappings.Any(m =>
               m.Source is MappingDataFormatParameter && (m.Source as MappingDataFormatParameter)?.MappedColumn.Name.ToString() == info.DisplayName))
            {
               var col = new Column
               {
                  Name = EnumHelper.ParseValue<Column.ColumnNames>(info.DisplayName)
               };
               var imageIndex = _importerTask.GetImageIndex(new MappingDataFormatParameter(info.DisplayName, col), _mappings.Select(m => m.Source));
               var item = new ImageComboBoxItem($"Mapping [{info.DisplayName}]") {ImageIndex = imageIndex, Description = info.DisplayName};
               editor.Properties.Items.Add(item);
            }
         }

         //MetaData
         foreach (var category in _metaDataCategories)
         {
            if (!_mappings.Any(m => m.Source is MetaDataFormatParameter && (m.Source as MetaDataFormatParameter).MetaDataId == category.DisplayName))
            {
               var imageIndex = _importerTask.GetImageIndex(new MetaDataFormatParameter(category.DisplayName, category.DisplayName), _mappings.Select(m => m.Source));
               var item = new ImageComboBoxItem($"Meta data [{category.DisplayName}]") {ImageIndex = imageIndex, Description = category.DisplayName};
               editor.Properties.Items.Add(item);
            }
         }

         editor.Properties.KeyDown += clearSelectionOnDeleteForComboBoxEdit;
         if (editor.Properties.Items.Count != 0) return;
         editor.Properties.NullText = Captions.Importer.NothingSelectableEditorNullText;
         editor.Enabled = false;
      }

      private void refreshButtons(ButtonEdit editor, MappingClass mappingRow)
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

      private void createButtons(ButtonEdit editor, MappingClass mappingRow)
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
         var view = sender as GridView;
         if (view == null) return;
         if (!e.Column.OptionsColumn.AllowEdit) return;

         var sourceGridColumn = view.Columns[ColumnMapping.GetName(ColumnMapping.ColumnName.ColumnName)];
         var sourceColumnName = (string)view.GetRowCellValue(e.RowHandle, sourceGridColumn);
         if (string.IsNullOrEmpty(sourceColumnName)) return;
         var mappingRow = _mappings.ElementAt(e.RowHandle);
         
         //the information of the current mappingRow is needed in both underlying event handlers.
         _contextMappingRow = mappingRow;

         ImageComboBoxEdit editorObject = _editorsForEditing.GetOrAdd(sourceColumnName, (index) =>
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

         if (_editorsForEditing.ContainsKey(sourceColumnName))
            editorObject = _editorsForEditing[sourceColumnName];
         else
         {
            editorObject = new ImageComboBoxEdit();
            _editorsForEditing.Add(sourceColumnName, editorObject);
            editorObject.EditValueChanged += onEditorEditValueChanged;
            Controls.Add(editorObject);
            var editor = editorObject.Properties;

            editor.AutoComplete = true;
            editor.AllowNullInput = DefaultBoolean.True;
            editor.CloseUpKey = new KeyShortcut(Keys.Enter);
         }

         editorObject.EditValue = mappingRow.ColumnName;
         // create individual list of values
         fillComboBoxItems(editorObject, mappingRow);
         refreshButtons(editorObject, mappingRow);

         if (editorObject.Properties.Items.Count == 0) return;
         e.RepositoryItem = editorObject.Properties;
         editorObject.SelectedIndex = 0;
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
         var editor = sender as ImageComboBoxEdit;
         if (editor == null) return;

         uxGrid.MainView.PostEditor();

         if (editor.EditValue != null && editor.EditValue.ToString() == editor.Properties.NullText)
            editor.EditValue = null;

         _contextMappingRow.Configuration = editor.EditValue.ToString();
         refreshButtons(editor, _contextMappingRow);
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