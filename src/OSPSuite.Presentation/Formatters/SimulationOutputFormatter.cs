using OSPSuite.Assets;
using OSPSuite.Presentation.DTO;
using OSPSuite.Utility.Format;

namespace OSPSuite.Presentation.Formatters
{
   public class SimulationOutputFormatter : IFormatter<SimulationQuantitySelectionDTO>
   {
      public string Format(SimulationQuantitySelectionDTO valueToFormat)
      {
         if (valueToFormat == null)
            return Captions.SimulationUI.NoneEditorNullText;

         return valueToFormat.ToString();
      }
   }
}