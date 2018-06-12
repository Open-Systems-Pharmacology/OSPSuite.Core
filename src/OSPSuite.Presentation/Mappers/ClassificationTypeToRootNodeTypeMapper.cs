using System;
using OSPSuite.Utility;
using OSPSuite.Core.Domain;
using OSPSuite.Presentation.Presenters.Nodes;

namespace OSPSuite.Presentation.Mappers
{
   public interface IClassificationTypeToRootNodeTypeMapper : IMapper<ClassificationType, RootNodeType>
   {
   }

   public class ClassificationTypeToRootNodeTypeMapper : IClassificationTypeToRootNodeTypeMapper
   {
      public RootNodeType MapFrom(ClassificationType classificationType)
      {
         switch (classificationType)
         {
            case ClassificationType.ObservedData:
               return RootNodeTypes.ObservedDataFolder;
            case ClassificationType.Simulation:
               return RootNodeTypes.SimulationFolder;
            case ClassificationType.Comparison:
               return RootNodeTypes.ComparisonFolder;
            case ClassificationType.ParameterIdentification:
               return RootNodeTypes.ParameterIdentificationFolder;
            case ClassificationType.SensitiviyAnalysis:
               return RootNodeTypes.SensitivityAnalysisFolder;
            case ClassificationType.QualificationPlan:
               return RootNodeTypes.QualificationPlanFolder;
            default:
               throw new ArgumentOutOfRangeException(nameof(classificationType));
         }
      }
   }
}