using OSPSuite.Core.Domain;

namespace OSPSuite.Core.Serialization.Xml
{
   public abstract class SimulationQuantitySelectionXmlSerializer<T> : OSPSuiteXmlSerializer<T> where T: SimulationQuantitySelection
   {
      public override void PerformMapping()
      {
         Map(x => x.QuantitySelection);
         MapReference(x => x.Simulation);
      }
   }

   public class SimulationQuantitySelectionXmlSerializer : SimulationQuantitySelectionXmlSerializer<SimulationQuantitySelection>
   {
      
   }

   public class ParameterSelectionXmlSerializer : SimulationQuantitySelectionXmlSerializer<ParameterSelection>
   {
      
   }
}