using OSPSuite.Core.Domain.ParameterIdentifications;

namespace OSPSuite.Core.Serialization.Xml
{
   public class OptimizationRunPropertiesXmlSerializer : OSPSuiteXmlSerializer<OptimizationRunProperties>
   {
      public override void PerformMapping()
      {
         Map(x => x.NumberOfEvaluations);
      }
   }
}