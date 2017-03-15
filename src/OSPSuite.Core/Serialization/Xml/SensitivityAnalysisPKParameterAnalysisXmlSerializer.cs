using OSPSuite.Core.Chart.SensitivityAnalyses;

namespace OSPSuite.Core.Serialization.Xml
{
   public class SensitivityAnalysisPKParameterAnalysisXmlSerializer : ObjectBaseXmlSerializer<SensitivityAnalysisPKParameterAnalysis>
   {
      public override void PerformMapping()
      {
         base.PerformMapping();
         Map(x => x.PKParameterName);
         Map(x => x.OutputPath);
      }
   }
}