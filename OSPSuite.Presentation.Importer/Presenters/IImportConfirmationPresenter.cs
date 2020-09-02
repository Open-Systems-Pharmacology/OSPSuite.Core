using OSPSuite.Presentation.Importer.Core;
using OSPSuite.Presentation.Presenters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OSPSuite.Presentation.Importer.Presenters
{
   public interface IImportConfirmationPresenter : IDisposablePresenter
   {
      void Show(IDataSource dataSource, IEnumerable<string> names);
   }
}
