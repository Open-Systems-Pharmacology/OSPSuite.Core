using System;
using System.Drawing;
using System.Xml.Linq;
using OSPSuite.Serializer;
using OSPSuite.Serializer.Xml.Extensions;

namespace OSPSuite.Core.Serialization.Xml
{
   public class SizeXmlSerializer : OSPSuiteXmlSerializer<Size>
   {
      public override Size CreateObject(XElement element, SerializationContext context)
      {
         return new Size { Height = getHeight(element), Width = getWidth(element) };
      }

      private int getWidth(XElement element)
      {
         return Convert.ToInt32(element.GetAttribute(Domain.Constants.Serialization.Attribute.Width));
      }

      private int getHeight(XElement element)
      {
         return Convert.ToInt32(element.GetAttribute(Domain.Constants.Serialization.Attribute.Height));
      }

      public override void PerformMapping()
      {
         Map(x => x.Height).WithMappingName(Domain.Constants.Serialization.Attribute.Height);
         Map(x => x.Width).WithMappingName(Domain.Constants.Serialization.Attribute.Width);
      }
   }
}