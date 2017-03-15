using System.Xml.Linq;
using OSPSuite.Utility.Extensions;

namespace OSPSuite.Core.Serialization.SimModel.Serializer
{
   public abstract class PrimitiveTypeSerializer<T> : SimModelSerializerBase<T>
   {
      public override void PerformMapping()
      {
      }

      protected override XElement TypedSerialize(T objectToSerialize, SimModelSerializationContext serializationContext)
      {
         var element = SerializerRepository.CreateElement(ElementName);
         element.SetValue(objectToSerialize.ConvertedTo<string>());
         return element;
      }
   }

   public class DoubleSerializer : PrimitiveTypeSerializer<double>
   {
   }

   public class IntSerializer : PrimitiveTypeSerializer<int>
   {
   }

   public class StringSerializer : PrimitiveTypeSerializer<string>
   {
   }
}