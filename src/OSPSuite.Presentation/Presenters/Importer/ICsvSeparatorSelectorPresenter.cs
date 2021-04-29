using OSPSuite.Presentation.Views.Importer;

namespace OSPSuite.Presentation.Presenters.Importer
{
   public interface ICsvSeparatorSelectorPresenter : IDisposablePresenter
   {
      void SetFileName(string fileName);
   }

   public class CsvSeparatorSelectorPresenter : AbstractDisposablePresenter<ICsvSeparatorSelectorView, ICsvSeparatorSelectorPresenter>,
      ICsvSeparatorSelectorPresenter
   {
      public CsvSeparatorSelectorPresenter(ICsvSeparatorSelectorView view) : base(view)
      {
      }

      public void SetFileName(string fileName)
      {
         _view.SetFileName(fileName);
      }
   }
}
