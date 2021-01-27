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
      private IDimensionFactory _dimensionFactory;
      private bool _columnMapping;

      public UnitDescription Unit
      {
         get
         {
            return new UnitDescription(_selectedUnit, _selectedColumn);
         }
      }

      public UnitsEditorPresenter(IUnitsEditorView view, IDimensionFactory dimensionFactory) : base(view)
      {
         _dimensionFactory = dimensionFactory;
      }

      public void SetOptions(Column importDataColumn, IEnumerable<IDimension> dimensions, IEnumerable<string> availableColumns)
      {
         _importDataColumn = importDataColumn;
         _dimensions = dimensions;

         _columnMapping = importDataColumn.Unit.ColumnName != null;
         View.SetParams(_columnMapping, useDimensionSelector());
         fillDimensions(importDataColumn.Unit.SelectedUnit);
         fillUnits(importDataColumn.Unit.SelectedUnit);
         _selectedUnit = importDataColumn.Unit.SelectedUnit;
         View.FillColumnComboBox(availableColumns);
         View.ShowToggle();
      }

      public void SelectDimension(string dimension)
      {
         this.DoWithinExceptionHandler(() =>
         {
            _selectedUnit = (_dimensions.FirstOrDefault(d => d.Name == dimension) ?? _dimensions.First()).DefaultUnit.Name;
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
         if (useDimensionSelector())
            View.FillDimensionComboBox(_dimensions, findSelectedOrDefaultDimension(selectedUnit).Name);
         else
            View.FillDimensionComboBox(new List<IDimension>(), "");
      }

      private void fillUnits(string selectedUnit)
      {
         if (useDimensionSelector())
         {
            View.FillUnitComboBox(findSelectedOrDefaultDimension(selectedUnit).Units, selectedUnit);
         }

         if (_dimensions == null || !_dimensions.Any())
            return;

         View.FillUnitComboBox(_dimensions.SelectMany(d => d.Units).Where(u => !string.IsNullOrEmpty(u.Name)), selectedUnit);
      }

      private IDimension findSelectedOrDefaultDimension(string selectedUnit)
      {
         return _dimensions.FirstOrDefault(d => d.Units.Any(u => u.Name == selectedUnit)) ?? _dimensions.First();
      }

      public void SetUnit()
      {
         this.DoWithinExceptionHandler(() =>
         {
            _importDataColumn.Unit = new UnitDescription(_selectedUnit);
         });
      }
   }
}
