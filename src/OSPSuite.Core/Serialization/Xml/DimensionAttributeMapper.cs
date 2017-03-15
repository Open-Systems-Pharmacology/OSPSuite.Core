using System;
using OSPSuite.Serializer.Attributes;
using OSPSuite.Utility.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.UnitSystem;

namespace OSPSuite.Core.Serialization.Xml
{
   public class DimensionAttributeMapper : AttributeMapper<IDimension, SerializationContext>
   {
      public override string Convert(IDimension dimension, SerializationContext context)
      {
         if (dimension == null)
            return string.Empty;

         if (string.Equals(dimension.Name, Constants.Dimension.DIMENSIONLESS))
            return string.Empty;

         return dimension.Name;
      }

      public override object ConvertFrom(string attributeValue, SerializationContext context)
      {
         if (string.IsNullOrEmpty(attributeValue))
            return Constants.Dimension.NO_DIMENSION;

         return context.DimensionByName(attributeValue);
      }

      public override bool IsMatch(Type attributeType)
      {
         return attributeType.IsAnImplementationOf<IDimension>();
      }
   }
}