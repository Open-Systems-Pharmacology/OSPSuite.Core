using OSPSuite.Presentation.Importer.Presenters;
using OSPSuite.Presentation.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OSPSuite.Presentation.Importer.Views
{
   public interface IImportConfirmationView : IView<IImportConfirmationPresenter> //IModalView<IImportConfirmationPresenter>
   {
      event NamingConventionChangedHandler OnNamingConventionChanged;

      event SelectedDataSetChangedHandler OnSelectedDataSetChanged;

      void SetDataSetNames(IEnumerable<string> names);

      void SetNamingConventions(IEnumerable<string> options, string selected = null);
   }

   public delegate void NamingConventionChangedHandler(string namingConvention);

   public delegate void SelectedDataSetChangedHandler(int index);
}
