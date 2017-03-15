using System.Xml.Linq;
using OSPSuite.Serializer.Xml.Extensions;
using OSPSuite.Utility.Container;
using OSPSuite.Utility.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Extensions;

namespace OSPSuite.Core.Serialization.Xml
{
   public class ObjectPathXmlSerializer<T> : OSPSuiteXmlSerializer<T> where T : class, IObjectPath
   {
      public ObjectPathXmlSerializer()
      {
      }

      public ObjectPathXmlSerializer(string name) : base(name)
      {
      }

      public override void PerformMapping()
      {
         //nothing to do here. The path will be serialized as string to optimized xml size
      }

      protected override XElement TypedSerialize(T objectPath, SerializationContext serializationContext)
      {
         var objectPathElement = base.TypedSerialize(objectPath, serializationContext);
         objectPathElement.AddAttribute(Constants.Serialization.Attribute.PATH, serializationContext.IdForString(objectPath.PathAsString));
         return objectPathElement;
      }

      protected override void TypedDeserialize(T objectPath, XElement objectPathElement, SerializationContext serializationContext)
      {
         base.TypedDeserialize(objectPath, objectPathElement, serializationContext);
         int pathId = int.Parse(objectPathElement.GetAttribute(Constants.Serialization.Attribute.PATH));
         var allPathEntries = serializationContext.StringForId(pathId).ToPathArray();
         allPathEntries.Each(objectPath.Add);
      }
   }

   public class ObjectPathXmlSerializer : ObjectPathXmlSerializer<ObjectPath>
   {
   }

   public class FormulaUsablePathXmlSerializer : ObjectPathXmlSerializer<FormulaUsablePath>
   {
      public FormulaUsablePathXmlSerializer() : base(Constants.Serialization.FORMULA_USABLE_PATH)
      {
      }

      protected override void TypedDeserialize(FormulaUsablePath objectPath, XElement objectPathElement, SerializationContext serializationContext)
      {
         base.TypedDeserialize(objectPath, objectPathElement, serializationContext);
         int aliasId = int.Parse(objectPathElement.GetAttribute(Constants.Serialization.Attribute.ALIAS));
         objectPath.Alias = serializationContext.StringForId(aliasId);

         string dimAttribute = objectPathElement.GetAttribute(Constants.Serialization.Attribute.Dimension);

         if (string.IsNullOrEmpty(dimAttribute))
            objectPath.Dimension = Constants.Dimension.NO_DIMENSION;
         else
            objectPath.Dimension = serializationContext.DimensionByName(serializationContext.StringForId(int.Parse(dimAttribute)));
      }

      protected override XElement TypedSerialize(FormulaUsablePath objectPath, SerializationContext serializationContext)
      {
         var objectPathElement = base.TypedSerialize(objectPath,serializationContext);
         objectPathElement.AddAttribute(Constants.Serialization.Attribute.ALIAS, serializationContext.IdForString(objectPath.Alias));

         string dimensionName = string.Empty;
         if (objectPath.Dimension != null && !string.Equals(objectPath.Dimension.Name, Constants.Dimension.DIMENSIONLESS))
            dimensionName = objectPath.Dimension.Name;

         if (!string.IsNullOrEmpty(dimensionName))
            objectPathElement.AddAttribute(Constants.Serialization.Attribute.Dimension, serializationContext.IdForString(dimensionName));

         return objectPathElement;
      }
   }

   public class TimePathXmlSerializer : OSPSuiteXmlSerializer<TimePath>
   {
      public override TimePath CreateObject(XElement element, SerializationContext serializationContext)
      {
         var objectPathFactory = serializationContext.Resolve<IObjectPathFactory>();
         return objectPathFactory.CreateTimePath(serializationContext.DimensionByName(Constants.Dimension.TIME));
      }

      public override void PerformMapping()
      {
         //Nothing to do, a new timepath object is enough
      }
   }
}