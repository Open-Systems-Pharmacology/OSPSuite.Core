using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Infrastructure.Import.Core;
using System.Collections.Generic;
using OSPSuite.Core.Domain;
using OSPSuite.Presentation.Views.Importer;

namespace OSPSuite.Presentation.Presenters.Importer
{
   public interface IMappingParameterEditorPresenter : IDisposablePresenter
   {
      void HideAll();
      void SetUnitOptions(Column importDataColumn, IEnumerable<IDimension> dimensions, IEnumerable<string> availableColumns);
      void SetLloqOptions(IEnumerable<string> columns, string selected, bool lloqColumnsSelection);
      void SetErrorTypeOptions(IEnumerable<string> types, string selected);
      int SelectedLloq { get; }
      int SelectedErrorType { get; }
      bool LloqFromColumn();
      UnitDescription Unit { get; }
   }

   public class MappingParameterEditorPresenter : AbstractDisposablePresenter<IMappingParameterEditorView, IMappingParameterEditorPresenter>, IMappingParameterEditorPresenter
   {
      private readonly IUnitsEditorPresenter _unitsEditorPresenter;
      private readonly ILloqEditorPresenter _lloqEditorPresenter;
      private readonly IOptionsEditorPresenter _errorEditorPresenter;
      private IEnumerable<IDimension> _dimensions;

      public int SelectedLloq { get => _lloqEditorPresenter.SelectedIndex; }
      public int SelectedErrorType { get => _errorEditorPresenter.SelectedIndex; }

      public bool LloqFromColumn()
      {
         return _lloqEditorPresenter.LloqFromColumn();
      }

      public UnitDescription Unit
      {
         get => _unitsEditorPresenter.Unit;
      }

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

      public void HideAll()
      {
         View.HideAll();
      }

      public void SetUnitOptions(Column importDataColumn, IEnumerable<IDimension> dimensions, IEnumerable<string> availableColumns)
      {
         _dimensions = dimensions;
         _unitsEditorPresenter.SetOptions(importDataColumn, _dimensions, availableColumns);
         View.ShowUnits();
      }

      public void SetLloqOptions(IEnumerable<string> columns, string selected, bool lloqColumnsSelection)
      {
         _lloqEditorPresenter.SetOptions(new Dictionary<string, IEnumerable<string>>() { { "", columns } }, lloqColumnsSelection);
         View.ShowLloq();
      }

      public void SetErrorTypeOptions(IEnumerable<string> types, string selected)
      {
         _errorEditorPresenter.SetOptions(new Dictionary<string, IEnumerable<string>>() { { "", types } }, selected);
         View.ShowErrorTypes();
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
