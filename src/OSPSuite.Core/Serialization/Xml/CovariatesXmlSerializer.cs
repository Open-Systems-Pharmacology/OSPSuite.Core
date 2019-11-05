using OSPSuite.Core.Domain.Populations;

namespace OSPSuite.Core.Serialization.Xml
{
   public class CovariatesXmlSerializer : OSPSuiteXmlSerializer<Covariates>
   {
      public override void PerformMapping()
      {
         Map(x => x.Attributes);
      }
   }
}