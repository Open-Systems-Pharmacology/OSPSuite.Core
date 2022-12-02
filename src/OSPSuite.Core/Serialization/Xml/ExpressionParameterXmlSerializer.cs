using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Serialization.Xml.Extensions;
using System.Xml.Linq;

namespace OSPSuite.Core.Serialization.Xml
{
   public class ExpressionParameterXmlSerializer : PathAndValueEntityXmlSerializer<ExpressionParameter>
   {
      public override void PerformMapping()
      {
         base.PerformMapping();
         Map(x => x.StartValue);
      }
   }
}
