using System.Collections.Generic;
using OSPSuite.Presentation.Core;
using OSPSuite.Presentation.Services;

namespace OSPSuite.Starter.Tasks
{
   public class ToolTipPartCreator : IToolTipPartCreator
   {
      public IList<ToolTipPart> ToolTipFor<T>(T objectRequestingToolTip)
      {
         return new List<ToolTipPart>();
      }

      public IList<ToolTipPart> ToolTipFor(string toolTipToDisplay)
      {
         return new List<ToolTipPart>();
      }
   }
}