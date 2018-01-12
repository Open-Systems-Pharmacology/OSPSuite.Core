using OSPSuite.Serializer.Attributes;
using OSPSuite.Serializer.Xml;
using OSPSuite.Core.Chart;
using OSPSuite.Core.Comparison;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Domain.PKAnalyses;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Serialization.Chart;

namespace OSPSuite.Core.Serialization.Xml
{
   public interface IOSPSuiteXmlSerializer : IXmlSerializer
   {
   }

   public interface IOSPSuiteXmlSerializerRepository : IXmlSerializerRepository<SerializationContext>
   {
      IAttributeMapperRepository<SerializationContext> AttributeMapperRepository { get; }
   }

   public class OSPSuiteXmlSerializerRepository : XmlSerializerRepositoryBase, IOSPSuiteXmlSerializerRepository
   {
      protected override void AddInitialMappers()
      {
         AttributeMapperRepository.AddAttributeMapper(new ParameterFlagAttributeMapper());
         AttributeMapperRepository.AddAttributeMapper(new DimensionAttributeMapper());
         AttributeMapperRepository.AddAttributeMapper(new ParameterBuildModeAttributeMapper());
         AttributeMapperRepository.AddAttributeMapper(new PKSimBuildingBlockTypeXmlAttributeMapper());
         AttributeMapperRepository.AddAttributeMapper(new OriginXmlAttributeMapper());
         AttributeMapperRepository.AddAttributeMapper(new DataColumnAttributeMapper());
         AttributeMapperRepository.AddAttributeMapper(new StringEnumerableAttributeMapper());
         AttributeMapperRepository.AddAttributeMapper(new TimeSpanAttributeMapper());
         AttributeMapperRepository.AddAttributeMapper(new EnumAttributeMapper<ContainerMode, SerializationContext>());
         AttributeMapperRepository.AddAttributeMapper(new EnumAttributeMapper<QuantityType, SerializationContext>());
         AttributeMapperRepository.AddAttributeMapper(new EnumAttributeMapper<TransportType, SerializationContext>());
         AttributeMapperRepository.AddAttributeMapper(new EnumAttributeMapper<ContainerType, SerializationContext>());
         AttributeMapperRepository.AddAttributeMapper(new EnumAttributeMapper<PKParameterMode, SerializationContext>());
         AttributeMapperRepository.AddAttributeMapper(new EnumAttributeMapper<AxisTypes, SerializationContext>());
         AttributeMapperRepository.AddAttributeMapper(new EnumAttributeMapper<Scalings, SerializationContext>());
         AttributeMapperRepository.AddAttributeMapper(new EnumAttributeMapper<NumberModes, SerializationContext>());
         AttributeMapperRepository.AddAttributeMapper(new EnumAttributeMapper<LegendPositions, SerializationContext>());
         AttributeMapperRepository.AddAttributeMapper(new EnumAttributeMapper<LineStyles, SerializationContext>());
         AttributeMapperRepository.AddAttributeMapper(new EnumAttributeMapper<Symbols, SerializationContext>());
         AttributeMapperRepository.AddAttributeMapper(new EnumAttributeMapper<InterpolationModes, SerializationContext>());
         AttributeMapperRepository.AddAttributeMapper(new EnumAttributeMapper<ReactionDimensionMode, SerializationContext>());
         AttributeMapperRepository.AddAttributeMapper(new EnumAttributeMapper<ClassificationType, SerializationContext>());
         AttributeMapperRepository.AddAttributeMapper(new EnumAttributeMapper<FormulaComparison, SerializationContext>());
         AttributeMapperRepository.AddAttributeMapper(new EnumAttributeMapper<CreationMode, SerializationContext>());
         AttributeMapperRepository.AddAttributeMapper(new EnumAttributeMapper<ColumnOrigins, SerializationContext>());
         AttributeMapperRepository.AddAttributeMapper(new EnumAttributeMapper<AuxiliaryType, SerializationContext>());
         AttributeMapperRepository.AddAttributeMapper(new LLOQModeXmlAttributeMapper<SerializationContext>());
         AttributeMapperRepository.AddAttributeMapper(new LLOQUsageXmlAttributeMapper<SerializationContext>());
         AttributeMapperRepository.AddAttributeMapper(new ValueOriginSourceXmlAttributeMapper<SerializationContext>());
         AttributeMapperRepository.AddAttributeMapper(new ValueOriginDeterminationMethodXmlAttributeMapper<SerializationContext>());
         AttributeMapperRepository.AddAttributeMapper(new RunStatusXmlAttributeMapper());

 
         AttributeMapperRepository.ReferenceMapper = new ObjectReferenceMapper();
      }

      protected override void AddInitialSerializer()
      {
         AddSerializersSimple<IOSPSuiteXmlSerializer>();
      }
   }
}