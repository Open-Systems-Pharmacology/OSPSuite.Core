using OSPSuite.Presentation.Importer.Core;
using OSPSuite.Presentation.Importer.Views;
using OSPSuite.Presentation.Presenters;
using OSPSuite.Core.Domain.UnitSystem;
using System.Collections.Generic;
using System.Linq;
using OSPSuite.Utility.Extensions;

namespace OSPSuite.Presentation.Importer.Presenters
{
   public class UnitsEditorPresenter : AbstractDisposablePresenter<IUnitsEditorView, IUnitsEditorPresenter>, IUnitsEditorPresenter
   {
      private Column _importDataColumn;
      private IEnumerable<IDimension> _dimensions;
      private string _selectedUnit { get; set; }
      private string _selectedColumn { get; set; }
      private bool _canClose = false;
      private IDimensionFactory _dimensionFactory;
      private bool _columnMapping;

      public UnitDescription Unit
      {
         get
         {
            return _columnMapping ? new UnitDescription(_=>"?", _selectedColumn) : new UnitDescription(_selectedUnit);
         }
      }

      public UnitsEditorPresenter(IUnitsEditorView view, IDimensionFactory dimensionFactory) : base(view)
      {
         _dimensionFactory = dimensionFactory;
      }

      public bool Canceled => _view.Canceled;

      public override bool CanClose => base.CanClose && _canClose;

      public void ShowFor(Column importDataColumn, IEnumerable<IDimension> dimensions, IEnumerable<string> availableColumns)
      {
         _selectedUnit = importDataColumn.Unit.SelectedUnit;
         _importDataColumn = importDataColumn;
         _dimensions = dimensions;

         fillDimensions();
         fillUnits();
         View.FillColumnComboBox(availableColumns);
         _columnMapping = importDataColumn.Unit.ColumnName != null;

         View.SetParams(_columnMapping, useDimensionSelector());
         _view.Display();
      }

      public void SelectDimension(string dimension)
      {
         this.DoWithinExceptionHandler(() =>
         {
            _selectedUnit = _dimensions.First(d => d.Name == dimension).DefaultUnit.Name;
            fillUnits();
         });
      }

      public void SelectColumn(string column)
      {
         this.DoWithinExceptionHandler(() => _selectedColumn = column);
      }

      public void SelectUnit(string unit)
      {
         this.DoWithinExceptionHandler(() =>
         {
            _selectedUnit = unit;
         });
      }

      private bool useDimensionSelector()
      {
         return _dimensions.Count() > 1;
      }

      private void fillDimensions()
      {
         if (useDimensionSelector())
            View.FillDimensionComboBox(_dimensions, findSelectedOrDefaultDimension().Name);
         else
            View.FillDimensionComboBox(new List<IDimension>(), "");
      }

      private void fillUnits()
      {
         if (useDimensionSelector())
         {
            View.FillUnitComboBox(findSelectedOrDefaultDimension().Units, _selectedUnit);
         }

         if (_dimensions == null || !_dimensions.Any())
            return;

         View.FillUnitComboBox(_dimensions.SelectMany(d => d.Units), _selectedUnit);
      }

      private IDimension findSelectedOrDefaultDimension()
      {
         return _dimensionFactory.DimensionForUnit(_selectedUnit) ?? _dimensions.First();
      }

      public void SetUnit()
      {
         this.DoWithinExceptionHandler(() =>
         {
            _importDataColumn.Unit = new UnitDescription(_selectedUnit);
            _canClose = true;
         });
      }
   }
}
