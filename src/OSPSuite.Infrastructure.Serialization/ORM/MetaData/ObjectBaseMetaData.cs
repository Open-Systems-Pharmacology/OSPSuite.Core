using NHibernate;

namespace OSPSuite.Infrastructure.ORM.MetaData
{
   public abstract class ObjectBaseMetaData<T> : MetaDataWithContent<string>, IUpdatableFrom<T> where T : ObjectBaseMetaData<T>
   {
      public virtual string Name { get; set; }
      public virtual string Description { get; set; }

      public virtual void UpdateFrom(T source, ISession session)
      {
         Name = source.Name;
         Description = source.Description;
         UpdateContentFrom(source);
      }
   }
}