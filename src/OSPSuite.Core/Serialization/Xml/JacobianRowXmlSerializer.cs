using OSPSuite.Core.Domain;

namespace OSPSuite.Core.Serialization.Xml
{
   public class JacobianRowXmlSerializer : OSPSuiteXmlSerializer<JacobianRow>
   {
      public override void PerformMapping()
      {
         Map(x => x.FullOutputPath);
         Map(x => x.Time);
         Map(x => x.Values);
      }
   }
}