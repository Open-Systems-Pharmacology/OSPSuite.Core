using OSPSuite.Core.Domain.Builder;

namespace OSPSuite.Core.Serialization.Xml
{
   public class UsedCalculationMethodXmlSerializer : OSPSuiteXmlSerializer<UsedCalculationMethod>
   {
      public override void PerformMapping()
      {
         Map(x => x.Category);
         Map(x => x.CalculationMethod);
      }
   }
}