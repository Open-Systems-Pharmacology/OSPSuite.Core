namespace OSPSuite.Infrastructure.ORM.MetaData
{
   public interface IMetaDataWithContent
   {
      MetaDataContent Content { get; }
   }

   public abstract class MetaDataWithContent<TKey> : MetaData<TKey>, IMetaDataWithContent
   {
      /// <summary>
      /// Serialization of the entire meta data
      /// </summary>
      public virtual MetaDataContent Content { get; }

      protected MetaDataWithContent()
      {
         Content = new MetaDataContent();
      }

      public virtual bool IsLoaded => Content.Data != null;

      protected virtual void UpdateContentFrom(MetaDataWithContent<TKey> source)
      {
         if (!(source.IsLoaded)) return;
         Content.Data = source.Content.Data;
      }
   }
}