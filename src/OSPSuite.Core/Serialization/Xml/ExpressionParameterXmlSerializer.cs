using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Serialization.Xml.Extensions;
using System.Xml.Linq;

namespace OSPSuite.Core.Serialization.Xml
{
   public class ExpressionParameterXmlSerialize : PathAndValueEntityXmlSerializer<ExpressionParameter>
   {
      public override void PerformMapping()
      {
         base.PerformMapping();
         Map(x => x.StartValue);
      }
   }
   
   /*
   public abstract class ExpressionParameterXmlSerialize<T> : EntityXmlSerializer<T> where T : ExpressionParameter
   {
      public override void PerformMapping()
      {
         base.PerformMapping();
         Map(x => x.Dimension);
         Map(x => x.ContainerPath);
         Map(x => x.StartValue);
         MapReference(x => x.Formula);
      }

      protected override void TypedDeserialize(T startValue, XElement startValueElement, SerializationContext serializationContext)
      {
         base.TypedDeserialize(startValue, startValueElement, serializationContext);
         startValueElement.UpdateDisplayUnit(startValue);
      }

      protected override XElement TypedSerialize(T startValue, SerializationContext serializationContext)
      {
         var startValueElement = base.TypedSerialize(startValue, serializationContext);
         return startValueElement.AddDisplayUnitFor(startValue);
      }
   }
   */
}
