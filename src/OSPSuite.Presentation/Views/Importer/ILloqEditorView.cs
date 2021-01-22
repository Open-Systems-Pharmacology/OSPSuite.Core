using System.Collections.Generic;
using OSPSuite.Presentation.Presenters.Importer;

namespace OSPSuite.Presentation.Views.Importer
{
   public interface ILloqEditorView : IView<ILloqEditorPresenter>
   {
      void FillComboBox(IEnumerable<string> columns, string defaultValue);
      void FillLloqSelector(IView view);
      void SetLloqToggle(bool lloqColumnsSelection);
      bool IsLloqToggleOn();
   }
}
