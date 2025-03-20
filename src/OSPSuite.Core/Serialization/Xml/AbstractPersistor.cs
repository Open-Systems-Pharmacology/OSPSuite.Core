using System.Xml.Linq;
using OSPSuite.Serializer.Xml;
using IContainer = OSPSuite.Utility.Container.IContainer;

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
      protected readonly IContainer _container;

      protected AbstractFilePersistor(IXmlSerializerRepository<SerializationContext> serializerRepository, IContainer container)
      {
         _serializerRepository = serializerRepository;
         _container = container;
      }

      public virtual void Save(T objectToSerialize, string fileName)
      {
         using (var serializationContext = SerializationTransaction.Create(_container))
         {
            var serializer = _serializerRepository.SerializerFor(objectToSerialize);
            var xel = serializer.Serialize(objectToSerialize, serializationContext);
            xel.PermissiveSave(fileName);
         }
      }

      public virtual void Load(T objectToLoad, string fileName)
      {
         using (var serializationContext = SerializationTransaction.Create(_container))
         {
            var serializer = _serializerRepository.SerializerFor(objectToLoad);
            var element = XElementSerializer.PermissiveLoad(fileName);
            serializer.Deserialize(objectToLoad, element, serializationContext);
         }
      }
   }
}