using System.Collections.Generic;
using OSPSuite.Presentation.Views.Importer;

namespace OSPSuite.Presentation.Presenters.Importer
{
   public class ErrorEditorPresenter : AbstractDisposablePresenter<IErrorEditorView, IErrorEditorPresenter>, IErrorEditorPresenter
   {
      public ErrorEditorPresenter(IErrorEditorView view) : base(view)
      {
      }

      public string ErrorColumn { get; private set; }
      public void SetErrorColumn(string column)
      {
         ErrorColumn = column;
      }

      public bool Canceled => _view.Canceled;

      public void ShowFor(IEnumerable<string> availableColumns, string defaultValue)
      {
         _view.FillComboBox(availableColumns, defaultValue);
         _view.Display();
      }
   }
}
