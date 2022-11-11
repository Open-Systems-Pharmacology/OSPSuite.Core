using System.Collections.Generic;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Utility;

namespace OSPSuite.Core.Serialization.Exchange
{
   public class SimulationTransfer : IWithId
   {
      public string Id { get; set; } = ShortGuid.NewGuid();

      public IModelCoreSimulation Simulation { get; set; }
      public OutputMappings OutputMappings { get; set; } = new OutputMappings();
      public IList<DataRepository> AllObservedData { get; set; } = new List<DataRepository>();

      //MetaData about the export
      public int PkmlVersion { get; set; } = Constants.PKML_VERSION;
      public Favorites Favorites { get; set; } = new Favorites();
      public string JournalPath { get; set; } = string.Empty;
   }
}