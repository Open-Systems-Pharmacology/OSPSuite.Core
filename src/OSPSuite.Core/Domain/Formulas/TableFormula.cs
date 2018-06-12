using System;
using System.Collections.Generic;
using System.Linq;
using OSPSuite.Utility.Extensions;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Core.Maths.Interpolations;
using OSPSuite.Utility;

namespace OSPSuite.Core.Domain.Formulas
{
   public class ValuePoint
   {
      /// <summary>
      ///    X value for the point (e.g Time)
      /// </summary>
      public double X { get; set; }

      /// <summary>
      ///    Y Value for the point (e.g fraction, dose etc..).
      /// </summary>
      public double Y { get; set; }

      /// <summary>
      ///    In case of a Time Value point used in SimModel, this flag indicates whether the solver
      ///    should be restarted when time == X. Default is false.
      /// </summary>
      public bool RestartSolver { get; set; }

      /// <summary>
      ///    returns a deep copy of the current value point
      /// </summary>
      /// <returns> </returns>
      public ValuePoint Clone()
      {
         return new ValuePoint(X, Y) {RestartSolver = RestartSolver};
      }

      [Obsolete("For serialization")]
      public ValuePoint()
      {
      }

      public ValuePoint(double x, double y)
      {
         X = x;
         Y = y;
         RestartSolver = false;
      }
   }

   public class TableFormula : Formula
   {
      private readonly IInterpolation _interpolation;
      private readonly IList<ValuePoint> _allPoints;
      private Unit _xDisplayUnit;
      private Unit _yDisplayUnit;

      /// <summary>
      ///    Indicates whether table values should be derived during solving
      ///    the ODE system. Default value is true
      /// </summary>
      public virtual bool UseDerivedValues { get; set; }

      /// <summary>
      ///    Dimension of the X Values (e.g. Time, NoDim)
      /// </summary>
      public virtual IDimension XDimension { get; set; } = Constants.Dimension.NO_DIMENSION;

      /// <summary>
      ///    Name of the value representing the X values( Time, pH..)
      /// </summary>
      public string XName { get; set; }

      /// <summary>
      ///    Name of the value representing the Y values( Fraction, Volume..)
      /// </summary>
      public string YName { get; set; }

      public TableFormula() : this(new LinearInterpolation())
      {
      }

      public TableFormula(IInterpolation interpolation)
      {
         _interpolation = interpolation;
         _allPoints = new List<ValuePoint>();
         UseDerivedValues = true;
      }

      public virtual IEnumerable<ValuePoint> AllPoints()
      {
         return _allPoints;
      }

      /// <summary>
      ///    Returns the yValue defined for the xValue in base unit given as parameter. If the table contains no point, 0 is
      ///    returned
      /// </summary>
      public virtual double ValueAt(double xValue)
      {
         if (_allPoints.Count == 0)
            return 0;

         var knownSamples = _allPoints.Select(point => new Sample(point.X, point.Y));
         return _interpolation.Interpolate(knownSamples, xValue);
      }

      /// <summary>
      ///    Returns the yValue defined for the xValue in display value unit. If the table is empty or if the dimension was not
      ///    defined, return the
      ///    value at <paramref name="xDisplayValue" />
      /// </summary>
      public virtual double DisplayValueAt(double xDisplayValue)
      {
         double xValue = XBaseValueFor(xDisplayValue);
         double yValue = ValueAt(xValue);
         return YDisplayValueFor(yValue);
      }

      /// <summary>
      /// Converts the <paramref name="xDisplayValue"/> to the corresponding base value. It is assumed, that <paramref name="xDisplayValue"/> is 
      /// in <see cref="XDisplayUnit"/>
      /// </summary>
      public virtual double XBaseValueFor(double xDisplayValue)
      {
         return baseValueFor(XDimension, XDisplayUnit, xDisplayValue);
      }

      /// <summary>
      /// Converts the <paramref name="xBaseValue"/> to the corresponding display value in <see cref="XDisplayUnit"/>
      /// </summary>
      public virtual double XDisplayValueFor(double xBaseValue)
      {
         return displayValueFor(XDimension, XDisplayUnit, xBaseValue);
      }

      /// <summary>
      /// Converts the <paramref name="yBaseValue"/> to the corresponding display value in <see cref="YDisplayUnit"/>
      /// </summary>
      public virtual double YDisplayValueFor(double yBaseValue)
      {
         return displayValueFor(Dimension, YDisplayUnit, yBaseValue);
      }

