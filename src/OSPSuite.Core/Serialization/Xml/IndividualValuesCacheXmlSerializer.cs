using OSPSuite.Core.Domain.Populations;

namespace OSPSuite.Core.Serialization.Xml
{
   public class IndividualValuesCacheXmlSerializer : OSPSuiteXmlSerializer<IndividualValuesCache>
   {
      public override void PerformMapping()
      {
         Map(x => x.IndividualIds);
         Map(x => x.ParameterValuesCache);
         Map(x => x.CovariateValuesCache);
      }
   }
}