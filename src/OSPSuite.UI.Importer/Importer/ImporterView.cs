using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DevExpress.XtraTab;
using OSPSuite.Assets;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Importer;
using OSPSuite.Core.Importer.Mappers;
using OSPSuite.Core.Services;
using OSPSuite.Presentation.Presenter;
using OSPSuite.Presentation.Services;
using OSPSuite.Presentation.View;
using OSPSuite.Presentation.Views;
using OSPSuite.UI.Extensions;
using OSPSuite.UI.Services;
using OSPSuite.UI.Views;
using OSPSuite.Utility.Collections;

namespace OSPSuite.UI.Importer
{
   public partial class ImporterView : BaseModalView, IImporterView
   {
      private const string CAPTION_IMPORTS_PAGE = "Imports";
      private const string OVERWRITE_QUERY_TEXT = "Overwrite existing tables?";
      private const string CANCEL_QUERY_TEXT = "Do you really want to cancel and lose your mapping information?";
      private const string SINGLE_MODE_QUERY_TEXT = "Only the first table ({0}) will be imported. Continue?";

      private DataSet _imports = new DataSet();
      private OpenSourceFileControl _openSourceFileControl;
      private SourceFilePreviewControl _sourceFilePreviewControl;
      private ColumnMappingControl _columnMappingControl;
      private DataSetControl _dataSetControl;
      private Presentation.Services.Importer _importer;
      private ImportDataTable _importDataTable;
      private Dictionary<string, ColumnMappingControl> _columnMappingControls;
      private bool _importFlag;
      private IReadOnlyList<ColumnInfo> _columnInfos;
      private IColumnInfosToImportDataTableMapper _importMapper;
      private IImportDataTableToDataRepositoryMapper _dataRepositoryMapper;
      private readonly IImageListRetriever _imageListRetriever;
      private readonly IDialogCreator _dialogCreator;
      private readonly IImporterTask _importerTask;
      private IView _namingView;
      private Mode _mode;
      private Cache<string, Rectangle> _rangesCache;
      private IImporterPresenter _presenter;
      private readonly IColumnCaptionHelper _columnCaptionHelper;

      public ImporterView(IColumnInfosToImportDataTableMapper importMapper, IImportDataTableToDataRepositoryMapper dataRepositoryMapper, IImageListRetriever imageListRetriever,
         IDialogCreator dialogCreator, IImporterTask importerTask, IColumnCaptionHelper columnCaptionHelper)
      {
         _importMapper = importMapper;
         _dataRepositoryMapper = dataRepositoryMapper;
         _imageListRetriever = imageListRetriever;
         _dialogCreator = dialogCreator;
         _importerTask = importerTask;
         _columnCaptionHelper = columnCaptionHelper;
         InitializeComponent();
      }

      private void changeTabs(IXtraTabPage page, IXtraTabPage prevPage)
      {
         if (page != null)
            page.Appearance.Header.Font = Fonts.SelectedTabHeaderFont;
         if (prevPage != null)
            prevPage.Appearance.Header.Font = Fonts.NonSelectedTabHeaderFont;
      }

      public override void InitializeResources()
      {
         base.InitializeResources();
         layoutItemSelectRange.AdjustLongButtonSize();
         layoutItemImportAll.AdjustButtonSize();
         layoutItemImport.AdjustButtonSize();
         btnSelectRange.InitWithImage(ApplicationIcons.PreviewOriginData, Captions.Importer.PreviewExcelData);
         resetImportButtonText();
      }

