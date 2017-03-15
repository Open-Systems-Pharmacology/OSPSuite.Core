using System;
using System.Drawing;
using OSPSuite.Utility.Reflection;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.UnitSystem;

namespace OSPSuite.Core.Chart
{
   public enum AxisTypes
   {
      X = 0,
      Y = 1,
      Y2 = 2,
      Y3 = 3
   }

   public enum NumberModes
   {
      Normal,
      Scientific,
      Relative
   }

   public interface IAxis : INotifier, IWithDimension
   {
      AxisTypes AxisType { get; }
      string Caption { get; set; }
      Scalings Scaling { get; set; }
      NumberModes NumberMode { get; set; }
      string UnitName { get; set; }
      bool GridLines { get; set; }
      float? Min { get; set; }
      float? Max { get; set; }
      Unit Unit { get; }

      LineStyles DefaultLineStyle { get; set; } // if not None, then default for curve, when assigned to this axis; 
      Color DefaultColor { get; set; } // if not isWhite, then default for curve, when assigned to this axis; 

      /// <summary>
      /// When false, this will force the axis to be hidden from view. When true
      /// the axis can still be invisible based on number of curves that are visible
      /// </summary>
      bool Visible { get; set; }

      /// <summary>
      ///    Sets range without PropertyChanged event.
      /// </summary>
      void SetRange(float? min, float? max);

      void ResetRange();

      //for simultaneous setting of values to avoid temporary x > y in EventHandler on PropertyChanged
      void UpdateFrom(IAxis axis);

      IAxis Clone();
   }

   public class Axis : Notifier, IAxis
   {
      private readonly AxisTypes _axisType;
      private IDimension _dimension;
      private bool _gridLines;
      private float? _max;
      private float? _min;
      private NumberModes _numberMode;
      private Scalings _scaling;
      private string _unitName;
      private string _caption;
      private LineStyles _defaultLineStyle;
      private Color _defaultColor;
      private bool _visible;

      [Obsolete("For serialization")]
      public Axis() : this(AxisTypes.X)
      {
      }

      public Axis(AxisTypes axisType)
      {
         _axisType = axisType;
         _caption = string.Empty;
         _scaling = Scalings.Linear;
         _numberMode = NumberModes.Normal;
         _dimension = null;
         _unitName = string.Empty;
         _gridLines = false;
         _min = null;
         _max = null;
         Visible = true;

         switch (axisType)
         {
            case AxisTypes.Y:
               _defaultLineStyle = LineStyles.Solid;
               break;
            case AxisTypes.Y2:
               _defaultLineStyle = LineStyles.Dash;
               break;
            case AxisTypes.Y3:
               _defaultLineStyle = LineStyles.Dot;
               break;
            default:
               _defaultLineStyle = LineStyles.None;
               break;
         }

         _defaultColor = Color.White;
      }

      public bool IsYAxis()
      {
         return _axisType.ToString().Contains("Y");
      }

      public void ResetRange()
      {
         SetRange(null, null);
      }

      public void UpdateFrom(IAxis axis)
      {
         Caption = axis.Caption;
         Scaling = axis.Scaling;
         NumberMode = axis.NumberMode;
         Dimension = axis.Dimension;
         UnitName = axis.UnitName;
         GridLines = axis.GridLines;
         SetRange(axis.Min, axis.Max);
         DefaultLineStyle = axis.DefaultLineStyle;
         DefaultColor = axis.DefaultColor;
         Visible = axis.Visible;
      }

      public IAxis Clone()
      {
         var clone = new Axis(AxisType);
         clone.UpdateFrom(this);
         return clone;
      }

      public AxisTypes AxisType => _axisType;

      public string Caption
      {
         get { return _caption; }
         set
         {
            if (Equals(_caption, value))
               return;

            _caption = value;
            OnPropertyChanged();
         }
      }

      public Scalings Scaling
      {
         get { return _scaling; }
         set
         {
            if (_scaling == value)
               return;

            _scaling = value;
            OnPropertyChanged();
         }
      }

      public NumberModes NumberMode
      {
         get { return _numberMode; }
         set
         {
            if (_numberMode == value)
               return;

            _numberMode = value;
            OnPropertyChanged();
         }
      }

      public IDimension Dimension
      {
         get { return _dimension; }
         set
         {
            if (_dimension == value)
               return;

            _dimension = value;
            updateUnitIfItDoesNotMatchDimension();

            OnPropertyChanged();
            OnPropertyChanged(() => UnitName);
         }
      }

      public string UnitName
      {
         get { return _unitName; }
         set
         {
            if (_unitName == value)
               return;

            _unitName = value;
            updateUnitIfItDoesNotMatchDimension();
            OnPropertyChanged();
         }
      }

      private void updateUnitIfItDoesNotMatchDimension()
      {
         if (_dimension == null)
            return;

         if (_dimension.HasUnit(_unitName))
            return;

         _unitName = _dimension.DefaultUnitName;
      }

      public bool Visible
      {
         get { return _visible; }
         set
         {
            if (_visible == value)
               return;

            _visible = value;
            OnPropertyChanged();
         }
      }

      public bool GridLines
      {
         get { return _gridLines; }
         set
         {
            if (_gridLines == value)
               return;

            _gridLines = value;
            OnPropertyChanged();
         }
      }

      public float? Min
      {
         get { return _min; }
         set
         {
            if (_min == value)
               return;

            _min = value;
            OnPropertyChanged();
         }
      }

      public float? Max
      {
         get { return _max; }
         set
         {
            if (_max == value)
               return;

            _max = value;
            OnPropertyChanged();
         }
      }

      public Unit Unit => _dimension?.Unit(UnitName);

      public LineStyles DefaultLineStyle
      {
         get { return _defaultLineStyle; }
         set
         {
            if (_defaultLineStyle == value)
               return;

            _defaultLineStyle = value;
            OnPropertyChanged();
         }
      }

      public Color DefaultColor
      {
         get { return _defaultColor; }
         set
         {
            if (_defaultColor == value)
               return;

            _defaultColor = value;
            OnPropertyChanged();
         }
      }

      public void SetRange(float? min, float? max)
      {
         Min = min;
         Max = max;
      }
   }
}