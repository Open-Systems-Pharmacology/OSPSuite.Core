using System.IO;
using OSPSuite.Core.Domain;
using OSPSuite.Utility;

namespace OSPSuite.CLI.Core.Services
{
   public class SimulationExportOptions
   {
      private string _logCategory;
      public SimulationExportMode ExportMode { get; set; }
      public string ProjectName { get; set; }
      public string Category { get; set; }
      public bool UseDefaultExportName { get; set; } = true;
      public string OutputFolder { get; set; }
      public bool PrependProjectName { get; set; } = false;

      public string LogCategory
      {
         set => _logCategory = value;
         get => _logCategory ?? ProjectName;
      }

      public string TargetPathFor(ISimulation simulation, string extension) =>
         TargetPathFor(simulation.Name, extension);

      public string TargetPathFor(string fileName, string extension) =>
         pathUnder(fileName, extension);

      public string TargetCSVPathFor(string fileName) => TargetPathFor(fileName, Constants.Filter.CSV_EXTENSION);

      private string pathUnder(string fileName, string extension)
      {
         var fileNameWithPrefix = fileName;
         if (PrependProjectName && !string.IsNullOrEmpty(ProjectName))
            fileNameWithPrefix = $"{ProjectName}-{fileName}";

         return Path.Combine(OutputFolder, $"{FileHelper.RemoveIllegalCharactersFrom(fileNameWithPrefix)}{extension}");
      }
   }
}