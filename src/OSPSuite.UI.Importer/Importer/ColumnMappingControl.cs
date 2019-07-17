using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
using DevExpress.Utils;
using DevExpress.Utils.Menu;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraGrid.Menu;
using DevExpress.XtraGrid.Views.Base;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;
using OSPSuite.Assets;
using OSPSuite.Core.Importer;
using OSPSuite.Core.Importer.Mappers;
using OSPSuite.Presentation.Services;
using OSPSuite.UI.Controls;
using OSPSuite.UI.Services;

namespace OSPSuite.UI.Importer
{
   public class MappingCompletedEventArgs : EventArgs
   {
      public string SheetName { set; get; }
   }
   public partial class ColumnMappingControl : XtraUserControl
   {
      private DataTable _sourceTable;
      private ImportDataTable _table;
      private readonly IImageListRetriever _imageListRetriever;
      private ColumnMapping _contextMappingRow;
      private readonly UxGridControl _grid;
      private readonly Dictionary<string, ImageComboBoxEdit> _editorsForDisplay;
      private readonly Dictionary<string, ImageComboBoxEdit> _editorsForEditing;
      private IColumnCaptionHelper _columnCaptionHelper;
      private readonly IImporterTask _importerTask;

      private enum Buttons
      {
         MetaData = 2,
         UnitInformation
      }

      private enum ColumnMappingProperties
      {
         SourceColumn,
         Target,
         Metadata,
         InputParameters,
         SelectedUnit,
         IsUnitExplicitlySet
      }

      public ColumnMappingControl(DataTable sourceTable, ImportDataTable table, IImageListRetriever imageListRetriever, IImporterTask importerTask, IColumnCaptionHelper columnCaptionHelper)
      {
         InitializeComponent();

         _sourceTable = sourceTable;
         _table = table;
         _imageListRetriever = imageListRetriever;
         _importerTask = importerTask;
         _columnCaptionHelper = columnCaptionHelper;
         _editorsForDisplay = new Dictionary<string, ImageComboBoxEdit>();
         _editorsForEditing = new Dictionary<string, ImageComboBoxEdit>();

         createMapping();
         createAutoMapping();

         _grid = new UxGridControl { DataSource = Mapping };
         Controls.Add(_grid);
         _grid.Dock = DockStyle.Fill;

         var mv = new GridView();
         _grid.MainView = mv;
         mv.OptionsView.ShowGroupPanel = false;
         mv.OptionsMenu.EnableColumnMenu = false;
         mv.CellValueChanged += (s, e) => ValidateMapping();

         var columnMapping = new ColumnMapping();

         configureColumn(columnMapping, ColumnMappingProperties.SourceColumn, mv, visible: true);

         configureColumn(columnMapping, ColumnMappingProperties.Target, mv, visible: true, allowEdit: true);

         configureColumn(columnMapping, ColumnMappingProperties.Metadata, mv, visible: false);

         configureColumn(columnMapping, ColumnMappingProperties.InputParameters, mv, visible: false);

         configureColumn(columnMapping, ColumnMappingProperties.SelectedUnit, mv, visible: false);

         configureColumn(columnMapping, ColumnMappingProperties.IsUnitExplicitlySet, mv, visible: false);

         mv.CustomRowCellEdit += onCustomRowCellEdit;
         mv.CustomRowCellEditForEditing += onCustomRowCellEditForEditing;
         mv.OptionsView.ShowButtonMode = ShowButtonModeEnum.ShowOnlyInEditor;
         mv.OptionsBehavior.EditorShowMode = EditorShowMode.MouseUp;
         mv.MouseDown += onMouseDown;
         _grid.ToolTipController = new ToolTipController();
         _grid.ToolTipController.GetActiveObjectInfo += onGetActiveObjectInfo;

      }

