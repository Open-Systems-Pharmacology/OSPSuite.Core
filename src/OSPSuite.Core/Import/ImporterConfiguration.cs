using OSPSuite.Core.Domain;
using System.Collections.Generic;

namespace OSPSuite.Core.Import
{
   public class ImporterConfiguration : IWithId
   {
      private List<DataFormatParameter> _parameters = new List<DataFormatParameter>();
      public List<DataFormatParameter> Parameters { 
         get => _parameters;
      }
      public void CloneParametersFrom(IReadOnlyList<DataFormatParameter> parameters)
      {
         _parameters = new List<DataFormatParameter>(parameters);
      }
      public void AddParameter(DataFormatParameter parameter) { _parameters.Add(parameter); }
      public List<string> LoadedSheets { get; } = new List<string>();
      public string FileName { get; set; }
      public string NamingConventions { get; set; }
      public string FilterString { get; set; }
      public NanSettings NanSettings { get; set; }
      public string Id { get; set; }
   }
}