      public void StartImport(string sourceFile, ImportTableConfiguration configuration, Mode mode)
      {
         var metaDataCategories = configuration.MetaDataCategories;
         _mode = mode;
         _columnInfos = configuration.ColumnInfos;

         var importDataTable = _importMapper.ConvertToImportDataTable(metaDataCategories, _columnInfos);

         if (importDataTable == null)
            throw new InvalidArgumentException("Could not map to import table");

         MaximizeBox = true;
         namingImportPanel.FillWith(_namingView);
         _importer = new Presentation.Services.Importer(_dataRepositoryMapper, _columnInfos, _importerTask, _dialogCreator);
         _importDataTable = importDataTable;
         _columnMappingControls = new Dictionary<string, ColumnMappingControl>();

         // Page Source
         _openSourceFileControl = new OpenSourceFileControl(_dialogCreator, sourceFile) {Dock = DockStyle.Fill};
         
         openSourceFileControlPanel.FillWith(_openSourceFileControl);
         _openSourceFileControl.OnOpenSourceFile += openSourceFileEvent;
         xtraTabControl.SelectedPageChanged += (s, e) => OnEvent(() => changeTabs(e.Page, e.PrevPage));
         xtraTabControl.SelectedTabPage.Appearance.Header.Font = Fonts.SelectedTabHeaderFont;

         _rangesCache = new Cache<string, Rectangle>();

         createSourceFilePreviewControl(sourceFile);
         btnImport.Click += (s, e) => OnEvent(importData);
         btnSelectRange.Click += (s, e) => OnEvent(selectRange);
         btnImportAll.Click += (s, e) => OnEvent(importAllData);

         FormClosing += onFormClosing;

         // Page Imports
         _dataSetControl = new DataSetControl(_imports, _columnInfos, true);
         panelImportedTabs.FillWith(_dataSetControl);
         _dataSetControl.MissingRequiredData += (s, e) => OnEvent(() => enableOKButton(false));
         _dataSetControl.RequiredDataCompleted += (s, e) => OnEvent(() => enableOKButton(true));
         _dataSetControl.TableDeleted += (s, e) => OnEvent(enableImportsPage);
         enableImportsPage();

         enableOKButton(false);
      }

      private void openSourceFileEvent(object sender, OpenSourceFileControl.OpenSourceFileEventArgs e)
      {
         OnEvent(() => openSourceFile(e.SourceFile));
      }

      private void openSourceFile(string sourceFile)
      {
         _rangesCache = new Cache<string, Rectangle>();
         _importer.ClearCache();
         createSourceFilePreviewControl(sourceFile);
      }

      private void updateRange(Rectangle? range)
      {
         var sheetName = _sourceFilePreviewControl.SelectedSheetName;

         if (_rangesCache.Contains(sheetName))
         {
            var oldRange = _rangesCache[sheetName];
            if (range.Equals(oldRange))
               return;
         }
         _rangesCache.Remove(sheetName);

         if (range != null)
            _rangesCache.Add(sheetName, (Rectangle) range);

         var data = _importer.GetPreview(_openSourceFileControl.SourceFile, _rangesCache);

         var table = data.Tables[sheetName];
         _sourceFilePreviewControl.UpdateTable(table.TableName, table);

         if (_columnMappingControls.ContainsKey(table.TableName))
            _columnMappingControls.Remove(table.TableName);

         layoutColumnMappingControl(table);
      }

      private void selectRange()
      {
         if (_presenter.SelectRange(_openSourceFileControl.SourceFile, _sourceFilePreviewControl.SelectedSheetName))
         {
            updateRange(_presenter.GetRange());
         }
      }

