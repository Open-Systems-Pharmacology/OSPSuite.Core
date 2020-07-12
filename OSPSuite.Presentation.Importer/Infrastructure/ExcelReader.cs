using System.Collections;
using System.Collections.Generic;
using System.Data;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System.IO;
using NPOI.HSSF.UserModel;
using OSPSuite.Presentation.Importer.Core;

namespace OSPSuite.Presentation.Importer.Infrastructure
{
   public class ExcelReader //or maybe rename it to excelReaderExtensions 
   {
      public IWorkbook loadWorkbook(string path)
      {
         IWorkbook book;
         using (FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
         {
            book = WorkbookFactory.Create(fs);
         }

         //could possibly throw an exception if something goes wring with reading
         return book;
      }
      private IRow GetCurrentExcelRow(IEnumerator enumerator)
      {
         IRow row;
         try //discuss with Abdel: better an try-catch block, or should we actually do an if-esle and check the extension?
         {
            row = (XSSFRow) enumerator.Current;
         }
         catch
         {
            row = (HSSFRow)enumerator.Current;
         }
         return row;
      }

      //this cannot be used for readTable, bacouse we would have to transpose the list....we could do a seperate method for range.
      //this would not ensure minimum code duplication, but is probably the most logical solution
      //alternatively we can get the list and then traverse it to make the transposition (or with LINQ)
      public List<string> getExcelRowAsListOfStrings(ISheet sheet, int columnOffset)
      {
         var rangeList = new List<string>();

         System.Collections.IEnumerator excelRows = sheet.GetRowEnumerator();
         excelRows.MoveNext();

         var currentExcelRow = GetCurrentExcelRow(excelRows);

         for (int i = columnOffset; i < currentExcelRow.LastCellNum; i++)
         {
            ICell cell = currentExcelRow.GetCell(i);

            if (cell != null)
               rangeList.Add(cell.ToString());
            else
               rangeList.Add(""); //we allow empty values for the headers in this variation - but if there are throw exception somewhere "fail with no proper format"
         }
         return rangeList;
      }


      public int determineFirstColumn(ISheet sheet)
      {
         System.Collections.IEnumerator excelRows = sheet.GetRowEnumerator();
         excelRows.MoveNext();

         var currentExcelRow = GetCurrentExcelRow(excelRows);

         int tableStart = 0;

         for (int j = 0; j < currentExcelRow.LastCellNum; j++)
         {
            ICell cell = currentExcelRow.GetCell(j);

            if (cell == null)
               tableStart++;
            else
               break;
         }
         return tableStart;
      }

      public List<List<string>> reaDataTable(ISheet sheet, int tableStart, int size)
      {
         var rows = new List<List<string>>(size);
         for (var j = 0; j < size; j++)
            rows.Add(new List<string>());

         System.Collections.IEnumerator excelRows = sheet.GetRowEnumerator();
         excelRows.MoveNext();

         while (excelRows.MoveNext())
         {
            var excelRowsCurrent = GetCurrentExcelRow(excelRows);

            for (int j = tableStart; j < excelRowsCurrent.LastCellNum; j++)
            {
               var cell = excelRowsCurrent.GetCell(j);

               if (cell != null)
               {
                  rows[j - tableStart].Add(cell.ToString());
               }
               else
               {
                  rows[j - tableStart].Add("");
               }
            }
         }
         return rows;
      }
   }
}