using System;
using System.Collections.Generic;
using System.Linq;
using OSPSuite.Assets;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Services;
using OSPSuite.Infrastructure.Import.Core;
using OSPSuite.Infrastructure.Import.Core.DataFormat;
using OSPSuite.Infrastructure.Import.Services;
using OSPSuite.Presentation.Core;
using OSPSuite.Presentation.Views.Importer;
using OSPSuite.Utility.Collections;

namespace OSPSuite.Presentation.Presenters.Importer
{ 
   class ImporterDataPresenter : AbstractPresenter<IImporterDataView, IImporterDataPresenter>, IImporterDataPresenter
   {
      private readonly IDataViewingPresenter _dataViewingPresenter;
      private readonly ISourceFilePresenter _sourceFilePresenter;
      private readonly IImporter _importer;
      private IDataFormat _format;
      private DataSource _dataSource;
      private IDataSourceFile _dataSourceFile;
      private IReadOnlyList<ColumnInfo> _columnInfos;
      private IReadOnlyList<MetaDataCategory> _metaDataCategories;
      private DataImporterSettings _dataImporterSettings;
      private IEnumerable<IDataFormat> _availableFormats;
      private IList<Cache<string, IDataSheet>> _sheets = new List<Cache<string, IDataSheet>>();
      private readonly IDialogCreator _dialogCreator;


      public event EventHandler<FormatChangedEventArgs> OnFormatChanged = delegate { };
      public event EventHandler<TabChangedEventArgs> OnTabChanged;

      public event EventHandler<ImportSheetsEventArgs> OnImportSheets = delegate { };

      public event EventHandler<SourceFileChangedEventArgs> OnSourceFileChanged = delegate { };

      public ImporterDataPresenter
      (
         IImporterDataView dataView, 
         IDataViewingPresenter dataViewingPresenter, 
         ISourceFilePresenter sourceFilePresenter,
         IImporter importer,
         IDialogCreator dialogCreator
      ) : base(dataView)
      {
         _dialogCreator = dialogCreator;
         _importer = importer;
         _view.AddDataViewingControl(dataViewingPresenter.View);
         _view.AddSourceFileControl(sourceFilePresenter.View);
         _importer = importer;

         _dataViewingPresenter = dataViewingPresenter;
         _sourceFilePresenter = sourceFilePresenter;

         _sourceFilePresenter.Title = Captions.Importer.PleaseSelectDataFile;
         _sourceFilePresenter.Filter = Captions.Importer.ImportFileFilter;
         _sourceFilePresenter.DirectoryKey = Constants.DirectoryKey.OBSERVED_DATA;

         AddSubPresenters(_dataViewingPresenter);

         _sourceFilePresenter.OnSourceFileChanged += onSourceFileChanged;

         _dataSource = new DataSource(_importer);

         _sourceFilePresenter.CheckBeforeSelectFile = () =>
            !_dataSource.DataSets.Any() || _dialogCreator.MessageBoxYesNo(Captions.Importer.OpenFileConfirmation) == ViewResult.Yes;
      }
      public void ImportDataForConfirmation()
      {
         var sheets = getAllSheets();
         _sheets.Add(sheets);
         OnImportSheets.Invoke(this, new ImportSheetsEventArgs { DataSource = getDataSource(sheets) });
      }

      public void onMissingMapping()
      {
         View.DisableImportButtons();
      }

      public void onCompletedMapping()
      {
         View.EnableImportButtons();
         /* this could be here just for the refreshing of the confirmation view
         _dataSource.DataSets.Clear();
         //ToDo: Hmmm...why are we even doing this???
         foreach (var sheet in _sheets)
         {
            getDataSource(sheet);
         }*/
      }

      public void ImportDataForConfirmation(string sheetName)
      {
         var sheets = new Cache<string, IDataSheet>();
         sheets.Add(sheetName, getSingleSheet(sheetName));
         _sheets.Add(sheets);
         OnImportSheets.Invoke(this, new ImportSheetsEventArgs { DataSource = getDataSource(sheets) });
      }

      public IEnumerable<string> GetNamingConventions()
      {
         return _dataImporterSettings.NamingConventions;
      }

      private IDataSource getDataSource(Cache<string, IDataSheet> sheets)
      {
         var mappings = _dataSourceFile.Format.Parameters.OfType<MetaDataFormatParameter>().Select(md => new MetaDataMappingConverter()
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

         _dataSource.SetMappings(_dataSourceFile.Path, mappings);
         //Todo: this here as a test!!!
         _dataSource.SetDataFormat(_format);  //_columnMappingPresenter.GetDataFormat()  
         _dataSource.AddSheets( sheets, _columnInfos);

         return _dataSource;
      }

      private Cache<string, IDataSheet> getAllSheets()
      {
         return _dataSourceFile.DataSheets;
      }

      private IDataSheet getSingleSheet(string sheetName)
      {
         return _dataSourceFile.DataSheets[sheetName];
      }

      private void onSourceFileChanged(object sender, SourceFileChangedEventArgs e)
      {
         SetDataSource(e.FileName);
         OnSourceFileChanged.Invoke(sender, e);
      }

      //TODO: not sure we need this
      public void SetDataFormat(IDataFormat format, IEnumerable<IDataFormat> availableFormats)
      {
         var dataFormats = availableFormats.ToList();
         _availableFormats = dataFormats;
         _format = format; //ToDo: BUT WE SHOULD ACTUALLY KEEP IT ONLY IN COLUMNMAPPINGPRESENTER OR HERE
         OnFormatChanged.Invoke(this, new FormatChangedEventArgs() {Format = format});
      }

      public void SetSettings(IReadOnlyList<MetaDataCategory> metaDataCategories, IReadOnlyList<ColumnInfo> columnInfos, DataImporterSettings dataImporterSettings)
      {
         _dataImporterSettings = dataImporterSettings;
         _columnInfos = columnInfos;
         _metaDataCategories = metaDataCategories;
      }

      public void SetDataSource(string dataSourceFileName)
      {
         if (string.IsNullOrEmpty(dataSourceFileName)) return;
         _dataSource.DataSets.Clear();
         _sheets = new List<Cache<string, IDataSheet>>();
         _dataSourceFile = _importer.LoadFile(_columnInfos, dataSourceFileName, _metaDataCategories);
         _dataViewingPresenter.SetDataSource(_dataSourceFile);
         _sourceFilePresenter.SetFilePath(dataSourceFileName);
         SetDataFormat(_dataSourceFile.Format, _dataSourceFile.AvailableFormats);
         View.ClearTabs();
         View.AddTabs(_dataViewingPresenter.GetSheetNames());
      }

      public void SelectTab(string tabName)
      {
         OnTabChanged.Invoke(this, new TabChangedEventArgs() { TabData = _dataSourceFile.DataSheets[tabName].RawData });
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