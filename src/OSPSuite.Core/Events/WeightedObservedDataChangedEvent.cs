using OSPSuite.Core.Domain;

namespace OSPSuite.Core.Events
{
   public class WeightedObservedDataChangedEvent
   {
      public OutputMapping OutputMapping { get; private set; }

      public WeightedObservedDataChangedEvent(OutputMapping outputMapping)
      {
         OutputMapping = outputMapping;
      }
   }
}
