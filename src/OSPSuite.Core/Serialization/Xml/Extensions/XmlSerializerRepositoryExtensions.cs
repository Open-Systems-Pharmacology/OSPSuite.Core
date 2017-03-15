using System.Linq;
using System.Xml.Linq;
using OSPSuite.Serializer.Xml;
using OSPSuite.Serializer.Xml.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Formulas;

namespace OSPSuite.Core.Serialization.Xml.Extensions
{
   public static class XmlSerializerRepositoryExtensions
   {
      public static void AddFormulaCacheElement(this IXmlSerializerRepository<SerializationContext> serializerRepository, XElement element, SerializationContext serializationContext)
      {
         var formulasSerializer = serializerRepository.SerializerFor(serializationContext.Formulas);
         var formulaCacheElement = formulasSerializer.Serialize(serializationContext.Formulas, serializationContext);
         serializationContext.ClearFomulaCache();
         if (!formulaCacheElement.HasElements)
            return;

         //only one node and that node is the string map list
         if (formulaCacheElement.Descendants().Count() == 1 && formulaCacheElement.Element(Constants.Serialization.STRING_MAP_LIST) != null)
            return;

         element.AddElement(formulaCacheElement);
      }

      public static IFormulaCache DeserializeFormulaCacheIn(this IXmlSerializerRepository<SerializationContext> serializerRepository, XElement element, SerializationContext serializationContext)
      {
         var formulaCacheSerializer = serializerRepository.SerializerFor(serializationContext.Formulas);
         var formulaNode = element.Element(formulaCacheSerializer.ElementName);
         if (formulaNode == null)
            return new FormulaCache();

         return formulaCacheSerializer.Deserialize<IFormulaCache>(formulaNode, serializationContext);
      }
   }
}