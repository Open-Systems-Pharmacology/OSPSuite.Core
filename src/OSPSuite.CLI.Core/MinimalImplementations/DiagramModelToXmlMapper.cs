using OSPSuite.Core.Diagram;
using OSPSuite.Core.Serialization.Diagram;
using System.Xml;

namespace OSPSuite.CLI.Core.MinimalImplementations
{
   public class DiagramModelToXmlMapper : IDiagramModelToXmlMapper
   {
      public string ElementName => null;

      public void AddElementBaseNodeBindingFor<T>(T node)
      {
         //nothing to do
      }

      public void Deserialize(IDiagramModel model, XmlDocument xmlDoc)
      {
         //nothing to do
      }

      public XmlDocument DiagramModelToXmlDocument(IDiagramModel diagramModel)
      {
         return new XmlDocument();
      }

      public IDiagramModel XmlDocumentToDiagramModel(XmlDocument xmlDoc)
      {
         return new DiagramModel();
      }
   }
}