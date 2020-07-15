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


            var reader = new ExcelReader(path);

            while (reader.MoveToNextSheet())
            {
               var sheetName = reader.CurrentSheet.SheetName;
               var headers = new List<string>();
               IDataSheet dataSheet = new DataSheet();
               dataSheet.RawData = new UnformattedData();

               var tableStart = reader.DetermineFirstColumn(); //rename this to columnOffset

               if (reader.MoveToNextRow(tableStart))
                  headers = reader.CurrentRow;

               for (var j = 0; j < headers.Count; j++)
                  dataSheet.RawData.AddColumn(headers[j], j);

               while (reader.MoveToNextRow(tableStart))
               {
                  var rowToAdd = reader.CurrentRow; //OK, so this error here is really weird
                  dataSheet.RawData.AddRow(rowToAdd); 
               }

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
