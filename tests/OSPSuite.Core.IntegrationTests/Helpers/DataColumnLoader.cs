using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Data;
using System;
using System.Collections.Generic;
using System.IO;

namespace OSPSuite.Core.Helpers
{
   internal static class DataColumnLoader
   {
      public static DataColumn GetDataColumnFrom(string csvName)
      {
         var fileName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, $"Data\\{csvName}.csv");
         var timeValues = new List<float>();
         var concValues = new List<float>();
         foreach (string line in File.ReadLines(fileName))
         {
            var cells = line.Split(';');
            timeValues.Add(float.Parse(cells[0]));
            concValues.Add(float.Parse(cells[1]));
         }

         var baseGrid = new BaseGrid(
            "Time", 
            "Time", 
            Constants.Dimension.NO_DIMENSION
         ) { 
            Values = timeValues.ToArray() 
         };

         return
            new DataColumn(
               "Value",
               csvName, 
               Constants.Dimension.NO_DIMENSION, 
               baseGrid
            ) { 
               Values = concValues.ToArray() 
            };
      }
   }
}
