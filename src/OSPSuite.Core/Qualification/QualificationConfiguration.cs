using OSPSuite.Assets;
using OSPSuite.Core.Domain;
using OSPSuite.Utility.Validation;

namespace OSPSuite.Core.Qualification
{
   public class QualificationConfiguration : IValidatable
   {
      /// <summary>
      /// Typically name of the project as referenced in the configuration fie
      /// </summary>
      public string Project { get; set; }

      /// <summary>
      ///    Path of project snapshot file used for this qualification run
      /// </summary>
      public string SnapshotFile { get; set; }

      /// <summary>
      ///    Output folder where project artifacts will be exported. It will be created if it does not exist
      /// </summary>
      public string OutputFolder { get; set; }

      /// <summary>
      ///    Folder were observed data will be exported
      /// </summary>
      public string ObservedDataFolder { get; set; }

      /// <summary>
      ///    Folder were input data will be exported
      /// </summary>
      public string InputsFolder { get; set; }

      /// <summary>
      ///    Path of mapping file that will be created for the project.
      /// </summary>
      public string MappingFile { get; set; }

      /// <summary>
      ///    Temp folder where potential artifacts can be exported
      /// </summary>
      public string TempFolder { get; set; }

      /// <summary>
      ///    Path of configuration file that will be created as part of the qualification run
      /// </summary>
      public string ReportConfigurationFile { get; set; }

      /// <summary>
      /// List of simulation plots (in PK-Sim typically) to export automatically
      /// </summary>
      public SimulationPlot[] SimulationPlots { get; set; }

      public Input[] Inputs { get; set; }

      public BuildingBlockSwap[] BuildingBlocks { get; set; }

      public SimulationParameterSwap[] SimulationParameters { get; set; }

      /// <summary>
      /// List of simulation names to export. Of this array is empty or null, no simulation will be exported.
      /// It is an error to have a SimulationPlots defined for a simulation not listed in this array.
      /// The simulations must be defined in the project or will be ignored
      /// </summary>
      public string[] Simulations { get; set; }

      public IBusinessRuleSet Rules { get; } = new BusinessRuleSet();

      public ApplicationType Application { get; set; } = ApplicationType.PKSim;

      public QualificationConfiguration()
      {
         Rules.AddRange(new[]
         {
            GenericRules.FileExists<QualificationConfiguration>(x => x.SnapshotFile),
            GenericRules.NonEmptyRule<QualificationConfiguration>(x => x.OutputFolder, Error.QualificationOutputFolderNotDefined),
            GenericRules.NonEmptyRule<QualificationConfiguration>(x => x.MappingFile, Error.QualificationMappingFileNotDefined),
            GenericRules.NonEmptyRule<QualificationConfiguration>(x => x.ReportConfigurationFile, Error.QualificationReportConfigurationFileNotDefined),
            GenericRules.NonEmptyRule<QualificationConfiguration>(x => x.ObservedDataFolder, Error.QualificationObservedDataFolderNotDefined)
         });
      }
   }
   public enum ApplicationType
   {
      PKSim,
      MoBi
   }
}