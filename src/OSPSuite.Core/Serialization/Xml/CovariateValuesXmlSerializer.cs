using OSPSuite.Core.Domain.Populations;

namespace OSPSuite.Core.Serialization.Xml
{
   public class CovariateValuesXmlSerializer : OSPSuiteXmlSerializer<CovariateValues>
   {
      public override void PerformMapping()
      {
         Map(x => x.CovariateName);
         Map(x => x.Values);
      }
   }
}