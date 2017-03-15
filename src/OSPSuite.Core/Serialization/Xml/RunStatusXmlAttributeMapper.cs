using OSPSuite.Serializer.Attributes;
using OSPSuite.Utility;
using OSPSuite.Core.Domain.ParameterIdentifications;

namespace OSPSuite.Core.Serialization.Xml
{
   public class RunStatusXmlAttributeMapper : AttributeMapper<RunStatus, SerializationContext>
   {
      public override object ConvertFrom(string runStatusId, SerializationContext context)
      {
         return RunStatus.FindById(EnumHelper.ParseValue<RunStatusId>(runStatusId));
      }

      public override string Convert(RunStatus runStatus, SerializationContext context)
      {
         return runStatus.Id.ToString();
      }
   }
}