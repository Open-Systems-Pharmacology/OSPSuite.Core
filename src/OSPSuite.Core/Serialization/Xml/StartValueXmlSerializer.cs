using System.Xml.Linq;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Serialization.Xml.Extensions;

namespace OSPSuite.Core.Serialization.Xml
{
   public abstract class StartValueXmlSerializer<T> : EntityXmlSerializer<T> where T : PathAndValueEntity
   {
      public override void PerformMapping()
      {
         base.PerformMapping();
         Map(x => x.Dimension);
         Map(x => x.ContainerPath);
         Map(x => x.Value);
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

   public class InitialConditionXmlSerializer : StartValueXmlSerializer<InitialCondition>
   {
      public override void PerformMapping()
      {
         base.PerformMapping();
         Map(x => x.ScaleDivisor);
         Map(x => x.IsPresent);
         Map(x => x.NegativeValuesAllowed);
      }
   }

   public class ParameterValueXmlSerializer : StartValueXmlSerializer<ParameterValue>
   {
      public override void PerformMapping()
      {
         base.PerformMapping();
         Map(x => x.ValueOrigin);
         Map(x => x.IsDefault);
      }
   }
}