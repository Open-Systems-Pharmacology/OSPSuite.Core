using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace OSPSuite.Infrastructure.Import.Core
{
   //When parsing the DataSets we  group by metadata and we need to remember the values,
   //so we can use them for the naming conventions. This way an instantiated metadata represents
   //the value of a metadata (Id) on each group
   public class InstantiatedMetaData
   {
      public int Id { get; set; }
      public string Value { get; set; }
   }

   /// <summary>
   /// Data from a single experiment
   /// </summary>
   public interface IDataSet
   {
      IEnumerable<ParsedDataSet> Data { get; set; }
      void ClearNan(double indicator);
      void ThrowsOnNan(double indicator);
   }

   public class DataSet : IDataSet
   {
      public IEnumerable<ParsedDataSet> Data { get; set; } = new List<ParsedDataSet>();

      public void ThrowsOnNan(double indicator)
      {
         if (Data.Any(dataSet => dataSet.Data.Any(pair => pair.Key.ColumnInfo.IsMandatory && pair.Value.Any(point => point.Value == indicator || double.IsNaN(point.Value)))))
            throw new NanException();
      }

      public void ClearNan(double indicator)
      {
         foreach (var dataSet in Data)
         {
            var mainColumns = dataSet.Data.Where(pair => pair.Key.ColumnInfo.IsMandatory).Select(pair => pair.Value).ToList();
            var i = 0;
            while (i < mainColumns.First().Count())
            {
               if (mainColumns.Any(points => points.Any(p => p.Value == indicator || double.IsNaN(p.Value))))
               {
                  foreach (var simulationPoints in dataSet.Data.Values)
                  {
                     simulationPoints.RemoveAt(i);
                  }
               }
               else
               {
                  i++;
               }
            }
         }
      }
   }

}