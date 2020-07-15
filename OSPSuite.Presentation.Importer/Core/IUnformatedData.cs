using System;
using System.Collections.Generic;

namespace OSPSuite.Presentation.Importer.Core
{
   public class ColumnDescription
   {
      public enum MeasurmentLevel
      {
         DISCRETE,
         NUMERIC
      }

      public string Name { get; set; }

      public MeasurmentLevel Level { get; set; }

      public List<string> Values { get; set; }
   }

   public interface IUnformatedData
   {
      //UnformatedData
      IList<string> GetRow(int index);
      IList<string> GetColumn(int index);
      IList<string> GetColumn(string columnName);

      Dictionary<string, ColumnDescription> Headers { get; set; }

      IList<IList<string>> GetRows(Func<IList<string>, bool> filter);


      void AddRow(IList<string> row);
      void AddColumn(IList<string> row, string columnName);

   }

   public class UnformatedData : IUnformatedData
   {
      private List<List<string>> rawData;

      public Dictionary<string, ColumnDescription> Headers { get; set; }

      //private Dictionary<string, ColumnDescription> headers; //we have to ensure headers and RawData sizes match
      //unparsed name   //datatype, List<strings> values, 

      public void AddColumn(IList<string> row, string columnName)
      {
         throw new NotImplementedException();
      }

      public void AddRow(IList<string> row)
      {
         //if (headers.Count != 0) //do not add Data to a DataSheet without column names
            

         throw new NotImplementedException();
      }

      public IList<string> GetColumn(int index)
      {
         throw new NotImplementedException();
      }

      public IList<string> GetColumn(string columnName)
      {
         throw new NotImplementedException();
      }

      public IList<string> GetRow(int index)
      {
         throw new NotImplementedException();
      }

      public IList<IList<string>> GetRows(Func<IList<string>, bool> filter)
      {
         throw new NotImplementedException();
      }
   }
}
