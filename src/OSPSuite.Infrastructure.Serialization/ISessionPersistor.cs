using NHibernate;

namespace OSPSuite.Infrastructure
{
   public interface ISessionPersistor<T>
   {
      void Save(T target, ISession session);
      T Load(ISession session);
   }
}
