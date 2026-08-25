using System.Collections.Generic;
using System.Linq;
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

   /// <summary>
   ///    Registered as a singleton and accessed from concurrent operations (e.g. building blocks being lazy-loaded while
   ///    simulations are mapped or constructed in parallel). All access to the underlying cache is synchronized.
   /// </summary>
   public class WithIdRepository : IWithIdRepository
   {
      private readonly object _locker = new object();
      protected readonly ICache<string, IWithId> _entities = new Cache<string, IWithId>(x => x.Id, x => null);

      public IEnumerable<IWithId> All()
      {
         lock (_locker)
         {
            return _entities.ToList();
         }
      }

      public virtual void Register(IWithId objectWithId)
      {
         if (string.IsNullOrEmpty(objectWithId.Id))
            return;

         lock (_locker)
         {
            if (!_entities.Contains(objectWithId.Id))
            {
               _entities.Add(objectWithId);
               return;
            }

            var existing = _entities[objectWithId.Id];
            if (ReferenceEquals(existing, objectWithId))
               return;

            throw new NotUniqueIdException(objectWithId.Id);
         }
      }

      public virtual bool ContainsObjectWithId(string id)
      {
         if (string.IsNullOrEmpty(id))
            return false;

         lock (_locker)
         {
            return _entities.Contains(id);
         }
      }

      public virtual void Unregister(string id)
      {
         if (string.IsNullOrEmpty(id))
            return;

         lock (_locker)
         {
            if (!_entities.Contains(id))
               return;

            _entities.Remove(id);
         }
      }

      public virtual T Get<T>(string id) where T : class, IWithId => Get(id) as T;

      public virtual IWithId Get(string id)
      {
         lock (_locker)
         {
            return _entities[id];
         }
      }

      public void Clear()
      {
         lock (_locker)
         {
            _entities.Clear();
         }
      }
   }
}
