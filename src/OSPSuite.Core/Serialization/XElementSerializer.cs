using System.IO;
using System.Xml;
using System.Xml.Linq;

namespace OSPSuite.Core.Serialization
{
   public static class XElementSerializer
   {
      /// <summary>
      /// Saves XElement to <paramref name="fileName"/> allowing illegal characters to be escaped and saved
      /// </summary>
      public static void PermissiveSave(XElement element, string fileName)
      {
         using (var xmlWriter = XmlWriter.Create(fileName, new XmlWriterSettings
                {
                   CheckCharacters = false,
                   Indent = true
                }))
         {
            element.Save(xmlWriter);
         }
      }

      public static XElement PermissiveLoad(Stream stream)
      {
         using (var xmlReader = XmlReader.Create(stream, new XmlReaderSettings { CheckCharacters = false }))
         {
            return XElement.Load(xmlReader);
         }
      }

      public static XElement PermissiveLoad(string fileName)
      {
         using (var xmlReader = XmlReader.Create(fileName, new XmlReaderSettings { CheckCharacters = false }))
         {
            return XElement.Load(xmlReader);
         }
      }
   }
}