using System.Collections;
using System.Collections.Generic;
using System.Data;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System.IO;
using NPOI.HSSF.UserModel;
using NPOI.OpenXmlFormats.Dml;
using OSPSuite.Core.Importer.Mappers;
using OSPSuite.Presentation.Importer.Core;

namespace OSPSuite.Presentation.Importer.Infrastructure
{
   public class ExcelReader //or maybe rename it to excelReaderExtensions - or make it into extensions
   {   //we could get this info here or in getCurrentRow, but make sure we do it once
      private IWorkbook book;
      private IEnumerator<ISheet> sheetEnumerator;
      private IEnumerator rowEnumerator;
      private bool columnOffsetOn;
      private int columnOffset = 0;

      public ExcelReader(string path, bool ColumnOffsetOn = true )
      {
         using (FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
         {
            book = WorkbookFactory.Create(fs);
         }

         columnOffsetOn = ColumnOffsetOn;

         sheetEnumerator = book.GetEnumerator();
      }
      public ExcelReader(bool ColumnOffsetOn = true)
      {
         columnOffsetOn = ColumnOffsetOn;
      }
      public ISheet CurrentSheet { get; set; } //why is this public even????
      public List<string> CurrentRow { get; set; } = new List<string>();

      public void LoadNewWorkbook(string path, bool ColumnOffsetOn = true)
      {
         using (FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
         {
            book = WorkbookFactory.Create(fs);
         }
         sheetEnumerator = book.GetEnumerator();
         columnOffsetOn = ColumnOffsetOn;
      }

      public bool MoveToNextSheet()
      {
         if (!sheetEnumerator.MoveNext())
            return false;

         CurrentSheet = sheetEnumerator.Current;
         rowEnumerator = CurrentSheet.GetEnumerator();
         determineFirstColumn();

         return true;
      }

      public bool MoveToNextRow()
      {
         if (!rowEnumerator.MoveNext())
            return false;

         CurrentRow = new List<string>();

         var currentExcelRow = getCurrentExcelRow(rowEnumerator);

         for (int i = columnOffset; i < currentExcelRow.LastCellNum; i++)
         {
            ICell cell = currentExcelRow.GetCell(i);

            if (cell != null)
               CurrentRow.Add(cell.ToString());
            else
               CurrentRow.Add(""); //we allow empty values for the headers in this variation - but if there are throw exception somewhere "fail with no proper format"
         }
         return true;
      }

      //should this here know if MeasurementLevel???or should it actually read just the Numeric or not
      public List<ColumnDescription.MeasurementLevel> GetMeasurementLevels() //IMPORTANT: should be called after headers have been read!!!!
      {
         var resultList = new List<ColumnDescription.MeasurementLevel>();
         var currentExcelRow = getCurrentExcelRow(rowEnumerator);

         for (int i = columnOffset; i < currentExcelRow.LastCellNum; i++)
         {
            ICell cell = currentExcelRow.GetCell(i);

            if (cell.CellType == CellType.Numeric)
               resultList.Add(ColumnDescription.MeasurementLevel.NUMERIC);
            else
               resultList.Add(ColumnDescription.MeasurementLevel.DISCRETE);
         }
         return resultList;
      }

      private void determineFirstColumn()
      {
         if (!columnOffsetOn)
         {
            columnOffset = 0;
            return;
         }

         System.Collections.IEnumerator excelRows = CurrentSheet.GetRowEnumerator();
         excelRows.MoveNext();

         var currentExcelRow = getCurrentExcelRow(excelRows);

         int tableStart = 0;

         for (int j = 0; j < currentExcelRow.LastCellNum; j++)
         {
            ICell cell = currentExcelRow.GetCell(j);

            if (cell == null)
               tableStart++;
            else
               break;
         }
         columnOffset = tableStart;
      }

      private IRow getCurrentExcelRow(IEnumerator enumerator)
      {
         IRow row; //we do this every time we try to read a row - we have to actually store the info at the beginning
         try //discuss with Abdel: better a try-catch block, or should we actually do an if-esle and check the extension?
         {
            row = (XSSFRow)enumerator.Current;
         }
         catch
         {
            row = (HSSFRow)enumerator.Current;
         }
         return row;
      }
   }
}