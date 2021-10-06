using System.Collections.Generic;

namespace OSPSuite.Presentation.Presenters.Importer
{
   public interface ILloqEditorPresenter : IDisposablePresenter
   {
      string LloqColumn { get; }
      void SetOptions(IReadOnlyDictionary<string, IEnumerable<string>> options, bool lloqColumnsSelection, string selected = null );
      bool LloqFromColumn();
   }
}
