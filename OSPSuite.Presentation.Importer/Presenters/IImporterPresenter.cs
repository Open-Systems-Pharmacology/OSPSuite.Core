using OSPSuite.Presentation.Importer.Views;
using OSPSuite.Presentation.Presenters;

namespace OSPSuite.Presentation.Importer.Presenters
{
   public interface IImporterPresenter : IPresenter<IImporterView>, ICommandCollectorPresenter
   {
      IDataViewingPresenter DataViewingPresenter { get; }
      IColumnMappingPresenter ColumnMappingPresenter { get; }
   }
}
