using NPOI.SS.Formula.Atp;
using NPOI.SS.Formula.Functions;
using OSPSuite.Presentation.Views;
using System;
using System.Collections.Generic;
using System.Text;

namespace OSPSuite.Presentation.Presenters.Importer
{
   public interface IOptionsEditorPresenter : IDisposablePresenter
   {
      void SetOptions(IReadOnlyDictionary<string, IEnumerable<string>> options, string selected = null);
      void Clear();
      int SelectedIndex { get; }
      string SelectedText { get; }

      event EventHandler<OptionChangedEventArgs> OnOptionSelectionChanged;
   }

   public class OptionsEditorPresenter : AbstractDisposablePresenter<IOptionsEditorView, IOptionsEditorPresenter>, IOptionsEditorPresenter
   {
      public OptionsEditorPresenter(IOptionsEditorView view) : base(view)
      {
         View.OnOptionChanged += (s, a) => optionsChanged(a.Index, a.Text);
      }

      public void SetOptions(IReadOnlyDictionary<string, IEnumerable<string>> options, string selected = null)
      {
         View.SetOptions(options, selected);
      }

      public int SelectedIndex { get; private set; }
      public string SelectedText { get; private set; }

      public void Clear()
      {
         View.Clear();
      }

      private void optionsChanged( int selectedIndex, string selectedText)
      {
         SelectedIndex = selectedIndex; 
         SelectedText = selectedText;
         OnOptionSelectionChanged.Invoke(this, new OptionChangedEventArgs {Index = selectedIndex, Text = selectedText});
      }
      public event EventHandler<OptionChangedEventArgs> OnOptionSelectionChanged = delegate { };
   }

   public class OptionChangedEventArgs : EventArgs
   {
      public int Index { get; set; }
      public string Text { get; set; }
   }

   public interface IOptionsEditorView : IView<IOptionsEditorPresenter>
   {
      void SetOptions(IReadOnlyDictionary<string, IEnumerable<string>> options, string selected = null);
      void Clear();
      event EventHandler<OptionChangedEventArgs> OnOptionChanged;
   }
}
