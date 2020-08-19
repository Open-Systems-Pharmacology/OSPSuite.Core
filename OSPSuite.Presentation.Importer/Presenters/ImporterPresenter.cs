using OSPSuite.Presentation.Importer.Views;
using OSPSuite.Presentation.Presenters;

namespace OSPSuite.Presentation.Importer.Presenters
{
   //internal - because of the accessibility of AbstractCommandCollectorPresenter

   internal class ImporterPresenter : AbstractCommandCollectorPresenter<IImporterView, IImporterPresenter>, IImporterPresenter
   {
      private readonly IImporterView _importerView;
      public IDataViewingPresenter DataViewingPresenter { get; }
      public IColumnMappingPresenter ColumnMappingPresenter { get; }


      public ImporterPresenter(IImporterView view, IDataViewingPresenter dataViewingPresenter, IColumnMappingPresenter columnMappingPresenter) : base(view)
      {
         _importerView = view;
         _view.AddDataViewingControl(dataViewingPresenter.View);
         _view.AddColumnMappingControl(columnMappingPresenter.View);

         DataViewingPresenter = dataViewingPresenter;
         ColumnMappingPresenter = columnMappingPresenter;

         AddSubPresenters(DataViewingPresenter, ColumnMappingPresenter);
      }
   }
}