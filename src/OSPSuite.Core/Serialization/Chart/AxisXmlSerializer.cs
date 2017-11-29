using System.Xml.Linq;
using OSPSuite.Serializer.Xml;
using OSPSuite.Core.Chart;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Core.Serialization.Xml;
using OSPSuite.Serializer;
using OSPSuite.Serializer.Xml.Extensions;

namespace OSPSuite.Core.Serialization.Chart
{
   public class AxisXmlSerializer : OSPSuiteXmlSerializer<Axis>
   {
      public override void PerformMapping()
      {
         Map(x => x.AxisType);
         Map(x => x.Caption);
         Map(x => x.Dimension).AsAttribute();
         Map(x => x.UnitName).WithMappingName(Constants.Serialization.Attribute.UNIT_NAME);
         Map(x => x.Min);
         Map(x => x.Max);
         Map(x => x.Scaling);
         Map(x => x.NumberMode);
         Map(x => x.GridLines);
         Map(x => x.Visible);
         Map(x => x.DefaultLineStyle);
         Map(x => x.DefaultColor);
      }

      protected override void TypedDeserialize(Axis axis, XElement element, SerializationContext serializationContext)
      {
         base.TypedDeserialize(axis, element, serializationContext);
         axis.Dimension = serializationContext.DimensionFactory.OptimalDimension(axis.Dimension);

         //deserialized unit might not have been kept if the axis was using a merged dimension. We need to reset the Unit Name after the dimension was updated
         axis.UnitName = element.GetAttribute(Constants.Serialization.Attribute.UNIT_NAME);
      }
   }
}