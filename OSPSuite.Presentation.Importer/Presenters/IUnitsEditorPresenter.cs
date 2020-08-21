using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Presentation.Importer.Core;
using OSPSuite.Presentation.Importer.Views;
using OSPSuite.Presentation.Presenters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OSPSuite.Presentation.Importer.Presenters
{
   public delegate void OKHandler(string units);

   public interface IUnitsEditorPresenter : IPresenter<IUnitsEditorView>
   {
      void SetParams(Column importDataColumn, IEnumerable<IDimension> dimensions);

      event OKHandler OnOK;
   }
}
