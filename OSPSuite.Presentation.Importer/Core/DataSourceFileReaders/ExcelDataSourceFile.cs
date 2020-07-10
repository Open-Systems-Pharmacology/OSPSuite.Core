using System;
using System.Collections.Generic;
using OSPSuite.Core.Services;
using OSPSuite.Infrastructure.Import.Services;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System.IO;
using NPOI.HSSF.UserModel;
using OSPSuite.Presentation.Importer.Infrastructure;

namespace OSPSuite.Presentation.Importer.Core.DataSourceFileReaders
{
   public class ExcelDataSourceFile : DataSourceFile
   {
      public ExcelDataSourceFile(string path, IImportLogger logger) : base(path, logger) { }
      override protected Dictionary<string, IDataTable> LoadFromFile(string path) //this is too long and should probably be broken down
      {
         try
         {
            var loadedData = new Dictionary<string, IDataTable>();

            var reader = new ExcelReader();
            IWorkbook book = reader.loadWorkbook(path); //could even be extensions of IWorkbook fe

            for (var i = 0; i < book.NumberOfSheets; i++)
            {
               ISheet sheet = book.GetSheetAt(i);
               var sheetName = sheet.SheetName;

               var tableStart = reader.readFirstColumn(sheet);
               var headers = reader.readHeadersList(sheet);
               var rows = reader.reaDataTable(sheet, tableStart, headers.Count);

               IDataTable dataTable = new DataTable(); //also not sure about the naming after all - it is exactly the same with a well known C# class
               dataTable.RawData = new Dictionary<string, IList<string>>();

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
