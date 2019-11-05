using OSPSuite.Core.Domain.Populations;

namespace OSPSuite.Core.Serialization.Xml
{
   public class IndividualValuesCacheXmlSerializer : OSPSuiteXmlSerializer<IndividualValuesCache>
   {
      public override void PerformMapping()
      {
         Map(x => x.ParameterValuesCache);
         MapEnumerable(x => x.AllCovariates, cache => cache.AllCovariates.Add);
      }
   }
}