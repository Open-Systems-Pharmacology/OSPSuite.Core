using System.Collections.Generic;
using OSPSuite.Utility.Collections;

namespace OSPSuite.Core.Domain
{
   public interface IWithIdRepository : IRepository<IWithId>
   {
      void Register(IWithId objectWithId);
      bool ContainsObjectWithId(string id);
      void Unregister(string id);
      T Get<T>(string id) where T : class, IWithId;
      IWithId Get(string id);
      void Clear();
   }

   public class WithIdRepository : IWithIdRepository
   {
      protected readonly ICache<string, IWithId> _entities = new Cache<string, IWithId>(x => x.Id, x => null);
      public IEnumerable<IWithId> All() => _entities;

      public virtual void Register(IWithId objectWithId)
      {
         if (string.IsNullOrEmpty(objectWithId.Id))
            return;

         if (!ContainsObjectWithId(objectWithId.Id))
         {
            _entities.Add(objectWithId);
            return;
         }

         var existing = Get(objectWithId.Id);
         if (ReferenceEquals(existing, objectWithId))
            return;

         throw new NotUniqueIdException(objectWithId.Id);
      }

      public virtual bool ContainsObjectWithId(string id)
      {
         if (string.IsNullOrEmpty(id))
            return false;

         return _entities.Contains(id);
      }

      public virtual void Unregister(string id)
      {
         if (!ContainsObjectWithId(id)) return;
         _entities.Remove(id);
      }

      public virtual T Get<T>(string id) where T : class, IWithId
      {
         return Get(id) as T;
      }

      public virtual IWithId Get(string id)
      {
         return _entities[id];
      }

      public void Clear()
      {
         _entities.Clear();
      }
   }
}