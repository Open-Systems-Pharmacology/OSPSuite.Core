using System;
using System.Collections.Generic;
using System.Drawing;
using OSPSuite.Assets;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Utility.Reflection;
using OSPSuite.Utility.Validation;

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

   public class Axis : Notifier, IWithDimension, IValidatable
   {
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
      public AxisTypes AxisType { get; }
      public virtual IBusinessRuleSet Rules { get; }

      [Obsolete("For serialization")]
      public Axis() : this(AxisTypes.X)
      {
      }

      public Axis(AxisTypes axisType)
      {
         AxisType = axisType;
         _caption = string.Empty;
         _scaling = Scalings.Linear;
         _numberMode = NumberModes.Normal;
         _dimension = null;
         _unitName = string.Empty;
         _gridLines = false;
         _min = null;
         _max = null;
         Visible = true;
         _defaultLineStyle = defaultLineStyleForAxisType();
         _defaultColor = Color.White;
         Rules = new BusinessRuleSet(ValidationRules.AllRules());
      }

      private static class ValidationRules
      {
         internal static IEnumerable<IBusinessRule> AllRules()
         {
            yield return maxGreaterThanOrEqualToMin;
            yield return minLessThanOrEqualToMax;
            yield return maxGreaterThanZero;
         }

         private static IBusinessRule maxGreaterThanOrEqualToMin { get; } = createMaxGreaterThanOrEqualToMinRuleForAxis();
         private static IBusinessRule minLessThanOrEqualToMax { get; } = createMinLessThanOrEqualToMaxRuleForAxis();
         private static IBusinessRule maxGreaterThanZero { get; } = createMaxGreaterThanZeroRuleForLogarithmicAxis();

         private static IBusinessRule createMinLessThanOrEqualToMaxRuleForAxis() => CreateRule.For<Axis>()
               .Property(axis => axis.Min)
               .WithRule((axis, value) => !value.HasValue || !axis.Max.HasValue || axis.Max >= value)
               .WithError((axis, value) => Validation.AxisMinMustBeLessThanOrEqualToAxisMax(axis.Max));
      
         private static IBusinessRule createMaxGreaterThanZeroRuleForLogarithmicAxis() => CreateRule.For<Axis>()
               .Property(axis => axis.Max)
               .WithRule((axis, value) => !value.HasValue || axis.Scaling != Scalings.Log || value > 0F)
               .WithError((axis, value) => Validation.LogAxisMaxMustBeGreaterThanZero);
      
         private static IBusinessRule createMaxGreaterThanOrEqualToMinRuleForAxis() => CreateRule.For<Axis>()
               .Property(axis => axis.Max)
               .WithRule((axis, value) => !value.HasValue || !axis.Min.HasValue || value >= axis.Min)
               .WithError((axis, value) => Validation.AxisMaxMustBeGreaterThanOrEqualToAxisMin(axis.Min));
      }

      private LineStyles defaultLineStyleForAxisType()
      {
         switch (AxisType)
         {
            case AxisTypes.Y:
               return LineStyles.Solid;
            case AxisTypes.Y2:
               return LineStyles.Dash;
            case AxisTypes.Y3:
               return LineStyles.Dot;
            default:
               return LineStyles.None;
         }
      }

      public bool IsYAxis => AxisType != AxisTypes.X;
      public bool IsXAxis => AxisType == AxisTypes.X;

      public void ResetRange()
      {
         SetRange(null, null);
      }

      public void UpdateFrom(Axis axis)
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

      public virtual Axis Clone()
      {
         var clone = new Axis(AxisType);
         clone.UpdateFrom(this);
         return clone;
      }

      public string Caption
      {
         get => _caption;
         set => SetProperty(ref _caption, value);
      }

      public Scalings Scaling
      {
         get => _scaling;
         set => SetProperty(ref _scaling, value);
      }

      public NumberModes NumberMode
      {
         get => _numberMode;
         set => SetProperty(ref _numberMode, value);
      }

      public IDimension Dimension
      {
         get => _dimension;
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

      public void Reset()
      {
         Dimension = null;
         UnitName = null;
         ResetRange();
      }

      public string UnitName
      {
         get => _unitName;
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

      /// <summary>
      ///    When false, this will force the axis to be hidden from view. When true
      ///    the axis can still be invisible based on number of curves that are visible
      /// </summary>
      public bool Visible
      {
         get => _visible;
         set => SetProperty(ref _visible, value);
      }

      public bool GridLines
      {
         get => _gridLines;
         set => SetProperty(ref _gridLines, value);
      }

      public float? Min
      {
         get => _min;
         set => SetProperty(ref _min, value);
      }

      public float? Max
      {
         get => _max;
         set => SetProperty(ref _max, value);
      }

      public Unit Unit => _dimension?.Unit(UnitName);

      /// <summary>
      ///    if not None, then default for curve, when assigned to this axis;
      /// </summary>
      public LineStyles DefaultLineStyle
      {
         get => _defaultLineStyle;
         set => SetProperty(ref _defaultLineStyle, value);
      }

      /// <summary>
      ///    if not isWhite, then default for curve, when assigned to this axis
      /// </summary>
      public Color DefaultColor
      {
         get => _defaultColor;
         set => SetProperty(ref _defaultColor, value);
      }

      public void SetRange(float? min, float? max)
      {
         Min = min;
         Max = max;
      }
   }
}