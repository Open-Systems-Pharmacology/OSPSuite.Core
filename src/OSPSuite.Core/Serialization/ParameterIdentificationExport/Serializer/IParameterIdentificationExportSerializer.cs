using OSPSuite.Serializer.Xml;

namespace OSPSuite.Core.Serialization.ParameterIdentificationExport.Serializer
{
   public interface IParameterIdentificationExportSerializer : IXmlSerializer
   {
   }

   public abstract class ParameterIdentificationExportSerializerBase<TObject> : XmlSerializer<TObject, ParameterIdentificationExportSerializationContext>, IParameterIdentificationExportSerializer
   {
   }
}