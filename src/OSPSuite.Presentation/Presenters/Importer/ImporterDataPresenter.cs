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
      private IDataSourceFile _dataSourceFile;
      private IReadOnlyList<ColumnInfo> _columnInfos;
      private IReadOnlyList<MetaDataCategory> _metaDataCategories;
      private IList<Cache<string, IDataSheet>> _sheets = new List<Cache<string, IDataSheet>>();

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
         _importer = importer;
         _view.AddDataViewingControl(dataViewingPresenter.View);
         _view.AddSourceFileControl(sourceFilePresenter.View);

         _dataViewingPresenter = dataViewingPresenter;
         _sourceFilePresenter = sourceFilePresenter;

         _sourceFilePresenter.Title = Captions.Importer.PleaseSelectDataFile;
         _sourceFilePresenter.Filter = Captions.Importer.ImportFileFilter;
         _sourceFilePresenter.DirectoryKey = Constants.DirectoryKey.OBSERVED_DATA;

         AddSubPresenters(_dataViewingPresenter);

         _sourceFilePresenter.OnSourceFileChanged += (s, e) => SetDataSource(e.FileName);

         _sourceFilePresenter.CheckBeforeSelectFile = () =>
            dialogCreator.MessageBoxYesNo(Captions.Importer.OpenFileConfirmation) == ViewResult.Yes;
//ToDo: We should move the SourceFilePresenter directly under the ImporterPresenter. Grouping these 2 presenters makes 0 sense
//then we can also move the correct call for the func there
        // !_dataSource.DataSets.Any() || dialogCreator.MessageBoxYesNo(Captions.Importer.OpenFileConfirmation) == ViewResult.Yes;
      }

      public void ImportDataForConfirmation()
      {
         var sheets = getAllSheets();
         _sheets.Add(sheets);
         OnImportSheets.Invoke(this, new ImportSheetsEventArgs { DataSourceFile = _dataSourceFile, Sheets = sheets });
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
         OnImportSheets.Invoke(this, new ImportSheetsEventArgs { DataSourceFile = _dataSourceFile, Sheets = sheets});
      }

      private Cache<string, IDataSheet> getAllSheets()
      {
         return _dataSourceFile.DataSheets;
      }

      private IDataSheet getSingleSheet(string sheetName)
      {
         return _dataSourceFile.DataSheets[sheetName];
      }

      //TODO: not sure we need this
      public void SetDataFormat(IDataFormat format, IEnumerable<IDataFormat> availableFormats)
      {
         OnFormatChanged.Invoke(this, new FormatChangedEventArgs() {Format = format});
      }

      public void SetSettings(IReadOnlyList<MetaDataCategory> metaDataCategories, IReadOnlyList<ColumnInfo> columnInfos)
      {
         _columnInfos = columnInfos;
         _metaDataCategories = metaDataCategories;
      }

      public void SetDataSource(string dataSourceFileName)
      {
         if (string.IsNullOrEmpty(dataSourceFileName)) return;
         _sheets = new List<Cache<string, IDataSheet>>();
         _dataSourceFile = _importer.LoadFile(_columnInfos, dataSourceFileName, _metaDataCategories);
         _dataViewingPresenter.SetDataSource(_dataSourceFile);
         _sourceFilePresenter.SetFilePath(dataSourceFileName);
         SetDataFormat(_dataSourceFile.Format, _dataSourceFile.AvailableFormats);
         View.ClearTabs();
         View.AddTabs(_dataViewingPresenter.GetSheetNames());
         OnSourceFileChanged.Invoke(this, new SourceFileChangedEventArgs {FileName = dataSourceFileName});
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