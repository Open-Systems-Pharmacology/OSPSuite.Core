using OSPSuite.Core.Domain.ParameterIdentifications;

namespace OSPSuite.Core.Serialization.Xml
{
   public class ParameterIdentificationAlgorithmXmlSerializer : OSPSuiteXmlSerializer<OptimizationAlgorithmProperties>
   {
      public override void PerformMapping()
      {
         Map(x => x.Name);
         MapEnumerable(x => x, x => x.Add);
      }
   }
}