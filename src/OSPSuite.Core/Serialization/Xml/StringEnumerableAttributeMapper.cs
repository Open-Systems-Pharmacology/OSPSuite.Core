using System.Collections.Generic;
using OSPSuite.Serializer.Attributes;
using OSPSuite.Core.Extensions;

namespace OSPSuite.Core.Serialization.Xml
{
   public class StringEnumerableAttributeMapper : AttributeMapper<IEnumerable<string>, SerializationContext>
   {
      public override string Convert(IEnumerable<string> stringList, SerializationContext context)
      {
         return stringList.ToPathString();
      }

      public override object ConvertFrom(string attributeValue, SerializationContext context)
      {
         if (attributeValue == string.Empty)
            return new List<string>();

         return attributeValue.ToPathArray();
      }
   }
}