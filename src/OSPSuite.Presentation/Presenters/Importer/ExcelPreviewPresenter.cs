using System.Data;
using System.Drawing;
using OSPSuite.Presentation.Views.Importer;

namespace OSPSuite.Presentation.Presenters.Importer
{
   public interface IExcelPreviewPresenter : IDisposablePresenter
   {
      /// <summary>
      ///    Sets the range rectangle according to user input on the view
      /// </summary>
      /// <param name="range">The new range</param>
      void UpdateRange(Rectangle? range);

      /// <summary>
      ///    Starts the presenter and view with a preview of the datatable
      /// </summary>
      /// <param name="exportDataTable">The table to be previewed</param>
      bool SelectRange(DataTable exportDataTable);

      /// <summary>
      ///    This is the range that has been selected by the presenter
      /// </summary>
      Rectangle? Range { get; }
   }

   public class ExcelPreviewPresenter : AbstractDisposablePresenter<IExcelPreviewView, IExcelPreviewPresenter>, IExcelPreviewPresenter
   {
      public Rectangle? Range { get; private set; }

      public ExcelPreviewPresenter(IExcelPreviewView view)
         : base(view)
      {
      }

      public void UpdateRange(Rectangle? range)
      {
         Range = range;
      }

      public bool SelectRange(DataTable exportDataTable)
      {
         Range = null;
         _view.BindTo(exportDataTable);
         _view.Display();

         return !_view.Canceled;
      }
   }
}