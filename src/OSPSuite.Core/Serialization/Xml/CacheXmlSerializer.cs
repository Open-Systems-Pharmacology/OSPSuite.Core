using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using OSPSuite.Utility.Collections;
using OSPSuite.Core.Domain;

namespace OSPSuite.Core.Serialization.Xml
{
   public abstract class CacheXmlSerializer<TKey, TValue> : OSPSuiteXmlSerializer<Cache<TKey, TValue>>
   {
      protected CacheXmlSerializer(string name)
         : base(name)
      {
      }

      protected CacheXmlSerializer()
         : this(string.Format("{0}{1}Cache", typeof(TKey).Name, typeof(TValue).Name))
      {
      }

      public override void PerformMapping()
      {
      }

      protected override void TypedDeserialize(Cache<TKey, TValue> cache, XElement element, SerializationContext serializationContext)
      {
         base.TypedDeserialize(cache, element, serializationContext);
         var keysElement = element.Element(Constants.Serialization.KEYS);
         var valuesElement = element.Element(Constants.Serialization.VALUES);
         var keyListSerializer = SerializerRepository.SerializerFor<List<TKey>>();
         var valueListSerializer = SerializerRepository.SerializerFor<List<TValue>>();
         var keys = keyListSerializer.Deserialize<List<TKey>>(keysElement, serializationContext);
         var values = valueListSerializer.Deserialize<List<TValue>>(valuesElement, serializationContext);

         for (int i = 0; i < keys.Count; i++)
         {
            cache.Add(keys[i], values[i]);
         }
      }

      protected override XElement TypedSerialize(Cache<TKey, TValue> cache, SerializationContext serializationContext)
      {
         var element = base.TypedSerialize(cache, serializationContext);
         var keyListSerializer = SerializerRepository.SerializerFor<List<TKey>>();
         var valueListSerializer = SerializerRepository.SerializerFor<List<TValue>>();
         var keysElement = keyListSerializer.Serialize(cache.Keys.ToList(), serializationContext);
         keysElement.Name = Constants.Serialization.KEYS;
         var valuesElement = valueListSerializer.Serialize(cache.ToList(), serializationContext);
         valuesElement.Name = Constants.Serialization.VALUES;
         element.Add(keysElement);
         element.Add(valuesElement);
         return element;
      }
   }

   public class StringStringCacheXmlSerializer : CacheXmlSerializer<string, string>
   {
   }

   public class StringNullableDoubleCacheXmlSerializer : CacheXmlSerializer<string, double?>
   {
      public StringNullableDoubleCacheXmlSerializer()
         : base(string.Format("StringNullableDoubleCache"))
      {
      }
   }
}
