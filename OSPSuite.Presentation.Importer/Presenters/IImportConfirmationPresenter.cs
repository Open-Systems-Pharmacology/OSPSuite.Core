using OSPSuite.Presentation.Importer.Core;
using OSPSuite.Presentation.Presenters;
using System.Collections.Generic;

namespace OSPSuite.Presentation.Importer.Presenters
{
   public interface IImportConfirmationPresenter : IDisposablePresenter
   {
      void Show(IDataSource dataSource, IEnumerable<string> names);
      bool Canceled { get; }
   }
}
