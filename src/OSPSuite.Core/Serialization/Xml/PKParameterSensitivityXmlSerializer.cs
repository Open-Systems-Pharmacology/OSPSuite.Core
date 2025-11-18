using OSPSuite.Core.Domain.SensitivityAnalyses;

namespace OSPSuite.Core.Serialization.Xml
{
   public class PKParameterSensitivityXmlSerializer : OSPSuiteXmlSerializer<PKParameterSensitivity>
   {
      public override void PerformMapping()
      {
         //No need to serialize the Id as it is a readonly field based on all other properties
         Map(x => x.QuantityPath);
         Map(x => x.ParameterName);
         Map(x => x.ParameterPath);
         Map(x => x.PKParameterName);
         Map(x => x.Value);
         Map(x => x.State);
      }
   }
}