using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using OSPSuite.Presentation.Importer.Core;

namespace OSPSuite.Presentation.Importer.Infrastructure
{
   public class ExcelReader //or maybe rename it to excelReaderExtensions - or make it into extensions
   {
      //we could get this info here or in getCurrentRow, but make sure we do it once
      private IWorkbook _book;
      private IEnumerator<ISheet> _sheetEnumerator;
      private IEnumerator _rowEnumerator;
      private bool _columnOffsetOn;
      private int _columnOffset;
      private bool _isTypeXlsx;

      public ExcelReader(string path, bool columnOffsetOn = true)
      {
         using (var fs = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
         {
            _book = WorkbookFactory.Create(fs);
         }

         _columnOffsetOn = columnOffsetOn;
         _sheetEnumerator = _book.GetEnumerator();
         _isTypeXlsx = Path.GetExtension(path).Equals(".xlsx");
      }

      public ExcelReader(bool columnOffsetOn = true)
      {
         _columnOffsetOn = columnOffsetOn;
      }

      public ISheet CurrentSheet { get; set; } //why is this public even????
      public List<string> CurrentRow { get; set; } = new List<string>();

      public void LoadNewWorkbook(string path, bool columnOffsetOn = true)
      {
         using (var fs = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
         {
            _book = WorkbookFactory.Create(fs);
         }

         _sheetEnumerator = _book.GetEnumerator();
         _columnOffsetOn = columnOffsetOn;
         _isTypeXlsx = Path.GetExtension(path).Equals(".xlsx");
      }

      public bool MoveToNextSheet()
      {
         if (!_sheetEnumerator.MoveNext())
            return false;

         CurrentSheet = _sheetEnumerator.Current;

         _rowEnumerator = CurrentSheet?.GetEnumerator();
         determineFirstColumn();

         return true;
      }

      public bool MoveToNextRow()
      {
         if (!_rowEnumerator.MoveNext())
            return false;

         CurrentRow = new List<string>();

         var currentExcelRow = getCurrentExcelRow(_rowEnumerator);

         for (var i = _columnOffset; i < currentExcelRow.LastCellNum; i++)
         {
            var cell = currentExcelRow.GetCell(i);

            CurrentRow.Add(cell == null ? "" : cell.ToString());
         }

         return true;
      }

      private bool isNumeric(ICell cell) //TODO write tests for the LLOQ
      {
         if (cell.CellType == CellType.Numeric)
            return true;
         
         var value = cell.ToString().TrimStart();
         if (value.IndexOf('<') == 0)
            value = value.Substring(1);

         return double.TryParse(value, out _);
      }

      public List<ColumnDescription.MeasurementLevel> GetMeasurementLevels() //IMPORTANT: should be called after headers have been read!!!!
      {
         var resultList = new List<ColumnDescription.MeasurementLevel>();
         var currentExcelRow = getCurrentExcelRow(_rowEnumerator);

         for (var i = _columnOffset; i < currentExcelRow.LastCellNum; i++)
         {
            var cell = currentExcelRow.GetCell(i);

            if (cell == null)
               resultList.Add(ColumnDescription.MeasurementLevel.NotSet);
            else if (isNumeric(cell)) //TODO < and the rest numeric in the case of LLOQ
               resultList.Add(ColumnDescription.MeasurementLevel.Numeric);
            else
               resultList.Add(ColumnDescription.MeasurementLevel.Discrete);
         }

         return resultList;
      }

      private void determineFirstColumn()
      {
         if (!_columnOffsetOn)
         {
            _columnOffset = 0;
            return;
         }

         var excelRows = CurrentSheet.GetRowEnumerator();

         if (!excelRows.MoveNext())
         {
            _columnOffset = 0;
            return;
         }

         var currentExcelRow = getCurrentExcelRow(excelRows);

         var tableStart = 0;

         for (var i = 0; i < currentExcelRow.LastCellNum; i++)
         {
            var cell = currentExcelRow.GetCell(i);

            if (cell == null)
               tableStart++;
            else
               break;
         }

         _columnOffset = tableStart;
      }

      private IRow getCurrentExcelRow(IEnumerator enumerator)
      {
         IRow row;
         if (_isTypeXlsx)
         {
            row = (XSSFRow) enumerator.Current;
         }
         else
         {
            row = (HSSFRow) enumerator.Current;
         }

         return row;
      }
   }
}