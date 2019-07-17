using OSPSuite.Utility.Format;

namespace OSPSuite.Presentation.Formatters
{
   public class BooleanAsStringFormatter : IFormatter<bool>
   {
      public string Format(bool valueToFormat)
      {
         return valueToFormat ? "True" : "False";
      }
   }
}