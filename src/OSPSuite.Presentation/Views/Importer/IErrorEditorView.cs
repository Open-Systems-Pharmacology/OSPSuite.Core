using System.Collections.Generic;
using OSPSuite.Presentation.Presenters.Importer;

namespace OSPSuite.Presentation.Views.Importer
{
   public interface IErrorEditorView : IModalView<IErrorEditorPresenter>
   {
      void FillComboBox(IEnumerable<string> columns, string defaultValue);
   }
}
