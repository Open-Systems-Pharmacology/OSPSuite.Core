using OSPSuite.Serializer;
using OSPSuite.Serializer.Attributes;
using OSPSuite.Serializer.Xml;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Serialization.Xml;

namespace OSPSuite.Core.Serialization.ParameterIdentificationExport.Serializer
{
   public interface IParameterIdentificationExportSerializerRepository : IXmlSerializerRepository<ParameterIdentificationExportSerializationContext>
   {
   }

   public class ParameterIdentificationExportSerializerRepository : XmlSerializerRepository<ParameterIdentificationExportSerializationContext>, IParameterIdentificationExportSerializerRepository
   {
      public ParameterIdentificationExportSerializerRepository()
      {
         Namespace = ParameterIdentificationExportSchemaConstants.Namespace;
         initialize();
      }

      private void initialize()
      {
         var attributeMapperRepository = new AttributeMapperRepository<ParameterIdentificationExportSerializationContext>();
         attributeMapperRepository.AddDefaultAttributeMappers();

         attributeMapperRepository.AddAttributeMapper(new LLOQModeXmlAttributeMapper<ParameterIdentificationExportSerializationContext>());
         attributeMapperRepository.AddAttributeMapper(new LLOQUsageXmlAttributeMapper<ParameterIdentificationExportSerializationContext>());
         attributeMapperRepository.AddAttributeMapper(new DimensionExportAttributeMapper());
         attributeMapperRepository.AddAttributeMapper(new EnumAttributeMapper<Scalings, ParameterIdentificationExportSerializationContext>());

         //register all available serializer
         this.AddSerializers(x =>
         {
            x.Implementing<IParameterIdentificationExportSerializer>();
            x.InAssemblyContainingType<ParameterIdentificationExportSerializerRepository>();
            x.UsingAttributeRepository(attributeMapperRepository);
         });

         PerformMapping();
      }
   }
}