using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Presentation.Importer.Core;
using OSPSuite.Presentation.Presenters;
using System.Collections.Generic;

namespace OSPSuite.Presentation.Importer.Presenters
{
   public interface IUnitsEditorPresenter : IDisposablePresenter
   {
      void ShowFor(Column importDataColumn, IEnumerable<IDimension> dimensions);

      string SelectedUnit { get; }

      bool Canceled { get; }

      void SetUnit();

      void SelectDimension(string dimension);

      void SelectUnit(string unit);
   }
}
