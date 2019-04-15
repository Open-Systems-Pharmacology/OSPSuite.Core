using System.Collections.Generic;
using System.Linq;

namespace OSPSuite.Core.Domain.Descriptors
{
   public class EntityDescriptorMapList<T> : List<EntityDescriptorMap<T>> where T : IEntity
   {
      public EntityDescriptorMapList()
      {
      }

      public EntityDescriptorMapList(IEnumerable<EntityDescriptorMap<T>> collection) : base(collection)
      {
      }

      public IReadOnlyList<T> AllSatisfiedBy(DescriptorCriteria criteria)
      {
         return this.Where(x => criteria.IsSatisfiedBy(x.Descriptor)).Select(x => x.Entity).ToList();
      }
   }
}