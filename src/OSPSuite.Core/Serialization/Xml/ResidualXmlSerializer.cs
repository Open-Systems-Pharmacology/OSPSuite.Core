using OSPSuite.Core.Domain.ParameterIdentifications;

namespace OSPSuite.Core.Serialization.Xml
{
   public class ResidualXmlSerializer : OSPSuiteXmlSerializer<Residual>
   {
      public override void PerformMapping()
      {
         Map(x => x.Time);
         Map(x => x.Value);
         Map(x => x.Weight);
      }
   }
}