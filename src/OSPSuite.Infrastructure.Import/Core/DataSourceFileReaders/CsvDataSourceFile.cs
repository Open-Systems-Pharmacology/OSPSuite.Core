using System;
using System.Collections.Generic;
using System.Linq;
using OSPSuite.Core.Services;
using OSPSuite.Infrastructure.Import.Services;
using OSPSuite.Utility.Collections;

namespace OSPSuite.Infrastructure.Import.Core.DataSourceFileReaders
{
   public interface ICsvDataSourceFile : IDataSourceFile {}

   public class CsvDataSourceFile : DataSourceFile, ICsvDataSourceFile
   {
      private readonly ICsvSeparatorSelector _csvSeparatorSelector;

      public CsvDataSourceFile(IImportLogger logger, ICsvSeparatorSelector csvSeparatorSelector) : base(logger)
      {
         _csvSeparatorSelector = csvSeparatorSelector;
      }
      protected override Cache<string, DataSheet> LoadFromFile(string path)
      {
         try
         {
            var separator = _csvSeparatorSelector.GetCsvSeparator(path);

            //if separator selection dialog was cancelled, abort
            if (!(separator is char separatorCharacter)) return null;
 
            using (var reader = new CsvReaderDisposer(path, separatorCharacter))
            {
               var csv = reader.Csv;
               var headers = csv.GetFieldHeaders();
               var rows = new List<List<string>>(headers.Length);

               var dataSheet = new DataSheet {RawData = new UnformattedData()};

               for (var i = 0; i < headers.Length; i++)
                  dataSheet.RawData.AddColumn(headers[i], i);
               var currentRow = new string[csv.FieldCount];

               while (csv.ReadNextRecord())
               {
                  csv.CopyCurrentRecordTo(currentRow);
                  var rowList = currentRow.ToList();
                  var levels = getMeasurementLevels(rowList);
                  dataSheet.RawData.CalculateColumnDescription(levels);
                  dataSheet.RawData.AddRow(rowList);
               }

               dataSheet.RawData.RemoveEmptyColumns();
               dataSheet.RawData.RemoveEmptyRows();

               var loadedData = new Cache<string, DataSheet>()
               {
                  { "", dataSheet }
               };
               return loadedData;
            }
         }
         catch (Exception e)
         {
            _logger.AddError(e.ToString());
            throw new InvalidFileException();
         }
      }

      private List<ColumnDescription.MeasurementLevel> getMeasurementLevels(List<string> dataList)
      {
         var resultList = new List<ColumnDescription.MeasurementLevel>();

         foreach (var item in dataList)
         {
            if (Double.TryParse(item, out var doubleValue))
               resultList.Add(ColumnDescription.MeasurementLevel.Numeric);
            else
               resultList.Add(ColumnDescription.MeasurementLevel.Discrete);
         }

         return resultList;
      }
   }
}
