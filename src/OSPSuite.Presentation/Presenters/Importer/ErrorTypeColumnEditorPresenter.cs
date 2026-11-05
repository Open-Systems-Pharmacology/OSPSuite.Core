using System;
using System.Collections.Generic;
using OSPSuite.Presentation.Views.Importer;

namespace OSPSuite.Presentation.Presenters.Importer
{
   public interface IErrorTypeColumnEditorPresenter : IDisposablePresenter
   {
      string ErrorTypeColumn { get; }
      void SetOptions(IEnumerable<string> columns, bool errorTypeFromColumn, string selected = null);
      bool ErrorTypeFromColumn();
      void ErrorTypeFromColumnChanged();
      event EventHandler OnErrorTypeFromColumnChanged;
   }

   public class ErrorTypeColumnEditorPresenter : AbstractDisposablePresenter<IErrorTypeColumnEditorView, IErrorTypeColumnEditorPresenter>, IErrorTypeColumnEditorPresenter
   {
      private readonly IOptionsEditorPresenter _optionsEditorPresenter;

      public event EventHandler OnErrorTypeFromColumnChanged = delegate { };

      public ErrorTypeColumnEditorPresenter(IErrorTypeColumnEditorView view, IOptionsEditorPresenter optionsEditorPresenter) : base(view)
      {
         _optionsEditorPresenter = optionsEditorPresenter;
         View.FillErrorTypeColumnSelector(_optionsEditorPresenter.BaseView);
      }

      public string ErrorTypeColumn => _optionsEditorPresenter.SelectedText;

      public void SetOptions(IEnumerable<string> columns, bool errorTypeFromColumn, string selected = null)
      {
         View.SetErrorTypeColumnToggle(errorTypeFromColumn);
         _optionsEditorPresenter.SetOptions(new Dictionary<string, IEnumerable<string>> { { "", columns } }, selected);
      }

      public bool ErrorTypeFromColumn()
      {
         return View.IsErrorTypeColumnToggleOn();
      }

      public void ErrorTypeFromColumnChanged()
      {
         OnErrorTypeFromColumnChanged(this, EventArgs.Empty);
      }
   }
}
