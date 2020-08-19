using OSPSuite.Presentation.Importer.Presenters;
using OSPSuite.Presentation.Views;

namespace OSPSuite.Presentation.Importer.Views
{
   interface IImporterPresenterView : IView<IImporterPresenter>
   {
      void AddDataViewingControl(IDataViewingControl dataViewingControl);
      void AddColumnMappingControl(IColumnMappingControl columnMappingControl);
   }

  
}
