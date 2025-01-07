using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using OSPSuite.Core.Services;
using OSPSuite.Infrastructure.Import.Services;

namespace OSPSuite.Infrastructure.Import.Core.DataSourceFileReaders
{
   public interface ICsvDataSourceFile : IDataSourceFile
   {
   }

   public class CsvDataSourceFile : DataSourceFile, ICsvDataSourceFile
   {
      private readonly ICsvSeparatorSelector _csvSeparatorSelector;

      public CsvDataSourceFile(IImportLogger logger, ICsvSeparatorSelector csvSeparatorSelector) : base(logger)
      {
         _csvSeparatorSelector = csvSeparatorSelector;
      }

      protected override void LoadFromFile(string path)
      {
         var csvSeparators = _csvSeparatorSelector.GetCsvSeparator(path);

         //if separator selection dialog was cancelled, abort
         if (csvSeparators == null)
            return;

         //we keep a copy of the already loaded sheets, in case the reading fails
         var alreadyLoadedDataSheets = DataSheets.Clone();
         DataSheets.Clear();

         try
         {
            using (var reader = new CsvReaderDisposer(path, csvSeparators.ColumnSeparator))
            {
               var csv = reader.Csv;
               var headers = csv.GetFieldHeaders();

               var dataSheet = new DataSheet
               {
                  SheetName = ""
               };

               for (var i = 0; i < headers.Length; i++)
                  dataSheet.AddColumn(headers[i], i);
               var currentRow = new string[csv.FieldCount];

               while (csv.ReadNextRecord())
               {
                  csv.CopyCurrentRecordTo(currentRow);
                  var currentCultureDecimalSeparator = Convert.ToChar(Thread.CurrentThread.CurrentCulture.NumberFormat.NumberDecimalSeparator);
                  var rowList = currentRow.Select(x => x.Replace(csvSeparators.DecimalSeparator, currentCultureDecimalSeparator)).ToList();
                  var levels = getMeasurementLevels(rowList);
                  dataSheet.CalculateColumnDescription(levels);
                  dataSheet.AddRow(rowList);
               }

               dataSheet.RemoveEmptyColumns();
               dataSheet.RemoveEmptyRowsAtTheEnd();

               DataSheets.AddSheet(dataSheet);
            }

            //if the file was empty
            if (DataSheets.GetDataSheetNames().Count == 0)
               throw new ImporterEmptyFileException();
         }
         catch (Exception e)
         {
            DataSheets.CopySheetsFrom(alreadyLoadedDataSheets);
            _logger.AddError(e.Message);
            throw new InvalidObservedDataFileException(e.Message);
         }
      }

      private List<ColumnDescription.MeasurementLevel> getMeasurementLevels(List<string> dataList)
      {
         var resultList = new List<ColumnDescription.MeasurementLevel>();

         foreach (var item in dataList)
         {
            if (double.TryParse(item, out _))
               resultList.Add(ColumnDescription.MeasurementLevel.Numeric);
            else
               resultList.Add(ColumnDescription.MeasurementLevel.Discrete);
         }

         return resultList;
      }
   }
}