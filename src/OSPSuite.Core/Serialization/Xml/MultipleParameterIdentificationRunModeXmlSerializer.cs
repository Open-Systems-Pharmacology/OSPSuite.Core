using OSPSuite.Core.Domain.ParameterIdentifications;

namespace OSPSuite.Core.Serialization.Xml
{
   public class MultipleParameterIdentificationRunModeXmlSerializer : OSPSuiteXmlSerializer<MultipleParameterIdentificationRunMode>
   {
      public override void PerformMapping()
      {
         Map(x => x.NumberOfRuns);
      }
   }
}