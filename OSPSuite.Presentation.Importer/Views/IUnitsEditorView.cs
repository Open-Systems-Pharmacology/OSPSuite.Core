using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Presentation.Importer.Presenters;
using OSPSuite.Presentation.Views;
using System.Collections.Generic;

namespace OSPSuite.Presentation.Importer.Views
{
   public delegate void UnitChangeHandler(string unit);
   public delegate void DimensionChangeHandler(string dimension);
   public delegate void OKHandler();
   public interface IUnitsEditorView : IView<IUnitsEditorPresenter>
   {
      void SetParams(bool useDimensionSelector);

      void FillDimensionComboBox(IEnumerable<IDimension> dimensions, string defaultValue);

      void FillUnitComboBox(IEnumerable<Unit> units, string defaultValue);

      event UnitChangeHandler OnUnitChanged;

      event DimensionChangeHandler OnDimensionChanged;

      event OKHandler OnOK;
   }
}
