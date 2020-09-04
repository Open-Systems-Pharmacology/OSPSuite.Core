using OSPSuite.Presentation.Importer.Core;
using OSPSuite.Presentation.Importer.Views;
using OSPSuite.Presentation.Presenters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OSPSuite.Presentation.Importer.Presenters
{
   public class ImportConfirmationPresenter : AbstractDisposablePresenter<IImportConfirmationView, IImportConfirmationPresenter>, IImportConfirmationPresenter
   {
      public ImportConfirmationPresenter(IImportConfirmationView view) : base(view)
      {
      }

      public void Show(IDataSource dataSource, IEnumerable<string> names)
      {
         _view.SetDataSetNames(names);
         _view.Display();
      }

      public bool Canceled => _view.Canceled;
   }
}
