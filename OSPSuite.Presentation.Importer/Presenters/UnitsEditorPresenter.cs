using OSPSuite.Presentation.Importer.Core;
using OSPSuite.Presentation.Importer.Views;
using OSPSuite.Presentation.Presenters;
using OSPSuite.Core.Importer;
using System.Security.Cryptography.X509Certificates;
using OSPSuite.Core.Domain.UnitSystem;
using System.Collections.Generic;

namespace OSPSuite.Presentation.Importer.Presenters
{
   public class UnitsEditorPresenter : AbstractPresenter<IUnitsEditorView, IUnitsEditorPresenter>, IUnitsEditorPresenter
   {
      private Column _importDataColumn;
      private IEnumerable<IDimension> _dimensions;
      private string _selectedUnit;

      public UnitsEditorPresenter(IUnitsEditorView view) : base(view)
      {
      }

      public void SetParams(Column importDataColumn, IEnumerable<IDimension> dimensions)
      {
         View.SetParams(importDataColumn, dimensions);
      }
   }
}
