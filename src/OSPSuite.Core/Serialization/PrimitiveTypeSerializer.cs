using System.Xml.Linq;
using OSPSuite.Serializer.Xml;
using OSPSuite.Utility.Extensions;

namespace OSPSuite.Core.Serialization
{
   public abstract class PrimitiveTypeSerializer<T, TContext> : XmlSerializer<T, TContext>
   {
      public override void PerformMapping()
      {
      }

      protected override XElement TypedSerialize(T objectToSerialize, TContext serializationContext)
      {
         var element = SerializerRepository.CreateElement(ElementName);
         element.SetValue(objectToSerialize.ConvertedTo<string>());
         return element;
      }
   }
}