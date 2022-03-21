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
      protected override void LoadFromFile(string path)
      {
         var separator = _csvSeparatorSelector.GetCsvSeparator(path);

         //if separator selection dialog was cancelled, abort
         if (!(separator is char separatorCharacter)) return;

         DataSheets.Initialize(); //first we clear the sheet collection, in case there were some sheets left from previously loading

         try
         {
            using (var reader = new CsvReaderDisposer(path, separatorCharacter))
            {
               var csv = reader.Csv;
               var headers = csv.GetFieldHeaders();
               var rows = new List<List<string>>(headers.Length);

               var dataSheet = new UnformattedSheetData();

               for (var i = 0; i < headers.Length; i++)
                  dataSheet.AddColumn(headers[i], i);
               var currentRow = new string[csv.FieldCount];

               while (csv.ReadNextRecord())
               {
                  csv.CopyCurrentRecordTo(currentRow);
                  var rowList = currentRow.ToList();
                  var levels = getMeasurementLevels(rowList);
                  dataSheet.CalculateColumnDescription(levels);
                  dataSheet.AddRow(rowList);
               }

               dataSheet.RemoveEmptyColumns();
               dataSheet.RemoveEmptyRows();

               DataSheets.AddSheet("", dataSheet);
            }
         }
         catch (Exception e)
         {
            DataSheets.ResetPreviousDataSheets();
            _logger.AddError(e.Message);
            throw new InvalidObservedDataFileException(e.Message);
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
