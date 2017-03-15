using System.Xml.Linq;
using OSPSuite.Core.Domain;

namespace OSPSuite.Core.Serialization.Xml
{
   public class ClassificationXmlSerializer : OSPSuiteXmlSerializer<Classification>
   {
      public override void PerformMapping()
      {
         Map(x => x.Id);
         Map(x => x.Name);
         Map(x => x.ClassificationType);
         MapReference(x => x.Parent);
      }

      protected override void TypedDeserialize(Classification classification, XElement outputToDeserialize, SerializationContext serializationContext)
      {
         base.TypedDeserialize(classification, outputToDeserialize, serializationContext);
         serializationContext.Register(classification);
      }
   }
}