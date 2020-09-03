using System.Collections.Generic;
using System.Linq;
using OSPSuite.Assets;
using OSPSuite.Core.Importer;
using OSPSuite.Presentation.Core;
using OSPSuite.Presentation.Importer.Core;
using OSPSuite.Presentation.Importer.Services;
using OSPSuite.Presentation.Importer.Views;
using OSPSuite.Presentation.Presenters;
using OSPSuite.Utility.Extensions;
using OSPSuite.Core.Domain;


namespace OSPSuite.Presentation.Importer.Presenters
{ 
   internal class ImporterPresenter : AbstractPresenter<IImporterView, IImporterPresenter>, IImporterPresenter
   {
      private readonly IDataViewingPresenter _dataViewingPresenter;
      private readonly IColumnMappingPresenter _columnMappingPresenter;
      private readonly ISourceFilePresenter _sourceFilePresenter;
      private readonly IImporter _importer;
      private readonly IApplicationController _applicationController;
      private IDataSourceFile _dataSourceFile;
      private IReadOnlyList<ColumnInfo> _columnInfos;

      private string _namingConvention;

      private IEnumerable<IDataFormat> _availableFormats;

      public event FormatChangedHandler OnFormatChanged = delegate { };
      public event OnTriggerImportHandler OnTriggerImport = delegate { };

      public ImporterPresenter
      (
         IImporterView view, 
         IDataViewingPresenter dataViewingPresenter, 
         IColumnMappingPresenter columnMappingPresenter, 
         ISourceFilePresenter sourceFilePresenter,
         IApplicationController applicationController,
         IImporter importer
      ) : base(view)
      {
         _importer = importer;
         _applicationController = applicationController;
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
         _view.OnTabChanged +=  SelectTab;
         _view.OnImportAllSheets += ShowImportConfirmation;
         _view.OnImportSingleSheet += ShowImportConfirmation;
         _view.OnFormatChanged += (formatName) => this.DoWithinExceptionHandler(() =>
         {
            var format = _availableFormats.First(f => f.Name == formatName); //TODO hmmm...this throws an error if I try to edit the format name
            SetDataFormat(format, _availableFormats); //dropbox should not be editable
            OnFormatChanged(format);
         });
         _view.OnNamingConventionChanged += (namingConvention) => this.DoWithinExceptionHandler(() =>
         {
            _namingConvention = namingConvention;
         });
      }
      public void ShowImportConfirmation()
      {
         startImport(_dataSourceFile.DataSheets.Values, _dataSourceFile.DataSheets.Keys);
      }
      public void ShowImportConfirmation( string sheetName )
      {
         IEnumerable<IDataSheet> sheets = new[] {_dataSourceFile.DataSheets[sheetName]};
         IEnumerable<string> sheetNames = new[] {sheetName};

         startImport(sheets, sheetNames);
      }

      private void startImport( IEnumerable<IDataSheet> sheets, IEnumerable<string> sheetNames)
      {
         var dataSource = _importer.ImportFromFile(_dataSourceFile.Format, sheets, _columnInfos);

         using (var importConfirmationPresenter = _applicationController.Start<IImportConfirmationPresenter>())
         {
            importConfirmationPresenter.Show(dataSource, sheetNames);

            if (!importConfirmationPresenter.Canceled)
               OnTriggerImport.Invoke(dataSource);
         }
      }

      private void onSourceFileChanged(object sender, SourceFileChangedEventArgs e)
      {
         SetDataSource(e.FileName);
      }

      public void SetDataFormat(IDataFormat format, IEnumerable<IDataFormat> availableFormats)
      {
         var dataFormats = availableFormats.ToList();
         _availableFormats = dataFormats;
         _columnMappingPresenter.SetDataFormat (format, _availableFormats);
         View.SetFormats(dataFormats.Select(f => f.Name), format.Name);
      }

      public void SetSettings(IReadOnlyList<MetaDataCategory> metaDataCategories, IReadOnlyList<ColumnInfo> columnInfos, DataImporterSettings dataImporterSettings)
      {
         View.SetNamingConventions(dataImporterSettings.NamingConventions);
         _columnMappingPresenter.SetSettings(metaDataCategories, columnInfos);
         _columnInfos = columnInfos;
      }

      public void SetDataSource(string dataSourceFileName)
      {
         if (string.IsNullOrEmpty(dataSourceFileName)) return;
         _dataSourceFile =  _importer.LoadFile(_columnInfos, dataSourceFileName);
         _dataViewingPresenter.SetDataSource(_dataSourceFile);
         _sourceFilePresenter.SetFilePath(dataSourceFileName);
         _columnMappingPresenter.SetDataFormat(_dataSourceFile.Format, _dataSourceFile.AvailableFormats);
         SetDataFormat(_dataSourceFile.Format, _dataSourceFile.AvailableFormats);
         View.ClearTabs();
         View.AddTabs(_dataViewingPresenter.GetSheetNames());
      }

      public void SelectTab(string tabName)
      {
         _dataViewingPresenter.SetTabData(tabName);
      }
   }
}