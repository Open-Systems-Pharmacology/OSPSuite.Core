using System.Xml;
using OSPSuite.Core.Diagram;

namespace OSPSuite.Presentation.Diagram.Elements
{
   public interface IContainerBaseXmlSerializer
   {
      XmlDocument ContainerToXmlDocument(IContainerBase containerBase);
   }
}