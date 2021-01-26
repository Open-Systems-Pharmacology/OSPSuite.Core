using System;
using System.Collections.Generic;
using System.Linq;

namespace OSPSuite.Infrastructure.Import.Core
{
   /// <summary>
   ///    e.g. a sheet in an excel file
   /// </summary>
   public interface IDataSheet
   {
      UnformattedData RawData { get; set; }
   }

   public class DataSheet : IDataSheet
   {
      public UnformattedData RawData { get; set; }
   }
}