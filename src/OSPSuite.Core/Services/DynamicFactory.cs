using OSPSuite.Utility.Container;

namespace OSPSuite.Core.Services
{
   public class DynamicFactory<T>
   {
      public T Create() => IoC.Resolve<T>();

      public U Create<U>() where U : T => IoC.Resolve<U>();
   }
}