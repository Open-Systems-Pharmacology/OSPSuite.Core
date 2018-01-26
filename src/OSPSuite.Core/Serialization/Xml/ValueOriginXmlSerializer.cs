using System.Xml.Linq;
using OSPSuite.Core.Domain;
using OSPSuite.Serializer;

namespace OSPSuite.Core.Serialization.Xml
{
   public class ValueOriginXmlSerializer : OSPSuiteXmlSerializer<ValueOrigin>
   {
      public ValueOriginXmlSerializer() : base(Constants.Serialization.VALUE_ORIGIN)
      {
      }

      public override void PerformMapping()
      {
         Map(x => x.Id);
         Map(x => x.Source);
         Map(x => x.Method);
         Map(x => x.Description).WithMappingName(Constants.Serialization.Attribute.DESCRIPTION);
      }

      protected override XElement TypedSerialize(ValueOrigin valueOrigin, SerializationContext context)
      {
         //do not serialize undefined value origin
         if (valueOrigin.IsUndefined)
            return null;

         return base.TypedSerialize(valueOrigin, context);
      }
   }
}