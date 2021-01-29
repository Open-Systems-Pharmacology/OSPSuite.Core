using System.Collections.Generic;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Core.Import;

namespace OSPSuite.Presentation.Presenters.Importer
{
   public interface IUnitsEditorPresenter : IDisposablePresenter
   {
      void SetOptions(Column importDataColumn, IEnumerable<IDimension> dimensions, IEnumerable<string> availableColumns);
      UnitDescription Unit { get; }
      void SetUnit();
      void SelectDimension(string dimension);
      void SelectUnit(string unit);
      void SelectColumn(string column);
      void SetUnitColumnSelection();
      void SetUnitsManualSelection();
      void ShowColumnToggle();
   }
}
