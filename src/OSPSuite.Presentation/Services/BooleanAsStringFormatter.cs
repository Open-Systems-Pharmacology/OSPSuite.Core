using OSPSuite.Utility.Format;

namespace OSPSuite.Presentation.Services
{
   public class BooleanAsStringFormatter : IFormatter<bool>
   {
      public string Format(bool valueToFormat)
      {
         return valueToFormat ? "True" : "False";
      }
   }
}