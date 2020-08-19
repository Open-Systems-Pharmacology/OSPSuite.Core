using OSPSuite.Presentation.Importer.Views;
using OSPSuite.Presentation.Presenters;

namespace OSPSuite.Presentation.Importer.Presenters
{
   interface IImporterPresenter : IPresenter<IImporterPresenterView>, ICommandCollectorPresenter
   {
      IDataViewingPresenter DataViewingPresenter { get; }
      IColumnMappingPresenter ColumnMappingPresenter { get; }

   }
}
