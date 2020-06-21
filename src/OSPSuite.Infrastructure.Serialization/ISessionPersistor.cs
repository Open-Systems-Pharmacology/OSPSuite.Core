using NHibernate;

namespace OSPSuite.Infrastructure.Serialization
{
   public interface ISessionPersistor<T>
   {
      void Save(T target, ISession session);
      T Load(ISession session);
   }
}
