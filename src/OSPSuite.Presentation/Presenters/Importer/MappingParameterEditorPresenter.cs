using System.Collections.Generic;
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
      void SetErrorTypeOptions(IEnumerable<string> types, string selected);
      int SelectedErrorType { get; }
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


      public MappingParameterEditorPresenter(
         IMappingParameterEditorView view,
         IUnitsEditorPresenter unitsEditorPresenter,
         ILloqEditorPresenter lloqEditorPresenter,
         IOptionsEditorPresenter errorEditorPresenter
      ) : base(view)
      {
         _unitsEditorPresenter = unitsEditorPresenter;
         _lloqEditorPresenter = lloqEditorPresenter;
         _errorEditorPresenter = errorEditorPresenter;
         View.FillUnitsView(_unitsEditorPresenter.BaseView);
         View.FillLloqView(_lloqEditorPresenter.BaseView);
         View.FillErrorView(_errorEditorPresenter.BaseView);
         errorEditorPresenter.OnOptionSelectionChanged += errorSelectionChanged;
      }

      public string LloqColumn => _lloqEditorPresenter.LloqColumn;

      public int SelectedErrorType => _errorEditorPresenter.SelectedIndex;

      public bool LloqFromColumn()
      {
         return _lloqEditorPresenter.LloqFromColumn();
      }

      public UnitDescription Unit => _unitsEditorPresenter.Unit;

      public IDimension Dimension => _errorEditorPresenter.SelectedText == Constants.STD_DEV_GEOMETRIC ? 
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

      public void SetErrorTypeOptions(IEnumerable<string> types, string selected)
      {
         _errorEditorPresenter.SetOptions(new Dictionary<string, IEnumerable<string>>() {{"", types}}, selected);
         View.ShowErrorTypes();

         if (selected != null && selected.Equals(Constants.STD_DEV_GEOMETRIC))
            View.HideUnits();
      }

      private void errorSelectionChanged(object sender, OptionChangedEventArgs e)
      {
         if (e.Text.Equals(Constants.STD_DEV_ARITHMETIC))
            View.ShowUnits();
         else if (e.Text.Equals(Constants.STD_DEV_GEOMETRIC))
            View.HideUnits();
      }
   }
}