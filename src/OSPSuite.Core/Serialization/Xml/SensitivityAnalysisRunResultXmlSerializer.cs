using OSPSuite.Core.Domain.SensitivityAnalyses;

namespace OSPSuite.Core.Serialization.Xml
{
   public class SensitivityAnalysisRunResultXmlSerializer : OSPSuiteXmlSerializer<SensitivityAnalysisRunResult>
   {
      public override void PerformMapping()
      {
         MapEnumerable(x => x.AllPKParameterSensitivities, x => x.AddPKParameterSensitivity);
      }
   }
}