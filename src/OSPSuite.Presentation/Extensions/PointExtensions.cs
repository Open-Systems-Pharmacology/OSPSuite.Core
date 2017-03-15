using System;
using System.Drawing;

namespace OSPSuite.Presentation.Extensions
{
   public static class PointExtensions
   {
      public static PointF Center(this RectangleF rectangle)
      {
         return new PointF(rectangle.Left + rectangle.Width * 0.5F, rectangle.Top + rectangle.Height * 0.5F);
      }

      public static SizeF Times(this SizeF size, float factor)
      {
         return new SizeF(size.Width * factor, size.Height * factor);
      }

      public static PointF Plus(this PointF p1, PointF p2)
      {
         return new PointF(p1.X + p2.X, p1.Y + p2.Y);
      }

      public static PointF Minus(this PointF p1, PointF p2)
      {
         return new PointF(p1.X - p2.X, p1.Y - p2.Y);
      }

      public static float DistanceTo(this PointF p1, PointF p2)
      {
         return Convert.ToSingle(Math.Sqrt((p1.X - p2.X) * (p1.X - p2.X) + (p1.Y - p2.Y) * (p1.Y - p2.Y)));
      }
   }
}