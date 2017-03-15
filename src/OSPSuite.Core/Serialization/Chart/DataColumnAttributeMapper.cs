using System;
using OSPSuite.Serializer.Attributes;
using OSPSuite.Utility.Extensions;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Serialization.Xml;

namespace OSPSuite.Core.Serialization.Chart
{
   public class DataColumnAttributeMapper : AttributeMapper<DataColumn, SerializationContext>
   {
      private static readonly string Null = "null";

      public override string Convert(DataColumn dataColumn, SerializationContext context)
      {
         return dataColumn == null ? Null : dataColumn.Id;
      }

      public override object ConvertFrom(string attributeValue, SerializationContext context)
      {
         if (attributeValue == Null) return null;
         return context.DataColumnById(attributeValue);
      }

      public override bool IsMatch(Type attributeType)
      {
         return attributeType.IsAnImplementationOf<DataColumn>();
      }
   }
}