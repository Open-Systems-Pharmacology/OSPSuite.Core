using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Presentation.Importer.Core;
using OSPSuite.Presentation.Importer.Presenters;
using OSPSuite.Presentation.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OSPSuite.Presentation.Importer.Views
{
   public delegate void OKHandler(string units);

   public interface IUnitsEditorView : IView<IUnitsEditorPresenter>
   {
      void SetParams(Column importDataColumn, IEnumerable<IDimension> dimensions);

      event OKHandler OnOK;
   }
}
