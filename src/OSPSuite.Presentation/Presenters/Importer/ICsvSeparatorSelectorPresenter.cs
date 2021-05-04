using OSPSuite.Presentation.Views.Importer;

namespace OSPSuite.Presentation.Presenters.Importer
{
   public interface ICsvSeparatorSelectorPresenter : IDisposablePresenter
   {
      void SetFileName(string fileName);
      char GetCsvSeparator();
      bool Canceled();
      char SelectedSeparator { get; set; }
   }

   public class CsvSeparatorSelectorPresenter : AbstractDisposablePresenter<ICsvSeparatorSelectorView, ICsvSeparatorSelectorPresenter>,
      ICsvSeparatorSelectorPresenter
   {
      public char SelectedSeparator { get; set; }
      public CsvSeparatorSelectorPresenter(ICsvSeparatorSelectorView view) : base(view)
      {
      }

      public void SetFileName(string fileName)
      {
         _view.SetFileName(fileName);
         _view.Display();
      }

      public char GetCsvSeparator()
      {
         return SelectedSeparator;
      }

      public bool Canceled()
      {
         return _view.Canceled;
      }
   }
}
