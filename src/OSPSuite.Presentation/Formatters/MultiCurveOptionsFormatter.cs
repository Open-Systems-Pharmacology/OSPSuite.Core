using System.Drawing;
using NPOI.OpenXmlFormats.Dml;
using OSPSuite.Assets;
using OSPSuite.Core.Chart;
using OSPSuite.Core.Domain;
using OSPSuite.Utility.Format;

namespace OSPSuite.Presentation.Formatters
{
   public class BooleanYesNoFormatter : IFormatter<bool?>
   {
      public string Format(bool? valueToFormat)
      {
         if (valueToFormat == null)
            return Captions.Chart.MultiCurveOptions.CurrentValue;

         return (bool)valueToFormat ? "Yes" : "No";
      }
   }

   public class LineStylesFormatter : IFormatter<LineStyles?>
   {
      public string Format(LineStyles? valueToFormat)
      {
         if (valueToFormat == null)
            return Captions.Chart.MultiCurveOptions.CurrentValue;

         return ((LineStyles)valueToFormat).ToString();
      }
   }
   public class SymbolsFormatter : IFormatter<Symbols?>
   {
      public string Format(Symbols? valueToFormat)
      {
         if (valueToFormat == null)
            return Captions.Chart.MultiCurveOptions.CurrentValue;

         return ((Symbols)valueToFormat).ToString();
      }
   }
}
