using OSPSuite.Core.Domain.SensitivityAnalyses;

namespace OSPSuite.Core.Serialization.Xml
{
   public class SensitivityParameterXmlSerializer : ContainerXmlSerializer<SensitivityParameter>
   {
      public override void PerformMapping()
      {
         base.PerformMapping();
         Map(x => x.ParameterSelection);
      }
   }
}