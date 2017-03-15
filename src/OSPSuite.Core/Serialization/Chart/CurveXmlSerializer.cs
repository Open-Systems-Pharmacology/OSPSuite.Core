using System.Xml.Linq;
using OSPSuite.Serializer.Xml;
using OSPSuite.Core.Chart;
using OSPSuite.Core.Serialization.Xml;

namespace OSPSuite.Core.Serialization.Chart
{
   public class CurveXmlSerializer : OSPSuiteXmlSerializer<Curve>
   {
      public override void PerformMapping()
      {
         Map(x => x.Name);
         Map(x => x.Description);
         Map(x => x.xData).AsAttribute();
         Map(x => x.yData).AsAttribute();
         Map(x => x.CurveOptions);
      }

      protected override void TypedDeserialize(Curve curve, XElement outputToDeserialize, SerializationContext serializationContext)
      {
         base.TypedDeserialize(curve, outputToDeserialize, serializationContext);

         if (curve.xData != null)
            curve.SetxData(curve.xData, serializationContext.DimensionFactory);

         if (curve.yData != null)
            curve.SetyData(curve.yData, serializationContext.DimensionFactory);
      }
   }
}