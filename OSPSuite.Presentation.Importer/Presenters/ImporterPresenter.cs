using System;
using System.Collections.Generic;
using System.Linq;
using OSPSuite.Core.Commands.Core;
using OSPSuite.Core.Importer;
using OSPSuite.Presentation.Importer.Core;
using OSPSuite.Presentation.Importer.Views;
using OSPSuite.Presentation.Presenters;
using OSPSuite.Utility.Extensions;
using OSPSuite.Presentation.Importer.Services;

namespace OSPSuite.Presentation.Importer.Presenters
{
   //internal - because of the accessibility of AbstractCommandCollectorPresenter

   //THIS DOES NOT NEED TO BE A COMMANDCOLLECTOR PRESENTER -- to be honest not sure even if the abastractPresenter here
   //is an overkill
   internal class ImporterPresenter : AbstractPresenter<IImporterView, IImporterPresenter>, IImporterPresenter
   {
      private readonly IDataViewingPresenter _dataViewingPresenter;
      private readonly IColumnMappingPresenter _columnMappingPresenter;
      private readonly ISourceFilePresenter _sourceFilePresenter;
      private readonly IImporter _importer;
      private IReadOnlyList<ColumnInfo> _columnInfos;



      private IEnumerable<IDataFormat> _availableFormats;

      public event FormatChangedHandler OnFormatChanged = delegate { };


      public ImporterPresenter(IImporter importer, IImporterView view, IDataViewingPresenter dataViewingPresenter, IColumnMappingPresenter columnMappingPresenter, ISourceFilePresenter sourceFilePresenter) : base(view)
      {
         _view.AddDataViewingControl(dataViewingPresenter.View);
         _view.AddColumnMappingControl(columnMappingPresenter.View);
         _view.AddSourceFileControl(sourceFilePresenter.View);
         _importer = importer;

         _dataViewingPresenter = dataViewingPresenter;
         _columnMappingPresenter = columnMappingPresenter;
         _sourceFilePresenter = sourceFilePresenter;

         AddSubPresenters(_dataViewingPresenter, _columnMappingPresenter);

         _sourceFilePresenter.OnSourceFileChanged += onSourceFileChanged;
         _view.OnTabChanged +=  SelectTab;
         _view.OnFormatChanged += (formatName) => this.DoWithinExceptionHandler(() =>
         {
            var format = _availableFormats.First(f => f.Name == formatName); //TODO hmmm...wrong. this probably takes just the first one
            SetDataFormat(format, _availableFormats);
            OnFormatChanged(format);
         });

      }

      private void onSourceFileChanged(object sender, SourceFileChangedEventArgs e)
      {
         SetDataSource(e.FileName);
      }

      public void InitializeWith(ICommandCollector initializer)
      {
         throw new NotImplementedException();
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
         _columnMappingPresenter.SetSettings(metaDataCategories, columnInfos, dataImporterSettings);
         _columnInfos = columnInfos;
      }

      public void SetDataSource(string dataSourceFileName)
      {
         if (dataSourceFileName == "") return;
         var sourceFile =  _importer.LoadFile(_columnInfos, dataSourceFileName);
         _dataViewingPresenter.SetDataSource(sourceFile);
         _sourceFilePresenter.SetFilePath(dataSourceFileName);
         _columnMappingPresenter.SetDataFormat(sourceFile.Format, sourceFile.AvailableFormats);
         View.ClearTabs();
         View.AddTabs(_dataViewingPresenter.GetSheetNames());
      }

      public void SelectTab(string tabName)
      {
         _dataViewingPresenter.SetTabData(tabName);
      }

      public void AddCommand(ICommand command)
      {
         throw new System.NotImplementedException();
      }

      public IEnumerable<ICommand> All()
      {
         throw new System.NotImplementedException();
      }

      public ICommandCollector CommandCollector { get; }
   }
}