using OSPSuite.Presentation.Importer;
using OSPSuite.Presentation.Views;

namespace OSPSuite.UI.Views.Importer
{
   public interface IDataViewingControl : IView<IDataViewingPresenter>
   {
      void SetGridSource(string tabName = null);
   }
}
