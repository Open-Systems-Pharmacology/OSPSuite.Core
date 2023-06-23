using OSPSuite.Core.Domain.Formulas;

namespace OSPSuite.Core.Serialization.Xml
{
   public class DistributionMetaDataXmlSerializer : OSPSuiteXmlSerializer<DistributionMetaData>
   {
      public override void PerformMapping()
      {
         Map(x => x.Mean);
         Map(x => x.Deviation);
         Map(x => x.Distribution);
      }
   }
}