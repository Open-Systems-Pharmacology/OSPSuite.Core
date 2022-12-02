using System.Xml.Linq;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Serialization.Xml.Extensions;

namespace OSPSuite.Core.Serialization.Xml
{
   public abstract class ExpressionParameterXmlSerializer : PathAndValueEntityXmlSerializer<ExpressionParameter>
   {
      public override void PerformMapping()
      {
         base.PerformMapping();
         Map(x => x.StartValue);
      }
   }
}
