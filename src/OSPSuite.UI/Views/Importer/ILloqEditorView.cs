using OSPSuite.Presentation.Importer;
using OSPSuite.Presentation.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OSPSuite.UI.Views.Importer
{
   public interface ILloqEditorView : IModalView<ILloqEditorPresenter>
   {
      void FillComboBox(IEnumerable<string> columns, string defaultValue);
   }
}
