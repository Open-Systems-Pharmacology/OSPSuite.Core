using OSPSuite.Core.Extensions;
using OSPSuite.Utility.Format;

namespace OSPSuite.Presentation.Formatters
{
   public class SplitToUpperCaseFormatter : IFormatter<string>
   {
      public string Format(string valueToFormat)
      {
         return valueToFormat.SplitToUpperCase();
      }
   }
}