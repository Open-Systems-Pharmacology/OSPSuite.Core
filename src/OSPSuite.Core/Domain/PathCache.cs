using System.Collections.Generic;
using OSPSuite.Utility.Collections;
using OSPSuite.Utility.Extensions;
using OSPSuite.Core.Domain.Services;

namespace OSPSuite.Core.Domain
{
   /// <summary>
   ///    Represents a special cache used in the application that caches parameter with their entity path
   ///    The cache also returns null if a value is not found with a given path
   /// </summary>
   /// <typeparam name="TEntity"></typeparam>
   public class PathCache<TEntity> : Cache<string, TEntity> where TEntity : class, IEntity
   {
      public PathCache(IEntityPathResolver entityPathResolver)
         : base(entityPathResolver.PathFor, s => null)
      {
      }

      public PathCache<TEntity> For(IEnumerable<TEntity> entities)
      {
         entities.Each(Add);
         return this;
      }
   }
}