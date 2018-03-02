using OSPSuite.Assets;
using OSPSuite.Core.Domain;

namespace OSPSuite.Presentation.Presenters.Nodes
{
   public static class RootNodeTypes
   {
      public static readonly RootNodeType SimulationFolder = new RootNodeType(Captions.SimulationFolder, ApplicationIcons.SimulationFolder, ClassificationType.Simulation);
      public static readonly RootNodeType ObservedDataFolder = new RootNodeType(Captions.ObservedDataFolder, ApplicationIcons.ObservedDataFolder, ClassificationType.ObservedData);
      public static readonly RootNodeType ComparisonFolder = new RootNodeType(Captions.ComparisonFolder, ApplicationIcons.ComparisonFolder, ClassificationType.Comparison);
      public static readonly RootNodeType ParameterIdentificationFolder = new RootNodeType(Captions.ParameterIdentificationFolder, ApplicationIcons.ParameterIdentificationFolder, ClassificationType.ParameterIdentification);
      public static readonly RootNodeType SensitivityAnalysisFolder = new RootNodeType(Captions.SensitivityAnalysisFolder, ApplicationIcons.SensitivityAnalysisFolder, ClassificationType.SensitiviyAnalysis);
      public static readonly RootNodeType QualificationPlanFolder = new RootNodeType(Captions.QualificationPlanFolder, ApplicationIcons.Folder, ClassificationType.QualificationPlan);
   }
}