using System;

namespace OSPSuite.Presentation.Core
{

   [Flags]
   public enum DragDropKeyFlags
   {
      None = 0,
      LeftMouseButton = 1,
      RightMouseButton = 2,
      ShiftKey= 4,
      CtrlKey = 8,
      MiddleMouseButton = 16,
      AltKey = 32
   }

}
