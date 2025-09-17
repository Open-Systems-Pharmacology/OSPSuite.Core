using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Data;
using System;
using System.Collections.Generic;
using System.Text;

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
