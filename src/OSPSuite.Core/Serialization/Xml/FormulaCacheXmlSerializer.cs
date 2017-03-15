using System.Xml.Linq;
using OSPSuite.Serializer;
using OSPSuite.Serializer.Xml.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Formulas;

namespace OSPSuite.Core.Serialization.Xml
{
   public class FormulaCacheXmlSerializer : OSPSuiteXmlSerializer<FormulaCache>
   {
      public override void PerformMapping()
      {
         MapEnumerable(x => x, x => x.Add).WithMappingName(Constants.Serialization.FORMULAS);
      }

      protected override XElement TypedSerialize(FormulaCache formulaCache, SerializationContext serializationContext)
      {
         var formulaCacheElement = base.TypedSerialize(formulaCache, serializationContext);
         var stringMapListElement = SerializerRepository.CreateElement(Constants.Serialization.STRING_MAP_LIST);
         foreach (var stringMap in serializationContext.StringMap.KeyValues)
         {
            var stringMapElement = SerializerRepository.CreateElement(Constants.Serialization.STRING_MAP);
            stringMapElement.AddAttribute(Constants.Serialization.Attribute.STRING, stringMap.Key);
            stringMapElement.AddAttribute(Constants.Serialization.Attribute.ID, stringMap.Value.ToString());
            stringMapListElement.Add(stringMapElement);
         }
         formulaCacheElement.Add(stringMapListElement);
         return formulaCacheElement;
      }

      protected override void TypedDeserialize(FormulaCache formulaCache, XElement formulaCacheNode, SerializationContext serializationContext)
      {
         //before deserializing formula cache, load id path map
         serializationContext.IdStringMap.Clear();
         cacheStringMap(formulaCacheNode.Element(Constants.Serialization.STRING_MAP_LIST), serializationContext);
         base.TypedDeserialize(formulaCache, formulaCacheNode, serializationContext);
      }

      private static void cacheStringMap(XElement stringMapListElement, SerializationContext serializationContext)
      {
         if (stringMapListElement == null) return;
         foreach (var stringMapElement in stringMapListElement.Elements(Constants.Serialization.STRING_MAP))
         {
            var mappedString = stringMapElement.GetAttribute(Constants.Serialization.Attribute.STRING);
            var id = int.Parse(stringMapElement.GetAttribute(Constants.Serialization.Attribute.ID));
            serializationContext.IdStringMap.Add(id, mappedString);
         }
      }
   }
}