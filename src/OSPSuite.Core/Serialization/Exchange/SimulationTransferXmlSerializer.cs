using OSPSuite.Core.Domain;
using OSPSuite.Core.Serialization.Xml;
using OSPSuite.Serializer;

namespace OSPSuite.Core.Serialization.Exchange
{
   public class SimulationTransferXmlSerializer : OSPSuiteXmlSerializer<SimulationTransfer>
   {
      public override void PerformMapping()
      {
         Map(x => x.Id);
         Map(x => x.Simulation);
         Map(x => x.JournalPath);
         Map(x => x.Favorites);
         Map(x => x.PkmlVersion).WithMappingName(Constants.Serialization.Attribute.VERSION);

         MapEnumerable(x => x.AllObservedData, x => x.AllObservedData.Add);
         //This needs to be done after observed data serialization to ensure that the observed data were deserialized
         Map(x => x.OutputMappings);
      }
   }
}