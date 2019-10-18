using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using OSPSuite.Core.Importer;
using SmartXLS;

namespace OSPSuite.Presentation.Services
{
   static class SmartXLS
   {
      /// <summary>
      ///    This function reads a given excel file into a SmartXLS workbook object.
      /// </summary>
      /// <param name="fileName">Name of the excel file including full path.</param>
      /// <returns>A <see cref="WorkBook" /> object.</returns>
      public static WorkBook ReadExcelFile(string fileName)
      {
         if (String.IsNullOrEmpty(fileName)) return null;
         var wb = new WorkBook();

         var extension = Path.GetExtension(fileName);
         if (extension != null && extension.ToUpper() == ".xlsx".ToUpper())
         {
            using (var fs = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
               wb.readXLSX(fs);
            }
         }
         else
         {
            wb.CSVSeparator = getCSVSeparator(fileName);
            using (var fs = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
               wb.read(fs);
            }
         }
         return wb;
      }

      /// <summary>
      ///    This methods tries to find the used separator of an CSV file.
      /// </summary>
      /// <remarks>The occurance of the delimiter must be a constant over all lines.</remarks>
      private static char getCSVSeparator(string fileName)
      {
         char[] candidates = {';', ',', '\t', '.', ':', ' '};
         var found = 0;

         for (var i = candidates.Length - 1; i >= 0; i--)
         {
            var separator = candidates[i];
            var usable = true;
            using (var fs = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
               var lastcount = 0;
               using (var fsreader = new StreamReader(fs))
               {
                  while (fsreader.Peek() > -1)
                  {
                     var line = fsreader.ReadLine();
                     if (line == null) continue;
                     if (line.Length <= 2) continue;
                     var thiscount = line.ToCharArray().Where(x => x == separator).Count();
                     if (thiscount == 0 || (thiscount != lastcount && lastcount != 0))
                     {
                        usable = false;
                        break;
                     }
                     lastcount = thiscount;
                  }
               }
            }
            if (usable) found = i;
         }

         return candidates[found];
      }

      /// <summary>
      ///    This method retrieves a list of all sheet names of a workbook.
      /// </summary>
      /// <param name="wb">A <see cref="WorkBook" /> object.</param>
      /// <returns>List of strings representing the sheet names of the workbook.</returns>
      public static IList<string> GetSheetNames(WorkBook wb)
      {
         IList<string> sheetNames = new List<string>(wb.NumSheets);
         for (int i = 0; i < wb.NumSheets; i++)
         {
            sheetNames.Add(wb.getSheetName(i));
         }

         return sheetNames;
      }

      /// <summary>
      ///    This method sets the given sheet name as actual sheet in given workbook.
      /// </summary>
      /// <param name="fileName">Name of the excel file.</param>
      /// <param name="wb"><see cref="WorkBook" /> object.</param>
      /// <param name="sheetName">Name of the sheet to be the selected one.</param>
      /// <exception cref="SheetNotFoundException"> is thrown, when sheet not exists in workbook.</exception>
      public static void SelectSheet(string fileName, WorkBook wb, string sheetName)
      {
         var sheet = wb.findSheetByName(sheetName);
         if (sheet < 0)
            throw new SheetNotFoundException(sheetName, fileName);
         wb.Sheet = sheet;
      }

      /// <summary>
      ///    This method reads the specified row and retrieves all column values as a list of strings.
      /// </summary>
      /// <param name="wb">A <see cref="WorkBook" /> object already set to a sheet.</param>
      /// <param name="row"></param>
      /// <returns>List of strings with all values of specified row.</returns>
      public static IList<string> GetRowValues(WorkBook wb, int row)
      {
         IList<string> values = new List<string>();
         for (int i = 0; i <= wb.LastCol; i++)
         {
            values.Add(wb.getText(row, i));
         }

         return values;
      }

      /// <summary>
      ///    Gets the range selecting all filled cells of a sheet.
      /// </summary>
      public static Rectangle DefaultRange(WorkBook wb)
      {
         return new Rectangle(0, 0, wb.LastCol + 1, wb.LastRow + 1);
      }

      /// <summary>
      ///    This function determines whether the given column in a workbook contains date values.
      /// </summary>
      /// <param name="workbook">A SmartXLS workbook object.</param>
      /// <param name="col">Column to check.</param>
      /// <param name="firstRow">First row of the sheet.</param>
      /// <param name="maxRows">Number of rows to check.</param>
      /// <returns><c>True</c>, if column is a date.</returns>
      /// <remarks>
      ///    <para>In excel date columns contain just double values, but they are formatted as dates.</para>
      ///    <para>
      ///       Therefore the format of the range is checked and also the values are checked. If both checks succeed the column
      ///       will be identified as date column.
      ///    </para>
      /// </remarks>
      public static bool IsDateColumn(WorkBook workbook, int col, int firstRow, int maxRows)
      {
         RangeStyle rs = workbook.getRangeStyle(firstRow, col, firstRow + maxRows, col);
         string customFormat = rs.CustomFormat;

         short cellType;
         int row = 0;
         do
            cellType = Math.Abs(workbook.getType(firstRow + row++, col)); while ((cellType == WorkBook.TypeEmpty || cellType == WorkBook.TypeError) && row < maxRows);

         string text = workbook.getFormattedText(firstRow + row, col);
         if (customFormat == "General")
            return false;
         if (!customFormat.Contains("h") && !customFormat.Contains("m") && !customFormat.Contains("s") && !customFormat.Contains("d") && !customFormat.Contains("t") && !customFormat.Contains("y") && !customFormat.Contains("j"))
            return false;

         DateTime dtval;
         return DateTime.TryParse(text, out dtval);
      }
   }

   public static class WorkBookExtensions
   {
      /// <summary>
      ///    Returns the index of the sheet based on the name.
      /// </summary>
      /// <param name="wb">The workbook being searched</param>
      /// <param name="sheetName">the name of the sheet to look for</param>
      /// <returns>-1 if no sheet found, otherwise the index</returns>
      public static int GetIndexFromSheetName(this WorkBook wb, string sheetName)
      {
         for (var i = 0; i < wb.NumSheets; i++)
         {
            if (wb.getSheetName(i).Equals(sheetName))
               return i;
         }
         return -1;
      }

      public static string GetCurrentSheetName(this WorkBook wb)
      {
         return wb.getSheetName(wb.Sheet);
      }
   }
}