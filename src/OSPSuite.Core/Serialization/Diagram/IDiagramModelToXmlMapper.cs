using System.Xml;
using OSPSuite.Core.Diagram;

namespace OSPSuite.Core.Serialization.Diagram
{
   public interface IDiagramModelToXmlMapper
   {
      string ElementName { get; }
      IDiagramModel XmlDocumentToDiagramModel(XmlDocument xmlDoc);
      XmlDocument DiagramModelToXmlDocument(IDiagramModel diagramModel);
      void Deserialize(IDiagramModel model, XmlDocument xmlDoc);
      void AddElementBaseNodeBindingFor<T>(T node);
   }
}