      private void onFormClosing(object sender, FormClosingEventArgs e)
      {
         if (DialogResult == DialogResult.Cancel)
         {
            if (XtraMessageBox.Show(CANCEL_QUERY_TEXT, Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
               e.Cancel = true;

            return;
         }

         //OK was clicked
         if (_mode != Mode.SingleRepository || _imports.Tables.Count <= 1) return;

         var firstTableName = _imports.Tables[0].TableName;
         XtraMessageBox.Show(string.Format(SINGLE_MODE_QUERY_TEXT, firstTableName), Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
      }

      public IList<ImportDataTable> Imports
      {
         get
         {
            var retVal = new List<ImportDataTable>();
            foreach (ImportDataTable table in _imports.Tables)
            {
               var newTable = table.Copy();
               newTable.RemapMetaDataListOfValueSelection();
               retVal.Add(newTable);
            }
            return retVal;
         }
      }

      public void SetIcon(ApplicationIcon icon)
      {
         if (icon == null) return;
         Icon = icon;
      }

      public void SetNamingView(IView view)
      {
         _namingView = view;
      }

      /// <summary>
      ///    Method for creating a new preview control showing the sheets of given excel file.
      /// </summary>
      /// <param name="sourceFile">Full path and name of the excel source file.</param>
      private void createSourceFilePreviewControl(string sourceFile)
      {
         sourceFilePreviewControlPanel.Controls.Clear();
         columnMappingControlPanel.Controls.Clear();
         cleanColumnMappingControls();
         _columnMappingControls.Clear();

         _sourceFilePreviewControl = new SourceFilePreviewControl(sourceFile, _rangesCache, _importer) {Dock = DockStyle.Fill};
         _sourceFilePreviewControl.SheetSelected += (s, e) => OnEvent(() => layoutColumnMappingControl(e.SheetData));

         sourceFilePreviewControlPanel.Controls.Add(_sourceFilePreviewControl);
         layoutColumnMappingControl(_sourceFilePreviewControl.SelectedSheetData);
      }

      /// <summary>
      ///    Method for creating a new column mapping control for selected sheet.
      /// </summary>
      /// <param name="sourceTable">Data table of a sheet of the source excel file.</param>
      private void layoutColumnMappingControl(DataTable sourceTable)
      {
         layoutControlSourceTab.BeginUpdate();
         columnMappingControlPanel.BeginInit();
         try
         {
            if (!_columnMappingControls.TryGetValue(sourceTable.TableName, out _columnMappingControl))
            {
               _columnMappingControl = createColumnMappingControl(sourceTable);
            }
            _columnMappingControl.ValidateMapping();
            columnMappingControlPanel.Controls.Clear();
            columnMappingControlPanel.Controls.Add(_columnMappingControl);
         }
         finally
         {
            columnMappingControlPanel.EndInit();
            layoutControlSourceTab.EndUpdate();
         }
      }

      private ColumnMappingControl createColumnMappingControl(DataTable sourceTable)
      {
         var columnMappingControl = new ColumnMappingControl(sourceTable, _importDataTable, _imageListRetriever, _importerTask, _columnCaptionHelper) {Dock = DockStyle.Fill};
         _columnMappingControls.Add(sourceTable.TableName, columnMappingControl);
         columnMappingControl.OnMappingCompleted += onColumnMappingMappingCompleted;
         columnMappingControl.OnMissingMapping += onColumnMappingMissingMapping;

         return columnMappingControl;
      }

      /// <summary>
      ///    If there is an error in the mapping the column mapping control brings an error message which is used as tooltip and
      ///    a missing data icon informs the user directly that something is wrong.
      /// </summary>
      private void onColumnMappingMissingMapping(object sender, ColumnMappingControl.MissingMappingEventArgs e)
      {
         Mapping.Image = ApplicationIcons.MissingData;
         Mapping.OptionsToolTip.ToolTip = e.Message;
         resetImportButtonText();
         enableImportButton(false);
      }

      private void resetImportButtonText()
      {
         btnImportAll.InitWithImage(ApplicationIcons.ImportAll, Captions.Importer.ImportAll);
         btnImport.InitWithImage(ApplicationIcons.ImportAction, Captions.Importer.Import);
      }

      /// <summary>
      ///    If the mapping is ok the user gets directly informed by removing a missing data icon and tooltip.
      /// </summary>
      private void onColumnMappingMappingCompleted(object sender, EventArgs e)
      {
         var args = e as MappingCompletedEventArgs;

         Mapping.Image = ApplicationIcons.EmptyIcon;
         Mapping.OptionsToolTip.ToolTip = String.Empty;
         if (args != null)
            updateImportCount(args.SheetName);
         enableImportButton(true);
      }

      /// <summary>
      ///    Method for enabling the import button.
      /// </summary>
      private void enableImportButton(bool value)
      {
         btnImport.Enabled = value;
         btnImportAll.Enabled = value;
      }

      /// <summary>
      ///    Method for enabling the imports page.
      /// </summary>
      private void enableImportsPage()
      {
         var importsPage = xtraTabControl.TabPages[1];
         importsPage.PageEnabled = (_imports.Tables.Count > 0);
         importsPage.PageVisible = (_imports.Tables.Count > 0);

         importsPage.Text = _imports.Tables.Count > 0
            ? $"{CAPTION_IMPORTS_PAGE} ({_imports.Tables.Count})"
            : CAPTION_IMPORTS_PAGE;

         //show this page automatically
         if (!importsPage.PageEnabled) return;
         if (!importsPage.PageVisible) return;
         xtraTabControl.SelectedTabPage = importsPage;
         _dataSetControl.SetSelectedTabPageIndex(_imports.Tables.Count - 1);
      }

      /// <summary>
      ///    Method for enabling the transfer button.
      /// </summary>
      private void enableOKButton(bool value)
      {
         OkEnabled = value;
      }

      /// <summary>
      ///    This method handles the import of the sheet data to new import data tables.
      /// </summary>
      private void importData()
      {
         var currentCursor = Cursor.Current;
         Cursor.Current = Cursors.WaitCursor;

         try
         {
            var excelFile = _openSourceFileControl.SourceFile;
            var sheet = _sourceFilePreviewControl.SelectedSheetName;
            var cms = _columnMappingControl.Mapping;


            var newTables = _importer.ImportDataTables(_importDataTable, excelFile, sheet, cms, _rangesCache);
            if (newTables.Count == 0)
            {
               XtraMessageBox.Show("Nothing imported!", Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
               return;
            }

            shouldOverwrite(newTables);

            addDataTablesToImports(newTables, excelFile, sheet);

            _dataSetControl.ActualizeData();
            enableImportsPage();
         }
         catch (Exception e)
         {
            XtraMessageBox.Show(e.Message, "An error occurred:", MessageBoxButtons.OK, MessageBoxIcon.Error);
         }
         finally
         {
            Cursor.Current = currentCursor;
         }
      }

      private void shouldOverwrite(IList<ImportDataTable> newTables)
      {
         var overwrite = false;
         var source = newTables[0].Source;
         if (_imports.Tables.Cast<ImportDataTable>().Any(existingTable => existingTable.Source == source))
         {
            overwrite =
            (XtraMessageBox.Show(OVERWRITE_QUERY_TEXT, Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question) ==
             DialogResult.Yes);
         }

         if (overwrite)
            removeAllTableOf(source);
      }

      private void addDataTablesToImports(IList<ImportDataTable> newTables, string excelFile, string sheet)
      {
         foreach (var dataTable in newTables)
         {
            var tableName = getTableName(excelFile, newTables, sheet, dataTable, newTables.Count);
            dataTable.TableName = tableName;

            // find unique name for table
            giveUniqueName(dataTable, tableName);

            _imports.Tables.Add(dataTable);
         }
      }

      private string getTableName(string excelFile, IList<ImportDataTable> newTables, string sheet, ImportDataTable dataTable, int tableCount)
      {
         var fileNameWithoutExtension = Path.GetFileNameWithoutExtension(excelFile);
         var tableName = (tableCount > 1)
            ? nameWithSheetIndex(fileNameWithoutExtension, sheet, newTables, dataTable)
            : nameWithoutSheetIndex(fileNameWithoutExtension, sheet);

         tableName = (dataTable.TableName.Length > 0) ? $"{tableName} ({dataTable.TableName})" : tableName;
         return tableName;
      }

      private static string nameWithoutSheetIndex(string fileNameWithoutExtension, string sheet)
      {
         return $"{fileNameWithoutExtension}.{sheet}";
      }

      private static string nameWithSheetIndex(string fileNameWithoutExtension, string sheet, IList<ImportDataTable> newTables, ImportDataTable dataTable)
      {
         return $"{fileNameWithoutExtension}.{sheet}.{newTables.IndexOf(dataTable) + 1}";
      }

      private void removeAllTableOf(string source)
      {
         for (var i = _imports.Tables.Count - 1; i >= 0; i--)
         {
            var existingTable = _imports.Tables[i] as ImportDataTable;
            if (existingTable == null) continue;
            if (existingTable.Source != source) continue;
            _imports.Tables.Remove(existingTable);
         }
      }

      private void updateImportCount(string sheet)
      {
         var excelFile = _openSourceFileControl.SourceFile;
         var preview = _sourceFilePreviewControl.PreviewData;
         if (!preview.Tables.Contains(sheet)) return;
         try
         {
            var countDataTables = _importer.CountDataTables(_importDataTable, excelFile, sheet, _columnMappingControl.Mapping, _rangesCache);
            updateImportButtons(countDataTables);
         }
         catch (Exception)
         {
            updateImportButtons(0);
            throw;
         }
      }

      private void updateImportButtons(int countDataTables)
      {
         setImportCount(countDataTables);
         enableImportButton(countDataTables > 0);
      }

      private void setImportCount(int count)
      {
         btnImport.Text = $"{Captions.Importer.Import} ({count})";
      }

      /// <summary>
      ///    This method handles the import of all sheet data to new import data tables.
      /// </summary>
      private void importAllData()
      {
         var currentCursor = Cursor.Current;
         Cursor.Current = Cursors.WaitCursor;
         try
         {
            var excelFile = _openSourceFileControl.SourceFile;
            var sheets = _importer.GetSheetNames(excelFile);
            var preview = _sourceFilePreviewControl.PreviewData;

            var newTables = collectAllTables(sheets, preview, excelFile);

            if (newTables.Count == 0)
            {
               XtraMessageBox.Show("Nothing imported!", Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
               return;
            }

            var overwrite = promptForOverwrite(sheets, excelFile);

            // do the real import
            foreach (var sheet in sheets)
            {
               var sourceSheet = $"{excelFile}.{sheet}";

               if (overwrite)
                  removeExistingTables(sourceSheet);

               if (!preview.Tables.Contains(sheet)) continue;

               foreach (ImportDataTable dataTable in newTables)
               {
                  if (dataTable.Source != sourceSheet) continue;
                  var tableName = getTableName(excelFile, newTables, sheet, dataTable, newTables.Count(dt => dt.Source == sourceSheet));
                  dataTable.TableName = tableName;

                  // find unique name for table
                  giveUniqueName(dataTable, tableName);

                  _imports.Tables.Add(dataTable);
               }
            }

            _dataSetControl.ActualizeData();
            enableImportsPage();
         }
         catch (Exception e)
         {
            XtraMessageBox.Show(e.Message, "An error occurred:", MessageBoxButtons.OK, MessageBoxIcon.Error);
         }
         finally
         {
            Cursor.Current = currentCursor;
         }
      }

      private void removeExistingTables(string sourceSheet)
      {
         for (var i = _imports.Tables.Count - 1; i >= 0; i--)
         {
            var existingTable = _imports.Tables[i] as ImportDataTable;
            if (existingTable == null) continue;
            if (!existingTable.Source.StartsWith(sourceSheet)) continue;
            _imports.Tables.Remove(existingTable);
         }
      }

      private bool promptForOverwrite(IList<string> sheets, string excelFile)
      {
         var overwrite = false;
         var asked = false;
         foreach (var sheet in sheets)
         {
            var sourceSheet = $"{excelFile}.{sheet}";
            if (_imports.Tables.Cast<ImportDataTable>().Any(existingTable => existingTable.Source == sourceSheet))
            {
               overwrite =
               (XtraMessageBox.Show(OVERWRITE_QUERY_TEXT, Text, MessageBoxButtons.YesNo,
                   MessageBoxIcon.Question) ==
                DialogResult.Yes);
               asked = true;
            }
            if (asked) break;
         }
         return overwrite;
      }

      private List<ImportDataTable> collectAllTables(IList<string> sheets, DataSet preview, string excelFile)
      {
         var newTables = new List<ImportDataTable>();

         //collect all tables for the import
         foreach (var sheet in sheets)
         {
            if (!preview.Tables.Contains(sheet)) continue;
            var cmc = _columnMappingControls.ContainsKey(sheet)
               ? getExistingColumnMappingControl(sheet)
               : createNewColumnMappingControl(preview, sheet);
            cmc.ValidateMapping();
            configureEventsFor(cmc);

            if (!_importFlag) continue;
            var tables = _importer.ImportDataTables(_importDataTable, excelFile, sheet, cmc.Mapping, _rangesCache);
            newTables.AddRange(tables);
         }
         return newTables;
      }

      private void configureEventsFor(ColumnMappingControl cmc)
      {
         cmc.OnMappingCompleted += onColumnMappingMappingCompleted;
         cmc.OnMissingMapping += onColumnMappingMissingMapping;
         cmc.OnMappingCompleted -= setImportFlagToTrue;
         cmc.OnMissingMapping -= setImportFlagToFalse;
      }

      private ColumnMappingControl createNewColumnMappingControl(DataSet preview, string sheet)
      {
         var cmc = new ColumnMappingControl(preview.Tables[sheet], _importDataTable, _imageListRetriever, _importerTask, _columnCaptionHelper) {Dock = DockStyle.Fill};
         cmc.OnMappingCompleted += setImportFlagToTrue;
         cmc.OnMissingMapping += setImportFlagToFalse;
         _columnMappingControls.Add(sheet, cmc);
         return cmc;
      }

      private ColumnMappingControl getExistingColumnMappingControl(string sheet)
      {
         var cmc = _columnMappingControls[sheet];
         cmc.OnMappingCompleted += setImportFlagToTrue;
         cmc.OnMissingMapping += setImportFlagToFalse;
         cmc.OnMappingCompleted -= onColumnMappingMappingCompleted;
         cmc.OnMissingMapping -= onColumnMappingMissingMapping;
         return cmc;
      }

      private void giveUniqueName(DataTable dataTable, string tableName)
      {
         var i = 1;
         while (_imports.Tables.Contains(dataTable.TableName))
            dataTable.TableName = $"{tableName}.{i++}";
      }

      private void setImportFlagToFalse(object sender, EventArgs e)
      {
         _importFlag = false;
      }

      private void setImportFlagToTrue(object sender, EventArgs e)
      {
         _importFlag = true;
      }

      private void cleanColumnMappingControls()
      {
         if (_columnMappingControls == null) return;

         foreach (var cmc in _columnMappingControls.Values)
         {
            cmc.OnMappingCompleted -= setImportFlagToTrue;
            cmc.OnMissingMapping -= setImportFlagToFalse;
            cmc.OnMappingCompleted -= onColumnMappingMappingCompleted;
            cmc.OnMissingMapping -= onColumnMappingMissingMapping;
            cmc.Dispose();
         }
      }

      private void cleanMemory()
      {
         _namingView = null;
         if (_openSourceFileControl != null)
         {
            _openSourceFileControl.OnOpenSourceFile -= openSourceFileEvent;
            _openSourceFileControl = null;
         }

         if (_imports != null)
         {
            foreach (ImportDataTable table in _imports.Tables)
            {
               table.MetaData?.Dispose();
               table.Dispose();
            }
            _imports.Dispose();
         }
         if (_importDataTable != null)
         {
            _importDataTable.MetaData?.Dispose();
            _importDataTable.Dispose();
         }

         CleanUpHelper.ReleaseEvents(_dataSetControl);
         _dataSetControl?.Dispose();
         CleanUpHelper.ReleaseEvents(_sourceFilePreviewControl);
         _sourceFilePreviewControl?.Dispose();
         CleanUpHelper.ReleaseControls(Controls);
         Controls.Clear();

         _imports = null;
         _openSourceFileControl = null;
         _sourceFilePreviewControl = null;
         _columnMappingControl = null;
         _dataSetControl = null;
         _importDataTable = null;
         _importer = null;
         _presenter = null;
         _dataRepositoryMapper = null;
         _importMapper = null;
         _columnInfos = null;

         cleanColumnMappingControls();
         _columnMappingControls?.Clear();
         _columnMappingControls = null;

         columnMappingControlPanel.Controls.Clear();
      }

      public void AttachPresenter(IImporterPresenter presenter)
      {
         _presenter = presenter;
      }
   }
}