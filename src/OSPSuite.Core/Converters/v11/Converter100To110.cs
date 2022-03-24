using System.Linq;
using System.Xml.Linq;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Serialization;
using OSPSuite.Utility.Extensions;

namespace OSPSuite.Core.Converters.v11
{
   public class Converter100To110 : IObjectConverter
   {
      public bool IsSatisfiedBy(int version) => version == PKMLVersion.V10_0;
      private bool _converted;

      public (int convertedToVersion, bool conversionHappened) Convert(object objectToUpdate)
      {
         return (PKMLVersion.V11_0, false);
      }

      public (int convertedToVersion, bool conversionHappened) ConvertXml(XElement element)
      {
         element.DescendantsAndSelf("QuantityPKParameter").Each(convertQuantityPKParameterElement);
         return (PKMLVersion.V11_0, _converted);
      }

      private void convertQuantityPKParameterElement(XElement element)
      {
         // Serialized XML Looks like
         // <QuantityPKParameter dimension = "Concentration (mass)" name = "C_max" quantityPath = "A|B|C">
         //    <Values>AAEAAAD /////AQAAAAAAAAAPAQAAAAMAAAALAACgQAAAgEAAAABACw==</Values>
         //  </QuantityPKParameter>

         var valuesElement = element.Element(Constants.Serialization.VALUES);
         var floats = valuesElement.Value.ToFloatArray();
         var individualIds = Enumerable.Range(0, floats.Length).ToArray();

         //Remove from parent node before adding it under the cache
         valuesElement.Remove();
         var valueCache = new XElement("ValueCache");

         valueCache.Add(valuesElement);
         valueCache.Add(new XElement(Constants.Serialization.KEYS, individualIds.ToByteArray()));
         element.Add(valueCache);
         _converted = true;
      }
   }
}