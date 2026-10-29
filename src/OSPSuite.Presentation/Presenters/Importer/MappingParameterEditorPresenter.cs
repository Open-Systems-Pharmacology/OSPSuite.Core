using System;
using System.Collections.Generic;
using OSPSuite.Assets;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Core.Import;
using OSPSuite.Presentation.Views.Importer;

namespace OSPSuite.Presentation.Presenters.Importer
{
   public interface IMappingParameterEditorPresenter : IDisposablePresenter
   {
      void HideAll();
      void SetUnitOptions(Column importDataColumn, IReadOnlyList<IDimension> dimensions, IEnumerable<string> availableColumns);
      void SetLloqOptions(IEnumerable<string> columns, string selected, bool lloqColumnsSelection);
      void SetErrorTypeOptions(IEnumerable<string> types, string selected, Func<string, string> onTypeChanged);
      void SetErrorTypeColumnOptions(IEnumerable<string> columns, string selected, bool errorTypeFromColumn);
      int SelectedErrorType { get; }
      bool ErrorTypeFromColumn();
      string ErrorTypeColumn { get; }
      bool LloqFromColumn();
      UnitDescription Unit { get; }
      IDimension Dimension { get; }
      string LloqColumn { get; }
      void SetUnitColumnSelection();
      void SetUnitsManualSelection();
      void InitView();
   }

   public class MappingParameterEditorPresenter : AbstractDisposablePresenter<IMappingParameterEditorView, IMappingParameterEditorPresenter>, IMappingParameterEditorPresenter
   {
      private readonly IUnitsEditorPresenter _unitsEditorPresenter;
      private readonly ILloqEditorPresenter _lloqEditorPresenter;
      private readonly IOptionsEditorPresenter _errorEditorPresenter;
      private readonly IErrorTypeColumnEditorPresenter _errorTypeColumnEditorPresenter;
      private Func<string, string> _unitValueOnTypeChanged;


      public MappingParameterEditorPresenter(
         IMappingParameterEditorView view,
         IUnitsEditorPresenter unitsEditorPresenter,
         ILloqEditorPresenter lloqEditorPresenter,
         IOptionsEditorPresenter errorEditorPresenter,
         IErrorTypeColumnEditorPresenter errorTypeColumnEditorPresenter
      ) : base(view)
      {
         _unitsEditorPresenter = unitsEditorPresenter;
         _lloqEditorPresenter = lloqEditorPresenter;
         _errorEditorPresenter = errorEditorPresenter;
         _errorTypeColumnEditorPresenter = errorTypeColumnEditorPresenter;
         View.FillUnitsView(_unitsEditorPresenter.BaseView);
         View.FillLloqView(_lloqEditorPresenter.BaseView);
         View.FillErrorView(_errorEditorPresenter.BaseView);
         View.FillErrorTypeColumnView(_errorTypeColumnEditorPresenter.BaseView);
         errorEditorPresenter.OnOptionSelectionChanged += errorSelectionChanged;
         errorTypeColumnEditorPresenter.OnErrorTypeFromColumnChanged += (o, e) => errorTypeFromColumnChanged();
      }

      public string LloqColumn => _lloqEditorPresenter.LloqColumn;

      public int SelectedErrorType => _errorEditorPresenter.SelectedIndex;

      public bool ErrorTypeFromColumn()
      {
         return _errorTypeColumnEditorPresenter.ErrorTypeFromColumn();
      }

      public string ErrorTypeColumn => _errorTypeColumnEditorPresenter.ErrorTypeColumn;

      public bool LloqFromColumn()
      {
         return _lloqEditorPresenter.LloqFromColumn();
      }

      public UnitDescription Unit => _unitsEditorPresenter.Unit;

      //when the error type comes from a column, arithmetic data sets need the units even if the default type is geometric
      public IDimension Dimension => isAlwaysGeometric(_errorEditorPresenter.SelectedText) ?
         Constants.Dimension.NO_DIMENSION :
         _unitsEditorPresenter.Dimension;

      public void SetUnitColumnSelection()
      {
         _unitsEditorPresenter.SetUnitColumnSelection();
      }

      public void SetUnitsManualSelection()
      {
         _unitsEditorPresenter.SetUnitsManualSelection();
      }

      public void InitView()
      {
         View.ShowUnits();
         _unitsEditorPresenter.ShowColumnToggle();
      }


      public void HideAll()
      {
         View.HideAll();
      }

      public void SetUnitOptions(Column importDataColumn, IReadOnlyList<IDimension> dimensions, IEnumerable<string> availableColumns)
      {
         _unitsEditorPresenter.SetOptions(importDataColumn, dimensions, availableColumns);
      }

      public void SetLloqOptions(IEnumerable<string> columns, string selected, bool lloqColumnsSelection)
      {
         _lloqEditorPresenter.SetOptions(new Dictionary<string, IEnumerable<string>>() {{"", columns}}, lloqColumnsSelection, selected);
         View.ShowLloq();
      }

      public void SetErrorTypeOptions(IEnumerable<string> types, string selected, Func<string, string> unitValueOnTypeChanged)
      {
         _unitValueOnTypeChanged = unitValueOnTypeChanged;
         _errorEditorPresenter.SetOptions(new Dictionary<string, IEnumerable<string>>() {{"", types}}, selected);
         View.ShowErrorTypes();
         updateViewForErrorType(selected);
      }

      public void SetErrorTypeColumnOptions(IEnumerable<string> columns, string selected, bool errorTypeFromColumn)
      {
         _errorTypeColumnEditorPresenter.SetOptions(columns, errorTypeFromColumn, selected);
         updateViewForErrorType(_errorEditorPresenter.SelectedText);
      }

      private bool isAlwaysGeometric(string errorType) => errorType == Constants.STD_DEV_GEOMETRIC && !ErrorTypeFromColumn();

      private void updateViewForErrorType(string errorType)
      {
         View.SetErrorTypeCaption(ErrorTypeFromColumn() ? Captions.Importer.DefaultErrorType : Captions.ErrorType);

         if (isAlwaysGeometric(errorType))
            View.HideUnits();
         else
            View.ShowUnits();
      }

      private void errorTypeFromColumnChanged()
      {
         var errorType = _errorEditorPresenter.SelectedText;
         updateViewForErrorType(errorType);

         //a geometric default has no unit but arithmetic data sets need the unit of an arithmetic error
         if (ErrorTypeFromColumn() && errorType == Constants.STD_DEV_GEOMETRIC)
            selectUnitFor(Constants.STD_DEV_ARITHMETIC);
      }

      private void errorSelectionChanged(object sender, OptionChangedEventArgs e)
      {
         updateViewForErrorType(e.Text);
         selectUnitFor(e.Text);
      }

      private void selectUnitFor(string errorType)
      {
         var unit = _unitValueOnTypeChanged(errorType);
         if (unit == null)
            return;

         _unitsEditorPresenter.SelectUnit(unit);
      }
   }
}