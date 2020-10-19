using OSPSuite.Presentation.Presenters.Importer;

namespace OSPSuite.Presentation.Views.Importer
{
   public interface IDataViewingControl : IView<IDataViewingPresenter>
   {
      void SetGridSource(string tabName = null);
   }
}
