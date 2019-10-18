using NHibernate;

namespace OSPSuite.Infrastructure.ORM.MetaData
{
   public interface IMetaDataWithProperties
   {
      MetaDataContent Properties { get; }
   }

   public abstract class ObjectBaseMetaDataWithProperties<T> : ObjectBaseMetaData<T>, IMetaDataWithProperties where T : ObjectBaseMetaDataWithProperties<T>
   {
      /// <summary>
      ///    Serialization of the property that will be always laoded if availbable in order to display info in the UI
      /// </summary>
      public virtual MetaDataContent Properties { get; private set; }

      protected ObjectBaseMetaDataWithProperties()
      {
         Properties = new MetaDataContent();
      }

      public override void UpdateFrom(T source, ISession session)
      {
         base.UpdateFrom(source, session);
         Properties.Data = source.Properties.Data;
      }
   }
}