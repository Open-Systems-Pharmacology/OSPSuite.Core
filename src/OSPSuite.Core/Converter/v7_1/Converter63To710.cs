using System.Xml.Linq;
using OSPSuite.Core.Serialization;
using OSPSuite.Core.Serialization.Xml.Extensions;
using OSPSuite.Serializer.Xml.Extensions;
using OSPSuite.Utility.Extensions;

namespace OSPSuite.Core.Converter.v7_1
{
   public class Converter63To710 : IObjectConverter
   {
      private bool _converted;

      public bool IsSatisfiedBy(int version)
      {
         return version == PKMLVersion.V6_3_1;
      }

      public (int convertedToVersion, bool conversionHappened) Convert(object objectToUpdate)
      {
         return (PKMLVersion.V7_1_0, false);
      }

      public (int convertedToVersion, bool conversionHappened) ConvertXml(XElement element)
      {
         _converted = true;
         element.DescendantsAndSelfNamed("CurveChartTemplate").Each(convertCurveChartTemplate);
         return (PKMLVersion.V7_1_0, _converted);
      }

      private void convertCurveChartTemplate(XElement chartTemplateElement)
      {
         chartTemplateElement.Descendants("CurveTemplate").Each(convertCurveTemplate);
      }

      private void convertCurveTemplate(XElement element)
      {
         var xDataPath = element.GetAttribute("xDataPath");
         var yDataPath = element.GetAttribute("yDataPath");
         var xQuantityType = element.GetAttribute("xQuantityType");
         var yQuantityType = element.GetAttribute("yQuantityType");

         element.Add(dataTemplateFor("xData", xDataPath, xQuantityType));
         element.Add(dataTemplateFor("yData", yDataPath, yQuantityType));
         _converted = true;
      }

      private XElement dataTemplateFor(string name, string dataPath, string quantityType)
      {
         var element = new XElement(name);
         element.SetAttributeValue("path", dataPath);
         element.SetAttributeValue("quantityType", quantityType);
         return element;
      }
   }
}