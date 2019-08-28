using System.Collections.Generic;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Utility;

namespace OSPSuite.Core.Serialization.Exchange
{
   public class SimulationTransfer : IWithId
   {
      public IModelCoreSimulation Simulation { get; set; }
      public IList<DataRepository> AllObservedData { get; set; }
      public int PkmlVersion { get; set; }
      public string Id { get; set; }
      public Favorites Favorites { get; set; }
      public string JournalPath { get; set; }

      public SimulationTransfer()
      {
         AllObservedData = new List<DataRepository>();
         PkmlVersion = Constants.PKML_VERSION;
         Id = ShortGuid.NewGuid();
         Favorites = new Favorites();
         JournalPath = string.Empty;
      }
   }
}