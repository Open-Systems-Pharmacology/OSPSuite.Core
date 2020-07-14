using System;
using System.Collections.Generic;
using OSPSuite.Core.Services;
using OSPSuite.Infrastructure.Import.Services;
using NPOI.SS.UserModel;
using System.IO;
using NPOI.HSSF.UserModel;
using OSPSuite.Presentation.Importer.Infrastructure;

namespace OSPSuite.Presentation.Importer.Core.DataSourceFileReaders
{
   public class ExcelDataSourceFile : DataSourceFile
   {
      public ExcelDataSourceFile(string path, IImportLogger logger) : base(path, logger) { }
      override protected Dictionary<string, IDataSheet> LoadFromFile(string path)
      {
         try
         {
            var loadedData = new Dictionary<string, IDataSheet>();

            var reader = new ExcelReader();
            IWorkbook book = reader.LoadWorkbook(path);

            for (var i = 0; i < book.NumberOfSheets; i++)
            {
               ISheet sheet = book.GetSheetAt(i);
               var sheetName = sheet.SheetName;
               //sheet.GetRow(i);

               var tableStart = reader.DetermineFirstColumn(sheet);
               var headers = reader.GetExcelRowAsListOfStrings(sheet, tableStart);
               var rows = reader.ReaDataTable(sheet, tableStart, headers.Count);

               /*
               var rows = new List<List<string>>(headers.Count);
               while (csv.ReadNextRecord())
               {
                  for (var i = 0; i < headers.Length; i++)
                     rows[i].Add(csv[i]);
               }
*/

               IDataSheet dataSheet = new DataSheet(); //DataSheet also exists (under Microsoft.Graph.Application - but is by far less common).
                                                      //if we really want to we could even do it OSPSuiteDataSheet
               dataSheet.RawData = new Dictionary<string, IList<string>>();

               for (var j = 0; j < headers.Count; j++)
                  dataSheet.RawData.Add(headers[j], rows[j]);

               loadedData.Add(sheetName, dataSheet);
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
