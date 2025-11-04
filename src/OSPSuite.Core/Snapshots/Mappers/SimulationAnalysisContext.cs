using System.Collections.Generic;
using OSPSuite.Utility.Extensions;

namespace OSPSuite.Core.Snapshots.Mappers
{
   public class SimulationAnalysisContext : SnapshotContext
   {
      private readonly List<Domain.Data.DataRepository> _dataRepositories = new List<Domain.Data.DataRepository>();

      public bool RunSimulation { get; set; }

      public IReadOnlyList<Domain.Data.DataRepository> DataRepositories => _dataRepositories;

      public SimulationAnalysisContext(IEnumerable<Domain.Data.DataRepository> dataRepositories, SnapshotContext baseContext) : base(baseContext)
      {
         AddDataRepositories(dataRepositories);
      }

      public void AddDataRepositories(IEnumerable<Domain.Data.DataRepository> dataRepositories)
      {
         dataRepositories?.Each(AddDataRepository);
      }

      public void AddDataRepository(Domain.Data.DataRepository dataRepository)
      {
         if (dataRepository != null)
            _dataRepositories.Add(dataRepository);
      }
   }
}