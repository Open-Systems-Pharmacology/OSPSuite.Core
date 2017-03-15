using OSPSuite.Core.Domain.ParameterIdentifications;

namespace OSPSuite.Core.Serialization.Xml
{
   public class CategorialParameterIdentificationRunModeXmlSerializer : OSPSuiteXmlSerializer<CategorialParameterIdentificationRunMode>
   {
      public override void PerformMapping()
      {
         Map(x => x.AllTheSame);
         Map(x => x.AllTheSameSelection);
         Map(x => x.CalculationMethodsCache);
      }
   }
}