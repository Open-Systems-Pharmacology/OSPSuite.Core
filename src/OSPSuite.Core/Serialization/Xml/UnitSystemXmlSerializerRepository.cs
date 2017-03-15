using OSPSuite.Serializer.Xml;

namespace OSPSuite.Core.Serialization.Xml
{
   public interface IUnitSystemXmlSerializer : IXmlSerializer
   {
   }

   public interface IUnitSystemXmlSerializerRepository : IXmlSerializerRepository<SerializationContext>
   {
   }

   public class UnitSystemXmlSerializerRepository : XmlSerializerRepositoryBase, IUnitSystemXmlSerializerRepository
   {
      protected override void AddInitialMappers()
      {
      }

      protected override void AddInitialSerializer()
      {
         AddSerializersSimple<IUnitSystemXmlSerializer>();
      }
   }
}