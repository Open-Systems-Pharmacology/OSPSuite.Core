using OSPSuite.Serializer;
using OSPSuite.Serializer.Attributes;
using OSPSuite.Serializer.Xml;

namespace OSPSuite.Core.Serialization.SimModel.Serializer
{
   public interface ISimModelSerializerRepository : IXmlSerializerRepository<SimModelSerializationContext>
   {
   }

   public class SimModelSerializerRepository : XmlSerializerRepository<SimModelSerializationContext>, ISimModelSerializerRepository
   {
      
      public SimModelSerializerRepository()
      {
         Namespace = SimModelSchemaConstants.Namespace;
         initialize();
      }

      private void initialize()
      {
         var attributeMapperRepository = new AttributeMapperRepository<SimModelSerializationContext>();
         attributeMapperRepository.AddDefaultAttributeMappers();

         //remove double attribute mapper since we need a special implementation in order to export nan, and inf 
         //properly
         attributeMapperRepository.RemoveAttributeMapperFor<double>();

         attributeMapperRepository.AddAttributeMapper(new SimModelDoubleAttributeMapper());

         //register all available serializer
         this.AddSerializers(x =>
                                {
                                   x.Implementing<ISimModelSerializer>();
                                   x.InAssemblyContainingType<SimModelSerializerRepository>();
                                   x.UsingAttributeRepository(attributeMapperRepository);
                                });

         PerformMapping();
      }
   }
}