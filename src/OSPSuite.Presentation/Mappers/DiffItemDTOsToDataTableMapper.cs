using System.Collections.Generic;
using System.Data;
using OSPSuite.Assets;
using OSPSuite.Core.Extensions;
using OSPSuite.Presentation.DTO;
using OSPSuite.Utility.Extensions;

namespace OSPSuite.Presentation.Mappers
{
   public interface IDiffItemDTOsToDataTableMapper
   {
      DataTable MapFrom(IEnumerable<DiffItemDTO> diffItemDTOs, string leftCaption, string rightCaption);
   }

   public class DiffItemDTOsToDataTableMapper : IDiffItemDTOsToDataTableMapper
   {
      private static readonly string PATH_AS_STRING = Captions.Comparisons.PathAsString;
      private static readonly string DESCRIPTION = Captions.Comparisons.Description;
      private static readonly string OBJECT_NAME = Captions.Comparisons.ObjectName;
      private static readonly string PROPERTY = Captions.Comparisons.Property;

      public DataTable MapFrom(IEnumerable<DiffItemDTO> diffItemDTOs, string leftCaption, string rightCaption)
      {
         var dataTable = new DataTable(Captions.Comparisons.Comparison);
         dataTable.AddColumn(PATH_AS_STRING);
         dataTable.AddColumn(OBJECT_NAME);
         dataTable.AddColumn(PROPERTY);
         dataTable.AddColumn(leftCaption);
         dataTable.AddColumn(rightCaption);
         dataTable.AddColumn(DESCRIPTION);

         foreach (var diffItemDTO in diffItemDTOs)
         {
            var row = dataTable.NewRow();
            row[PATH_AS_STRING] = diffItemDTO.PathForExport;
            row[OBJECT_NAME] = diffItemDTO.ObjectName;
            row[PROPERTY] = diffItemDTO.Property;
            row[leftCaption] = diffItemDTO.Value1;
            row[rightCaption] = diffItemDTO.Value2;
            row[DESCRIPTION] = diffItemDTO.Description;

            dataTable.Rows.Add(row);
         }
         return dataTable;
      }
   }
}