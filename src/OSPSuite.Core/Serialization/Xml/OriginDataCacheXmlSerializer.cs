using System.Xml.Linq;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Utility.Collections;
using OSPSuite.Utility.Extensions;

namespace OSPSuite.Core.Serialization.Xml
{
   public class OriginDataCacheXmlSerializer : OSPSuiteXmlSerializer<OriginDataCache>
   {
      // public class CategoryItemXmlSerializer<TItem> : ObjectBaseXmlSerializer<TItem> where TItem : CategoryItem
      // {
      //    public override void PerformMapping()
      //    {
      //       base.PerformMapping();
      //       Map(x => x.DisplayName);
      //       Map(x => x.Category);
      //    }
      // }
      //
      // public class CalculationMethodXmlSerializer : CategoryItemXmlSerializer<CalculationMethod>
      // {
      //    public override void PerformMapping()
      //    {
      //       base.PerformMapping();
      //       Map(x => x.AllModels);
      //       Map(x => x.AllSpecies);
      //    }
      // }

      protected OriginDataCacheXmlSerializer(string name)
         : base(name)
      {
      }

      protected OriginDataCacheXmlSerializer() 
      {
      }

      public override void PerformMapping()
      {
         Map(x => x.ValueOrigin);
      }

      protected override void TypedDeserialize(OriginDataCache cache, XElement element, SerializationContext serializationContext)
      {
         base.TypedDeserialize(cache, element, serializationContext);
         var cacheElement = element.Element(Constants.Serialization.VALUES);
         var stringStringCacheSerializer = SerializerRepository.SerializerFor<Cache<string, string>>();
         var values = stringStringCacheSerializer.Deserialize<Cache<string,string>>(cacheElement, serializationContext);
         values.Keys.Each(x => cache.Add(x, values[x]));
      }

      protected override XElement TypedSerialize(OriginDataCache cache, SerializationContext serializationContext)
      {
         var element = base.TypedSerialize(cache, serializationContext);
         var stringStringCacheSerializer = SerializerRepository.SerializerFor<Cache<string, string>>();
         var cacheElement = stringStringCacheSerializer.Serialize(cache, serializationContext);
         cacheElement.Name = Constants.Serialization.VALUES;
         element.Add(cacheElement);
         return element;
      }
   }
}