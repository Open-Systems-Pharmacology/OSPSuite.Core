using OSPSuite.Utility.Collections;

namespace OSPSuite.Core.Domain.Builder
{
   public class ObjectBaseCache<T> : Cache<string, T> where T : class, IObjectBase
   {
      public ObjectBaseCache() : base(x => x.Name, x => null)
      {
      }
   }
}