using System.Xml.Linq;
using OSPSuite.Serializer;
using OSPSuite.Serializer.Xml;
using OSPSuite.Serializer.Xml.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Data;

namespace OSPSuite.Core.Serialization.Xml
{
   public class DataColumnXmlSerializer<T> : OSPSuiteXmlSerializer<T> where T : DataColumn, new()
   {
      public DataColumnXmlSerializer(string name) : base(name)
      {
      }

      public override void PerformMapping()
      {
         Map(x => x.Id).WithMappingName(Constants.Serialization.Attribute.ID);
         Map(x => x.Name);
         Map(x => x.DataInfo);
         Map(x => x.QuantityInfo);
         Map(x => x.Dimension).AsAttribute();
         Map(x => x.InternalValues).WithMappingName(Constants.Serialization.VALUES);
         Map(x => x.RelatedColumnsCache);
         Map(x => x.IsInternal);
         MapReference(x => x.BaseGrid);
      }

      public override T CreateObject(XElement element, SerializationContext serializationContext)
      {
         var id = element.GetAttribute(Constants.Serialization.Attribute.ID);
         return new T().WithId(id).WithDimension(Constants.Dimension.NO_DIMENSION);
      }

      protected override void TypedDeserialize(T column, XElement element, SerializationContext context)
      {
         base.TypedDeserialize(column, element, context);
         context.Register(column);
      }
   }

   public class DataColumnXmlSerializer : DataColumnXmlSerializer<DataColumn>
   {
      public DataColumnXmlSerializer() : base(Constants.Serialization.DATA_COLUMN)
      {
      }
   }

   public class BaseGridXmlSerializer : DataColumnXmlSerializer<BaseGrid>
   {
      public BaseGridXmlSerializer() : base(Constants.Serialization.BASE_GRID)
      {
      }
   }
}