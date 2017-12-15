using System;
using System.Drawing;
using System.Xml.Linq;
using OSPSuite.Serializer;
using OSPSuite.Serializer.Xml.Extensions;
using OSPSuite.Core.Domain;

namespace OSPSuite.Core.Serialization.Xml
{
   public class PointXmlSerializer : OSPSuiteXmlSerializer<Point>
   {
      public override Point CreateObject(XElement element, SerializationContext context)
      {
         return new Point {X = getX(element), Y = getY(element)};
      }

      private int getX(XElement element)
      {
         return getValue(element.GetAttribute(Constants.Serialization.Attribute.X));
      }

      private int getValue(string attributeValue)
      {
         return Convert.ToInt32(attributeValue);
      }

      private int getY(XElement element)
      {
         return getValue(element.GetAttribute(Constants.Serialization.Attribute.Y));
      }

      public override void PerformMapping()
      {
         Map(x => x.X).WithMappingName(Constants.Serialization.Attribute.X);
         Map(x => x.Y).WithMappingName(Constants.Serialization.Attribute.Y);
      }
   }
}