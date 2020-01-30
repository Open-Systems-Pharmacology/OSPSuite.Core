using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Core.Extensions;
using OSPSuite.Core.Serialization;

namespace OSPSuite.Core.Converter.v9
{
   public class Converter730To90 : IObjectConverter
   {
      private readonly IDimensionFactory _dimensionFactory;

      public Converter730To90(IDimensionFactory dimensionFactory)
      {
         _dimensionFactory = dimensionFactory;
      }

      public bool IsSatisfiedBy(int version) => version == PKMLVersion.V7_3_0;

      public (int convertedToVersion, bool conversionHappened) Convert(object objectToUpdate)
      {
         return (PKMLVersion.V9_0, false);
      }

      public (int convertedToVersion, bool conversionHappened) ConvertXml(XElement element)
      {
         var conversionHappened = false;
         convertDimensionIn(element, ref conversionHappened);
         return (PKMLVersion.V9_0, conversionHappened);
      }

      private void convertDimensionIn(XElement element, ref bool conversionHappened)
      {
         //retrieve all elements with an attribute dimension
         var allDimensionAttributes = from child in element.DescendantsAndSelf()
            where child.HasAttributes
            let attr = child.Attribute(Constants.Serialization.Attribute.Dimension) ?? child.Attribute("dimension")
            where attr != null
            select attr;


         var allMappedDimensionIds = new HashSet<string>();
         foreach (var attribute in allDimensionAttributes)
         {
            string attributeValue = attribute.Value;
            if (int.TryParse(attribute.Value, out _))
               allMappedDimensionIds.Add(attributeValue);
            else
            {
               var newDimensionName = mapDimensionName(attributeValue);
               if (!string.Equals(newDimensionName, attributeValue))
               {
                  attribute.SetValue(newDimensionName);
                  conversionHappened = true;
               }
            }
         }


         var allMapAttributes = from child in element.Descendants(Constants.Serialization.STRING_MAP)
            let id = child.Attribute(Constants.Serialization.Attribute.ID)
            where allMappedDimensionIds.Contains(id.Value)
            select child.Attribute(Constants.Serialization.Attribute.STRING);

         foreach (var attribute in allMapAttributes)
         {
            var oldDimensionName = attribute.Value;
            var newDimensionName = mapDimensionName(attribute.Value);
            if (!string.Equals(oldDimensionName, newDimensionName))
            {
               attribute.SetValue(newDimensionName);
               conversionHappened = true;
            }
         }
      }

      private string mapDimensionName(string dimensionName)
      {
         if (_dimensionFactory.Has(dimensionName))
            return dimensionName;

         if (dimensionName.IsOneOf($"{Constants.Dimension.DIMENSIONLESS} per Time", $"{Constants.Dimension.DIMENSIONLESS} per time", "Fraction per time", "Fraction per Time"))
            return "Inversed time";

         // Some old dimensions that were not converted properly and that require some TLC 
         if (dimensionName.IsOneOf("Rate"))
            return Constants.Dimension.AMOUNT_PER_TIME;

         return dimensionName;
      }
   }
}