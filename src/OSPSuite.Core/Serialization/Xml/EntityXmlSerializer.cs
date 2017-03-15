using OSPSuite.Core.Domain;

namespace OSPSuite.Core.Serialization.Xml
{
   public abstract class EntityXmlSerializer<T> : ObjectBaseXmlSerializer<T> where T : IEntity
   {
      protected EntityXmlSerializer()
      {
      }

      protected EntityXmlSerializer(string name) : base(name)
      {
      }

      public override void PerformMapping()
      {
         base.PerformMapping();
         MapEnumerable(x => x.Tags,x => x.AddTag);
      }
   }
}