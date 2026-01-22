using OSPSuite.Core.Domain;
using OSPSuite.Core.Serialization.Xml;
using OSPSuite.Serializer;
using OSPSuite.Serializer.Attributes;

namespace OSPSuite.Presentation.Serialization.Extensions
{
   public static class XmlSerializerRepositoryExtensions
   {
      public static void AddPresentationSerializers(this IOSPSuiteXmlSerializerRepository serializerRepository)
      {
         serializerRepository.AddSerializers(x =>
         {
            x.Implementing<IPresentationXmlSerializer>();
            x.InAssemblyContainingType<IPresentationXmlSerializer>();
            x.UsingAttributeRepository(serializerRepository.AttributeMapperRepository);
         });

         serializerRepository.AttributeMapperRepository.AddAttributeMapper(new EnumAttributeMapper<ParameterGroupingModeId, SerializationContext>());
         serializerRepository.AttributeMapperRepository.AddAttributeMapper(new EnumAttributeMapper<ParameterGroupingModeIdForParameterAnalyzable, SerializationContext>());
      }
   }
}