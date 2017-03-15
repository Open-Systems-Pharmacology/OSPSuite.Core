using System.Collections.Generic;
using OSPSuite.Utility;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Domain.Services;

namespace OSPSuite.Core.Serialization.Exchange
{
   public class SimulationTransfer : IWithId
   {
      public IModelCoreSimulation Simulation { get; set; }
      public IList<DataRepository> AllObservedData { get; set; }
      public int PkmlVersion { get; set; }
      public ReactionDimensionMode ReactionDimensionMode { get; set; }
      public string Id { get; set; }
      public Favorites Favorites { get; set; }
      public string JournalPath { get; set; }

      public SimulationTransfer()
      {
         AllObservedData = new List<DataRepository>();
         PkmlVersion = Constants.PKML_VERSION;
         ReactionDimensionMode = ReactionDimensionMode.AmountBased;
         Id = ShortGuid.NewGuid();
         Favorites = new Favorites();
         JournalPath = string.Empty;
      }
   }
}