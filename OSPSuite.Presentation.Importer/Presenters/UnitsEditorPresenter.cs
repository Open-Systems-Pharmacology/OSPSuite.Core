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
      public string SelectedUnit { get; private set; }
      private bool _canClose = false;
      private IDimensionFactory _dimensionFactory;

      public UnitsEditorPresenter(IUnitsEditorView view, IDimensionFactory dimensionFactory) : base(view)
      {
         _dimensionFactory = dimensionFactory;
      }

      public bool Canceled => _view.Canceled;

      public override bool CanClose => base.CanClose && _canClose;

      public void ShowFor(Column importDataColumn, IEnumerable<IDimension> dimensions)
      {
         SelectedUnit = importDataColumn.Unit.SelectedUnit;
         _importDataColumn = importDataColumn;
         _dimensions = dimensions;

         fillDimensions();
         fillUnits();

         View.SetParams(useDimensionSelector());
         _view.Display();
      }

      public void SelectDimension(string dimension)
      {
         this.DoWithinExceptionHandler(() =>
         {
            SelectedUnit = _dimensions.First(d => d.Name == dimension).DefaultUnit.Name;
            fillUnits();
         });
      }

      public void SelectUnit(string unit)
      {
         this.DoWithinExceptionHandler(() =>
         {
            SelectedUnit = unit;
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
            View.FillUnitComboBox(findSelectedOrDefaultDimension().Units, SelectedUnit);
         }

         if (_dimensions == null || !_dimensions.Any())
            return;

         View.FillUnitComboBox(_dimensions.SelectMany(d => d.Units), SelectedUnit);
      }

      private IDimension findSelectedOrDefaultDimension()
      {
         return _dimensionFactory.DimensionForUnit(SelectedUnit) ?? _dimensions.First();
      }

      public void SetUnit()
      {
         this.DoWithinExceptionHandler(() =>
         {
            _importDataColumn.Unit = new UnitDescription(SelectedUnit);
            _canClose = true;
         });
      }
   }
}
