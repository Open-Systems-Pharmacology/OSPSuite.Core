using OSPSuite.Core.Domain;

namespace OSPSuite.Core.Importer
{
   public static class ImportDataColumnExtensions
   {
      public static string GetCaptionForColumn(this ImportDataColumn column)
      {
         var caption = !string.IsNullOrEmpty(column.Source) ? column.Source : column.DisplayName;
         if (string.IsNullOrEmpty(column.ActiveUnit.Name)) return caption;
         caption += $" [{column.ActiveUnit.Name}]";
         return caption;
      }

      public static bool ColumnContainsValidLLOQ(this ImportDataColumn importDataColumn)
      {
         if (!importDataColumn.ExtendedProperties.Contains(Constants.LLOQ)) return false;

         float existingLLOQ;
         return float.TryParse(importDataColumn.ExtendedProperties[Constants.LLOQ].ToString(), out existingLLOQ);
      }

      public static void AttachLLOQ(this ImportDataColumn importDataColumn, float lloq)
      {
         if (!float.IsNaN(lloq))
            importDataColumn.ExtendedProperties[Constants.LLOQ] = lloq;
      }

      public static float LLOQProperty(this ImportDataColumn importDataColumn)
      {
         if (importDataColumn.ColumnContainsValidLLOQ())
            return (float)importDataColumn.ExtendedProperties[Constants.LLOQ];

         return float.NaN;
      }
   }
}
