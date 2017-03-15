using System.Xml.Linq;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Serialization.Xml.Extensions;

namespace OSPSuite.Core.Serialization.Xml
{
   public class ModelXmlSerializer : ObjectBaseXmlSerializer<Model>
   {
      public override void PerformMapping()
      {
         base.PerformMapping();
         Map(x => x.Root);
         MapReference(x => x.Neighborhoods);
         //do not Map Neighborhood since this is only a reference to a child of the root.
         //Implementation is questionable at first glance.
         //We just save a reference to the neighborhoods object
      }

      protected override XElement TypedSerialize(Model model, SerializationContext serializationContext)
      {
         var element = base.TypedSerialize(model, serializationContext);
         SerializerRepository.AddFormulaCacheElement(element, serializationContext);
         return element;
      }

      protected override void TypedDeserialize(Model model, XElement element, SerializationContext serializationContext)
      {
         SerializerRepository.DeserializeFormulaCacheIn(element, serializationContext);
         base.TypedDeserialize(model, element, serializationContext);
      }
   }
}