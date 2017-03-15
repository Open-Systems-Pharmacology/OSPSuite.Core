using System;
using System.Collections.Generic;
using System.Xml.Linq;
using OSPSuite.Utility.Extensions;

namespace OSPSuite.Core.Serialization.Xml
{
   public abstract class ArrayXmlSerializer<T> : OSPSuiteXmlSerializer<T[]>
   {
      private readonly Func<string, T[]> _converter;

      protected ArrayXmlSerializer(Func<string, T[]> converter) : base($"{typeof (T).Name}Array")
      {
         _converter = converter;
      }

      public override void PerformMapping()
      {
         //nothing to do here
      }

      protected override XElement TypedSerialize(T[] array, SerializationContext serializationContext)
      {
         var element = SerializerRepository.CreateElement(ElementName);
         element.SetValue(array.ToByteString());
         return element;
      }

      protected virtual string StringValueFor(List<T> list)
      {
         return list.ToArray().ToByteString();
      }

      protected virtual T[] ListFrom(string value)
      {
         return _converter(value);
      }

      public override T[] CreateObject(XElement element, SerializationContext serializationContext)
      {
         if (element.Value != string.Empty)
            return ListFrom(element.Value);

         return null;
      }
   }

   public class FloatArrayXmlSerializer : ArrayXmlSerializer<float>
   {
      public FloatArrayXmlSerializer() : base(x => x.ToFloatArray())
      {
      }
   }

   public class DoubleArrayXmlSerializer : ArrayXmlSerializer<double>
   {
      public DoubleArrayXmlSerializer() : base(x => x.ToDoubleArray())
      {
      }
   }

   public class StringArrayXmlSerializer : ArrayXmlSerializer<string>
   {
      public StringArrayXmlSerializer() : base(x => x.ToStringArray())
      {
      }
   }

}