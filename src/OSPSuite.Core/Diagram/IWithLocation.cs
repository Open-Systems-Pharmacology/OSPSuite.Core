using System.Drawing;

namespace OSPSuite.Core.Diagram
{
   public interface IWithLocation
   {
      PointF Location { get; set; }
      PointF Center { get; set; }
      SizeF Size { get; set; }
      RectangleF Bounds { get; set; }
   }
}