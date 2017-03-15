using System.Collections.Generic;
using OSPSuite.Presentation.Core;

namespace OSPSuite.Presentation.Services
{
   public interface IToolTipPartCreator
   {
      IList<ToolTipPart> ToolTipFor<T>(T objectRequestingToolTip);
      IList<ToolTipPart> ToolTipFor(string toolTipToDisplay);
   }
}