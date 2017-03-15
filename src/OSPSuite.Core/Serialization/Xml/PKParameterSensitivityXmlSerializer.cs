using OSPSuite.Core.Domain.SensitivityAnalyses;

namespace OSPSuite.Core.Serialization.Xml
{
   public class PKParameterSensitivityXmlSerializer : OSPSuiteXmlSerializer<PKParameterSensitivity>
   {
      public override void PerformMapping()
      {
         Map(x => x.QuantityPath);
         Map(x => x.ParameterName);
         Map(x => x.PKParameterName);
         Map(x => x.Value);
      }
   }
}