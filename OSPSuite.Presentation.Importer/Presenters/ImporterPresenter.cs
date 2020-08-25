using System.Collections.Generic;
using System.Linq;
using OSPSuite.Core.Commands.Core;
using OSPSuite.Core.Importer;
using OSPSuite.Presentation.Importer.Core;
using OSPSuite.Presentation.Importer.Views;
using OSPSuite.Presentation.Presenters;

namespace OSPSuite.Presentation.Importer.Presenters
{
   //internal - because of the accessibility of AbstractCommandCollectorPresenter

   //THIS DOES NOT NEED TO BE A COMMANDCOLLECTOR PRESENTER -- to be honest not sure even if the abastractPresenter here
   //is an overkill
   internal class ImporterPresenter : AbstractPresenter<IImporterView, IImporterPresenter>, IImporterPresenter
   {
      private readonly IImporterView _importerView;
      private IDataViewingPresenter _dataViewingPresenter;
      private IColumnMappingPresenter _columnMappingPresenter;
      private ISourceFilePresenter _sourceFilePresenter;

      private IDataFormat _format;
      private IEnumerable<IDataFormat> _availableFormats;


      public ImporterPresenter(IImporterView view, IDataViewingPresenter dataViewingPresenter, IColumnMappingPresenter columnMappingPresenter, ISourceFilePresenter sourceFilePresenter) : base(view)
      {
         _importerView = view;
         _view.AddDataViewingControl(dataViewingPresenter.View);
         _view.AddColumnMappingControl(columnMappingPresenter.View);
         _view.AddSourceFileControl(sourceFilePresenter.View);

         _dataViewingPresenter = dataViewingPresenter;
         _columnMappingPresenter = columnMappingPresenter;
         _sourceFilePresenter = sourceFilePresenter;

         AddSubPresenters(_dataViewingPresenter, _columnMappingPresenter);
      }

      public void InitializeWith(ICommandCollector initializer)
      {
         throw new System.NotImplementedException();
      }

      public void SetDataFormat(IDataFormat format, IEnumerable<IDataFormat> availableFormats)
      {
         _format = format;
         _availableFormats = availableFormats;
         _columnMappingPresenter.SetDataFormat(_format, _availableFormats);
         View.SetFormats(availableFormats.Select(f => f.Name), format.Name);
      }

      public void SetSettings(IReadOnlyList<MetaDataCategory> metaDataCategories, IReadOnlyList<ColumnInfo> columnInfos, DataImporterSettings dataImporterSettings)
      {
         _columnMappingPresenter.SetSettings(metaDataCategories, columnInfos, dataImporterSettings);
      }

      public void SetDataSource(string path)
      {
         _dataViewingPresenter.SetDataSource(path);
         View.AddTabs(_dataViewingPresenter.GetSheetNames());
      }

      public void SelectTab(string tabName)
      {
         _dataViewingPresenter.GetSheet(tabName);
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