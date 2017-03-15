using OSPSuite.Core.Domain.ParameterIdentifications;

namespace OSPSuite.Core.Serialization.Xml
{
   public class ParameterIdentificationConfigurationXmlSerializer : OSPSuiteXmlSerializer<ParameterIdentificationConfiguration>
   {
      public override void PerformMapping()
      {
         Map(x => x.LLOQMode);
         Map(x => x.RemoveLLOQMode);
         Map(x => x.CalculateJacobian);
         Map(x => x.AlgorithmProperties);
         Map(x => x.RunMode);
      }
   }
}