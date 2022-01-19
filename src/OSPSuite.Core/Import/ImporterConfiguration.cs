using OSPSuite.Core.Domain;
using System.Collections.Generic;
using System.Linq;

namespace OSPSuite.Core.Import
{
   public class ImporterConfiguration : IWithId
   {
      private readonly HashSet<string> _loadedSheets = new HashSet<string>();
      public List<DataFormatParameter> Parameters { get; private set; } = new List<DataFormatParameter>();

      public void CloneParametersFrom(IReadOnlyList<DataFormatParameter> parameters)
      {
         Parameters = new List<DataFormatParameter>(parameters);
      }
      public void AddParameter(DataFormatParameter parameter) { Parameters.Add(parameter); }
      public List<string> LoadedSheets => _loadedSheets.ToList();
      public string FileName { get; set; }
      public string NamingConventions { get; set; }
      public string FilterString { get; set; }
      public NanSettings NanSettings { get; set; }
      public string Id { get; set; }
      public void AddToLoadedSheets(string sheet) => _loadedSheets.Add(sheet);
      public void RemoveFromLoadedSheets(string sheet) => _loadedSheets.Remove(sheet);
      public void ClearLoadedSheets() => _loadedSheets.Clear();
   }
}
