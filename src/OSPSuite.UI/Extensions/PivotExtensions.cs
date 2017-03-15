using DevExpress.XtraPivotGrid;

namespace OSPSuite.UI.Extensions
{
   public static class PivotExtensions
   {
      public static T Value<T>(this PivotDrillDownDataSource ds, PivotGridField field) where T : class
      {
         return Value<T>(ds, field.FieldName);
      }

      public static T Value<T>(this PivotDrillDownDataSource ds, string fieldName) where T : class
      {
         if (ds.RowCount == 0)
            return default(T);

         return Value<T>(ds[0],fieldName);
      }
      public static string StringValue(this PivotDrillDownDataSource ds, PivotGridField field)
      {
         return StringValue(ds, field.FieldName);
      }

      public static string StringValue(this PivotDrillDownDataSource ds, string fieldName)
      {
         return Value<string>(ds, fieldName);
      }

      public static T Value<T>(this PivotDrillDownDataRow dr, string fieldName) where T:class
      {
         return dr[fieldName] as T;
      }

      public static string StringValue(this PivotDrillDownDataRow dr, string fieldName)
      {
         return Value<string>(dr, fieldName);
      }


   }
}