using System.Collections.Generic;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Presentation.Presenters.Importer;

namespace OSPSuite.Presentation.Views.Importer
{
   public interface IUnitsEditorView : IView<IUnitsEditorPresenter>
   {
      void SetParams(bool columnMapping, bool useDimensionSelector);
      void FillDimensionComboBox(IEnumerable<IDimension> dimensions, string defaultValue);
      void FillUnitComboBox(IEnumerable<Unit> units, string defaultValue);
      void FillColumnComboBox(IEnumerable<string> columns);
      void SetUnitColumnSelection();
      void ShowToggle();
      void SetUnitsManualSelection();
   }
}
