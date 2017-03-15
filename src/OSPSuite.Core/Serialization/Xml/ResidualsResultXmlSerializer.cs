using OSPSuite.Core.Domain.ParameterIdentifications;

namespace OSPSuite.Core.Serialization.Xml
{
   public class ResidualsResultXmlSerializer:OSPSuiteXmlSerializer<ResidualsResult>
   {
      public override void PerformMapping()
      {
         Map(x => x.ExceptionOccured);
         Map(x => x.ExceptionMessage);
         MapEnumerable(x => x.AllOutputResiduals, x => x.AddOutputResiduals);

      }
   }
}