using OSPSuite.Core.Domain.ParameterIdentifications;

namespace OSPSuite.Core.Serialization.Xml
{
   public class ParameterIdentificationRunResultXmlSerializer : OSPSuiteXmlSerializer<ParameterIdentificationRunResult>
   {
      public override void PerformMapping()
      {
         Map(x => x.Index);
         Map(x => x.Description);
         Map(x => x.Properties);
         Map(x => x.BestResult);
         Map(x => x.Status);
         Map(x => x.Message);
         Map(x => x.Duration);
         Map(x => x.JacobianMatrix);
      }
   }
}