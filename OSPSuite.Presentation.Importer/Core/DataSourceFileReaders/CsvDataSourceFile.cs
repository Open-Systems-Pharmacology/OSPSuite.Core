using System;
using System.Collections.Generic;
using System.Linq;
using OSPSuite.Core.Services;
using OSPSuite.Infrastructure.Import.Services;


namespace OSPSuite.Presentation.Importer.Core.DataSourceFileReaders
{
   public interface ICsvDataSourceFile : IDataSourceFile { }

   public class CsvDataSourceFile : DataSourceFile, ICsvDataSourceFile
   {
      public CsvDataSourceFile(IImportLogger logger) : base(logger) { }
      override protected Dictionary<string, IDataSheet> LoadFromFile(string path)
      {
         try
         {
            using (var reader = new CsvReaderDisposer(path))
            {
               var csv = reader.Csv;
               var headers = csv.GetFieldHeaders();
               var rows = new List<List<string>>(headers.Length);

               IDataSheet dataSheet = new DataSheet();
               dataSheet.RawData = new UnformattedData();

               for (var i = 0; i < headers.Length; i++)
                  dataSheet.RawData.AddColumn(headers[i], i);
               string[] currentRow = new string[csv.FieldCount];

               while (csv.ReadNextRecord())
               {
                  csv.CopyCurrentRecordTo(currentRow);
                  var levels = getMeasurementLevels(currentRow.ToList());
                  dataSheet.RawData.CalculateColumnDescription(levels);
                  dataSheet.RawData.AddRow(currentRow.ToList());
               }

               var loadedData = new Dictionary<string, IDataSheet>();
               loadedData.Add("", dataSheet);
               return loadedData;
            }
         }
         catch (Exception e)
         {
            logger.AddError(e.ToString());
            return null;
         }
      }

      private List<ColumnDescription.MeasurementLevel> getMeasurementLevels(List<string> dataList)
      {
         var resultList = new List<ColumnDescription.MeasurementLevel>();

         foreach (var item in dataList)
         {
            if (Double.TryParse(item, out var doubleValue))
               resultList.Add(ColumnDescription.MeasurementLevel.NUMERIC);
            else
               resultList.Add(ColumnDescription.MeasurementLevel.DISCRETE);

         }
         return resultList;
      }
   }
}
