using System;

namespace OSPSuite.Presentation.Importer.Core
{
   /**
    * Helper class to keep track of the id of the data stored
    * and the column index it refers to
    */
   public class MetaDataMappingConverter
   {
      public string Id { get; set; }

      public Func<string, int> Index { get; set; }
   }
}
