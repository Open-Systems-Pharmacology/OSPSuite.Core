using OSPSuite.Core.Domain.Populations;

namespace OSPSuite.Core.Serialization.Xml
{
   public class CovariateValuesCacheXmlSerializer : OSPSuiteXmlSerializer<CovariateValuesCache>
   {
      public override void PerformMapping()
      {
         MapEnumerable(x => x.AllCovariateValues, cache => cache.Add);

      }
   }
}