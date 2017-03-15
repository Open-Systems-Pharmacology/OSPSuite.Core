using System;
using System.Linq.Expressions;
using System.Reflection;
using OSPSuite.Serializer.Xml;
using OSPSuite.Utility.Reflection;

namespace OSPSuite.Core.Serialization.Xml
{
   public abstract class OSPSuiteXmlSerializer<T> : XmlSerializer<T, SerializationContext>, IOSPSuiteXmlSerializer
   {
      protected OSPSuiteXmlSerializer()
      {
      }

      protected OSPSuiteXmlSerializer(string name) : base(name)
      {
      }

      protected static PropertyInfo Property(Expression<Func<T, object>> property)
      {
         return ReflectionHelper.PropertyFor(property);
      }
   }
}