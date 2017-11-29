using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using OSPSuite.Utility.Extensions;
using DevExpress.XtraCharts;
using OSPSuite.UI.Controls;

namespace OSPSuite.UI.Services
{
   public class DiagramZoomRectangleService
   {
      private readonly UxChartControl _chartControl;
      private readonly Action<Control, Rectangle> _zoomAction;

      private readonly Cursor _magnifierCursor;
      private static readonly Color _selectionRectColor = Color.FromArgb(89, Color.FromArgb(0xf1ea6f));
      private static readonly Color _selectionRectBorderColor = Color.FromArgb(89, Color.FromArgb(0xbaa500));

      private Point _firstSelectionCorner = Point.Empty;
      private Point _lastSelectionCorner = Point.Empty;
      private Rectangle _selectionRectangle = Rectangle.Empty;

      public DiagramZoomRectangleService(UxChartControl chartControl, Action<Control, Rectangle> zoomAction)
      {
         _chartControl = chartControl;
         _zoomAction = zoomAction;
         _magnifierCursor = Cursors.Cross;
         _magnifierCursor = new Cursor(GetType(), "MagnifierCursor.cur");
         _chartControl.CustomPaint += (o, e) => this.DoWithinExceptionHandler(() => OnCustomPaint(o, e));
         _chartControl.MouseDown += (o, e) => this.DoWithinExceptionHandler(() => OnMouseDown(o, e));
         _chartControl.MouseMove += (o, e) => this.DoWithinExceptionHandler(() => OnMouseMove(o, e));
         _chartControl.MouseUp += (o, e) => this.DoWithinExceptionHandler(() => OnMouseUp(o, e));

         if (_chartControl.XYDiagram != null)
         {
            _chartControl.XYDiagram.EnableAxisXZooming = true;
            _chartControl.XYDiagram.EnableAxisYZooming = true;
         }
      }

      // to draw the rectangle for the Zoom without Shift
      public void OnCustomPaint(object sender, CustomPaintEventArgs e)
      {
         if (_selectionRectangle.IsEmpty)
            return;

         Graphics g = e.Graphics;
         g.SmoothingMode = SmoothingMode.AntiAlias;

         g.FillRectangle(new SolidBrush(_selectionRectColor), _selectionRectangle);
         g.DrawRectangle(new Pen(_selectionRectBorderColor), _selectionRectangle);
      }

      public void OnMouseDown(object sender, MouseEventArgs e)
      {
         if (!isZooming(e)) return;
         if (!mouseIsOverChart(e)) return;
         if (isInLegend(e)) return;

         _firstSelectionCorner = _lastSelectionCorner = e.Location;
         _chartControl.Cursor = _magnifierCursor;
      }

      private bool isInLegend(MouseEventArgs mouseEventArgs)
      {
         var hitInfo = _chartControl.CalcHitInfo(mouseEventArgs.Location);
         return hitInfo.InLegend;
      }

      public void OnMouseMove(object sender, MouseEventArgs e)
      {
         if (isInLegend(e) || !mouseIsOverChart(e))
         {
            _chartControl.Cursor = Cursors.Default;
            return;
         }

         _chartControl.Cursor = _magnifierCursor;

         if (_firstSelectionCorner.IsEmpty)
            return;

         // To allow zooming outside the border
         _lastSelectionCorner = e.Location;
         if (!_lastSelectionCorner.IsEmpty && _firstSelectionCorner != _lastSelectionCorner)
         {
            _selectionRectangle = getRectangle(_firstSelectionCorner, _lastSelectionCorner);
            _chartControl.Invalidate();
         }
      }

      public void OnMouseUp(object sender, MouseEventArgs e)
      {
         _chartControl.Cursor = Cursors.Default;

         if (canZoom())
            _zoomAction(_chartControl, _selectionRectangle);

         _firstSelectionCorner = _lastSelectionCorner = Point.Empty;
         _selectionRectangle = Rectangle.Empty;
      }

      private bool canZoom()
      {
         // insensitive for "double click with small mouse movement"
         return !_selectionRectangle.IsEmpty && _selectionRectangle.Height > 3 && _selectionRectangle.Width > 3;
      }

      private bool mouseIsOverChart(MouseEventArgs e)
      {
         var pointLocation = pointLocationAt(e);
         return pointLocation != null && !pointLocation.IsEmpty;
      }

      private DiagramCoordinates pointLocationAt(MouseEventArgs e)
      {
         if (_chartControl.XYDiagram == null)
            return null;

         try
         {
            return _chartControl.XYDiagram.PointToDiagram(new Point(e.X, e.Y));
         }
         catch (Exception)
         {
            return null;
         }
      }

      private bool isZooming(MouseEventArgs e)
      {
         return e.Button == MouseButtons.Left && Control.ModifierKeys == Keys.None;
      }

      private static Rectangle getRectangle(Point point1, Point point2)
      {
         int xMin = Math.Min(point1.X, point2.X);
         int yMin = Math.Min(point1.Y, point2.Y);
         int width = Math.Abs(point2.X - point1.X);
         int height = Math.Abs(point2.Y - point1.Y);
         return new Rectangle(xMin, yMin, width, height);
      }
   }
}