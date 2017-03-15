using System.Xml.Linq;
using OSPSuite.Serializer.Xml.Extensions;
using OSPSuite.Utility.Extensions;
using OSPSuite.Core.Serialization;
using OSPSuite.Core.Serialization.Xml.Extensions;

namespace OSPSuite.Core.Converter.v6_4
{
   public class Converter63To64 : IObjectConverter
   {
      public bool IsSatisfiedBy(int version)
      {
         return version == PKMLVersion.V6_3_1;
      }

      public int Convert(object objectToUpdate)
      {
         return PKMLVersion.V6_4_2;
      }

      public int ConvertXml(XElement element)
      {
         element.DescendantsAndSelfNamed("CurveChartTemplate").Each(convertCurveChartTemplate);
         return PKMLVersion.V6_4_2;
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