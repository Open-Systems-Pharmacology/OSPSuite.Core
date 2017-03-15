using OSPSuite.Utility.Format;
using OSPSuite.Core.Extensions;

namespace OSPSuite.Presentation.Services
{
   public class SplitToUpperCaseFormatter : IFormatter<string>
   {
      public string Format(string valueToFormat)
      {
         return valueToFormat.SplitToUpperCase();
      }
   }
}