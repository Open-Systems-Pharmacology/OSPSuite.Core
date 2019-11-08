using OSPSuite.Utility.Container;

namespace OSPSuite.Core.Services
{
   public class DynamicFactory<T>
   {
      public T Create()
      {
         return IoC.Resolve<T>();
      }
   }
}