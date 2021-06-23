using System.Collections.Generic;
using MathNet.Numerics.Financial;
using OSPSuite.Presentation.Views.Importer;

namespace OSPSuite.Presentation.Presenters.Importer
{
   public class LloqEditorPresenter : AbstractDisposablePresenter<ILloqEditorView, ILloqEditorPresenter>, ILloqEditorPresenter
   {
      private readonly IOptionsEditorPresenter _optionsEditorPresenter;

      public LloqEditorPresenter(ILloqEditorView view, IOptionsEditorPresenter optionsEditorPresenter) : base(view)
      {
         _optionsEditorPresenter = optionsEditorPresenter;
         View.FillLloqSelector(_optionsEditorPresenter.BaseView);
      }

      public string LloqColumn => _optionsEditorPresenter.SelectedText;

      public void SetOptions(IReadOnlyDictionary<string, IEnumerable<string>> options, bool lloqColumnsSelection, string selected = null)
      {
         View.SetLloqToggle(lloqColumnsSelection);
         _optionsEditorPresenter.SetOptions(options, selected);
      }

      public bool LloqFromColumn()
      {
         return View.IsLloqToggleOn();
      }
   }
}
