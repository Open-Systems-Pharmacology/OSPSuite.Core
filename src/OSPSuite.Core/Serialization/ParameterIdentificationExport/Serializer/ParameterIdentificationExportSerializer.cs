using System.Xml.Linq;
using OSPSuite.Serializer;
using OSPSuite.Serializer.Xml.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.ParameterIdentifications;

namespace OSPSuite.Core.Serialization.ParameterIdentificationExport.Serializer
{
   public class ParameterIdentificationExportSerializer : ParameterIdentificationExportSerializerBase<ParameterIdentification>
   {
      public override void PerformMapping()
      {
         ElementName = ParameterIdentificationExportSchemaConstants.ParameterIdentification;
         Map(x => x.Configuration);
         MapEnumerable(x => x.AllIdentificationParameters).WithMappingName(ParameterIdentificationExportSchemaConstants.IdentificationParameterList);
         MapEnumerable(x => x.AllOutputMappings).WithMappingName(ParameterIdentificationExportSchemaConstants.OutputMappingList);
      }

      protected override XElement TypedSerialize(ParameterIdentification parameterIdentification, ParameterIdentificationExportSerializationContext context)
      {
         var element = base.TypedSerialize(parameterIdentification, context);
         element.AddAttribute(ParameterIdentificationExportSchemaConstants.Attributes.ObjectPathDelimiter, ObjectPath.PATH_DELIMITER);
         element.AddAttribute(Constants.Serialization.Attribute.VERSION, Constants.PKML_VERSION);
         return element;
      }
   }
}