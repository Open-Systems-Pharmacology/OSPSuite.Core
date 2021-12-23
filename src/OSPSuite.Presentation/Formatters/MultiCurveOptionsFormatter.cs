using OSPSuite.Assets;
using OSPSuite.Core.Chart;
using OSPSuite.Utility.Format;

namespace OSPSuite.Presentation.Formatters
{
   public class BooleanYesNoFormatter : IFormatter<bool?>
   {
      public string Format(bool? valueToFormat)
      {
         if (valueToFormat == null)
            return Captions.Chart.MultiCurveOptions.CurrentValue;

         return  valueToFormat.Value ? "Yes" : "No";
      }
   }

   public class LineStylesFormatter : IFormatter<LineStyles?>
   {
      public string Format(LineStyles? valueToFormat)
      {
         if (valueToFormat == null)
            return Captions.Chart.MultiCurveOptions.CurrentValue;

         return  valueToFormat.Value.ToString();
      }
   }

   public class SymbolsFormatter : IFormatter<Symbols?>
   {
      public string Format(Symbols? valueToFormat)
      {
         if (valueToFormat == null)
            return Captions.Chart.MultiCurveOptions.CurrentValue;

         return valueToFormat.Value.ToString();
      }
   }
}