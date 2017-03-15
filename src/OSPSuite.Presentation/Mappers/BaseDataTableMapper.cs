using System.Data;
using System.Text;

namespace OSPSuite.Presentation.Mappers
{
   public abstract class BaseDataTableMapper
   {
      protected abstract void CloseDataCell(StringBuilder outputBuilder);
      protected abstract void OpenDataCell(StringBuilder outputBuilder);
      protected abstract void CloseTable(StringBuilder outputBuilder);
      protected abstract void CloseRow(StringBuilder outputBuilder);
      protected abstract void CloseHeaderCell(StringBuilder outputBuilder);
      protected abstract void OpenHeaderCell(StringBuilder outputBuilder);
      protected abstract void OpenRow(StringBuilder outputBuilder);
      protected abstract void OpenTable(StringBuilder outputBuilder);

      protected void AddDataRows(DataTable dataTable, StringBuilder outputBuilder)
      {
         foreach (DataRow row in dataTable.Rows)
         {
            OpenRow(outputBuilder);
            addDataRow(row, outputBuilder);
            CloseRow(outputBuilder);
         }
      }

      private void addDataRow(DataRow row, StringBuilder outputBuilder)
      {
         foreach (var item in row.ItemArray)
         {
            OpenDataCell(outputBuilder);
            outputBuilder.Append(item);
            CloseDataCell(outputBuilder);
         }
      }

      protected void AddHeaderRow(DataTable dataTable, StringBuilder outputBuilder)
      {
         OpenRow(outputBuilder);
         foreach (DataColumn dataColumn in dataTable.Columns)
         {
            OpenHeaderCell(outputBuilder);
            outputBuilder.Append(dataColumn.Caption);
            CloseHeaderCell(outputBuilder);
         }
         CloseRow(outputBuilder);
      }

      public string MapFrom(DataTable dataTable, bool includeHeaders)
      {
         var outputBuilder = new StringBuilder();
         OpenTable(outputBuilder);

         if (includeHeaders)
            AddHeaderRow(dataTable, outputBuilder);

         AddDataRows(dataTable, outputBuilder);

         CloseTable(outputBuilder);

         return outputBuilder.ToString();
      }
   }
}