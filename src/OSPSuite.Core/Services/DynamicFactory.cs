using OSPSuite.Utility.Container;

namespace OSPSuite.Core.Services
{
   public class DynamicFactory<T>
   {
      private readonly IContainer _container;

      public DynamicFactory(IContainer container)
      {
         _container = container;
      }

      public T Create() => _container.Resolve<T>();

      public U Create<U>() where U : T => _container.Resolve<U>();
   }
}