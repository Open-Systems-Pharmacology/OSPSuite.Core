using System.Linq;
using System.Xml.Linq;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Serializer.Xml.Extensions;

namespace OSPSuite.Core.Serialization.Xml;

public abstract class ParameterValueXmlSerializer<T> : PathAndValueEntityXmlSerializer<T> where T : ParameterValue
{
   public override void PerformMapping()
   {
      base.PerformMapping();
      Map(x => x.Origin);
      Map(x => x.Info);
      Map(x => x.IsDefault);
   }
}

public abstract class ParameterValueWithInitialStateSerializer<T> : ParameterValueXmlSerializer<T> where T : ParameterValueWithInitialState
{
   public override void PerformMapping()
   {
      base.PerformMapping();
      Map(x => x.InitialValue);
      Map(x => x.InitialFormulaId);
   }

   protected override XElement TypedSerialize(T pathAndValueEntity, SerializationContext serializationContext)
   {
      var element = base.TypedSerialize(pathAndValueEntity, serializationContext);
      if (pathAndValueEntity.InitialUnit is not null)
         element.AddAttribute(Constants.Serialization.Attribute.INITIAL_UNIT, pathAndValueEntity.InitialUnit.Name);

      return element;
   }

   protected override void TypedDeserialize(T pathAndValueEntity, XElement element, SerializationContext serializationContext)
   {
      base.TypedDeserialize(pathAndValueEntity, element, serializationContext);

      // element.GetAttribute will return empty string if the attribute is not there. That's a problem if
      // the dimension is Dimensionless because the empty string is a valid unit. If the attribute is not there
      // we need InitialUnit to be null, not empty string
      if (!element.Attributes(Constants.Serialization.Attribute.INITIAL_UNIT).Any())
         return;

      var initialUnit = element.GetAttribute(Constants.Serialization.Attribute.INITIAL_UNIT);
      pathAndValueEntity.InitialUnit = pathAndValueEntity.Dimension.UnitOrDefault(initialUnit);
   }
}

public class ParameterValueXmlSerializer : ParameterValueXmlSerializer<ParameterValue>
{
}

public class IndividualParameterXmlSerializer : ParameterValueWithInitialStateSerializer<IndividualParameter>
{
}

public class ExpressionParameterXmlSerializer : ParameterValueWithInitialStateSerializer<ExpressionParameter>
{
}