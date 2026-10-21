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

      /// <summary>
      ///    Original values of the quantities changed in the simulation since it was created from its building blocks
      /// </summary>
      public IList<OriginalQuantityValue> OriginalQuantityValues { get; set; } = new List<OriginalQuantityValue>();

      /// <summary>
      ///    True if the simulation has changes that cannot be traced back to a building block (e.g. after a project conversion)
      /// </summary>
      public bool HasUntraceableChanges { get; set; }

      //MetaData about the export
      public int PkmlVersion { get; set; } = Constants.PKML_VERSION;
      public Favorites Favorites { get; set; } = new Favorites();
      public string JournalPath { get; set; } = string.Empty;
   }
}