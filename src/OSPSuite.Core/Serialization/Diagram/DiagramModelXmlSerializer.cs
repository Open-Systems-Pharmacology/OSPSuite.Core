using System.Xml;
using System.Xml.Linq;
using OSPSuite.Utility.Container;
using OSPSuite.Core.Diagram;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Serialization.Xml;

namespace OSPSuite.Core.Serialization.Diagram
{
   public class DiagramModelXmlSerializer : OSPSuiteXmlSerializer<IDiagramModel>
   {
      public DiagramModelXmlSerializer() : base(Constants.Serialization.DIAGRAM_MODEL)
      {
      }

      public override void PerformMapping()
      {
         //nothing to do here
      }

      protected override void TypedDeserialize(IDiagramModel diagramModel, XElement diagramElement, SerializationContext serializationContext)
      {
         var diagramModelToXmlMapper = IoC.Resolve<IDiagramModelToXmlMapper>();
         var xmlDoc = new XmlDocument();
         xmlDoc.Load(diagramElement.CreateReader());
         diagramModelToXmlMapper.Deserialize(diagramModel, xmlDoc);
      }

      protected override XElement TypedSerialize(IDiagramModel diagramModel, SerializationContext serializationContext)
      {
         var diagramModelToXmlMapper = IoC.Resolve<IDiagramModelToXmlMapper>();
         var xmlDoc = diagramModelToXmlMapper.DiagramModelToXmlDocument(diagramModel);

         //xmlDoc is empty return null
         if (string.IsNullOrEmpty(xmlDoc.OuterXml))
            return null;

         return XDocument.Load(new XmlNodeReader(xmlDoc)).Root;
      }

      public override IDiagramModel CreateObject(XElement element, SerializationContext serializationContext)
      {
         return IoC.Resolve<IDiagramModel>();
      }
   }
}