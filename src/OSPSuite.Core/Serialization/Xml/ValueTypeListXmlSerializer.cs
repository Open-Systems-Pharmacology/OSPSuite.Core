using System;
using System.Collections.Generic;
using System.Xml.Linq;
using OSPSuite.Utility.Compression;
using OSPSuite.Utility.Container;
using OSPSuite.Utility.Extensions;

namespace OSPSuite.Core.Serialization.Xml
{
   public abstract class ValueTypeListXmlSerializer<T> : OSPSuiteXmlSerializer<List<T>>
   {
      private readonly Func<string, T[]> _converter;

      protected ValueTypeListXmlSerializer(Func<string, T[]> converter, string name)
         : base(name)
      {
         _converter = converter;
      }

      protected ValueTypeListXmlSerializer(Func<string, T[]> converter)
         : this(converter, string.Format("{0}List", typeof(T).Name))
      {
      }

      public override void PerformMapping()
      {
         //nothing to do here
      }

      protected override XElement TypedSerialize(List<T> list, SerializationContext context)
      {
         var element = SerializerRepository.CreateElement(ElementName);
         element.SetValue(StringValueFor(list));
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

      public override List<T> CreateObject(XElement element, SerializationContext context)
      {
         var list = new List<T>();
         if (element.Value != string.Empty)
            list.AddRange(ListFrom(element.Value));

         return list;
      }
   }

   public class DoubleListXmlSerializer : ValueTypeListXmlSerializer<double>
   {
      public DoubleListXmlSerializer()
         : base(x => x.ToDoubleArray())
      {
      }
   }

   public class FloatListXmlSerializer : ValueTypeListXmlSerializer<float>
   {
      public FloatListXmlSerializer()
         : base(x => x.ToFloatArray())
      {
      }
   }

   public class NullableDoubleListXmlSerializer : ValueTypeListXmlSerializer<double?>
   {
      public NullableDoubleListXmlSerializer()
         : base(x => x.ToNullableDoubleArray(), "NullableDoubleList")
      {
      }
   }

   public class StringListXmlSerializer : ValueTypeListXmlSerializer<string>
   {
      private readonly IStringCompression _stringCompression;

      public StringListXmlSerializer()
         : base(x => x.ToStringArray())
      {
         _stringCompression = IoC.Resolve<IStringCompression>();
      }

      protected override string StringValueFor(List<string> list)
      {
         var value = base.StringValueFor(list);
         return _stringCompression.Compress(value);
      }

      protected override string[] ListFrom(string value)
      {
         var decompressedValue = _stringCompression.Decompress(value);
         return base.ListFrom(decompressedValue);
      }
   }

   public class IntegerListXmlSerializer : ValueTypeListXmlSerializer<int>
   {
      public IntegerListXmlSerializer()
         : base(x => x.ToIntegerArray())
      {
      }
   }
}