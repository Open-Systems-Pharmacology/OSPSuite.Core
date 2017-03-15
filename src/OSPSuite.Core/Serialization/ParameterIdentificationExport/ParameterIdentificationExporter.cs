using OSPSuite.Serializer.Xml;
using OSPSuite.Core.Domain.ParameterIdentifications;
using OSPSuite.Core.Serialization.ParameterIdentificationExport.Serializer;

namespace OSPSuite.Core.Serialization.ParameterIdentificationExport
{
   public interface IParameterIdentificationExporter
   {
      /// <summary>
      ///    Creates a ParameterIdentification representation of the given <paramref name="parameterIdentification" /> and save
      ///    it to the file
      /// </summary>
      void Export(ParameterIdentification parameterIdentification, string fileName);
   }

   public class ParameterIdentificationExporter : IParameterIdentificationExporter
   {
      private readonly IParameterIdentificationExportSerializerRepository _serializerRepository;

      public ParameterIdentificationExporter(IParameterIdentificationExportSerializerRepository serializerRepository)
      {
         _serializerRepository = serializerRepository;
      }

      public void Export(ParameterIdentification parameterIdentification, string fileName)
      {
         var serializer = _serializerRepository.SerializerFor(parameterIdentification);
         var element = serializer.Serialize(parameterIdentification, new ParameterIdentificationExportSerializationContext());
         XmlHelper.SaveXmlElementToFile(element, fileName);
      }
   }
}