using OSPSuite.Core.Domain;

namespace OSPSuite.Core.Serialization.Xml
{
   public class QuantityPKParameterXmlSerializer : OSPSuiteXmlSerializer<QuantityPKParameter>
   {
      public override void PerformMapping()
      {
         Map(x => x.Dimension);
         Map(x => x.Name);
         Map(x => x.QuantityPath);
         Map(x => x.Values);
      }
   }
}