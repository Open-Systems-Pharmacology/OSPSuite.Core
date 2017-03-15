using System.Xml.Linq;
using OSPSuite.Serializer.Xml;
using OSPSuite.Core.Chart;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Core.Serialization.Xml;

namespace OSPSuite.Core.Serialization.Chart
{
   public class AxisXmlSerializer : OSPSuiteXmlSerializer<Axis>
   {
      public override void PerformMapping()
      {
         Map(x => x.AxisType);
         Map(x => x.Caption);
         Map(x => x.Dimension).AsAttribute();
         Map(x => x.UnitName);
         Map(x => x.Min);
         Map(x => x.Max);
         Map(x => x.Scaling);
         Map(x => x.NumberMode);
         Map(x => x.GridLines);
         Map(x => x.Visible);
         Map(x => x.DefaultLineStyle);
         Map(x => x.DefaultColor);
      }

      protected override void TypedDeserialize(Axis axis, XElement outputToDeserialize, SerializationContext serializationContext)
      {
         base.TypedDeserialize(axis, outputToDeserialize, serializationContext);
         axis.Dimension = serializationContext.DimensionFactory.OptimalDimension(axis.Dimension);
      }
   }
}