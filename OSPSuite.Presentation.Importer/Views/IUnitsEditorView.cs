using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Presentation.Importer.Presenters;
using OSPSuite.Presentation.Views;
using System.Collections.Generic;

namespace OSPSuite.Presentation.Importer.Views
{
   public interface IUnitsEditorView : IModalView<IUnitsEditorPresenter>
   {
      void SetParams(bool columnMapping, bool useDimensionSelector);
      void FillDimensionComboBox(IEnumerable<IDimension> dimensions, string defaultValue);
      void FillUnitComboBox(IEnumerable<Unit> units, string defaultValue);
      void FillColumnComboBox(IEnumerable<string> columns);
   }
}
