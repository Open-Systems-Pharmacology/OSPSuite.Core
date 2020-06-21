using NHibernate;

namespace OSPSuite.Infrastructure.Serialization.ORM.MetaData
{
   public interface IUpdatableFrom<T>
   {
      void UpdateFrom(T source, ISession session);
   }
}