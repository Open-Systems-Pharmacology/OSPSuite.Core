using System.Data;
using OSPSuite.Presentation.Importer.Presenter;
using OSPSuite.Presentation.Views;

namespace OSPSuite.Presentation.Importer.View
{
   public interface IExcelPreviewView : IModalView<IExcelPreviewPresenter>
   {
      /// <summary>
      /// Sets the datatable that should be viewed
      /// </summary>
      void BindTo(DataTable exportDataTable);
   }
}