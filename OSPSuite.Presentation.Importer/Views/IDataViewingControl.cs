using OSPSuite.Presentation.Importer.Presenters;
using OSPSuite.Presentation.Views;

namespace OSPSuite.Presentation.Importer.Views
{
   public interface IDataViewingControl : IView<IDataViewingPresenter>
   {
      void SetGridSource(string tabName = null);
   }
}
