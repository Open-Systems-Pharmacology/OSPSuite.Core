using System;
using System.Collections.Generic;
using System.Linq;
using OSPSuite.Assets;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Services;
using OSPSuite.Infrastructure.Import.Core;
using OSPSuite.Infrastructure.Import.Core.DataFormat;
using OSPSuite.Infrastructure.Import.Core.Mappers;
using OSPSuite.Infrastructure.Import.Services;
using OSPSuite.Presentation.Views.Importer;
using OSPSuite.Utility.Collections;

namespace OSPSuite.Presentation.Presenters.Importer
{
   public class ImporterPresenter : AbstractPresenter<IImporterView, IImporterPresenter>, IImporterPresenter
   {
      private readonly IImporterDataPresenter _importerDataPresenter;
      private readonly IColumnMappingPresenter _columnMappingPresenter;
      private readonly IImportConfirmationPresenter _confirmationPresenter;
      private readonly ISourceFilePresenter _sourceFilePresenter;
      private readonly IDataSetToDataRepositoryMapper _dataRepositoryMapper;
      private DataImporterSettings _dataImporterSettings;
      private IReadOnlyList<ColumnInfo> _columnInfos;
      private readonly INanPresenter _nanPresenter;
      private readonly IDataSource _dataSource;
      private IDataSourceFile _dataSourceFile;

      public ImporterPresenter(
         IImporterView view, 
         IDataSetToDataRepositoryMapper dataRepositoryMapper, 
         IImporter importer, 
         INanPresenter nanPresenter, 
         IImporterDataPresenter importerDataPresenter, 
         IImportConfirmationPresenter confirmationPresenter, 
         IColumnMappingPresenter columnMappingPresenter,
         ISourceFilePresenter sourceFilePresenter,
         IDialogCreator dialogCreator
      ) : base(view)
      {
         _importerDataPresenter = importerDataPresenter;
         _confirmationPresenter = confirmationPresenter;
         _columnMappingPresenter = columnMappingPresenter;
         _nanPresenter = nanPresenter;
         _sourceFilePresenter = sourceFilePresenter;
         _dataRepositoryMapper = dataRepositoryMapper;
         _dataSource = new DataSource(importer);

         _sourceFilePresenter.Title = Captions.Importer.PleaseSelectDataFile;
         _sourceFilePresenter.Filter = Captions.Importer.ImportFileFilter;
         _sourceFilePresenter.DirectoryKey = Constants.DirectoryKey.OBSERVED_DATA;
         _sourceFilePresenter.OnSourceFileChanged += (s, e) => SetDataSource(e.FileName);
         _sourceFilePresenter.CheckBeforeSelectFile = () => 
            !_dataSource.DataSets.Any() || dialogCreator.MessageBoxYesNo(Captions.Importer.OpenFileConfirmation) == ViewResult.Yes;

         _view.AddColumnMappingControl(columnMappingPresenter.View);
         _view.AddSourceFileControl(sourceFilePresenter.View);
         _view.AddNanView(nanPresenter.View);
         _confirmationPresenter.OnImportData += ImportData;
         _confirmationPresenter.OnDataSetSelected += plotDataSet;
         _confirmationPresenter.OnNamingConventionChanged += (s, a) =>
         {
            _dataSource.SetNamingConvention(a.NamingConvention); 
            _confirmationPresenter.SetDataSetNames(_dataSource.NamesFromConvention());
         };
         _importerDataPresenter.OnImportSheets += ImportSheetsFromDataPresenter;
         _nanPresenter.OnNaNSettingsChanged += (s,a) =>_columnMappingPresenter.ValidateMapping();
         _view.AddImporterView(_importerDataPresenter.View);
         AddSubPresenters(_importerDataPresenter, _confirmationPresenter, _columnMappingPresenter, _sourceFilePresenter);
         _importerDataPresenter.OnFormatChanged += onFormatChanged;
         _importerDataPresenter.OnTabChanged += onTabChanged;
         _columnMappingPresenter.OnMissingMapping += onMissingMapping;
         _columnMappingPresenter.OnMappingCompleted += onCompletedMapping;
         View.DisableConfirmationView();
      }

      private void plotDataSet(object sender, DataSetSelectedEventArgs e)
      {
         var dataRepository = _dataRepositoryMapper.ConvertImportDataSet(_dataSource, e.Index, e.Key);
         _confirmationPresenter.PlotDataRepository(dataRepository);
      } 
      
