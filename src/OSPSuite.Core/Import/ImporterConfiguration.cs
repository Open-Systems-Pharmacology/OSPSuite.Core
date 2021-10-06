using OSPSuite.Core.Domain;
using System.Collections.Generic;
using System.Linq;

namespace OSPSuite.Core.Import
{
   public class ImporterConfiguration : IWithId
   {
      private List<DataFormatParameter> _parameters = new List<DataFormatParameter>();

      private HashSet<string> _loadedSheets = new HashSet<string>();
      public List<DataFormatParameter> Parameters { 
         get => _parameters;
      }
      public void CloneParametersFrom(IReadOnlyList<DataFormatParameter> parameters)
      {
         _parameters = new List<DataFormatParameter>(parameters);
      }
      public void AddParameter(DataFormatParameter parameter) { _parameters.Add(parameter); }
      public List<string> LoadedSheets { get => _loadedSheets.ToList(); }
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
