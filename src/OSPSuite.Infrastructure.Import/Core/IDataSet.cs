﻿using System.Collections.Generic;
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
      IReadOnlyList<ParsedDataSet> Data { get; }
      void ClearNan(double indicator);
      void ThrowsOnNan(double indicator);
      void AddData(IEnumerable<ParsedDataSet> range);
   }

   public class DataSet : IDataSet
   {
      private List<ParsedDataSet> _data { get; } = new List<ParsedDataSet>();

      public IReadOnlyList<ParsedDataSet> Data { get => _data; }

      public void AddData(IEnumerable<ParsedDataSet> range)
      {
         _data.AddRange(range);
      }

      public void ThrowsOnNan(double indicator)
      {
         if (_data.Any(dataSet => dataSet.Data.Any(pair => pair.Key.ColumnInfo.IsMandatory && pair.Value.Any(point => point.Value == indicator || double.IsNaN(point.Value)))))
            throw new NanException();
      }

      public void ClearNan(double indicator)
      {
         foreach (var dataSet in _data)
         {
            var mainColumns = dataSet.Data.Where(pair => pair.Key.ColumnInfo.IsMandatory).Select(pair => pair.Value).ToList();
            foreach (var column in mainColumns)
            {
               var elements = column.Where(p => p.Value == indicator || double.IsNaN(p.Value));
               removeRowsContainingElements(elements.ToList(), column, dataSet);
            }
         }
      }

      private void removeRowsContainingElements(IList<SimulationPoint> elements, IList<SimulationPoint> column, ParsedDataSet dataSet)
      {
         foreach (var element in elements)
         {
            var index = column.IndexOf(element);
            foreach (var value in dataSet.Data.Values)
            {
               value.RemoveAt(index);
            }
         }
      }
   }

}