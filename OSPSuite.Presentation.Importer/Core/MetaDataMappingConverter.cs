using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OSPSuite.Presentation.Importer.Core
{
   public class MetaDataMappingConverter
   {
      public string Id { get; set; }

      public Func<string, int> Index { get; set; }
   }
}
