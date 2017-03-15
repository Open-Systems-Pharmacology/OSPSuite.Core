using OSPSuite.Core.Domain.SensitivityAnalyses;

namespace OSPSuite.Core.Domain
{
   public class ClassifiableSensitivityAnalysis : Classifiable<SensitivityAnalysis>
   {
      public ClassifiableSensitivityAnalysis() : base(ClassificationType.SensitiviyAnalysis)
      {
      }

      public SensitivityAnalysis SensitivityAnalysis => Subject;
   }
}