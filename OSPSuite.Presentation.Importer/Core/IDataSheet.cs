using System;
using System.Collections.Generic;
using OSPSuite.Core.Commands;

namespace OSPSuite.Presentation.Importer.Core
{
   public interface IColumn
   {
      string Name { get; set; }
      string Unit { get; set; }
   }

   public class Column : IColumn
   {
      public string Name 
      {
         get;
         set; 
      }

      public string Unit 
      { 
         get;
         set; 
      }
   }

   /// <summary>
   /// e.g. a sheet in an excel file
   /// </summary>
   public interface IDataSheet
   {
      IUnformatedData RawData { get; set; }
      IDataFormat Format { get; set; }
   }

   public class DataSheet : IDataSheet
   {
      public IDataFormat Format
      {
         get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException();
      }
      public IUnformatedData RawData { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
   }
}
