using System.Collections.Generic;
using OSPSuite.Presentation.Presenters.Importer;

namespace OSPSuite.Presentation.Views.Importer
{
   public interface IImportConfirmationView : IView<IImportConfirmationPresenter> //IModalView<IImportConfirmationPresenter>
   {
      void SetDataSetNames(IEnumerable<string> names);
      void SetNamingConventions(IEnumerable<string> options, string selected = null);
      void SetNamingConventionKeys(IEnumerable<string> keys);
   }
}
