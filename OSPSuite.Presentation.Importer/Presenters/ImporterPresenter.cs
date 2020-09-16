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
using OSPSuite.Presentation.Importer.Core.DataFormat;

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
      private DataImporterSettings _dataImporterSettings;

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
            var format = _availableFormats.First(f => f.Name == formatName);
            //SetDataFormat(format, _availableFormats);
            OnFormatChanged(format.Name);
         });
      }
      public void ShowImportConfirmation()
      {
         startImport(_dataSourceFile.DataSheets);
      }
      public void ShowImportConfirmation(string sheetName)
      {
         startImport(new Dictionary<string, IDataSheet>() { { sheetName, _dataSourceFile.DataSheets[sheetName] } });
      }

      private void startImport(IReadOnlyDictionary<string, IDataSheet> sheets)
      {
         _dataSourceFile.Format = _columnMappingPresenter.GetDataFormat();
         var dataSource = _importer.ImportFromFile(_dataSourceFile.Format, sheets, _columnInfos);

         using (var importConfirmationPresenter = _applicationController.Start<IImportConfirmationPresenter>())
         {
            importConfirmationPresenter.Show
            (
               _dataSourceFile.Path.Split('\\').Last(),
               dataSource,
               _dataImporterSettings.NamingConventions,
               _dataSourceFile.Format.Parameters.OfType<MetaDataFormatParameter>().Select(md => new MetaDataMappingConverter() 
               {
                  Id = md.MetaDataId,
                  Index = sheetName => sheets[sheetName].RawData.GetColumnDescription(md.ColumnName).Index
               }).Union
               (
                  _dataSourceFile.Format.Parameters.OfType<GroupByDataFormatParameter>().Select(md => new MetaDataMappingConverter()
                  {
                     Id = md.ColumnName,
                     Index = sheetName => sheets[sheetName].RawData.GetColumnDescription(md.ColumnName).Index
                  })
               )
            );

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
         _columnMappingPresenter.SetDataFormat(format);
         View.SetFormats(dataFormats.Select(f => f.Name), format.Name);
      }

      public void SetSettings(IReadOnlyList<MetaDataCategory> metaDataCategories, IReadOnlyList<ColumnInfo> columnInfos, DataImporterSettings dataImporterSettings)
      {
         _dataImporterSettings = dataImporterSettings;
         _columnMappingPresenter.SetSettings(metaDataCategories, columnInfos);
         _columnInfos = columnInfos;
      }

      public void SetDataSource(string dataSourceFileName)
      {
         if (string.IsNullOrEmpty(dataSourceFileName)) return;
         _dataSourceFile =  _importer.LoadFile(_columnInfos, dataSourceFileName);
         _dataViewingPresenter.SetDataSource(_dataSourceFile);
         _sourceFilePresenter.SetFilePath(dataSourceFileName);
         _columnMappingPresenter.SetDataFormat(_dataSourceFile.Format);
         SetDataFormat(_dataSourceFile.Format, _dataSourceFile.AvailableFormats);
         View.ClearTabs();
         View.AddTabs(_dataViewingPresenter.GetSheetNames());
      }

      public void SelectTab(string tabName)
      {
         _dataViewingPresenter.SetTabData(tabName);
      }

      public void RemoveTab(string tabName) //IMPORTANT it seems that in MS Excel you cannot have 2 sheets
      //with the same name. Still we should be checking this...
      {
         _dataViewingPresenter.RemoveTabData(tabName);
         View.ClearTabs();
         View.AddTabs(_dataViewingPresenter.GetSheetNames());
      }

      public void RemoveAllButThisTab(string tabName)
      {
         View.ClearTabs();
         _dataViewingPresenter.RemoveAllButThisTabData(tabName);
         //those under here could go in a private called refresh (is there maybe smthing like this already existing????)
         View.AddTabs(_dataViewingPresenter.GetSheetNames());
      }
   }
}