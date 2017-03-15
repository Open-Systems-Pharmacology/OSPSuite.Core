using OSPSuite.Core.Domain;

namespace OSPSuite.Core.Serialization.Xml
{
   public class PartialDerivativesXmlSerializer : OSPSuiteXmlSerializer<PartialDerivatives>
   {
      public override void PerformMapping()
      {
         Map(x => x.FullOutputPath);
         Map(x => x.ParameterNames);
         MapEnumerable(x => x.AllPartialDerivatives, x => x.AddPartialDerivative);
      }
   }
}