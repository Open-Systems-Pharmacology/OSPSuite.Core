using OSPSuite.Presentation.Presenters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OSPSuite.Presentation.Importer.Presenters
{
   public interface ILloqEditorPresenter : IDisposablePresenter
   {
      bool Canceled { get; }

      string LloqColumn { get; }

      void SetLloqColumn(string column);

      void ShowFor(IEnumerable<string> availableColumns, string defaultValue);
   }
}
