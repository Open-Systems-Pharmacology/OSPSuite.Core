using NHibernate;

namespace OSPSuite.Infrastructure.ORM.MetaData
{
   public interface IUpdatableFrom<T>
   {
      void UpdateFrom(T source, ISession session);
   }
}