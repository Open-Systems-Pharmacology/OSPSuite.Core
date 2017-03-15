using System.Xml.Linq;
using OSPSuite.Serializer.Xml.Extensions;
using OSPSuite.Utility.Collections;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Data;

namespace OSPSuite.Core.Serialization.Xml
{
   public class DataColumnCacheXmlSerializer : OSPSuiteXmlSerializer<Cache<AuxiliaryType, DataColumn>>
   {
      public DataColumnCacheXmlSerializer() : base(Constants.Serialization.COLUMN_CACHE)
      {
      }

      public override void PerformMapping()
      {
      }

      public override Cache<AuxiliaryType, DataColumn> CreateObject(XElement outputToDeserialize, SerializationContext serializationContext)
      {
         //Delegate for Key has to be defined in constructor
         return new Cache<AuxiliaryType, DataColumn>(x => x.DataInfo.AuxiliaryType);
      }

      protected override XElement TypedSerialize(Cache<AuxiliaryType, DataColumn> objectToSerialize, SerializationContext serializationContext)
      {
         var xel = base.TypedSerialize(objectToSerialize, serializationContext);

         foreach (var relatedColumn in objectToSerialize)
         {
            xel.AddElement(new XElement(Constants.Serialization.Attribute.COLUMN_ID, relatedColumn.Id));
         }

         return xel;
      }

      protected override void TypedDeserialize(Cache<AuxiliaryType, DataColumn> cache, XElement xElement, SerializationContext serializationContext)
      {
         base.TypedDeserialize(cache, xElement, serializationContext);
         var collectionReference = serializationContext.AddDataCollectionReference(cache);
         foreach (var subXel in xElement.Elements())
         {
            collectionReference.Add(subXel.Value);
         }
      }
   }
}