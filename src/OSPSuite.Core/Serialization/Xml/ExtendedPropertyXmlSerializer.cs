using OSPSuite.Core.Domain;

namespace OSPSuite.Core.Serialization.Xml
{
   public abstract class ExtendedPropertyXmlSerializer<T> : OSPSuiteXmlSerializer<ExtendedProperty<T>>
   {
      protected ExtendedPropertyXmlSerializer() : this($"{typeof (T).Name}ExtendedProperty")
      {
      }

      protected ExtendedPropertyXmlSerializer(string name)
         : base(name)
      {
      }

      public override void PerformMapping()
      {
         Map(x => x.Name);
         Map(x => x.ReadOnly);
         Map(x => x.Value);
         Map(x => x.FullName);
         Map(x => x.Description);
      }
   }

   public class StringExtendedPropertyXmlSerializer : ExtendedPropertyXmlSerializer<string>
   {
   }

   public class DoubleExtendedPropertyXmlSerializer : ExtendedPropertyXmlSerializer<double>
   {
   }

   public class IntExtendedPropertyXmlSerializer : ExtendedPropertyXmlSerializer<int>
   {
   }

   public class UintExtendedPropertyXmlSerializer : ExtendedPropertyXmlSerializer<uint>
   {
   }

   public class BoolExtendedPropertyXmlSerializer : ExtendedPropertyXmlSerializer<bool>
   {
   }

   public class NullableDoubleExtendedPropertyXmlSerializer : ExtendedPropertyXmlSerializer<double?>
   {
      protected NullableDoubleExtendedPropertyXmlSerializer() : base($"{"NullableDouble"}ExtendedProperty")
      {
      }
   }
}