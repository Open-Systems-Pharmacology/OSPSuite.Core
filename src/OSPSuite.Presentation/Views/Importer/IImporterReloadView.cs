using System.Collections.Generic;
using OSPSuite.Presentation.Presenters.Importer;

namespace OSPSuite.Presentation.Views.Importer
{
   public interface IImporterReloadView : IModalView<IImporterReloadPresenter>
   {
      void AddDeletedDataSets(IEnumerable<string> allNames);
      void AddNewDataSets(IEnumerable<string> names);
      void AddOverwrittenDataSets(IEnumerable<string> names);
   }
}
