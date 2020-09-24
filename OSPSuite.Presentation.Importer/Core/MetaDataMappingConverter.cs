using System;

namespace OSPSuite.Presentation.Importer.Core
{
   public class MetaDataMappingConverter
   {
      public string Id { get; set; }

      public Func<string, int> Index { get; set; }
   }
}
