using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using OSPSuite.Core.Domain;

namespace OSPSuite.Infrastructure.Import.Core.DataSourceFileReaders
{
   public class ExcelReader : IDisposable
   {
      private IWorkbook _book;
      private IEnumerator<ISheet> _sheetEnumerator;
      private IEnumerator _rowEnumerator;
      private bool _columnOffsetOn;
      private int _columnOffset;
      private bool _isTypeXlsx;
      private readonly IFormulaEvaluator _formulaEvaluator;

      public ISheet CurrentSheet { get; private set; } 
      public List<string> CurrentRow { get; private set; } = new List<string>();

      public ExcelReader(string path, bool columnOffsetOn = true)
      {
         using (var fs = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
         {
            _book = WorkbookFactory.Create(fs);
         }

         _columnOffsetOn = columnOffsetOn;
         _sheetEnumerator = _book.GetEnumerator();
         _isTypeXlsx = Path.GetExtension(path.ToLower()).Equals(Constants.Filter.XLSX_EXTENSION);
         _formulaEvaluator = _book.GetCreationHelper().CreateFormulaEvaluator();
      }

      public ExcelReader(bool columnOffsetOn = true)
      {
         _columnOffsetOn = columnOffsetOn;
      }

      public void LoadNewWorkbook(string path, bool columnOffsetOn = true)
      {
         using (var fs = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
         {
            _book = WorkbookFactory.Create(fs);
         }

         _sheetEnumerator = _book.GetEnumerator();
         _columnOffsetOn = columnOffsetOn;
         _isTypeXlsx = Path.GetExtension(path.ToLower()).Equals(Constants.Filter.XLSX_EXTENSION);
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

            CurrentRow.Add(cell == null ? "" : getCellStringValue(cell));
         }

         return true;
      }

      public IEnumerable<string> RetrieveExcelSheets(bool excludeEmptySheets)
      {
         for (var i = 0; i < _book.NumberOfSheets; i++)
         {
            if (!excludeEmptySheets || !isSheetEmpty(_book.GetSheetAt(i)))
               yield return _book.GetSheetName(i);
         }
      }

      private bool isSheetEmpty(ISheet sheet)
      {

         return sheet.LastRowNum == -1 || sheet.GetRow(sheet.LastRowNum)?.GetCell(0) == null;
      }

      private string getCellStringValue(ICell cell)
      {

         var cellValue = _formulaEvaluator.Evaluate(cell);

         if (cellValue == null) return "";

         switch (cellValue.CellType)
         {
            case CellType.Boolean:
               return cellValue.BooleanValue.ToString();
            case CellType.Numeric:
               return cellValue.NumberValue.ToString();
            case CellType.String:
               return cellValue.StringValue;
         }

         return "";
      }

      private bool isNumeric(ICell cell)
      {
         if (cell.CellType == CellType.Numeric)
            return true;
         
         var value = cell.ToString().TrimStart();
         if (value.IndexOf('<') == 0)
            value = value.Substring(1);

         return double.TryParse(value, out _);
      }

      public List<ColumnDescription.MeasurementLevel> GetMeasurementLevels(int rowLength) //IMPORTANT: should be called after headers have been read!!!!
      {
         var resultList = new List<ColumnDescription.MeasurementLevel>();
         var currentExcelRow = getCurrentExcelRow(_rowEnumerator);

         for (var i = _columnOffset; i < rowLength + _columnOffset; i++)
         {
            var cell = currentExcelRow.GetCell(i);

            if (cell == null || cell.CellType == CellType.Blank)
               resultList.Add(ColumnDescription.MeasurementLevel.NotSet);
            else if (isNumeric(cell))
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
         if (_isTypeXlsx)
         {
            return (XSSFRow) enumerator.Current;
         }
         
         return (HSSFRow) enumerator.Current;
      }

      public void Dispose()
      {
         _sheetEnumerator?.Dispose();
      }
   }
}