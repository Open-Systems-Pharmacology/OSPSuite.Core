using System;
using System.Collections.Generic;
using OSPSuite.Core.Services;
using OSPSuite.Infrastructure.Import.Services;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System.IO;
using NPOI.HSSF.UserModel;

namespace OSPSuite.Presentation.Importer.Core.DataSourceFileReaders
{
   public class ExcelDataSourceFile : DataSourceFile
   {
      public ExcelDataSourceFile(string path, IImportLogger logger) : base(path, logger) { }
      override protected Dictionary<string, IDataTable> LoadFromFile(string path) //this is too long and should probably be broken down
      {
         try
         {
            IWorkbook book;
            using (FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
               book = WorkbookFactory.Create(fs);
            }
            var loadedData = new Dictionary<string, IDataTable>();


            if (book == null)
               return null; // actually even better throw an exception

            for (var i = 0; i < book.NumberOfSheets; i++)
            {
               ISheet sheet = book.GetSheetAt(i);
               var sheetName = sheet.SheetName;

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
                     tableStart++; //assuming there are no empty headers 
               }

               var rows = new List<List<string>>(headers.Count);
               for (var j = 0; j < headers.Count; j++)
                  rows.Add(new List<string>());


               while (excelRows.MoveNext())
               {
                  IRow excelRowsCurrent = (XSSFRow) excelRows.Current;

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

               for (var j = 0; j < headers.Count; j++)
                  dataTable.RawData.Add(headers[j], rows[j]);

               loadedData.Add(sheetName, dataTable);
            }

            return loadedData;
         }
         catch (Exception ex)
         {
            logger.AddError(ex.ToString());
            return null;
         }
      }
   }
}
