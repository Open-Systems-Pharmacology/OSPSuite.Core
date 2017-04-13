using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using OSPSuite.Utility.Extensions;
using SmartXLS;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Data;
using DataColumn = OSPSuite.Core.Domain.Data.DataColumn;

namespace OSPSuite.Core
{
   internal static class DataColumnLoader
   {
      public static DataColumn GetDataColumnFrom(string excelFileNameWithoutExtension)
      {
         var fileName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, $"Data\\{excelFileNameWithoutExtension}.xls");
         DataTable dataTable;
         using (var workBook = new WorkBook())
         {
            workBook.read(fileName);
            dataTable = workBook.ExportDataTable(0, 0, workBook.LastRow + 1, workBook.LastCol + 1);
         }

         var timeValues = new List<float>();
         var concValues = new List<float>();

         foreach (DataRow row in dataTable.Rows)
         {
            timeValues.Add(row[0].ConvertedTo<float>());
            concValues.Add(row[1].ConvertedTo<float>());
         }


         var baseGrid = new BaseGrid("Time", "Time", Constants.Dimension.NO_DIMENSION) {Values = timeValues.ToArray()};

         return new DataColumn("Value", excelFileNameWithoutExtension, Constants.Dimension.NO_DIMENSION, baseGrid) {Values = concValues.ToArray()};
      }
   }
}