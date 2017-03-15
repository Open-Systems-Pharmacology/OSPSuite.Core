using System.Xml.Linq;
using OSPSuite.Serializer.Xml;

namespace OSPSuite.Core.Serialization.Xml
{
   public interface IFilePersistor<T>
   {
      void Save(T objectToSerialize, string fileName);
      void Load(T objectToLoad, string fileName);
   }

   public abstract class AbstractFilePersistor<T> : IFilePersistor<T>
   {
      protected readonly IXmlSerializerRepository<SerializationContext> _serializerRepository;

      protected AbstractFilePersistor(IXmlSerializerRepository<SerializationContext> serializerRepository)
      {
         _serializerRepository = serializerRepository;
      }

      public virtual void Save(T objectToSerialize, string fileName)
      {
         using (var serializationContext = SerializationTransaction.Create())
         {
            var serializer = _serializerRepository.SerializerFor(objectToSerialize);
            var xel = serializer.Serialize(objectToSerialize, serializationContext);
            xel.Save(fileName);
         }
      }

      public virtual void Load(T objectToLoad, string fileName)
      {
         using (var serializationContext = SerializationTransaction.Create())
         {
            var serializer = _serializerRepository.SerializerFor(objectToLoad);
            var element = XElement.Load(fileName);
            serializer.Deserialize(objectToLoad, element, serializationContext);
         }
      }
   }
}