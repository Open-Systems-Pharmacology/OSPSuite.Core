using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
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
         get
         {
            switch (Source)
            {
               case IgnoredDataFormatParameter _:
                  return Captions.Importer.NoneEditorNullText;
               case GroupByDataFormatParameter _:
                  return Captions.GroupByTitle;
               default:
                  return Source.Configuration.ToString();
            }
         }
         set
         {
            if (value == Captions.Importer.GroupByEditorNullText)
            {
               Source = new IgnoredDataFormatParameter(ColumnName);
            }
            else if (value.StartsWith(Captions.GroupByPrefix))
            {
               Source = new GroupByDataFormatParameter(ColumnName);
            }
            else if (value.StartsWith(Captions.MappingPrefix))
            {
               var fullName = value.Substring(9, value.Length - 10);
               var parsedName = Regex.Match(fullName, "[^()]+");
               var name = parsedName.Value;
               parsedName = parsedName.NextMatch();
               var unit = parsedName.Value;

               Source = new MappingDataFormatParameter(ColumnName, new Column() { Name = EnumHelper.ParseValue<Column.ColumnNames>(name), Unit = unit });
            }
            else if (value.StartsWith(Captions.MetaDataPrefix))
            {
               Source = new MetaDataFormatParameter(ColumnName, value.Substring(11, value.Length - 12));
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

      public ColumnMappingControl(IImageListRetriever imageListRetriever, IImporterTask importerTask)
      {
         _importerTask = importerTask;
         _imageListRetriever = imageListRetriever;
         InitializeComponent();
         uxGridView.OptionsView.ShowGroupPanel = false;
         uxGridView.OptionsMenu.EnableColumnMenu = false;
         uxGridView.CellValueChanged += (s, e) => validateMapping();

         uxGridView.CustomRowCellEdit += onCustomRowCellEditForEditing;
         uxGridView.CustomRowCellEditForEditing += onCustomRowCellEditForEditing;
         uxGridView.OptionsView.ShowButtonMode = ShowButtonModeEnum.ShowOnlyInEditor;
         uxGridView.OptionsBehavior.EditorShowMode = EditorShowMode.MouseUp;
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
               superToolTip.Items.AddTitle(Captions.MappingTitle);
               superToolTip.Items.Add(Captions.MappingHint(mp.ColumnName, mp?.MappedColumn.Name.ToString(), mp.MappedColumn.Unit));
               break;
            case GroupByDataFormatParameter gp:
               superToolTip.Items.AddTitle(Captions.GroupByTitle);
               superToolTip.Items.Add(Captions.GroupByHint(gp.ColumnName));
               break;
            case MetaDataFormatParameter mp:
               superToolTip.Items.AddTitle(Captions.MetaDataTitle);
               superToolTip.Items.Add(Captions.MetaDataHint(mp.ColumnName, mp.MetaDataId));
               break;
            case IgnoredDataFormatParameter _:
               superToolTip.Items.AddTitle(Captions.IgnoredParameterTitle);
               superToolTip.Items.Add(Captions.IgnoredParameterHint);
               break;
            default:
               throw new Exception(Error.TypeNotSupported(parameter.GetType()));
         }

         return superToolTip;
      }

      private void onCustomRowCellEditForEditing(object sender, CustomRowCellEditEventArgs e)
      {
         var view = sender as GridView;
         if (view == null) return;
         if (!e.Column.OptionsColumn.AllowEdit) return;

         var mappingRow = _mappings.ElementAt(e.RowHandle);

         //the information of the current mappingRow is needed in both underlying event handlers.
         _contextMappingRow = mappingRow;

         var editorObject = _editorsForEditing.GetOrAdd(mappingRow.ColumnName, (index) =>
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
         editorObject.EditValue = mappingRow.ColumnName;
         // create individual list of values
         fillComboBoxItems(editorObject, mappingRow);
         refreshButtons(editorObject, mappingRow);

         if (editorObject.Properties.Items.Count == 0) return;
         e.RepositoryItem = editorObject.Properties;
      }

      private ImageComboBoxItem generateGroupByComboBoxItem(string columnName)
      {
         return new ImageComboBoxItem(Captions.GroupByDescription)
         {
            ImageIndex = _importerTask.GetImageIndex(new GroupByDataFormatParameter(columnName), _mappings.Select(m => m.Source))
         };
      }

      private ImageComboBoxItem generateMappingComboBoxItem(string columnName, Column column)
      {
         var imageIndex = _importerTask.GetImageIndex(new MappingDataFormatParameter(columnName, column), _mappings.Select(m => m.Source));
         return new ImageComboBoxItem(Captions.MappingDescription(column.Name.ToString(), column.Unit))
         {
            ImageIndex = imageIndex,
            Description = column.Name.ToString()
         };
      }

      private ImageComboBoxItem generateMetaDataComboBoxItem(string columnName, string metaDataId)
      {
         var imageIndex = _importerTask.GetImageIndex(new MetaDataFormatParameter(columnName, metaDataId), _mappings.Select(m => m.Source));
         return new ImageComboBoxItem(Captions.MetaDataDescription(metaDataId))
         {
            ImageIndex = imageIndex,
            Description = metaDataId
         };
      }

      private void fillComboBoxItems(ImageComboBoxEdit editor, MappingClass mappingRow)
      {
         editor.Properties.Items.Clear();
         editor.Properties.SmallImages = _imageListRetriever.AllImages16x16;
         editor.Properties.NullText = Captions.Importer.NoneEditorNullText;
         switch (mappingRow.Source)
         {
            case MappingDataFormatParameter mp:
               editor.Properties.Items.Add(generateMappingComboBoxItem(mp.ColumnName, mp.MappedColumn));
               break;
            case MetaDataFormatParameter mp:
               editor.Properties.Items.Add(generateMetaDataComboBoxItem(mp.ColumnName, mp.MetaDataId));
               break;
            case GroupByDataFormatParameter gp:
               editor.Properties.Items.Add(generateGroupByComboBoxItem(gp.ColumnName));                  
               break;
         }
         editor.Properties.Items.Add(new ImageComboBoxItem(editor.Properties.NullText));

         //GroupBy
         editor.Properties.Items.Add(generateGroupByComboBoxItem(mappingRow.ColumnName));

         //Mappings
         foreach (var info in _columnInfos)
         {
            if (!_mappings.Any(m =>
               m.Source is MappingDataFormatParameter && (m.Source as MappingDataFormatParameter)?.MappedColumn.Name.ToString() == info.DisplayName))
               {
                  editor.Properties.Items.Add(
                     generateMappingComboBoxItem(
                        info.DisplayName, 
                        new Column
                        {
                           Name = EnumHelper.ParseValue<Column.ColumnNames>(info.DisplayName)
                        })
                     );
               }
         }

         //MetaData
         foreach (var category in _metaDataCategories)
         {
            if (!_mappings.Any(m => m.Source is MetaDataFormatParameter && (m.Source as MetaDataFormatParameter).MetaDataId == category.DisplayName))
            {
               editor.Properties.Items.Add(generateMetaDataComboBoxItem(category.DisplayName, category.DisplayName));
            }
         }

         editor.Properties.KeyDown += clearSelectionOnDeleteForComboBoxEdit;
         editor.SelectedIndex = 0;
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
         var editor = sender as ImageComboBoxEdit;
         if (editor == null) return;

         var mappingRow = _contextMappingRow;
         if (mappingRow == null) return;

         if (editor.EditValue == null) return;

         if (e.Button.Caption == MenuNames.DeleteSubMenu)
         {
            editor.EditValue = Captions.Importer.GroupByEditorNullText;
            _contextMappingRow.Configuration = editor.EditValue?.ToString();
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

      private void createButtons(ButtonEdit editor, MappingClass mappingRow)
      {
         createDeleteButton(editor);
         editor.ButtonClick += onEditorButtonClick;
         if (!String.IsNullOrEmpty(mappingRow.ColumnName))
         {
            // create unit information button
            createUnitInformationButton(editor, false);
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
            view.ActiveEditor.EditValue = Captions.Importer.GroupByEditorNullText;
      }

      private void onEditorEditValueChanged(object sender, EventArgs e)
      {
         var editor = sender as ImageComboBoxEdit;
         if (editor == null) return;

         uxGrid.MainView.PostEditor();

         if (editor.EditValue != null && editor.EditValue.ToString() == editor.Properties.NullText)
            editor.EditValue = Captions.Importer.GroupByEditorNullText;

         _contextMappingRow.Configuration = editor.EditValue?.ToString();
         refreshButtons(editor, _contextMappingRow);
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