      public void SetSettings(IReadOnlyList<MetaDataCategory> metaDataCategories, IReadOnlyList<ColumnInfo> columnInfos, DataImporterSettings dataImporterSettings)
      {
         _columnInfos = columnInfos;
         _columnMappingPresenter.SetSettings(metaDataCategories, columnInfos);
         _importerDataPresenter.SetSettings(metaDataCategories, columnInfos);
         _dataImporterSettings = dataImporterSettings;
      }

      public void AddConfirmationView()
      {
         _view.AddConfirmationView(_confirmationPresenter.View);
      }

      public void SetDataSource(string dataSourceFileName)
      {
         if (string.IsNullOrEmpty(dataSourceFileName)) return;

         SetSourceFile(dataSourceFileName);
         AddDataMappingView();
         _view.DisableConfirmationView();
      }
      public void ImportData(object sender, EventArgs e)
      {
         OnTriggerImport.Invoke(this, new ImportTriggeredEventArgs { DataSource = _dataSource });
      }

      private void ImportSheetsFromDataPresenter(object sender, ImportSheetsEventArgs args)
      {
         try
         {
            importSheets(args.DataSourceFile, args.Sheets);
            _importerDataPresenter.DisableImportedSheets();
         }
         catch (Exception e)
         {
            _view.ShowErrorMessage(e.Message);
            _view.DisableConfirmationView();
            foreach (var sheetName in args.Sheets.Keys)
            {
               _importerDataPresenter.Sheets.Remove(sheetName);
            }
         }
      }


      private void importSheets(IDataSourceFile dataSourceFile, Cache<string, IDataSheet> sheets)
      {
         if (!sheets.Any()) return;

         var mappings = dataSourceFile.Format.Parameters.OfType<MetaDataFormatParameter>().Select(md => new MetaDataMappingConverter()
         {
            Id = md.MetaDataId,
            Index = sheetName => dataSourceFile.DataSheets[sheetName].RawData.GetColumnDescription(md.ColumnName).Index
         }).Union
         (
            dataSourceFile.Format.Parameters.OfType<GroupByDataFormatParameter>().Select(md => new MetaDataMappingConverter()
            {
               Id = md.ColumnName,
               Index = sheetName => dataSourceFile.DataSheets[sheetName].RawData.GetColumnDescription(md.ColumnName).Index
            })
         );

         _dataSource.SetMappings(dataSourceFile.Path, mappings);
         _dataSource.NanSettings = _nanPresenter.Settings;
         _dataSource.SetDataFormat(_columnMappingPresenter.GetDataFormat());
         _dataSource.AddSheets(sheets, _columnInfos);

         var keys = new List<string>()
         {
            Constants.FILE,
            Constants.SHEET
         };

         keys.AddRange(_dataSource.GetMappings().Select(m => m.Id));
         _confirmationPresenter.SetKeys(keys);
         _confirmationPresenter.SetNamingConventions(_dataImporterSettings.NamingConventions);
         AddConfirmationView();
         View.EnableConfirmationView();
      }

      private void onFormatChanged(object sender, FormatChangedEventArgs e)
      {
         _columnMappingPresenter.SetDataFormat(e.Format);
      }

      private void onTabChanged(object sender, TabChangedEventArgs e)
      {
         _columnMappingPresenter.SetRawData(e.TabData);
      }

      private void onMissingMapping(object sender, MissingMappingEventArgs missingMappingEventArgs)
      {
         _importerDataPresenter.onMissingMapping();
         _view.DisableConfirmationView();
      }

      private void onCompletedMapping(object sender, EventArgs args)
      {
         _importerDataPresenter.onCompletedMapping();
         _dataSource.DataSets.Clear();
         try
         {
            importSheets(_dataSourceFile, _importerDataPresenter.Sheets);
         }
         catch (Exception e)
         {
            _view.ShowErrorMessage(e.Message);
            _view.DisableConfirmationView();
         }
      }
      public void AddDataMappingView()
      {
         _view.AddImporterView(_importerDataPresenter.View);
      }

      public void SetSourceFile(string path)
      {
         _sourceFilePresenter.SetFilePath(path);
         _dataSourceFile = _importerDataPresenter.SetDataSource(path);
         _columnMappingPresenter.ValidateMapping();
      }

      public event EventHandler<ImportTriggeredEventArgs> OnTriggerImport = delegate { };
   }
}