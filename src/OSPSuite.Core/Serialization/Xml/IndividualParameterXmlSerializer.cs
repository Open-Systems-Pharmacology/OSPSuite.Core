using OSPSuite.Core.Domain.Builder;

namespace OSPSuite.Core.Serialization.Xml
{
   public class IndividualParameterXmlSerializer : PathAndValueEntityXmlSerializer<IndividualParameter>
   {
      public override void PerformMapping()
      {
         base.PerformMapping();
         Map(x => x.Origin);
         Map(x => x.Info);
         Map(x => x.IsDefault);
      }
   }
}