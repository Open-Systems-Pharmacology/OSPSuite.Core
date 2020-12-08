using System;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Infrastructure.Import.Core;
using OSPSuite.Presentation.Views;
using System.Collections.Generic;
using System.Linq;
using OSPSuite.Presentation.Views.Importer;

namespace OSPSuite.Presentation.Presenters.Importer
{
   public interface IMappingParameterEditorPresenter : IDisposablePresenter
   {
      void HideAll();
      void SetUnitOptions(Column importDataColumn, IEnumerable<IDimension> dimensions, IEnumerable<string> availableColumns);
      void SetLloqOptions(IEnumerable<string> columns, string selected);
      void SetErrorTypeOptions(IEnumerable<string> types, string selected);
      int SelectedUnit { get; }
      int SelectedLloq { get; }
      int SelectedErrorType { get; }
      UnitDescription Unit { get; }
   }

   public class MappingParameterEditorPresenter : AbstractDisposablePresenter<IMappingParameterEditorView, IMappingParameterEditorPresenter>, IMappingParameterEditorPresenter
   {
      private readonly IOptionsEditorPresenter _unitsEditorPresenter;
      private readonly IOptionsEditorPresenter _lloqEditorPresenter;
      private readonly IOptionsEditorPresenter _errorEditorPresenter;
      private IEnumerable<IDimension> _dimensions;
      private bool _columnMapping;

      public int SelectedUnit { get => _unitsEditorPresenter.SelectedIndex; }
      public int SelectedLloq { get => _lloqEditorPresenter.SelectedIndex; }
      public int SelectedErrorType { get => _errorEditorPresenter.SelectedIndex; }

      public UnitDescription Unit
      {
         get
         {
            return _columnMapping ? new UnitDescription(_ => "", _unitsEditorPresenter.SelectedText) : new UnitDescription(_unitsEditorPresenter.SelectedText);
         }
      }

      public MappingParameterEditorPresenter(
         IMappingParameterEditorView view,
         IOptionsEditorPresenter unitsEditorPresenter,
         IOptionsEditorPresenter lloqEditorPresenter,
         IOptionsEditorPresenter errorEditorPresenter
      ) : base(view)
      {
         _unitsEditorPresenter = unitsEditorPresenter;
         _lloqEditorPresenter = lloqEditorPresenter;
         _errorEditorPresenter = errorEditorPresenter;
         View.FillUnitsView(_unitsEditorPresenter.BaseView);
         View.FillLloqView(_lloqEditorPresenter.BaseView);
         View.FillErrorView(_errorEditorPresenter.BaseView);
      }

      public void HideAll()
      {
         View.HideAll();
      }

      public void SetUnitOptions(Column importDataColumn, IEnumerable<IDimension> dimensions, IEnumerable<string> availableColumns)
      {
         _dimensions = dimensions;
         _columnMapping = !string.IsNullOrEmpty(importDataColumn.Unit.ColumnName);

         if (_columnMapping)
         {
            _unitsEditorPresenter.SetOptions(new Dictionary<string, IEnumerable<string>>() {{"", availableColumns } }, importDataColumn.Unit.ColumnName);
         }
         else
         {
            _unitsEditorPresenter.SetOptions(_dimensions.ToDictionary(d => d.Name, d => d.Units.Select(u => u.Name)), importDataColumn.Unit.SelectedUnit);
         }
         View.ShowUnits();
      }

      public void SetLloqOptions(IEnumerable<string> columns, string selected)
      {
         _lloqEditorPresenter.SetOptions(new Dictionary<string, IEnumerable<string>>() { { "", columns } });
         View.ShowLloq();
      }

      public void SetErrorTypeOptions(IEnumerable<string> types, string selected)
      {
         _errorEditorPresenter.SetOptions(new Dictionary<string, IEnumerable<string>>() { { "", types } }, selected);
         View.ShowErrorTypes();
      }
   }
}
