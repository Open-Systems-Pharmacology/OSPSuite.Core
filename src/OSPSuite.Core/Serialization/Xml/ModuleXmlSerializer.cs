using System.Xml.Linq;
using OSPSuite.Core.Domain;

namespace OSPSuite.Core.Serialization.Xml
{
   public class ModuleXmlSerializer : ObjectBaseXmlSerializer<Module>
   {
      public override void PerformMapping()
      {
         base.PerformMapping();
         MapEnumerable(x => x.BuildingBlocks, x => x.Add);
         Map(x => x.PKSimVersion);
         Map(x => x.ModuleImportVersion);
         Map(x => x.MergeBehavior);
         Map(x => x.IsPKSimModule);
      }

      protected override void TypedDeserialize(Module objectToDeserialize, XElement outputToDeserialize, SerializationContext serializationContext)
      {
         base.TypedDeserialize(objectToDeserialize, outputToDeserialize, serializationContext);
         objectToDeserialize.Snapshot = outputToDeserialize.Element("Snapshot")?.Value ?? string.Empty;
      }

      protected override XElement TypedSerialize(Module objectToSerialize, SerializationContext context)
      {
         base.TypedSerialize(objectToSerialize, context);
         var element =  base.TypedSerialize(objectToSerialize, context);
         var snapShot = new XElement("Snapshot", objectToSerialize.Snapshot);
         element.Add(snapShot);

         return element;
      }
   }
}