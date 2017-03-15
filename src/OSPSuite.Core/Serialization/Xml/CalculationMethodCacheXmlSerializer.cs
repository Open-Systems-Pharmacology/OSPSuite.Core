using OSPSuite.Core.Domain;

namespace OSPSuite.Core.Serialization.Xml
{
   public class CalculationMethodCacheXmlSerializer : OSPSuiteXmlSerializer<CalculationMethodCache>
   {
      public override void PerformMapping()
      {
         MapEnumerable(x => x.All(), x => x.AddCalculationMethod);
      }
   }
}