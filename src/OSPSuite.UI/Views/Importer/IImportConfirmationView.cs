using OSPSuite.Presentation.Importer;
using OSPSuite.Presentation.Views;
using System.Collections.Generic;

namespace OSPSuite.UI.Views.Importer
{
   public interface IImportConfirmationView : IView<IImportConfirmationPresenter> //IModalView<IImportConfirmationPresenter>
   {
      void SetDataSetNames(IEnumerable<string> names);
      void SetNamingConventions(IEnumerable<string> options, string selected = null);
      void SetNamingConventionKeys(IEnumerable<string> keys);
   }
}
