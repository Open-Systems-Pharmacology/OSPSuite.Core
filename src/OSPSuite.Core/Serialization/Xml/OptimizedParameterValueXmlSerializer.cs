using OSPSuite.Core.Domain.ParameterIdentifications;

namespace OSPSuite.Core.Serialization.Xml
{
   public class OptimizedParameterValueXmlSerializer : OSPSuiteXmlSerializer<OptimizedParameterValue>
   {
      public override void PerformMapping()
      {
         Map(x => x.Name);
         Map(x => x.Value);
         Map(x => x.StartValue);
      }
   }
}