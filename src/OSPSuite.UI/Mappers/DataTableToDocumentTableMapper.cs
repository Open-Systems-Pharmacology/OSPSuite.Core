using System.Data;
using DevExpress.XtraRichEdit.API.Native;

namespace OSPSuite.UI.Mappers
{
   public interface IDataTableToDocumentTableMapper
   {
      /// <summary>
      /// Maps data from the <paramref name="analysisData"/> to the new RTF table and inserts it in the <paramref name="document"/>at the caret position
      /// </summary>
      /// <returns>The inserted table</returns>
      Table MapFrom(DataTable analysisData, Document document);
   }

   public class DataTableToDocumentTableMapper : IDataTableToDocumentTableMapper
   {
      public Table MapFrom(DataTable analysisData, Document document)
      {
         // +1 to allow for column headers
         var table = document.Tables.Create(document.CaretPosition, analysisData.Rows.Count + 1, analysisData.Columns.Count);
         var paragraphs = document.BeginUpdateParagraphs(table.Range);
         paragraphs.RightIndent = 0;
         document.EndUpdateParagraphs(paragraphs);
         addHeadersToTable(analysisData, document, table);
         addDataToTable(analysisData, document, table);
         return table;
      }

      private static void addHeadersToTable(DataTable analysisData, Document document, Table table)
      {
         foreach (DataColumn column in analysisData.Columns)
         {
            document.InsertText(table[0, analysisData.Columns.IndexOf(column)].Range.Start, column.Caption);
         }
      }

      private static void addDataToTable(DataTable analysisData, Document document, Table table)
      {
         table.BeginUpdate();
         try
         {
            table.ForEachCell(delegate(TableCell cell, int rowIndex, int cellIndex)
            {
               if (rowIndex > 0)
               {
                  document.InsertText(cell.Range.Start, analysisData.Rows[rowIndex - 1][cellIndex].ToString());
               }
            });
         }
         finally
         {
            table.EndUpdate();
         }
      }
   }
}