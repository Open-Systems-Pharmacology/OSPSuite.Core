using System;

namespace OSPSuite.Presentation.Core
{

   [Flags]
   public enum DragDropKeyState
   {
      None = 0,
      LeftMouseButton = 1,
      RightMouseButton = 2,
      ShiftKey= 4,
      CtrlKey = 8,
      MiddleMouseButton = 16,
      AltKey = 32
   }

   public static class DragDropKeyStateHelper
   {
      public static DragDropKeyState ConvertKeyState(int keyState)
      {
         DragDropKeyState dragDropKeyState = DragDropKeyState.None;

         if ((keyState & 1) == 1) dragDropKeyState |= DragDropKeyState.LeftMouseButton;
         if ((keyState & 2) == 2) dragDropKeyState |= DragDropKeyState.RightMouseButton;
         if ((keyState & 4) == 4) dragDropKeyState |= DragDropKeyState.ShiftKey;
         if ((keyState & 8) == 8) dragDropKeyState |= DragDropKeyState.CtrlKey;
         if ((keyState & 16) == 16) dragDropKeyState |= DragDropKeyState.MiddleMouseButton;
         if ((keyState & 32) == 32) dragDropKeyState |= DragDropKeyState.AltKey;

         return dragDropKeyState;
      }
   }
}
