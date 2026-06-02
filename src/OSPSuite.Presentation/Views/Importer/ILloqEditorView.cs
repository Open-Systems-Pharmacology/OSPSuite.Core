using System.Collections.Generic;
using OSPSuite.Presentation.Presenters.Importer;

namespace OSPSuite.Presentation.Views.Importer
{
   public interface ILloqEditorView : IView<ILloqEditorPresenter>
   {
      void FillLloqSelector(IView view);
      void SetLloqToggle(bool lloqColumnsSelection);
      bool IsLloqToggleOn();
   }
}
