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
            IDataSheet dataSheet = new DataSheet();
            dataSheet.RawData = new UnformattedData();


            var reader = new ExcelReader(path);

            do
            {
               var sheetName = reader.CurrentSheet.SheetName;
               var headers = new List<string>();

               var tableStart = reader.DetermineFirstColumn(); //rename this to columnOffset

               if (reader.MoveToNextRow(tableStart))
                  headers = reader.CurrentRow;

               for (var j = 0; j < headers.Count; j++)
                  dataSheet.RawData.AddColumn(headers[j], j);

               while (reader.MoveToNextRow(tableStart))
               {
                  dataSheet.RawData.AddRow(reader.CurrentRow);
               }

               loadedData.Add(sheetName, dataSheet);
            } while (reader.MoveToNextSheet());

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
