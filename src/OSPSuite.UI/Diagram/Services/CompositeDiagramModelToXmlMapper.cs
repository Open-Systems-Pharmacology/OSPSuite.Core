using System.Xml;
using OSPSuite.Core.Diagram;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Serialization.Diagram;
using OSPSuite.Presentation.Diagram.Elements;
using ReactionDiagramModelToXmlMapper = OSPSuite.Presentation.Diagram.Services.DiagramModelToXmlMapper;

namespace OSPSuite.UI.Diagram.Services
{
   public class CompositeDiagramModelToXmlMapper : IDiagramModelToXmlMapper, IContainerBaseXmlSerializer
   {
      private readonly DiagramModelToXmlMapper _goDiagramMapper = new DiagramModelToXmlMapper();
      private readonly ReactionDiagramModelToXmlMapper _reactionDiagramMapper = new ReactionDiagramModelToXmlMapper();

      public string ElementName => Constants.Serialization.DIAGRAM_MODEL;

      public IDiagramModel XmlDocumentToDiagramModel(XmlDocument xmlDoc)
      {
         return _goDiagramMapper.XmlDocumentToDiagramModel(xmlDoc);
      }

      public XmlDocument DiagramModelToXmlDocument(IDiagramModel diagramModel)
      {
         return mapperFor(diagramModel).DiagramModelToXmlDocument(diagramModel);
      }

      public void Deserialize(IDiagramModel model, XmlDocument xmlDoc)
      {
         mapperFor(model).Deserialize(model, xmlDoc);
      }

      public void AddElementBaseNodeBindingFor<T>(T node)
      {
         _goDiagramMapper.AddElementBaseNodeBindingFor(node);
      }

      public XmlDocument ContainerToXmlDocument(IContainerBase containerBase)
      {
         return serializerFor(containerBase).ContainerToXmlDocument(containerBase);
      }

      private IDiagramModelToXmlMapper mapperFor(IDiagramModel diagramModel)
      {
         return diagramModel is DiagramModel ? _reactionDiagramMapper : (IDiagramModelToXmlMapper) _goDiagramMapper;
      }

      private IContainerBaseXmlSerializer serializerFor(IContainerBase containerBase)
      {
         return containerBase is DiagramModel || containerBase is ContainerNode ? _reactionDiagramMapper : (IContainerBaseXmlSerializer) _goDiagramMapper;
      }

   }
}
