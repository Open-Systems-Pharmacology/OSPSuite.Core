using System.Collections.Generic;

namespace OSPSuite.Presentation.Presenters.Importer
{
   public interface ILloqEditorPresenter : IDisposablePresenter
   {
      bool Canceled { get; }

      string LloqColumn { get; }

      void SetLloqColumn(string column);

      void ShowFor(IEnumerable<string> availableColumns, string defaultValue);
   }
}
