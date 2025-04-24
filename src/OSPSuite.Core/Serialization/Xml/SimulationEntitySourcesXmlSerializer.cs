using OSPSuite.Core.Domain;

namespace OSPSuite.Core.Serialization.Xml
{
   public class SimulationEntitySourcesXmlSerializer : OSPSuiteXmlSerializer<SimulationEntitySources>
   {
      public override void PerformMapping()
      {
         MapEnumerable(x => x.All, x => x.Add);
      }
   }
}