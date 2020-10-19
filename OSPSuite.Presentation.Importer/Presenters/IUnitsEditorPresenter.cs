using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Presentation.Presenters;
using System.Collections.Generic;
using OSPSuite.Infrastructure.Import.Core;

namespace OSPSuite.Presentation.Importer.Presenters
{
   public interface IUnitsEditorPresenter : IDisposablePresenter
   {
      void ShowFor(Column importDataColumn, IEnumerable<IDimension> dimensions, IEnumerable<string> availableColumns);
      UnitDescription Unit { get; }
      bool Canceled { get; }
      void SetUnit();
      void SelectDimension(string dimension);
      void SelectUnit(string unit);
      void SelectColumn(string column);
   }
}
