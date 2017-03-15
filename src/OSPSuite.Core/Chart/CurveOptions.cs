using System.Drawing;
using OSPSuite.Utility;
using OSPSuite.Utility.Extensions;
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

   public class CurveOptions : Notifier, ILatchable
   {
      private Color _color;
      private InterpolationModes _interpolationMode;
      private LineStyles _lineStyle;
      private int _lineThickness;
      private Symbols _symbol;
      private bool _visible;
      private AxisTypes _yAxisType;
      public bool IsLatched { get; set; }
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
         VisibleInLegend = true;
      }

      public InterpolationModes InterpolationMode
      {
         get { return _interpolationMode; }
         set
         {
            _interpolationMode = value;
            OnPropertyChanged();
         }
      }

      public AxisTypes yAxisType
      {
         get { return _yAxisType; }
         set
         {
            _yAxisType = value;
            OnPropertyChanged();
         }
      }

      public bool Visible
      {
         get { return _visible; }
         set
         {
            _visible = value;
            OnPropertyChanged();
         }
      }

      /// <summary>
      /// Returns if the curve should really be displayed (a curve visible but without any symbols or line is in fact hidden)
      /// </summary>
      public bool IsReallyVisible => Visible && (LineStyle != LineStyles.None || Symbol != Symbols.None);

      public Color Color
      {
         get { return _color; }
         set
         {
            _color = value;
            OnPropertyChanged();
         }
      }

      public LineStyles LineStyle
      {
         get { return _lineStyle; }
         set
         {
            _lineStyle = value;
            OnPropertyChanged();
         }
      }

      public Symbols Symbol
      {
         get { return _symbol; }
         set
         {
            _symbol = value;
            OnPropertyChanged();
         }
      }

      public int LineThickness
      {
         get { return _lineThickness; }
         set
         {
            _lineThickness = value;
            OnPropertyChanged();
         }
      }

      public int? LegendIndex
      {
         get { return _legendIndex; }
         set
         {
            _legendIndex = value;
            OnPropertyChanged();
         }
      }

      public bool ShouldShowLLOQ
      {
         get { return _shouldShowLLOQ; }
         set
         {
            _shouldShowLLOQ = value;
            OnPropertyChanged();
         }
      }

      public bool VisibleInLegend
      {
         get { return _visibleInLegend; }
         set
         {
            _visibleInLegend = value;
            OnPropertyChanged();
         }
      }

      /// <summary>
      ///    Updates all values defined from the given <paramref name="curveOptions" /> without raising any change event
      /// </summary>
      public void UpdateFrom(CurveOptions curveOptions)
      {
         this.DoWithinLatch(() =>
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
         });
      }

      protected override void RaisePropertyChanged(string propertyName)
      {
         if (IsLatched) return;
         base.RaisePropertyChanged(propertyName);
      }

      public CurveOptions Clone()
      {
         var clone = new CurveOptions();
         clone.UpdateFrom(this);
         return clone;
      }
   }
}