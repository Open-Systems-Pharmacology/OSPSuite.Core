using System.Drawing;
using OSPSuite.Utility;
using OSPSuite.Utility.Reflection;

namespace OSPSuite.Core.Chart
{
   public enum LineStyles
   {
      None,
      Solid,
      Dash,
      Dot,
      DashDot
   }

   public enum Symbols
   {
      None,
      Circle,
      Diamond,
      Triangle,
      Square
   }

   public enum InterpolationModes
   {
      xLinear,
      yLinear
   }

   public class CurveOptions : Notifier
   {
      private Color _color;
      private InterpolationModes _interpolationMode;
      private LineStyles _lineStyle;
      private int _lineThickness;
      private Symbols _symbol;
      private bool _visible;
      private AxisTypes _yAxisType;
      private bool _visibleInLegend;
      private int? _legendIndex;
      private bool _shouldShowLLOQ;

      public CurveOptions()
      {
         _interpolationMode = InterpolationModes.xLinear;
         _yAxisType = AxisTypes.Y;
         _visible = true;
         _color = Color.Black;
         _lineStyle = LineStyles.Solid;
         _symbol = Symbols.None;
         _lineThickness = 2;
         _shouldShowLLOQ = true;
         _visibleInLegend = true;
      }

      public InterpolationModes InterpolationMode
      {
         get => _interpolationMode;
         set => SetProperty(ref _interpolationMode, value);
      }

      public AxisTypes yAxisType
      {
         get => _yAxisType;
         set => SetProperty(ref _yAxisType, value);
      }

      public bool Visible
      {
         get => _visible;
         set => SetProperty(ref _visible, value);
      }

      /// <summary>
      ///    Returns if the curve should really be displayed (a curve visible but without any symbols or line is in fact hidden)
      /// </summary>
      public bool IsReallyVisible => Visible && (LineStyle != LineStyles.None || Symbol != Symbols.None);

      public Color Color
      {
         get => _color;
         set => SetProperty(ref _color, value);
      }

      public LineStyles LineStyle
      {
         get => _lineStyle;
         set => SetProperty(ref _lineStyle, value);
      }

      public Symbols Symbol
      {
         get => _symbol;
         set => SetProperty(ref _symbol, value);
      }

      public int LineThickness
      {
         get => _lineThickness;
         set => SetProperty(ref _lineThickness, value);
      }

      /// <summary>
      ///    This value indicates relative place in the legend for this curve
      /// </summary>
      public int? LegendIndex
      {
         get => _legendIndex;
         set => SetProperty(ref _legendIndex, value);
      }

      public bool ShouldShowLLOQ
      {
         get => _shouldShowLLOQ;
         set => SetProperty(ref _shouldShowLLOQ, value);
      }

      public bool VisibleInLegend
      {
         get => _visibleInLegend;
         set => SetProperty(ref _visibleInLegend, value);
      }

      /// <summary>
      ///    Updates all values defined from the given <paramref name="curveOptions" />
      /// </summary>
      public void UpdateFrom(CurveOptions curveOptions)
      {
         InterpolationMode = curveOptions.InterpolationMode;
         yAxisType = curveOptions.yAxisType;
         Visible = curveOptions.Visible;
         Color = curveOptions.Color;
         LineStyle = curveOptions.LineStyle;
         Symbol = curveOptions.Symbol;
         LineThickness = curveOptions.LineThickness;
         VisibleInLegend = curveOptions.VisibleInLegend;
         LegendIndex = curveOptions.LegendIndex;
         ShouldShowLLOQ = curveOptions.ShouldShowLLOQ;
      }

      public CurveOptions Clone()
      {
         var clone = new CurveOptions();
         clone.UpdateFrom(this);
         return clone;
      }
   }
}