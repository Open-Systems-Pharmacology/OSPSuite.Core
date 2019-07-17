using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using OSPSuite.Assets;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Importer;
using OSPSuite.Core.Importer.Mappers;
using OSPSuite.Core.Services;
using OSPSuite.Presentation.Presenters;
using OSPSuite.Presentation.Services;
using OSPSuite.Presentation.View;
using OSPSuite.Utility.Extensions;

namespace OSPSuite.Presentation.Presenter
{
   public interface IImporterPresenter : IDisposablePresenter
   {
      IEnumerable<DataRepository> ImportDataSets(IReadOnlyList<MetaDataCategory> metaDataCategories, IReadOnlyList<ColumnInfo> columnInfos, DataImporterSettings dataImporterSettings, Mode mode);

      /// <summary>
      ///    Gets the range being set for the data table
      /// </summary>
      /// <returns>The range</returns>
      Rectangle? GetRange();

      /// <summary>
      ///    Selects a range of data from the data table for consideration (rather than the full table)
      /// </summary>
      /// <returns>true if the GetRange() method can be used to retrieve the range, otherwise false</returns>
      bool SelectRange(string sourceFile, string sheetName);
   }

   internal class ImporterPresenter : AbstractDisposablePresenter<IImporterView, IImporterPresenter>, IImporterPresenter
   {
      private readonly IImportDataTableToDataRepositoryMapper _repositoryMapper;
      private readonly INamingPatternPresenter _namingPatternPresenter;
      private readonly IRepositoryNamingTask _namingTask;
      private readonly IDialogCreator _dialogCreator;
      private readonly INamingPatternToRepositoryNameMapper _namingPatternToRepositoryNameMapper;
      private readonly IExcelPreviewPresenter _excelPreviewPresenter;

      public ImporterPresenter(
         IImporterView view,
         IImportDataTableToDataRepositoryMapper repositoryMapper,
         INamingPatternPresenter namingPatternPresenter,
         IDialogCreator dialogCreator,
         IExcelPreviewPresenter excelPreviewPresenter,
         IRepositoryNamingTask repositoryNamingTask,
         INamingPatternToRepositoryNameMapper namingPatternToRepositoryNameMapper)
         : base(view)
      {
         _repositoryMapper = repositoryMapper;
         _excelPreviewPresenter = excelPreviewPresenter;
         _namingPatternPresenter = namingPatternPresenter;
         _view.SetNamingView(_namingPatternPresenter.View);
         _dialogCreator = dialogCreator;
         _namingTask = repositoryNamingTask;
         _namingPatternToRepositoryNameMapper = namingPatternToRepositoryNameMapper;

         _subPresenterManager.Add(_excelPreviewPresenter);
         _subPresenterManager.Add(_namingPatternPresenter);
      }

      protected override void Cleanup()
      {
         try
         {
            _excelPreviewPresenter.Dispose();
         }
         finally
         {
            base.Cleanup();
         }
      }

      public IEnumerable<DataRepository> ImportDataSets(IReadOnlyList<MetaDataCategory> metaDataCategories, IReadOnlyList<ColumnInfo> columnInfos, DataImporterSettings dataImporterSettings, Mode mode)
      {
         var sourceFile = _dialogCreator.AskForFileToOpen(Captions.Importer.PleaseSelectDataFile, Captions.Importer.ImportFileFilter, Constants.DirectoryKey.OBSERVED_DATA);
         if (string.IsNullOrEmpty(sourceFile))
            return new List<DataRepository>();

         _namingPatternPresenter.WithToken(dataImporterSettings.Token);
         _namingTask.CreateNamingPatternsBasedOn(metaDataCategories, dataImporterSettings).Each((_namingPatternPresenter.AddPresetNamingPatterns));
         _view.Caption = dataImporterSettings.Caption;
         _view.SetIcon(dataImporterSettings.Icon);
         _view.StartImport(sourceFile, new ImportTableConfiguration {MetaDataCategories = metaDataCategories, ColumnInfos = columnInfos}, mode);

         _view.Display();

         if (_view.Canceled)
            return Enumerable.Empty<DataRepository>();

         bool errorNaN;
         var dataRepositoriesToImport = ConvertImportDataTableList(_view.Imports, columnInfos, out errorNaN);

         dataRepositoriesToImport.Each(dataRepository =>
         {
            dataRepository.RenameColumnsToSource();
            dataRepository.CutUnitFromColumnNames();
         });

         if (errorNaN)
            _dialogCreator.MessageBoxInfo(Error.MESSAGE_ERROR_NAN);

         _namingPatternToRepositoryNameMapper.RenameRepositories(dataRepositoriesToImport, _namingPatternPresenter.Pattern, dataImporterSettings);

         return dataRepositoriesToImport;
      }

      private bool selectRange(DataTable exportDataTable)
      {
         return _excelPreviewPresenter.SelectRange(exportDataTable);
      }

      public Rectangle? GetRange()
      {
         return _excelPreviewPresenter.Range;
      }

      /// <summary>
      ///    This method converts a list of import data tables to a list of DataRepository objects.
      /// </summary>
      /// <param name="importDataTables">List of import data tables.</param>
      /// <param name="columnInfos">Specification of columns including specification of meta data.</param>
      /// <param name="errorNaN">
      ///    This is a flag informing the caller about whether error have been changed to NaN because they
      ///    are out of definition.
      /// </param>
      /// <returns>List of DataRepository objects.</returns>
      public IReadOnlyList<DataRepository> ConvertImportDataTableList(IEnumerable<ImportDataTable> importDataTables, IReadOnlyList<ColumnInfo> columnInfos, out bool errorNaN)
      {
         var retVal = new List<DataRepository>();
         errorNaN = false;

         //convert tables
         foreach (var importDataTable in importDataTables)
         {
            bool tableErrorNaN;
            retVal.Add(_repositoryMapper.ConvertImportDataTable(importDataTable, columnInfos, out tableErrorNaN));
            errorNaN |= tableErrorNaN;
         }

         return retVal;
      }

      public bool SelectRange(string sourceFile, string sheetName)
      {
         var wb = Services.SmartXLS.ReadExcelFile(sourceFile);
         wb.Sheet = wb.GetIndexFromSheetName(sheetName);

         return selectRange(wb.ExportDataTable(0, 0, wb.LastRow + 1, wb.LastCol + 1, false, false));
      }
   }
}