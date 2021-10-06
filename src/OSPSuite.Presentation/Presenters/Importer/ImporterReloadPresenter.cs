using System.Collections.Generic;
using OSPSuite.Presentation.Views.Importer;

namespace OSPSuite.Presentation.Presenters.Importer
{
   public interface IImporterReloadPresenter : IDisposablePresenter, IPresenter<IImporterReloadView>
   {
      void Show();
      bool Canceled();
      void AddDeletedDataSets(IEnumerable<string> allNames);
      void AddNewDataSets(IEnumerable<string> except);
      void AddOverwrittenDataSets(IEnumerable<string> intersect);
   }

   public class ImporterReloadPresenter : AbstractDisposablePresenter<IImporterReloadView, IImporterReloadPresenter>, IImporterReloadPresenter
   {
      public ImporterReloadPresenter(
         IImporterReloadView view) : base(view)
      {
      }

      public void Show()
      {
         _view.Display();
      }

      public bool Canceled()
      {
         return _view.Canceled;
      }

      public void AddDeletedDataSets(IEnumerable<string> names)
      {
         _view.AddDeletedDataSets(names);
      }

      public void AddNewDataSets(IEnumerable<string> names)
      {
         _view.AddNewDataSets(names);
      }

      public void AddOverwrittenDataSets(IEnumerable<string> names)
      {
         _view.AddOverwrittenDataSets(names);
      }
   }
}