using OSPSuite.Serializer.Xml;
using OSPSuite.Core.Serialization.Xml;

namespace OSPSuite.Presentation.Serialization
{
   public interface IPresentationXmlSerializer : IXmlSerializer
   {
   }

   public abstract class PresentationXmlSerializer<T> : XmlSerializer<T, SerializationContext>, IPresentationXmlSerializer
   {
   }
}