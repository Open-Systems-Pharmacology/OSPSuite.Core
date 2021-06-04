using System.Collections.Generic;
using System.Linq;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Core.Import;
using OSPSuite.Presentation.Views.Importer;
using OSPSuite.Utility.Extensions;

namespace OSPSuite.Presentation.Presenters.Importer
{
   public class UnitsEditorPresenter : AbstractDisposablePresenter<IUnitsEditorView, IUnitsEditorPresenter>, IUnitsEditorPresenter
   {
      private Column _importDataColumn;
      private IEnumerable<IDimension> _dimensions;
      private string _selectedUnit { get; set; }
      private string _selectedColumn { get; set; }
      public IDimension Dimension { get; private set; }

      private bool _columnMapping;

      public UnitDescription Unit
      {
         get
         {
            return new UnitDescription(_selectedUnit, _selectedColumn);
         }
      }

      public UnitsEditorPresenter(IUnitsEditorView view) : base(view)
      {
      }

      public void SetOptions(Column importDataColumn, IEnumerable<IDimension> dimensions, IEnumerable<string> availableColumns)
      {
         _importDataColumn = importDataColumn;
         _dimensions = dimensions;

         _columnMapping = importDataColumn.Unit.ColumnName != null;
         View.SetParams(_columnMapping, useDimensionSelector());
         Dimension = importDataColumn.Dimension;
         fillDimensions(importDataColumn.Unit.SelectedUnit);
         fillUnits(importDataColumn.Unit.SelectedUnit);
         _selectedUnit = importDataColumn.Unit.SelectedUnit;
         View.FillColumnComboBox(availableColumns);
      }

      public void SelectDimension(string dimension)
      {
         this.DoWithinExceptionHandler(() =>
         {
            Dimension = _dimensions.FirstOrDefault(d => d.Name == dimension) ?? _dimensions.First();
            _selectedUnit = Dimension.DefaultUnit.Name;
            SetUnit();
            fillUnits(_selectedUnit);
         });
      }

      public void SelectColumn(string column)
      {
         this.DoWithinExceptionHandler(() => {
            _columnMapping = true;
            _selectedColumn = column;
         });
      }

      public void SetUnitColumnSelection()
      {
         View.SetUnitColumnSelection();
      }

      public void SetUnitsManualSelection()
      {
         View.SetUnitsManualSelection();
      }

      public void ShowColumnToggle()
      {
         View.ShowToggle();
      }

      public void SelectUnit(string unit)
      {
         this.DoWithinExceptionHandler(() =>
         {
            _columnMapping = false;
            _selectedColumn = null;
            _selectedUnit = unit;
         });
      }

      private bool useDimensionSelector()
      {
         return _dimensions.Count() > 1;
      }

      private void fillDimensions(string selectedUnit)
      {
         if (useDimensionSelector() && Dimension != null)
            View.FillDimensionComboBox(_dimensions, Dimension.Name);
         else if ( Dimension == null && selectedUnit == UnitDescription.InvalidUnit)
            View.FillDimensionComboBox(_dimensions, _dimensions.FirstOrDefault()?.Name);
         else
            View.FillDimensionComboBox(new List<IDimension>(), "");
      }

      private void fillUnits(string selectedUnit)
      {
         if (useDimensionSelector() && Dimension != null)
            View.FillUnitComboBox(Dimension.Units, selectedUnit);

         if (_dimensions == null || !_dimensions.Any() || Dimension == null)
            return;

         View.FillUnitComboBox(Dimension?.Units, selectedUnit);
      }

      public void SetUnit()
      {
         this.DoWithinExceptionHandler(() =>
         {
            _importDataColumn.Unit = new UnitDescription(_selectedUnit);
            _importDataColumn.Dimension = Dimension;
         });
      }
   }
}
