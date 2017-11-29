using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Services;

namespace OSPSuite.Starter.Services
{
   public class LazyLoadTask : ILazyLoadTask
   {
      public void Load<TObject>(TObject objectToLoad) where TObject : class, ILazyLoadable
      {
         
      }
   }
}