using System.Collections.Generic;

namespace OSPSuite.Presentation.Presenters.Importer
{
   public interface IErrorEditorPresenter : IDisposablePresenter
   {
      bool Canceled { get; }

      string ErrorColumn { get; }

      void SetErrorColumn(string column);

      void ShowFor(IEnumerable<string> availableColumns, string defaultValue);
   }
}
