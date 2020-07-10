//using System;
//using System.Collections.Generic;
//using OSPSuite.Core.Services;
//using OSPSuite.Infrastructure.Import.Services;

using System.Collections.Generic;
using System.Data;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System.IO;
using NPOI.HSSF.UserModel;
using OSPSuite.Presentation.Importer.Core;
using DataTable = OSPSuite.Presentation.Importer.Core.DataTable;

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

         if (book == null)
            return null; // actually even better throw an exception

         return book;
      }

      public List<string> readHeadersList(ISheet sheet)
      {
         System.Collections.IEnumerator excelRows = sheet.GetRowEnumerator();
         excelRows.MoveNext();

         IDataTable dataTable = new DataTable(); //also not sure about the naming after all - it is exactly the same with a well known C# class
         dataTable.RawData = new Dictionary<string, IList<string>>();

         IRow excelRow = (XSSFRow)excelRows.Current; //THIS HERE IS xlsx SPECIFIC

         var headers = new List<string>();
         int tableStart = 0; //in case the table does not start from the column A

         for (int j = 0; j < excelRow.LastCellNum; j++)
         {
            ICell cell = excelRow.GetCell(j);

            if (cell != null)
               headers.Add(cell.ToString());
            else
               tableStart++; //assuming there are no empty headers - but if there are throw exception somewhere "fail with no proper format"
         }

         return headers;
      }

      public int readFirstColumn(ISheet sheet)
      {
         System.Collections.IEnumerator excelRows = sheet.GetRowEnumerator();
         excelRows.MoveNext();
         IRow excelRow = (XSSFRow)excelRows.Current; //THIS HERE IS xlsx SPECIFIC

         int tableStart = 0;

         for (int j = 0; j < excelRow.LastCellNum; j++)
         {
            ICell cell = excelRow.GetCell(j);

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
            IRow excelRowsCurrent = (XSSFRow)excelRows.Current;

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