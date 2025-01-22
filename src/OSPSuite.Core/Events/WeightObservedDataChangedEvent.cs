using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace OSPSuite.Core.Events
{
   public class WeightObservedDataChangedEvent
   {
      public OutputMapping OutputMapping { get; private set; }

      public WeightObservedDataChangedEvent(OutputMapping outputMapping)
      {
         OutputMapping = outputMapping;
      }
   }
}
