using System.Collections.Generic;
using System.Linq;
using OSPSuite.Utility;
using DevExpress.Utils;
using OSPSuite.Presentation.Core;
using OSPSuite.UI.Extensions;

namespace OSPSuite.UI.Mappers
{
   public class ToolTipPartsToSuperToolTipMapper : IMapper<IEnumerable<ToolTipPart>, SuperToolTip>
   {
      public SuperToolTip MapFrom(IEnumerable<ToolTipPart> toolTipParts)
      {
         var superToolTip = new SuperToolTip();
         superToolTip.Appearance.TextOptions.HotkeyPrefix = HKeyPrefix.None;

         for (int i = 0; i < toolTipParts.Count(); i++)
         {
            var toolTipPart = toolTipParts.ElementAt(i);
            superToolTip.WithTitle(toolTipPart.Title);
            superToolTip.WithText(toolTipPart.Content);

            if (i != toolTipParts.Count() - 1)
               superToolTip.Items.AddSeparator();

         }

         return superToolTip;
      }
   }
}