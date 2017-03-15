using System.Data;
using OSPSuite.Presentation.Presenters;

namespace OSPSuite.Presentation.Views.ObservedData
{
   public interface IBaseDataRepositoryDataView<TPresenter> : IView<TPresenter> where TPresenter : IPresenter
   {
      void BindTo(DataTable dataTable);
   }
}