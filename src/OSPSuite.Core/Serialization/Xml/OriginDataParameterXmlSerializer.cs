using OSPSuite.Core.Domain;

namespace OSPSuite.Core.Serialization.Xml
{
   public class OriginDataParameterXmlSerializer : OSPSuiteXmlSerializer<OriginDataParameter>
   {
      public override void PerformMapping()
      {
         Map(x => x.Name);
         Map(x => x.Value);
         Map(x => x.Unit);
      }
   }
}