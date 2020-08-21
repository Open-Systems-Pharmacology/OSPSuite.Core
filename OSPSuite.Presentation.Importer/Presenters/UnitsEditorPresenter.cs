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
   public class UnitsEditorPresenter : AbstractDisposablePresenter<IUnitsEditorView, IUnitsEditorPresenter>, IUnitsEditorPresenter
   {
      private Column _importDataColumn;
      private IEnumerable<IDimension> _dimensions;
      public string SelectedUnit { get; private set; }
      private bool _canClose = false;

      public UnitsEditorPresenter(IUnitsEditorView view) : base(view)
      {
      }

      public bool Canceled => _view.Canceled;

      public override bool CanClose => base.CanClose && _canClose;

      public void ShowFor(Column importDataColumn, IEnumerable<IDimension> dimensions)
      {
         SelectedUnit = importDataColumn.Unit;
         _importDataColumn = importDataColumn;
         _dimensions = dimensions;

         fillDimensions();
         fillUnits();

         View.OnDimensionChanged += (dimension) => this.DoWithinExceptionHandler(() =>
         {
            SelectedUnit = _dimensions.First(d => d.Name == dimension).DefaultUnit.Name;
            fillUnits();
         });
         View.OnUnitChanged += (unit) => this.DoWithinExceptionHandler(() =>
         {
            SelectedUnit = unit;
         });
         View.OnOK += () => this.DoWithinExceptionHandler(() =>
         {
            OnOK(SelectedUnit);
            _importDataColumn.Unit = SelectedUnit;
            _canClose = true;
         });

         View.SetParams(useDimensionSelector());
         _view.Display();
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
            View.FillUnitComboBox(selectedDimension.Units, SelectedUnit);
         }

         if (_dimensions == null || !_dimensions.Any())
            return;

         View.FillUnitComboBox(_dimensions.SelectMany(d => d.Units), SelectedUnit);
      }

      private IDimension FindDimension()
      {
         foreach (var dimension in _dimensions)
         {
            if (dimension.Units.FirstOrDefault(u => u.Name == SelectedUnit) != null) return dimension;
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
