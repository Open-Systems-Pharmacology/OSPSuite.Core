using OSPSuite.Assets;
using OSPSuite.Core.Domain;
using OSPSuite.Utility.Validation;

namespace OSPSuite.Core.Qualification
{
   public class QualifcationConfiguration : IValidatable
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

      public SimulationPlot[] SimulationPlots { get; set; }

      public Input[] Inputs { get; set; }

      public BuildingBlockSwap[] BuildingBlocks { get; set; }

      public SimulationParameterSwap[] SimulationParameters { get; set; }

      public IBusinessRuleSet Rules { get; } = new BusinessRuleSet();

      public QualifcationConfiguration()
      {
         Rules.AddRange(new[]
         {
            GenericRules.FileExists<QualifcationConfiguration>(x => x.SnapshotFile),
            GenericRules.NonEmptyRule<QualifcationConfiguration>(x => x.OutputFolder, Error.QualificationOutputFolderNotDefined),
            GenericRules.NonEmptyRule<QualifcationConfiguration>(x => x.MappingFile, Error.QualificationMappingFileNotDefined),
            GenericRules.NonEmptyRule<QualifcationConfiguration>(x => x.ReportConfigurationFile, Error.QualificationReportConfigurationFileNotDefined),
            GenericRules.NonEmptyRule<QualifcationConfiguration>(x => x.ObservedDataFolder, Error.QualificationObservedDataFolderNotDefined)
         });
      }
   }
}