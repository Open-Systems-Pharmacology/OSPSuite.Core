using System.Collections.Generic;
using System.Linq;
using OSPSuite.Utility.Collections;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Domain.Services;

namespace OSPSuite.Core.Domain.Repositories
{
   public interface IObservedDataRepository : IRepository<DataRepository>
   {
      DataRepository FindFor(UsedObservedData usedObservedData);

      IEnumerable<DataRepository> AllObservedDataUsedBy(IUsesObservedData observedDataUser);

      IEnumerable<DataRepository> AllObservedDataUsedBy(IEnumerable<IUsesObservedData> observedDataUsers);
   }

   public class ObservedDataRepository : IObservedDataRepository
   {
      private readonly IProjectRetriever _projectRetriever;

      public ObservedDataRepository(IProjectRetriever projectRetriever)
      {
         _projectRetriever = projectRetriever;
      }

      public IEnumerable<DataRepository> All()
      {
         var project = _projectRetriever.CurrentProject;
         return project == null ? Enumerable.Empty<DataRepository>() : project.AllObservedData;
      }

      public DataRepository FindFor(UsedObservedData usedObservedData)
      {
         return All().FindById(usedObservedData.Id);
      }

      public IEnumerable<DataRepository> AllObservedDataUsedBy(IUsesObservedData observedDataUser)
      {
         return All().Where(observedDataUser.UsesObservedData);
      }

      public IEnumerable<DataRepository> AllObservedDataUsedBy(IEnumerable<IUsesObservedData> observedDataUsers)
      {
         return observedDataUsers.SelectMany(AllObservedDataUsedBy).Distinct();
      }
   }
}