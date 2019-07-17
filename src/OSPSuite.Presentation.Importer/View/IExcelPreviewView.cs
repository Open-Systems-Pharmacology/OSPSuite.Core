using System.Data;
using OSPSuite.Presentation.Presenter;
using OSPSuite.Presentation.Views;

namespace OSPSuite.Presentation.View
{
   public interface IExcelPreviewView : IModalView<IExcelPreviewPresenter>
   {
      /// <summary>
      /// Sets the datatable that should be viewed
      /// </summary>
      void BindTo(DataTable exportDataTable);
   }
}