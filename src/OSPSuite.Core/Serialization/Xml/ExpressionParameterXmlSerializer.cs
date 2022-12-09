using OSPSuite.Core.Domain.Builder;

namespace OSPSuite.Core.Serialization.Xml
{
   public class ExpressionParameterXmlSerializer : PathAndValueEntityXmlSerializer<ExpressionParameter>
   {
      public override void PerformMapping()
      {
         base.PerformMapping();
         Map(x => x.Value);
      }
   }
}