      /// <summary>
      /// Converts the <paramref name="yDisplayValue"/> to the corresponding base value. It is assumed, that <paramref name="yDisplayValue"/> is 
      /// in <see cref="YDisplayUnit"/>
      /// </summary>
      public virtual double YBaseValueFor(double yDisplayValue)
      {
         return baseValueFor(Dimension, YDisplayUnit, yDisplayValue);
      }

      private double baseValueFor(IDimension dimension, Unit displayUnit, double displayValue)
      {
         return dimension?.UnitValueToBaseUnitValue(displayUnit, displayValue) ?? displayValue;
      }

      private double displayValueFor(IDimension dimension, Unit displayUnit, double baseValue)
      {
         return dimension?.BaseUnitValueToUnitValue(displayUnit, baseValue) ?? baseValue;
      }

      /// <summary>
      ///    Unit in which the points were entered.
      /// </summary>
      public virtual Unit XDisplayUnit
      {
         get => displayUnit(_xDisplayUnit, XDimension);
         set => SetProperty(ref _xDisplayUnit, value);
      }

      /// <summary>
      ///    Unit in which the points were entered.
      /// </summary>
      public virtual Unit YDisplayUnit
      {
         get => displayUnit(_yDisplayUnit, Dimension);
         set => SetProperty(ref _yDisplayUnit, value);
      }

      private Unit displayUnit(Unit unit, IDimension dimension)
      {
         return unit ?? dimension?.DefaultUnit;
      }

      /// <summary>
      ///    Removes all points defined in the table
      /// </summary>
      public virtual void ClearPoints()
      {
         _allPoints.Clear();
         OnChanged();
      }

      protected override double CalculateFor(IEnumerable<IObjectReference> usedObjects, IUsingFormula dependentObject)
      {
         return ValueAt(0);
      }

      /// <summary>
      ///    Add a point to the table
      /// </summary>
      /// <param name="x"> x value in base unit for XDimension </param>
      /// <param name="y"> y value in base unit for Dimension </param>
      public virtual int AddPoint(double x, double y)
      {
         return AddPoint(new ValuePoint(x, y));
      }

      /// <summary>
      ///    Add a point to the table and returns the index where the point was added.
      ///    If index >=0, a point already existed with the given x and y coordinated and this is the index of the replacement
      ///    Otherwise, use ~index to now where the point was added in the list
      /// </summary>
      /// <param name="point"> Point to add. Value should be in base unit </param>
      public virtual int AddPoint(ValuePoint point)
      {
         var allXValues = _allPoints.Select(x => x.X).ToList();
         int index = allXValues.BinarySearch(point.X);
         //value already exist for the same x
         if (index >= 0)
         {
            var existingPoint = _allPoints[index];
            if (existingPoint.Y == point.Y)
               return index;

            throw new ValuePointAlreadyExistsForPointException(point);
         }

         //does not exist
         _allPoints.Insert(~index, point);
         OnChanged();
         return index;
      }

      /// <summary>
      ///    Remove the points from the table.
      ///    Returns the index where the point was removed if the point was present in the table or a negative value otherwise
      /// </summary>
      /// <param name="valuePoint"> point to remove </param>
      public virtual int RemovePoint(ValuePoint valuePoint)
      {
         if (valuePoint == null) return -1;
         return RemovePoint(valuePoint.X, valuePoint.Y);
      }

      /// <summary>
      ///    Remove the point having the same x and y from the table
      ///    Returns the index where the point was removed if the point was present in the table or a negative value otherwise
      /// </summary>
      /// <param name="x"> x value in base unit for XDimension </param>
      /// <param name="y"> y value in base unit for Dimension </param>
      public virtual int RemovePoint(double x, double y)
      {
         var allXValues = _allPoints.Select(p => p.X).ToList();
         int index = allXValues.BinarySearch(x);
         //does not exist
         if (index < 0)
            return index;

         var valuePoint = _allPoints[index];
         if (valuePoint.Y != y)
            return -1;

         _allPoints.RemoveAt(index);
         OnChanged();

         return index;
      }

      public override void UpdatePropertiesFrom(IUpdatable source, ICloneManager cloneManager)
      {
         base.UpdatePropertiesFrom(source, cloneManager);

         var tableFormula = source as TableFormula;
         if (tableFormula == null) return;
         XDimension = tableFormula.XDimension;
         XName = tableFormula.XName;
         YName = tableFormula.YName;
         UseDerivedValues = tableFormula.UseDerivedValues;
         _xDisplayUnit = tableFormula._xDisplayUnit;
         _yDisplayUnit = tableFormula._yDisplayUnit;

         ClearPoints();
         tableFormula.AllPoints().Each(p => AddPoint(p.Clone()));
      }
   }
}