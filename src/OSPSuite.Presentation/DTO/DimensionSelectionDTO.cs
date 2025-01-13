using System.Collections.Generic;
using System.Linq;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Infrastructure.Import.Core;

namespace OSPSuite.Presentation.DTO
{
   public class DimensionSelectionDTO
   {
      private readonly string _sheetName;

      public DimensionSelectionDTO(string sheetName, ColumnInfo columnInfo, IReadOnlyList<IDimension> supportingDimensions)
      {
         _sheetName = sheetName;
         Dimensions = supportingDimensions;
         Column = columnInfo;

         if(supportingDimensions.Count == 1)
            SelectedDimension = supportingDimensions.Single();
      }

      public ColumnInfo Column { get; }

      public IReadOnlyList<IDimension> Dimensions { get; }
      public IDimension SelectedDimension { get; set; }
      public string DisplayName => $"{_sheetName} - {Column.Name}";
   }
}