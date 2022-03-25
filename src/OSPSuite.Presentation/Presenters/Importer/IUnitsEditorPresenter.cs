using System.Collections.Generic;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Core.Import;

namespace OSPSuite.Presentation.Presenters.Importer
{
   public interface IUnitsEditorPresenter : IDisposablePresenter
   {
      void SetOptions(Column importDataColumn, IReadOnlyList<IDimension> dimensions, IEnumerable<string> availableColumns);
      UnitDescription Unit { get; }
      IDimension Dimension { get; }
      void SelectDimension(string dimensionName);
      void SelectUnit(string unit);
      void SelectColumn(string column);
      void SetUnitColumnSelection();
      void SetUnitsManualSelection();
      void ShowColumnToggle();
      void FillDimensions(string selectedUnit);
   }
}
