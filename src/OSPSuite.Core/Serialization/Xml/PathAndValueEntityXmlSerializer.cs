using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Serialization.Xml.Extensions;
using System.Xml.Linq;

namespace OSPSuite.Core.Serialization.Xml
{
   public abstract class PathAndValueEntityXmlSerializer<T> : EntityXmlSerializer<T> where T : PathAndValueEntity
   {
      public override void PerformMapping()
      {
         base.PerformMapping();
         Map(x => x.DistributionType);
         Map(x => x.Dimension);
         Map(x => x.ContainerPath);
         MapReference(x => x.Formula);
         Map(x => x.Value);
         Map(x => x.ValueOrigin);
      }

      protected override void TypedDeserialize(T pathAndValueEntity, XElement element, SerializationContext serializationContext)
      {
         base.TypedDeserialize(pathAndValueEntity, element, serializationContext);
         element.UpdateDisplayUnit(pathAndValueEntity);
      }

      protected override XElement TypedSerialize(T pathAndValueEntity, SerializationContext serializationContext)
      {
         var element = base.TypedSerialize(pathAndValueEntity, serializationContext);
         return element.AddDisplayUnitFor(pathAndValueEntity);
      }
   }

   public class InitialConditionXmlSerializer : PathAndValueEntityXmlSerializer<InitialCondition>
   {
      public override void PerformMapping()
      {
         base.PerformMapping();
         Map(x => x.ScaleDivisor);
         Map(x => x.IsPresent);
         Map(x => x.NegativeValuesAllowed);
      }
   }
}