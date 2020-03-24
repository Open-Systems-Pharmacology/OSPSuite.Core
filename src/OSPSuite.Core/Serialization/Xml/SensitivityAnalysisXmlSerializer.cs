using OSPSuite.Core.Domain.SensitivityAnalyses;

namespace OSPSuite.Core.Serialization.Xml
{
   public class SensitivityAnalysisXmlSerializer : ObjectBaseXmlSerializer<SensitivityAnalysis>
   {
      public override void PerformMapping()
      {
         base.PerformMapping();
         Map(x => x.Results);
         MapReference(x => x.Simulation);
         MapEnumerable(x => x.AllSensitivityParameters, x => x.AddSensitivityParameter);
         MapEnumerable(x => x.Analyses, x => x.AddAnalysis);
      }
   }
}