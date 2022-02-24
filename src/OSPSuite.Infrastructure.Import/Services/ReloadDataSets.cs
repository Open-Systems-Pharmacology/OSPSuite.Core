using System.Collections.Generic;
using System.Linq;
using OSPSuite.Core.Domain.Data;

namespace OSPSuite.Infrastructure.Import.Services
{
   public class ReloadDataSets
   {
      public IEnumerable<DataRepository> NewDataSets { get; }
      public IEnumerable<DataRepository> OverwrittenDataSets { get; }
      public IEnumerable<DataRepository> DataSetsToBeDeleted { get; }

      public ReloadDataSets()
      {
         NewDataSets = Enumerable.Empty<DataRepository>();
         OverwrittenDataSets = Enumerable.Empty<DataRepository>();
         DataSetsToBeDeleted = Enumerable.Empty<DataRepository>();
      }

      public ReloadDataSets(IEnumerable<DataRepository> newDataSets, IEnumerable<DataRepository> overwrittenDataSets, IEnumerable<DataRepository> dataSetsToBeDeleted)
      {
         NewDataSets = newDataSets;
         OverwrittenDataSets = overwrittenDataSets;
         DataSetsToBeDeleted = dataSetsToBeDeleted;
      }
   }
}