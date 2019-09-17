using OSPSuite.Core.Domain.Populations;

namespace OSPSuite.Core.Serialization.Xml
{
   public class ParameterValuesCacheXmlSerializer : OSPSuiteXmlSerializer<ParameterValuesCache>
   {
      public override void PerformMapping()
      {
         MapEnumerable(x => x.AllParameterValues, x => x.Add);
      }
   }
}