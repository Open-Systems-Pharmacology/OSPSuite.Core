using OSPSuite.Serializer;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Serialization.Xml;

namespace OSPSuite.Core.Serialization.Exchange
{
   public class SimulationTransferXmlSerializer : OSPSuiteXmlSerializer<SimulationTransfer>
   {
      public override void PerformMapping()
      {
         Map(x => x.Id);
         Map(x => x.Simulation);
         Map(x => x.ReactionDimensionMode);
         Map(x => x.JournalPath);
         Map(x => x.Favorites);
         MapEnumerable(x => x.AllObservedData, x => x.AllObservedData.Add);
         Map(x => x.PkmlVersion).WithMappingName(Constants.Serialization.Attribute.VERSION);
      }
   }
}