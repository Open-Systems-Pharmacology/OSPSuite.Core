using System.Collections.Generic;
using System.Linq;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Infrastructure.Import.Core;

namespace OSPSuite.Presentation.DTO
{
   public class DimensionSelectionDTO
   {
      private readonly string _sheetName;
      private readonly string _description;

      public DimensionSelectionDTO(string sheetName, IReadOnlyList<string> description, ColumnInfo columnInfo, IReadOnlyList<IDimension> supportingDimensions)
      {
         _sheetName = sheetName;
         _description = string.Join(" - ", description);
         Dimensions = supportingDimensions;
         Column = columnInfo;

         if(supportingDimensions.Count == 1)
            SelectedDimension = supportingDimensions.Single();
      }

      public ColumnInfo Column { get; }

      public IReadOnlyList<IDimension> Dimensions { get; }
      public IDimension SelectedDimension { get; set; }

      public string DisplayName
      {
         get
         {
            if (string.IsNullOrEmpty(_description))
               return $"{_sheetName} - {Column.Name}";
            return $"{_sheetName} - {Column.Name} - {_description}";
         }
      }
   }
}