      private void configureColumn(ColumnMapping columnMapping, ColumnMappingProperties column, ColumnView mv, bool visible, bool allowEdit = false)
      {
         var columnName = getNameFor(columnMapping, column);
         var col = mv.Columns.ColumnByFieldName(columnName);
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

      private string getNameFor(ColumnMapping columnMapping, ColumnMappingProperties column)
      {
         return columnMapping.GetType().GetProperties()[(int)column].Name;
      }

      private void onGetActiveObjectInfo(object sender, ToolTipControllerGetActiveObjectInfoEventArgs e)
      {
         if (sender != _grid.ToolTipController) return;
         var view = _grid.GetViewAt(e.ControlMousePosition) as GridView;
         if (view == null) return;
         GridHitInfo hitInfo = view.CalcHitInfo(e.ControlMousePosition);

         if (!hitInfo.InRow) return;
         var mappingRow = Mapping[hitInfo.RowHandle];

         e.Info = new ToolTipControlInfo(mappingRow, String.Empty);
         if (String.IsNullOrEmpty(mappingRow.Target)) return;
         var tableColumn = getTableColumn(mappingRow);
         if (tableColumn != null)
            e.Info.SuperTip = _importerTask.GetToolTipForImportDataColumn(tableColumn);

         if (mappingRow.Target == "<Group By>")
         {
            var superToolTip = new SuperToolTip();
            superToolTip.Items.AddTitle("Group By");
            superToolTip.Items.Add($"The source table will be grouped by <{mappingRow.SourceColumn}> and the distinct values separates the data into multiple tables.");

            e.Info.SuperTip = superToolTip;
         }

         if (_table.MetaData != null && _table.MetaData.Columns.ContainsName(mappingRow.Target))
         {
            var metaDataColumn = _table.MetaData.Columns.ItemByName(mappingRow.Target);
            var superToolTip = new SuperToolTip();
            var item = new ToolTipTitleItem { Text = Captions.Importer.MetaData, Image = ApplicationIcons.MetaData.ToImage() };
            superToolTip.Items.Add(item);
            superToolTip.Items.AddTitle(metaDataColumn.DisplayName);
            superToolTip.Items.Add(metaDataColumn.Description);
            superToolTip.Items.AddSeparator();
            superToolTip.Items.Add($"The source table will be grouped by <{mappingRow.SourceColumn}> and the distinct values are set to this meta data.");
            e.Info.SuperTip = superToolTip;
         }
         e.Info.ToolTipType = ToolTipType.SuperTip;
         _grid.ToolTipController.ShowHint(e.Info);
      }

      private void onMouseDown(object sender, MouseEventArgs mouseEventArgs)
      {
         if (mouseEventArgs.Button != MouseButtons.Right) return;
         var mv = sender as GridView;
         if (mv == null) return;

         var menu = new GridViewColumnMenu(mv);
         menu.Items.Clear();
         menu.Items.Add(new DXMenuItem("Create Mapping", onCreateAutoMappingClick));
         menu.Items.Add(new DXMenuItem("Clear Mapping", onClearMappingClick));
         menu.Show(mouseEventArgs.Location);
      }

      private void doMappingAction(Action action)
      {
         if (_grid == null) return;
         _grid.BeginUpdate();
         action();
         _grid.EndUpdate();
         ValidateMapping();
      }

      private void onCreateAutoMappingClick(object sender, EventArgs eventArgs)
      {
         doMappingAction(createAutoMapping);
      }

      private void onClearMappingClick(object sender, EventArgs eventArgs)
      {
         doMappingAction(clearMapping);
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

      private void fillComboBoxItems(ImageComboBoxEdit editor, ColumnMapping mappingRow, DataColumn sourceColumn)
      {
         editor.Properties.Items.Clear();
         editor.Properties.SmallImages = _imageListRetriever.AllImages16x16;
         editor.Properties.NullText = Captions.Importer.NoneEditorNullText;
         editor.Properties.Items.Add(new ImageComboBoxItem(editor.Properties.NullText));
         foreach (ImportDataColumn dataCol in _table.Columns)
         {
            if (!isDataColumnUsableForSourceColumn(dataCol, sourceColumn)) continue;
            var imageIndex = (dataCol.ColumnName == mappingRow.Target)
                                ? _importerTask.GetImageIndex(getTableColumn(mappingRow))
                                : _importerTask.GetImageIndex(dataCol);
            var item = new ImageComboBoxItem(dataCol.ColumnName) { ImageIndex = imageIndex, Description = dataCol.DisplayName };
            editor.Properties.Items.Add(item);
         }

         if (isGroupByAllowed(sourceColumn))
            editor.Properties.Items.Add(new ImageComboBoxItem(Captions.Importer.GroupByEditorNullText, ApplicationIcons.IconIndex(ApplicationIcons.GroupBy)));

         if (_table.MetaData != null)
         {
            foreach (MetaDataColumn metaDataCol in _table.MetaData.Columns)
            {
               if (!isDataColumnUsableForSourceColumn(metaDataCol, sourceColumn)) continue;
               if (isAlreadyMapped(mappingRow.SourceColumn, metaDataCol.ColumnName)) continue;
               var imageIndex = ApplicationIcons.IconIndex(ApplicationIcons.MetaData);
               var item = new ImageComboBoxItem(metaDataCol.ColumnName) { ImageIndex = imageIndex, Description = metaDataCol.DisplayName };
               editor.Properties.Items.Add(item);
            }
         }

         editor.Properties.KeyDown += clearSelectionOnDeleteForComboBoxEdit;
         if (editor.Properties.Items.Count != 0) return;
         editor.Properties.NullText = Captions.Importer.NothingSelectableEditorNullText;
         editor.Enabled = false;
      }

      private bool isGroupByAllowed(DataColumn sourceColumn)
      {
         if (_table.MetaData == null) return true;

         foreach (MetaDataColumn metaDataCol in _table.MetaData.Columns)
         {
            if (!isDataColumnUsableForSourceColumn(metaDataCol, sourceColumn)) continue;
            if (_columnCaptionHelper.IsEquivalent(sourceColumn.ColumnName, metaDataCol.ColumnName))
               return false;
         }
         return true;
      }

      /// <summary>
      /// This methods checks whether a concrete column mapping already exists.
      /// </summary>
      /// <remarks>This means is the target mapped to a different source than the given one?</remarks>
      /// <returns>True, if there is a mapping for given columns.</returns>
      private bool isAlreadyMapped(string sourceColumn, string target)
      {
         foreach (var mapping in Mapping)
         {
            if (mapping.SourceColumn == sourceColumn) continue;
            if (mapping.Target != target) continue;
            return true;
         }
         return false;
      }

      /// <summary>
      /// This method check whether a target column is already been used in mapping.
      /// </summary>
      /// <param name="target">Name to be checked.</param>
      /// <returns>True is column name is already mapped.</returns>
      private bool doesMappingExistsFor(string target)
      {
         foreach (var mapping in Mapping)
         {
            if (mapping.Target != target) continue;
            return true;
         }
         return false;
      }

      /// <summary>
      /// Method for getting the ImportDataColumn of a specified mapping row.
      /// </summary>
      private ImportDataColumn getTableColumn(ColumnMapping mappingRow)
      {
         if (!_table.Columns.ContainsName(mappingRow.Target)) return null;
         var tableColumn = _table.Columns.ItemByName(mappingRow.Target).Clone();
         if (mappingRow.MetaData != null)
            tableColumn.MetaData = mappingRow.MetaData.Copy();
         if (mappingRow.SelectedUnit.Name != null)
         {
            _importerTask.SetColumnUnit(tableColumn, mappingRow.SelectedUnit.Name, mappingRow.IsUnitExplicitlySet);
            if (mappingRow.InputParameters != null && tableColumn.ActiveDimension.InputParameters != null)
               tableColumn.ActiveDimension.InputParameters = new List<InputParameter>(mappingRow.InputParameters);
         }
         else
            tableColumn.SetColumnUnitDependingOnMetaData();
         return tableColumn;
      }

      public IList<ColumnMapping> Mapping { get; private set; }

      public delegate void MappingCompletedHandler(object sender, EventArgs e);

      /// <summary>
      /// Event raised when mapping is complete.
      /// </summary>
      public event MappingCompletedHandler OnMappingCompleted;

      /// <summary>
      /// Event arguments for OnMissingMapping event.
      /// </summary>
      public class MissingMappingEventArgs : EventArgs
      {
         /// <summary>
         /// Message describing what is missed.
         /// </summary>
         public string Message { get; set; }
      }

      /// <summary>
      /// Handler for OnMissingMapping event.
      /// </summary>
      public delegate void MissingMappingHandler(object sender, MissingMappingEventArgs e);

      /// <summary>
      /// Event raised when mapping is not complete.
      /// </summary>
      public event MissingMappingHandler OnMissingMapping;

      /// <summary>
      /// Method for validating the mapping.
      /// </summary>
      public void ValidateMapping()
      {
         try
         {
            _importerTask.CheckWhetherAllDataColumnsAreMapped(_table.Columns, Mapping);
            if (!IsMappingDone())
            {
               OnMissingMapping?.Invoke(this, new MissingMappingEventArgs { Message = "No mapping done!" });
            }
            else
               OnMappingCompleted?.Invoke(this, new MappingCompletedEventArgs { SheetName = _sourceTable.TableName });
         }
         catch (NoMappingForDataColumnException exception)
         {
            OnMissingMapping?.Invoke(this, new MissingMappingEventArgs { Message = exception.Message });
         }
      }

      /// <summary>
      /// Method for checking if mapping is complete.
      /// </summary>
      public bool IsMappingDone()
      {
         foreach (var cm in Mapping)
         {
            if (String.IsNullOrEmpty(cm.Target)) continue;
            return true;
         }
         return false;
      }

      /// <summary>
      /// This method defines whether the names of an import data column and a source column should be mapped.
      /// </summary>
      /// <returns>True, if columns should be mapped.</returns>
      private static bool checkNameForMapping(DataColumn dataCol, DataColumn sourceCol)
      {
         var columnName = dataCol.ColumnName.ToUpper();
         var displayName = dataCol.Caption.ToUpper();
         var sourceName = sourceCol.ColumnName.ToUpper();
         var sourceName2 = sourceCol.Caption.ToUpper();
         // reduce unit if column name contains unit information
         if (sourceCol.ColumnName.IndexOf('[') > 0)
            sourceName2 = sourceCol.ColumnName.Substring(0, sourceCol.ColumnName.IndexOf('[')).ToUpper().Trim();

         return ((columnName == sourceName) ||
                  columnName.StartsWith(sourceName) ||
                  sourceName.StartsWith(columnName) ||
                 (columnName == sourceName2) ||
                  columnName.StartsWith(sourceName2) ||
                  sourceName2.StartsWith(columnName) ||
                 (displayName == sourceName) ||
                  displayName.StartsWith(sourceName) ||
                  sourceName.StartsWith(displayName) ||
                 (displayName == sourceName2) ||
                  displayName.StartsWith(sourceName2) ||
                  sourceName2.StartsWith(displayName));
      }

      private void createMapping()
      {
         Mapping = new List<ColumnMapping>();
         foreach (DataColumn sourceCol in _sourceTable.Columns)
         {
            Mapping.Add(new ColumnMapping
            {
               SourceColumn = sourceCol.ColumnName,
               Target = String.Empty,
               MetaData = null,
               InputParameters = null,
               SelectedUnit = new Unit(),
               IsUnitExplicitlySet = false
            });
         }
      }

      private void createAutoMapping()
      {
         clearMapping();
         foreach (DataColumn sourceCol in _sourceTable.Columns)
         {
            var found = false;
            var tableColumn = String.Empty;
            var selectedUnit = new Unit();
            var isExplicitlySet = false;
            var sourceUnit = _importerTask.GetUnit(sourceCol);

            // try to select data column automatically.
            // if there is one fulfilling requisites and has name matching, take it.
            // if there is only one fulfilling requisites, take it

            if (!String.IsNullOrEmpty(sourceUnit))
            {
               // search for unit supported
               foreach (ImportDataColumn dataCol in _table.Columns)
               {
                  if (!isDataColumnUsableForSourceColumn(dataCol, sourceCol)) continue;
                  if (!dataCol.SupportsUnit(sourceUnit)) continue;
                  // is there a unit supported with equal name?
                  foreach (ImportDataColumn dataColName in _table.Columns)
                  {
                     if (!isDataColumnUsableForSourceColumn(dataColName, sourceCol)) continue;
                     if (!dataColName.SupportsUnit(sourceUnit)) continue;
                     if (!checkNameForMapping(dataColName, sourceCol)) continue;
                     tableColumn = dataColName.ColumnName;
                     found = true;
                     break;
                  }
                  if (found) break;
                  //is there a unit supported with same data type?
                  foreach (ImportDataColumn dataColName in _table.Columns)
                  {
                     if (!isDataColumnUsableForSourceColumn(dataColName, sourceCol)) continue;
                     if (!dataColName.SupportsUnit(sourceUnit)) continue;
                     if (dataColName.DataType != sourceCol.DataType) continue;
                     //before we take this one, lets see whether we could avoid multiple mapping
                     if (doesMappingExistsFor(dataColName.ColumnName))
                        foreach (ImportDataColumn dataCol2 in _table.Columns)
                        {
                           if (dataCol2 == dataColName) continue;
                           if (doesMappingExistsFor(dataCol2.ColumnName)) continue;
                           if (!isDataColumnUsableForSourceColumn(dataCol2, sourceCol)) continue;
                           if (!dataCol2.SupportsUnit(sourceUnit)) continue;
                           if (dataCol2.DataType != sourceCol.DataType) continue;
                           tableColumn = dataCol2.ColumnName;
                           found = true;
                           break;
                        }
                     if (found) break;
                     tableColumn = dataColName.ColumnName;
                     found = true;
                     break;
                  }
                  if (found) break;
                  tableColumn = dataCol.ColumnName;
                  found = true;
                  break;
               }
            }

            // search for equal names
            if (!found)
            {
               foreach (ImportDataColumn dataCol in _table.Columns)
               {
                  if (!isDataColumnUsableForSourceColumn(dataCol, sourceCol)) continue;
                  if (!checkNameForMapping(dataCol, sourceCol)) continue;
                  //is there an equal name with same data type?
                  foreach (ImportDataColumn dataColName in _table.Columns)
                  {
                     if (!isDataColumnUsableForSourceColumn(dataColName, sourceCol)) continue;
                     if (!checkNameForMapping(dataColName, sourceCol)) continue;
                     if (dataColName.DataType != sourceCol.DataType) continue;
                     //before we take this one, lets see whether we could avoid multiple mapping
                     if (doesMappingExistsFor(dataColName.ColumnName))
                        foreach (ImportDataColumn dataCol2 in _table.Columns)
                        {
                           if (dataCol2 == dataColName) continue;
                           if (doesMappingExistsFor(dataCol2.ColumnName)) continue;
                           if (!isDataColumnUsableForSourceColumn(dataCol2, sourceCol)) continue;
                           if (dataCol2.DataType != sourceCol.DataType) continue;
                           tableColumn = dataCol2.ColumnName;
                           found = true;
                           break;
                        }
                     if (found) break;
                     tableColumn = dataColName.ColumnName;
                     found = true;
                     break;
                  }
                  if (found) break;
                  tableColumn = dataCol.ColumnName;
                  found = true;
                  break;
               }
            }

            //search for equal names on meta data column 
            if (!found && _table.MetaData != null)
            {
               foreach (MetaDataColumn dataCol in _table.MetaData.Columns)
               {
                  if (!isDataColumnUsableForSourceColumn(dataCol, sourceCol)) continue;
                  if (isAlreadyMapped(sourceCol.ColumnName, dataCol.ColumnName)) continue;
                  if (!checkNameForMapping(dataCol, sourceCol)) continue;
                  //is there an equal name with same data type?
                  foreach (MetaDataColumn dataColName in _table.MetaData.Columns)
                  {
                     if (!isDataColumnUsableForSourceColumn(dataColName, sourceCol)) continue;
                     if (isAlreadyMapped(sourceCol.ColumnName, dataCol.ColumnName)) continue;
                     if (!checkNameForMapping(dataColName, sourceCol)) continue;
                     if (dataColName.DataType != sourceCol.DataType) continue;
                     tableColumn = dataColName.ColumnName;
                     found = true;
                     break;
                  }
                  if (found) break;
                  tableColumn = dataCol.ColumnName;
                  found = true;
                  break;
               }
            }
            // search for first with same datatype
            if (!found)
            {
               foreach (ImportDataColumn dataCol in _table.Columns)
               {
                  if (!isDataColumnUsableForSourceColumn(dataCol, sourceCol)) continue;
                  if (dataCol.DataType != sourceCol.DataType) continue;
                  //before we take this one, lets see whether we could avoid multiple mapping
                  if (doesMappingExistsFor(dataCol.ColumnName))
                     foreach (ImportDataColumn dataCol2 in _table.Columns)
                     {
                        if (dataCol2 == dataCol) continue;
                        if (doesMappingExistsFor(dataCol2.ColumnName)) continue;
                        if (!isDataColumnUsableForSourceColumn(dataCol2, sourceCol)) continue;
                        if (dataCol2.DataType != sourceCol.DataType) continue;
                        tableColumn = dataCol2.ColumnName;
                        found = true;
                        break;
                     }
                  if (found) break;
                  tableColumn = dataCol.ColumnName;
                  //                  found = true;
                  break;
               }
            }

            // set metadata and input parameters for mapping
            MetaDataTable metaData = null;
            IList<InputParameter> inputParameters = null;
            if (!String.IsNullOrEmpty(tableColumn) && _table.Columns.ContainsName(tableColumn))
            {
               var dataCol = _table.Columns.ItemByName(tableColumn).Clone();

               if (dataCol.MetaData != null)
                  metaData = dataCol.MetaData.Copy();

               var unit = _importerTask.GetUnit(sourceCol) ?? String.Empty;
               if (dataCol.SupportsUnit(unit))
               {
                  _importerTask.SetColumnUnit(dataCol, unit, true);
                  if (dataCol.ActiveDimension?.InputParameters != null)
                     inputParameters = new List<InputParameter>(dataCol.ActiveDimension.InputParameters);
                  selectedUnit = dataCol.ActiveUnit;
                  isExplicitlySet = dataCol.IsUnitExplicitlySet;
               }
            }

            foreach (var cm in Mapping)
            {
               if (cm.SourceColumn != sourceCol.ColumnName) continue;
               cm.Target = tableColumn;
               cm.MetaData = metaData;
               cm.InputParameters = inputParameters;
               cm.SelectedUnit = selectedUnit;
               cm.IsUnitExplicitlySet = isExplicitlySet;
            }
         }
      }

      private void clearMapping()
      {
         foreach (var cm in Mapping)
         {
            cm.Target = String.Empty;
            cm.MetaData = null;
            cm.InputParameters = null;
            cm.SelectedUnit = new Unit();
            cm.IsUnitExplicitlySet = false;
         }
      }

      /// <summary>
      /// Method for checking if target column could be mapped to source column.
      /// </summary>
      private static bool isDataColumnUsableForSourceColumn(DataColumn dataColumn, DataColumn sourceColumn)
      {
         return (sourceColumn.DataType == dataColumn.DataType || sourceColumn.DataType == typeof(string) || dataColumn.DataType == typeof(string));
      }

      /// <summary>
      /// Event raised before a cell will be rendered.
      /// </summary>
      void onCustomRowCellEdit(object sender, CustomRowCellEditEventArgs e)
      {
         var view = sender as GridView;
         if (view == null) return;
         if (!e.Column.OptionsColumn.AllowEdit) return;

         var excelColumn = getNameFor(new ColumnMapping(), ColumnMappingProperties.SourceColumn);
         var sourceGridColumn = view.Columns[excelColumn];
         var sourceColumnName = (string)view.GetRowCellValue(e.RowHandle, sourceGridColumn);
         if (String.IsNullOrEmpty(sourceColumnName)) return;
         var sourceColumn = _sourceTable.Columns[sourceColumnName];
         var mappingRow = Mapping[e.RowHandle];

         ImageComboBoxEdit editorObject;
         if (_editorsForDisplay.ContainsKey(sourceColumnName))
            editorObject = _editorsForDisplay[sourceColumnName];
         else
         {
            editorObject = new ImageComboBoxEdit();
            _editorsForDisplay.Add(sourceColumnName, editorObject);
            Controls.Add(editorObject);
            var editor = editorObject.Properties;

            editor.AutoComplete = true;
            editor.AllowNullInput = DefaultBoolean.True;
            editor.CloseUpKey = new KeyShortcut(Keys.Enter);
         }
         editorObject.EditValue = mappingRow.Target;
         // create individual list of values
         fillComboBoxItems(editorObject, mappingRow, sourceColumn);
         refreshButtons(editorObject, mappingRow);

         e.RepositoryItem = editorObject.Properties;
      }

      /// <summary>
      /// Create individual list of values for each row depending on data type, unit of source column.
      /// </summary>
      void onCustomRowCellEditForEditing(object sender, CustomRowCellEditEventArgs e)
      {
         var view = sender as GridView;
         if (view == null) return;
         if (!e.Column.OptionsColumn.AllowEdit) return;

         var excelColumn = getNameFor(new ColumnMapping(), ColumnMappingProperties.SourceColumn);
         var sourceGridColumn = view.Columns[excelColumn];
         var sourceColumnName = (string)view.GetRowCellValue(e.RowHandle, sourceGridColumn);
         if (String.IsNullOrEmpty(sourceColumnName)) return;
         var sourceColumn = _sourceTable.Columns[sourceColumnName];
         var mappingRow = Mapping[e.RowHandle];

         //the information of the current mappingRow is needed in both underlying event handlers.
         _contextMappingRow = mappingRow;

         ImageComboBoxEdit editorObject;
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

         editorObject.EditValue = mappingRow.Target;
         // create individual list of values
         fillComboBoxItems(editorObject, mappingRow, sourceColumn);
         refreshButtons(editorObject, mappingRow);

         if (editorObject.Properties.Items.Count == 0) return;
         e.RepositoryItem = editorObject.Properties;
      }

      private void createButtons(ButtonEdit editor, ColumnMapping mappingRow)
      {
         createDeleteButton(editor);
         editor.ButtonClick += onEditorButtonClick;
         if (!String.IsNullOrEmpty(mappingRow.Target))
         {
            var tableColumn = getTableColumn(mappingRow);
            if (tableColumn == null) return;

            // create meta data button
            createMetaDataButton(editor, (tableColumn.MetaData != null));

            // create unit information button
            var unit = _importerTask.GetUnit(_sourceTable.Columns[mappingRow.SourceColumn]);
            if (!String.IsNullOrEmpty(unit) && tableColumn.SupportsUnit(unit))
               _importerTask.SetColumnUnit(tableColumn, unit, mappingRow.IsUnitExplicitlySet);
            createUnitInformationButton(editor, (tableColumn.Dimensions != null));
         }
      }

      private void refreshButtons(ButtonEdit editor, ColumnMapping mappingRow)
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

      /// <summary>
      /// This event handler is mapped to dynamically created editor.
      /// </summary>
      private void onEditorEditValueChanged(object sender, EventArgs e)
      {
         var editor = sender as ImageComboBoxEdit;
         if (editor == null) return;

         var mappingRow = _contextMappingRow;
         if (mappingRow == null) return;

         _grid.MainView.PostEditor();

         if (editor.EditValue != null && editor.EditValue.ToString() == editor.Properties.NullText)
            editor.EditValue = null;

         mappingRow.Target = editor.EditValue?.ToString() ?? String.Empty;
         mappingRow.MetaData = null;
         mappingRow.InputParameters = null;

         refreshButtons(editor, mappingRow);
      }

      /// <summary>
      /// This event handler is mapped to dynamically created editor. 
      /// </summary>
      private void onEditorButtonClick(object sender, ButtonPressedEventArgs e)
      {
         var editor = sender as ImageComboBoxEdit;
         if (editor == null) return;

         var mappingRow = _contextMappingRow;
         if (mappingRow == null) return;

         if (editor.EditValue == null) return;

         if (editor.EditValue.ToString() != mappingRow.Target)
         {
            XtraMessageBox.Show($"Wrong row: {mappingRow.SourceColumn}!");
            return;
         }

         if (e.Button.Caption == "Delete")
         {
            editor.EditValue = null;
            return;
         }

         switch (e.Button.Collection.IndexOf(e.Button))
         {
            case (int)Buttons.MetaData:
               //meta data button
               {
                  if (String.IsNullOrEmpty(mappingRow.Target)) return;
                  var tableColumn = getTableColumn(mappingRow);
                  if (tableColumn?.MetaData == null) return;
                  if (mappingRow.MetaData == null)
                     mappingRow.MetaData = tableColumn.MetaData.Copy();

                  var icon = (ParentForm == null) ? ApplicationIcons.EmptyIcon : ParentForm.Icon;
                  var frm = new EditMetaDataView(mappingRow.MetaData) { StartPosition = FormStartPosition.CenterParent, Icon = icon };
                  frm.OnCopyMetaData += onCopyMetaData;
                  if (mappingRow.SelectedUnit.Name != null)
                     _importerTask.SetColumnUnit(tableColumn, mappingRow.SelectedUnit.Name, mappingRow.IsUnitExplicitlySet);
                  frm.StartPosition = FormStartPosition.CenterParent;
                  if (frm.ShowDialog() == DialogResult.OK)
                  {
                     tableColumn.MetaData = mappingRow.MetaData.Copy();
                     var item = editor.Properties.Items.GetItem(mappingRow.Target);
                     item.ImageIndex = _importerTask.GetImageIndex(tableColumn);
                     if (tableColumn.Dimensions != null)
                     {
                        tableColumn.SetColumnUnitDependingOnMetaData();
                        mappingRow.SelectedUnit = tableColumn.ActiveUnit;
                        mappingRow.IsUnitExplicitlySet = tableColumn.IsUnitExplicitlySet;
                     }
                  }
               }
               break;
            case (int)Buttons.UnitInformation:
               //unit information button
               {
                  if (String.IsNullOrEmpty(mappingRow.Target)) return;
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

               }
               break;
         }
      }

      /// <summary>
      /// This method reacts on copy (apply to others) unit information event.
      /// </summary>
      void onCopyUnitInfo(object sender, SetUnitView.CopyUnitInfoEventArgs e)
      {
         foreach (var cm in Mapping)
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
         _grid.MainView?.RefreshData();
      }

      /// <summary>
      /// This method reacts on Copy (apply to others) meta data event.
      /// </summary>
      void onCopyMetaData(object sender, EventArgs e)
      {
         foreach (var cm in Mapping)
         {
            if (cm.Target != _contextMappingRow.Target) continue;
            cm.MetaData = _contextMappingRow.MetaData.Copy();

            //after changing the meta data the selected unit might be invalid, so the unit must be changed
            var tableColumn = getTableColumn(cm);

            if (tableColumn.Dimensions != null)
            {
               tableColumn.SetColumnUnitDependingOnMetaData();
               cm.SelectedUnit = tableColumn.ActiveUnit;
               cm.IsUnitExplicitlySet = tableColumn.IsUnitExplicitlySet;
            }
         }
         _grid.MainView?.RefreshData();
      }

      private void cleanMemory()
      {
         _columnCaptionHelper = null;
         if(_grid.ToolTipController != null)
            _grid.ToolTipController.GetActiveObjectInfo -= onGetActiveObjectInfo;
         _contextMappingRow = null;
         if (_grid != null)
         {
            CleanUpHelper.ReleaseControls(_grid.Controls);
            _grid.DataSource = null;
            _grid.Dispose();
         }

         if (Mapping != null)
         {
            foreach (var cm in Mapping)
               cm.Dispose();
            Mapping.Clear();
         }
         Mapping = null;

         _sourceTable?.Dispose();
         _sourceTable = null;

         _table?.Dispose();
         _table = null;

         CleanUpHelper.ReleaseControls(Controls);
         Controls.Clear();
      }
   }
}
