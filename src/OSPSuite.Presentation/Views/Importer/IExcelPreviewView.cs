using System.Data;
using OSPSuite.Presentation.Presenters.Importer;

namespace OSPSuite.Presentation.Views.Importer
{
   public interface IExcelPreviewView : IModalView<IExcelPreviewPresenter>
   {
      /// <summary>
      /// Sets the datatable that should be viewed
      /// </summary>
      void BindTo(DataTable exportDataTable);
   }
}