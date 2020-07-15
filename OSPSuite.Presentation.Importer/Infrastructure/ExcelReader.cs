using System.Collections;
using System.Collections.Generic;
using System.Data;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System.IO;
using NPOI.HSSF.UserModel;
using OSPSuite.Core.Importer.Mappers;
using OSPSuite.Presentation.Importer.Core;

namespace OSPSuite.Presentation.Importer.Infrastructure
{
   public class ExcelReader //or maybe rename it to excelReaderExtensions - or make it into extensions
   {   //we could get this info here or in getCurrentRow, but make sure we do it once
      private IWorkbook book;
      private IEnumerator<ISheet> sheetEnumerator;
      private IEnumerator rowEnumerator;

      public ExcelReader(string path)
      {
         using (FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
         {
            book = WorkbookFactory.Create(fs);
         }

         sheetEnumerator = book.GetEnumerator();
         //CurrentSheet = sheetEnumerator.Current;
         //rowEnumerator = CurrentSheet.GetEnumerator();
      }
      public ISheet CurrentSheet { get; set; }
      public List<string> CurrentRow { get; set; } = new List<string>();

      public void LoadNewWorkbook(string path)
      {
         using (FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
         {
            book = WorkbookFactory.Create(fs);
         }
      }

      public bool MoveToNextSheet()
      {
         if (!sheetEnumerator.MoveNext())
            return false;

         CurrentSheet = sheetEnumerator.Current;
         rowEnumerator = CurrentSheet.GetEnumerator();

         return true;
      }

      //this cannot be used for readTable, because we would have to transpose the list....we could do a separate method for range.
         //this would not ensure minimum code duplication, but is probably the most logical solution
         //alternatively we can get the list and then traverse it to make the transposition (or with LINQ)
      public bool MoveToNextRow(int columnOffset)
      {
         if (!rowEnumerator.MoveNext())
            return false;

         CurrentRow.Clear();

         var currentExcelRow = getCurrentExcelRow(rowEnumerator);

         for (int i = columnOffset; i < currentExcelRow.LastCellNum; i++)
         {
            ICell cell = currentExcelRow.GetCell(i);

            if (cell != null)
               CurrentRow.Add(cell.ToString());
            else
               CurrentRow.Add(""); //we allow empty values for the headers in this variation - but if there are throw exception somewhere "fail with no proper format"
         }
         return true;
      }


      public int DetermineFirstColumn()
      {
         System.Collections.IEnumerator excelRows = CurrentSheet.GetRowEnumerator();
         excelRows.MoveNext();

         var currentExcelRow = getCurrentExcelRow(excelRows);

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

      public List<List<string>> ReaDataTable(ISheet sheet, int tableStart, int size)
      {
         var rows = new List<List<string>>(size);
         for (var j = 0; j < size; j++)
            rows.Add(new List<string>());

         System.Collections.IEnumerator excelRows = sheet.GetRowEnumerator();
         excelRows.MoveNext();

         while (excelRows.MoveNext())
         {
            var excelRowsCurrent = getCurrentExcelRow(excelRows);

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

      private IRow getCurrentExcelRow(IEnumerator enumerator)
      {
         IRow row; //we do this every time we try to read a row - we have to actually store the info at the beginning
         try //discuss with Abdel: better a try-catch block, or should we actually do an if-esle and check the extension?
         {
            row = (XSSFRow)enumerator.Current;
         }
         catch
         {
            row = (HSSFRow)enumerator.Current;
         }
         return row;
      }
   }
}