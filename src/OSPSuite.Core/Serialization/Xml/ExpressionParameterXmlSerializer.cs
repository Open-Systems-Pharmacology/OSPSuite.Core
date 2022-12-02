using OSPSuite.Core.Domain.Builder;

namespace OSPSuite.Core.Serialization.Xml
{
   public abstract class ExpressionParameterXmlSerialize : PathAndValueEntityXmlSerializer<ExpressionParameter>
   {
      public override void PerformMapping()
      {
         base.PerformMapping();
         Map(x => x.StartValue);
      }
   }
}
