using System;
using System.Linq;
using NUnit.Framework;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Chart;
using OSPSuite.Core.Serialization;
using OSPSuite.Utility.Container;

namespace OSPSuite.Core.Serializers
{
   public class CurveChartTemplateXmlSerializerSpecs : ModelingXmlSerializerBaseSpecs
   {
      [Test]
      public void TestSerialization()
      {
         var x1 = new CurveChartTemplate
         {
            Name = "Template",
            IsDefault = true,
            PreviewSettings = true,
            ChartType = CurveChartTypes.PredictedVsObserved
         };

         var x2 = SerializeAndDeserialize(x1);

         x2.Name.ShouldBeEqualTo(x1.Name);
         x2.IsDefault.ShouldBeEqualTo(x1.IsDefault);
         x2.PreviewSettings.ShouldBeEqualTo(x1.PreviewSettings);
         x2.ChartType.ShouldBeEqualTo(CurveChartTypes.PredictedVsObserved);
      }

      [Test]
      public void TestDeserializationOfTemplateCreatedBeforeChartTypeWasIntroduced()
      {
         var x1 = new CurveChartTemplate {Name = "Template", ChartType = CurveChartTypes.PredictedVsObserved};

         using (var serializationContext = SerializationTransaction.Create(IoC.Container))
         {
            var serializer = SerializerRepository.SerializerFor(x1);
            var xel = serializer.Serialize(x1, serializationContext);

            //legacy templates were serialized without the chart type attribute
            xel.Attributes().Single(x => string.Equals(x.Name.LocalName, "chartType", StringComparison.OrdinalIgnoreCase)).Remove();

            using (var deserializationContext = NewDeserializationContext())
            {
               var x2 = serializer.Deserialize<CurveChartTemplate>(xel, deserializationContext);
               x2.ChartType.ShouldBeEqualTo(CurveChartTypes.TimeProfile);
            }
         }
      }
   }
}
