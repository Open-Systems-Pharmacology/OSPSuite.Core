using NHibernate;

namespace OSPSuite.Infrastructure.ORM.MetaData
{
   public class ObservedDataMetaData : MetaData<string>, IUpdatableFrom<ObservedDataMetaData>
   {
      public virtual DataRepositoryMetaData DataRepository { get; set; }

      public void UpdateFrom(ObservedDataMetaData source, ISession session)
      {
         if (DataRepository == null)
            DataRepository = source.DataRepository;
         else
            DataRepository.UpdateFrom(source.DataRepository, session);
      }
   }
}