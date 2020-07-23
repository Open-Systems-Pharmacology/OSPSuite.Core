using System.Collections.Generic;

namespace OSPSuite.Presentation.Importer.Core
{
   public class Column
   {
      public enum ColumnNames
      {
         Time,
         Measurement,
         Error
      }

      public ColumnNames Name { get; set; }
      public string Unit { get; set; }
   }

   /// <summary>
   ///    e.g. a sheet in an excel file
   /// </summary>
   public interface IDataSheet
   {
      UnformattedData RawData { get; set; }
      IDataFormat Format { get; set; }

      IList<IDataFormat> AvailableFormats { get; set; }
   }

   public class DataSheet : IDataSheet
   {
      public IDataFormat Format { get; set; }

      public IList<IDataFormat> AvailableFormats { get; set; }

      public UnformattedData RawData { get; set; }
   }
}