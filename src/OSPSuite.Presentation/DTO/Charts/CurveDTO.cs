using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq.Expressions;
using OSPSuite.Assets;
using OSPSuite.Core.Chart;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Utility.Validation;

namespace OSPSuite.Presentation.DTO.Charts
{
   public class CurveDTO : DxValidatableDTO<Curve>
   {
      public Curve Curve { get; }
      private readonly CurveChart _chart;

      public CurveDTO(Curve curve, CurveChart chart) : base(curve)
      {
         Curve = curve;
         _chart = chart;
         Rules.AddRange(AllRules.All);
      }

      public string Name
      {
         get => Curve.Name;
         set => Curve.Name = value;
      }

      public bool Visible
      {
         get => Curve.Visible;
         set => Curve.Visible = value;
      }

      public AxisTypes yAxisType
      {
         get => Curve.yAxisType;
         set => Curve.yAxisType = value;
      }

      public InterpolationModes InterpolationMode
      {
         get => Curve.InterpolationMode;
         set => Curve.InterpolationMode = value;
      }

      public Color Color
      {
         get => Curve.Color;
         set => Curve.Color = value;
      }

      public LineStyles LineStyle
      {
         get => Curve.LineStyle;
         set => Curve.LineStyle = value;
      }

      public Symbols Symbol
      {
         get => Curve.Symbol;
         set => Curve.Symbol = value;
      }

      public int LineThickness
      {
         get => Curve.LineThickness;
         set => Curve.LineThickness = value;
      }

      public int? LegendIndex
      {
         get => Curve.LegendIndex;
         set => Curve.LegendIndex = value;
      }

      public bool VisibleInLegend
      {
         get => Curve.VisibleInLegend;
         set => Curve.VisibleInLegend = value;
      }

      public bool ShowLLOQ
      {
         get => Curve.ShowLLOQ;
         set => Curve.ShowLLOQ = value;
      }

      public DataColumn xData => Curve.xData;

      public DataColumn yData => Curve.yData;
      public string Id => Curve.Id;

      private bool dataDimensionMatchesXAxis()
      {
         var axisX = _chart.AxisBy(AxisTypes.X);
         if (axisX == null)
            return true;

         return Curve.XDimension.HasSharedUnitNamesWith(axisX.Dimension);
      }

      private bool dataDimensionMatchesYAxis(AxisTypes yAxisType)
      {
         var axisY = _chart.AxisBy(yAxisType);
         if (axisY == null)
            return true;

         return Curve.YDimension.HasSharedUnitNamesWith(axisY.Dimension);
      }

      private bool canConvertYAxisUnits()
      {
         var axisY = _chart.AxisBy(Curve.yAxisType);
         if (axisY == null)
            return true;

         return Curve.YDimension.CanConvertToUnit(axisY.UnitName);
      }

      private static class AllRules
      {
         public static IEnumerable<IBusinessRule> All
         {
            get
            {
               yield return dataDimensionMatchesXAxis;
//               yield return dataDimensionMatchesYAxis;
               yield return dataDimensionMatchesYAxisType;
//               yield return canConvertYDataToYAxisUnits;
               yield return canConvertYAxisTypeToYAxisUnits;
            }
         }

         private static IBusinessRule dataDimensionMatchesXAxis { get; } = CreateRule.For<CurveDTO>()
            .Property(x => x.xData)
            .WithRule((curveDTO, column) => curveDTO.dataDimensionMatchesXAxis())
            .WithError(Error.DifferentXAxisDimension);

//         private static IBusinessRule dataDimensionMatchesYAxis { get; } = dataDimensionMatches(x => x.yData);
         private static IBusinessRule canConvertYDataToYAxisUnits { get; } = canConvertYAxisUnits(x => x.yData);
         private static IBusinessRule canConvertYAxisTypeToYAxisUnits { get; } = canConvertYAxisUnits(x => x.yAxisType);

         private static IBusinessRule dataDimensionMatchesYAxisType {get;} = CreateRule.For<CurveDTO>()
            .Property(x=>x.yAxisType)
            .WithRule((curveDTO, axisType) => curveDTO.dataDimensionMatchesYAxis(axisType))
            .WithError(Error.DifferentYAxisDimension);

         private static IBusinessRule canConvertYAxisUnits<TProperty>(Expression<Func<CurveDTO, TProperty>> dataExpressionResolver) => CreateRule.For<CurveDTO>()
            .Property(dataExpressionResolver)
            .WithRule((curveDTO, column) => curveDTO.canConvertYAxisUnits())
            .WithError(Error.CannotConvertYAxisUnits);
      }
   }
}