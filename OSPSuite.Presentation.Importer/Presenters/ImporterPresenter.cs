using OSPSuite.Presentation.Importer.Views;
using OSPSuite.Presentation.Presenters;

namespace OSPSuite.Presentation.Importer.Presenters
{
   //internal - because of the accessibility of AbstractCommandCollectorPresenter
   internal class ImporterPresenter : AbstractCommandCollectorPresenter<IImporterPresenterView, IImporterPresenter>, IImporterPresenter
   {
      private readonly IImporterPresenterView _importerPresenterView;
      public IDataViewingPresenter DataViewingPresenter { get; }
      public IColumnMappingPresenter ColumnMappingPresenter { get; }


      public ImporterPresenter(IImporterPresenterView view, IDataViewingPresenter dataViewingPresenter, IColumnMappingPresenter columnMappingPresenter ) : base(view)
      {
         _importerPresenterView = view;
         _view.AddDataViewingControl(dataViewingPresenter.View);
         _view.AddColumnMappingControl(columnMappingPresenter.View);

         DataViewingPresenter = dataViewingPresenter;
         ColumnMappingPresenter = columnMappingPresenter;

         AddSubPresenters(DataViewingPresenter, ColumnMappingPresenter);
      }
   }
}