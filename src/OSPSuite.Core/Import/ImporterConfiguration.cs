using OSPSuite.Core.Domain;
using System.Collections.Generic;

namespace OSPSuite.Core.Import
{
   public class ImporterConfiguration : IWithId
   {
      public List<DataFormatParameter> Parameters { get; set; } = new List<DataFormatParameter>();
      public List<string> LoadedSheets { get; set; } = new List<string>();
      public string FileName { get; set; }
      public string NamingConventions { get; set; }
      public string FilterString { get; set; }
      public NanSettings NanSettings { get; set; }
      public string Id { get; set; }
   }
}
