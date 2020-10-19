using System.Collections.Generic;
using OSPSuite.Presentation.Views.Importer;

namespace OSPSuite.Presentation.Presenters.Importer
{
   public class LloqEditorPresenter : AbstractDisposablePresenter<ILloqEditorView, ILloqEditorPresenter>, ILloqEditorPresenter
   {
      public LloqEditorPresenter(ILloqEditorView view) : base(view)
      {
      }

      public string LloqColumn { get; private set; }

      public bool Canceled => _view.Canceled;

      public void ShowFor(IEnumerable<string> availableColumns, string defaultValue)
      {
         _view.FillComboBox(availableColumns, defaultValue);
         _view.Display();
      }

      public void SetLloqColumn(string column)
      {
         LloqColumn = column;
      }
   }
}
