using System;
using System.Collections.Generic;
using OSPSuite.Core.Commands;

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
   /// e.g. a sheet in an excel file
   /// </summary>
   public interface IDataSheet
   {
      UnformattedData RawData { get; set; }
      IDataFormat Format { get; set; }
   }

   public class DataSheet : IDataSheet
   {
      public IDataFormat Format
      {
         get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException();
      }
      public UnformattedData RawData { get; set; }
   }
}
