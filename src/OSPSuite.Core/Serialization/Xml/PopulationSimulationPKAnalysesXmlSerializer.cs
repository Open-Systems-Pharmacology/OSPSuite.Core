using OSPSuite.Core.Domain;

namespace OSPSuite.Core.Serialization.Xml
{
   public class PopulationSimulationPKAnalysesXmlSerializer : OSPSuiteXmlSerializer<PopulationSimulationPKAnalyses>
   {
      public override void PerformMapping()
      {
         MapEnumerable(x => x.All(), x => x.AddPKAnalysis);
      }
   }
}