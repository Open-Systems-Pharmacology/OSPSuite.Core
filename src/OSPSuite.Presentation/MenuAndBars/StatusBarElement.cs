using System.Drawing;

namespace OSPSuite.Presentation.MenuAndBars
{
   public enum StatusBarElementType
   {
      Text,
      ProgressBar
   }

   public enum StatusBarElementSize
   {
      None,
      Spring,
      Content,
   }

   public enum StatusBarElementAlignment
   {
      Left,
      Right
   }
   
   public class StatusBarElement
   {
      public StatusBarElementAlignment Alignment { get; private set; }
      public StatusBarElementType ElementType { get; private set; }
      public StatusBarElementSize AutoSize { get; private set; }
      public StringAlignment TextAlignment { get; private set; }
      public int Width { get; private set; }
      public int Index { get; set; }

      public StatusBarElement(StatusBarElementType elementType, StatusBarElementSize statusBarElementSize, StatusBarElementAlignment statusBarElementAlignment, int width, StringAlignment textAlignment)
      {
         ElementType = elementType;
         Alignment = statusBarElementAlignment;
         AutoSize = statusBarElementSize;
         TextAlignment = textAlignment;
         if (AutoSize != StatusBarElementSize.Content)
            Width = width;
      }
   }

 
}