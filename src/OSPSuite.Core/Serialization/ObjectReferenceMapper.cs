using OSPSuite.Serializer;
using OSPSuite.Serializer.Attributes;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Core.Serialization.Xml;

namespace OSPSuite.Core.Serialization
{
   public class ObjectReferenceMapper : IReferenceMapper<SerializationContext>
   {
      public string ReferenceFor(object valueToConvert, SerializationContext serializationContext)
      {
         //not an object with id or null... return null reference
         var withId = valueToConvert as IWithId;
         if (withId == null)
            return string.Empty;

   
         //add to cache if the reference is also a formula
         var formula = withId as IFormula;
         if (formula == null)
            return withId.Id;

         return serializationContext.AddFormulaToCache(formula);
      }

      public void ResolveReference(object objectToDeserialize, IPropertyMap propertyMap, string referenceValue, SerializationContext serializationContext)
      {
         if (string.IsNullOrEmpty(referenceValue)) return;
         serializationContext.AddModelReference(objectToDeserialize, propertyMap, referenceValue);
      }
   }
}
