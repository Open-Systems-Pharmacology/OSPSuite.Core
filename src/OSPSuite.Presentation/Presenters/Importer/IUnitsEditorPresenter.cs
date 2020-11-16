using System.Collections.Generic;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Infrastructure.Import.Core;

namespace OSPSuite.Presentation.Presenters.Importer
{
   public interface IUnitsEditorPresenter : IDisposablePresenter
   {
      void SetOptions(Column importDataColumn, IEnumerable<IDimension> dimensions, IEnumerable<string> availableColumns);
      UnitDescription Unit { get; }
      void SetUnit();
      void SelectDimension(string dimension);
      void SelectUnit(string unit);
      void SelectColumn(string column);
   }
}
