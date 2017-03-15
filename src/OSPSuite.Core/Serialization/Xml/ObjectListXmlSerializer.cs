using System.Collections.Generic;
using System.Xml.Linq;

namespace OSPSuite.Core.Serialization.Xml
{
   public abstract class ObjectListXmlSerializer<T> : OSPSuiteXmlSerializer<List<T>> where T : class
   {
      protected ObjectListXmlSerializer() : base($"{typeof (T).Name}List")
      {
      }

      public override void PerformMapping()
      {
         //nothing to do here
      }

      protected override XElement TypedSerialize(List<T> objectList, SerializationContext serializationContext)
      {
         var element = SerializerRepository.CreateElement(ElementName);
         foreach (var item in objectList)
         {
            var serializer = SerializerRepository.SerializerFor(item);
            element.Add(serializer.Serialize(item, serializationContext));
         }
         return element;
      }

      protected override void TypedDeserialize(List<T> objectList, XElement element, SerializationContext serializationContext)
      {
         foreach (var childElement in element.Elements())
         {
            var serializer = SerializerRepository.SerializerFor(childElement);
            objectList.Add(serializer.Deserialize<T>(childElement, serializationContext));
         }
      }
   }
}