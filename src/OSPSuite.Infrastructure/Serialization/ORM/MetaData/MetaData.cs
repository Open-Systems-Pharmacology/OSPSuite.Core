namespace OSPSuite.Infrastructure.Serialization.ORM.MetaData
{
   /// <summary>
   /// base class for all entity that should be serialized in a pksim project
   /// </summary>
   public abstract class MetaData<TKey>
   {
      public virtual TKey Id { get; set; }

      public override bool Equals(object obj)
      {
         var other = obj as MetaData<TKey>;
         return other != null && Id.Equals(other.Id);
      }

      public override int GetHashCode()
      {
         return Id.GetHashCode();
      }
   }
}