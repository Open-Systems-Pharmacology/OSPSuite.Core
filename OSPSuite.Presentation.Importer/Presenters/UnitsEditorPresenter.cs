using OSPSuite.Presentation.Importer.Core;
using OSPSuite.Presentation.Importer.Views;
using OSPSuite.Presentation.Presenters;
using OSPSuite.Core.Importer;
using System.Security.Cryptography.X509Certificates;
using OSPSuite.Core.Domain.UnitSystem;
using System.Collections.Generic;
using System.Linq;
using OSPSuite.Utility.Extensions;

namespace OSPSuite.Presentation.Importer.Presenters
{
   public class UnitsEditorPresenter : AbstractPresenter<IUnitsEditorView, IUnitsEditorPresenter>, IUnitsEditorPresenter
   {
      private Column _importDataColumn;
      private IEnumerable<IDimension> _dimensions;
      private string _selectedUnit;

      public UnitsEditorPresenter(IUnitsEditorView view) : base(view)
      {
         
      }

      public void SetParams(Column importDataColumn, IEnumerable<IDimension> dimensions)
      {
         _selectedUnit = importDataColumn.Unit;
         _importDataColumn = importDataColumn;
         _dimensions = dimensions;

         fillDimensions();
         fillUnits();

         View.OnDimensionChanged += (dimension) => this.DoWithinExceptionHandler(() =>
         {
            _selectedUnit = _dimensions.First(d => d.Name == dimension).DefaultUnit.Name;
            fillUnits();
         });
         View.OnUnitChanged += (unit) => this.DoWithinExceptionHandler(() =>
         {
            _selectedUnit = unit;
         });
         View.OnOK += () => this.DoWithinExceptionHandler(() =>
         {
            OnOK(_selectedUnit);
            _importDataColumn.Unit = _selectedUnit;
         });

         View.SetParams(useDimensionSelector());
      }

      private bool useDimensionSelector()
      {
         return _dimensions.Count() > 1;
      }

      private void fillDimensions()
      {
         if (useDimensionSelector())
            View.FillDimensionComboBox(_dimensions, selectedDimension.Name);
         else
            View.FillDimensionComboBox(new List<IDimension>(), "");
      }

      private void fillUnits()
      {
         if (useDimensionSelector())
         {
            View.FillUnitComboBox(selectedDimension.Units, _selectedUnit);
         }

         if (_dimensions == null || !_dimensions.Any())
            return;

         View.FillUnitComboBox(_dimensions.SelectMany(d => d.Units), _selectedUnit);
      }

      private IDimension FindDimension()
      {
         foreach (var dimension in _dimensions)
         {
            if (dimension.Units.FirstOrDefault(u => u.Name == _selectedUnit) != null) return dimension;
         }
         return _dimensions.First();
      }

      private IDimension selectedDimension
      {
         get
         {
            if (useDimensionSelector())
            {
               return FindDimension();
            }
            return _dimensions.ElementAt(0);
         }
      }

      public event OKHandler OnOK = delegate { };
   }
}
