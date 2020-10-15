using System;
using System.Collections.Generic;
using System.Linq;
using OSPSuite.Assets;
using OSPSuite.Core.Importer;
using OSPSuite.Presentation.Core;
using OSPSuite.Presentation.Importer.Core;
using OSPSuite.Presentation.Importer.Services;
using OSPSuite.Presentation.Importer.Views;
using OSPSuite.Presentation.Presenters;
using OSPSuite.Core.Domain;
using OSPSuite.Presentation.Importer.Core.DataFormat;
using OSPSuite.Utility.Collections;

namespace OSPSuite.Presentation.Importer.Presenters
{ 
   internal class ImporterPresenter : AbstractPresenter<IImporterView, IImporterPresenter>, IImporterPresenter
   {
      private readonly IDataViewingPresenter _dataViewingPresenter;
      private readonly IColumnMappingPresenter _columnMappingPresenter;
      private readonly ISourceFilePresenter _sourceFilePresenter;
      private readonly IImporter _importer;
      private IDataSourceFile _dataSourceFile;
      private IReadOnlyList<ColumnInfo> _columnInfos;
      private IReadOnlyList<MetaDataCategory> _metaDataCategories;
      private DataImporterSettings _dataImporterSettings;

      private IEnumerable<IDataFormat> _availableFormats;


      public event EventHandler<FormatChangedEventArgs> OnFormatChanged = delegate { };

      public event EventHandler<ImportSingleSheetEventArgs> OnImportSingleSheet = delegate { };

      public event EventHandler OnImportAllSheets = delegate { };

      public ImporterPresenter
      (
         IImporterView view, 
         IDataViewingPresenter dataViewingPresenter, 
         IColumnMappingPresenter columnMappingPresenter, 
         ISourceFilePresenter sourceFilePresenter,
         IImporter importer
      ) : base(view)
      {
         _importer = importer;
         _view.AddDataViewingControl(dataViewingPresenter.View);
         _view.AddColumnMappingControl(columnMappingPresenter.View);
         _view.AddSourceFileControl(sourceFilePresenter.View);
         _importer = importer;

         _dataViewingPresenter = dataViewingPresenter;
         _columnMappingPresenter = columnMappingPresenter;
         _sourceFilePresenter = sourceFilePresenter;

         _sourceFilePresenter.Title = Captions.Importer.PleaseSelectDataFile;
         _sourceFilePresenter.Filter = Captions.Importer.ImportFileFilter;
         _sourceFilePresenter.DirectoryKey = Constants.DirectoryKey.OBSERVED_DATA;

         AddSubPresenters(_dataViewingPresenter, _columnMappingPresenter);

         _sourceFilePresenter.OnSourceFileChanged += onSourceFileChanged;
         _columnMappingPresenter.OnMissingMapping += onMissingMapping;
         _columnMappingPresenter.OnMappingCompleted += onCompletedMapping;
      }
      public void ImportDataForConfirmation()
      {
         OnImportAllSheets.Invoke(this, EventArgs.Empty);
      }
      public void ImportDataForConfirmation(string sheetName)
      {

         OnImportSingleSheet.Invoke(this, new ImportSingleSheetEventArgs { SheetName = sheetName });
      }

      public void SetNewFormat(string formatName)
      {
         var format = _availableFormats.First(f => f.Name == formatName);
         SetDataFormat(format, _availableFormats);
         OnFormatChanged.Invoke(this, new FormatChangedEventArgs {Format = format.Name});
      }

      public void GetDataForImport(out string fileName, out IDataFormat format, out IReadOnlyList<ColumnInfo> columnInfos,
         out IEnumerable<string> namingConventions, out IEnumerable<MetaDataMappingConverter> mappings)
      {
         fileName = _dataSourceFile.Path;
         format = _columnMappingPresenter.GetDataFormat();
         columnInfos = _columnInfos;
         namingConventions = _dataImporterSettings.NamingConventions;
         mappings = _dataSourceFile.Format.Parameters.OfType<MetaDataFormatParameter>().Select(md => new MetaDataMappingConverter()
         {
            Id = md.MetaDataId,
            Index = sheetName => _dataSourceFile.DataSheets[sheetName].RawData.GetColumnDescription(md.ColumnName).Index
         }).Union
         (
            _dataSourceFile.Format.Parameters.OfType<GroupByDataFormatParameter>().Select(md => new MetaDataMappingConverter()
            {
               Id = md.ColumnName,
               Index = sheetName => _dataSourceFile.DataSheets[sheetName].RawData.GetColumnDescription(md.ColumnName).Index
            })
         );
      }

      public Cache<string, IDataSheet> GetAllSheets()
      {
         return _dataSourceFile.DataSheets;
      }

      public IDataSheet GetSingleSheet(string sheetName)
      {
         return _dataSourceFile.DataSheets[sheetName];
      }

      private void onSourceFileChanged(object sender, SourceFileChangedEventArgs e)
      {
         SetDataSource(e.FileName);
      }

      private void onMissingMapping(object sender, MissingMappingEventArgs missingMappingEventArgs)
      {
         View.DisableImportButtons();
      }
      private void onCompletedMapping(object sender, EventArgs e)
      {
         View.EnableImportButtons();
      }

      public void SetDataFormat(IDataFormat format, IEnumerable<IDataFormat> availableFormats)
      {
         var dataFormats = availableFormats.ToList();
         _availableFormats = dataFormats;
         _columnMappingPresenter.SetDataFormat(format);
         View.SetFormats(dataFormats.Select(f => f.Name), format.Name);
      }

      public void SetSettings(IReadOnlyList<MetaDataCategory> metaDataCategories, IReadOnlyList<ColumnInfo> columnInfos, DataImporterSettings dataImporterSettings)
      {
         _dataImporterSettings = dataImporterSettings;
         _columnMappingPresenter.SetSettings(metaDataCategories, columnInfos);
         _columnInfos = columnInfos;
         _metaDataCategories = metaDataCategories;
      }

      public void SetDataSource(string dataSourceFileName)
      {
         if (string.IsNullOrEmpty(dataSourceFileName)) return;
         _dataSourceFile =  _importer.LoadFile(_columnInfos, dataSourceFileName, _metaDataCategories);
         _dataViewingPresenter.SetDataSource(_dataSourceFile);
         _sourceFilePresenter.SetFilePath(dataSourceFileName);
         SetDataFormat(_dataSourceFile.Format, _dataSourceFile.AvailableFormats);
         View.ClearTabs();
         View.AddTabs(_dataViewingPresenter.GetSheetNames());
      }

      public void SelectTab(string tabName)
      {
         _columnMappingPresenter.SetRawData(_dataSourceFile.DataSheets[tabName].RawData);
         _dataViewingPresenter.SetTabData(tabName);
      }

      public void RemoveTab(string tabName)
      {
         _dataViewingPresenter.RemoveTabData(tabName);
      }

      public void RemoveAllButThisTab(string tabName)
      {
         View.ClearTabs();
         _dataViewingPresenter.RemoveAllButThisTabData(tabName);
         //those under here could go in a private called refresh (is there maybe smthing like this already existing????)
         View.AddTabs(_dataViewingPresenter.GetSheetNames());
      }

      public void RefreshTabs()
      {
         View.ClearTabs();
         View.AddTabs(_dataViewingPresenter.GetSheetNames());
      }
   }
}