using OSPSuite.Core.Domain.ParameterIdentifications;

namespace OSPSuite.Core.Serialization.Xml
{
   public class OutputResidualXmlSerializer : OSPSuiteXmlSerializer<OutputResiduals>
   {
      public override void PerformMapping()
      {
         Map(x => x.FullOutputPath);
         MapReference(x => x.ObservedData);
         MapEnumerable(x => x.Residuals, x => x.Add);
      }
   }
}