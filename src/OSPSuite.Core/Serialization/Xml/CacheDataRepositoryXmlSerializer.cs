using System.Xml.Linq;
using OSPSuite.Serializer.Xml.Extensions;
using OSPSuite.Utility.Collections;
using OSPSuite.Core.Domain.Data;

namespace OSPSuite.Core.Serialization.Xml
{
   public class CacheDataRepositoryXmlSerializer : OSPSuiteXmlSerializer<ICache<string, DataRepository>>
   {
      public override void PerformMapping()
      {
      }

      public override ICache<string, DataRepository> CreateObject(XElement outputToDeserialize, SerializationContext serializationContext)
      {
         return new Cache<string, DataRepository>(dr => dr.Name);
      }

      protected override XElement TypedSerialize(ICache<string, DataRepository> objectToSerialize, SerializationContext serializationContext)
      {
         var serializer = SerializerRepository.SerializerFor(typeof (DataRepository));
         var xel = new XElement("repositories");

         foreach (var dataRepository in objectToSerialize)
         {
            xel.AddElement(serializer.Serialize(dataRepository, serializationContext));
         }

         return xel;
      }

      protected override void TypedDeserialize(ICache<string, DataRepository> objectToDeserialize, XElement xel, SerializationContext serializationContext)
      {
         var serializer = SerializerRepository.SerializerFor(typeof (DataRepository));

         foreach (var subXel in xel.Elements())
         {
            var dataRepository = new DataRepository();
            serializer.Deserialize(dataRepository, subXel, serializationContext);
            objectToDeserialize.Add(dataRepository);
         }
      }
   }
}