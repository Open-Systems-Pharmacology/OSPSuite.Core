using DevExpress.XtraRichEdit.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OSPSuite.Presentation.Importer.Core.DataFormat
{
   public class DataFormat_Nonmem : AbstractColumnsDataFormat
   {
      public override string Name => "Nonmem Oriented";

      public override string Description => "https://github.com/Open-Systems-Pharmacology/OSPSuite.Core/issues/797";

      protected override Func<int, string> extractUnits(string description, IUnformattedData data, List<string> keys)
      {
         var unitKey = data.GetHeaders().FirstOrDefault(h => h.ToUpper() == (description.ToUpper() + "_UNIT"));
         if (unitKey == null)
         {
            return _ => "?";
         }
         keys.Remove(unitKey);
         var units = data.GetColumn(unitKey).ToList();
         var def = units.FirstOrDefault(u => !string.IsNullOrEmpty(u));
         return i => (i > 0) ? units[i] : def;
      }
   }
}
