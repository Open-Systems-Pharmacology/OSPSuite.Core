using System.Collections.Generic;
using System.Drawing;
using OSPSuite.Assets;
using OSPSuite.Core.Chart;
using OSPSuite.Core.Domain;
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

      private bool dataDimensionMatchesXAxis(DataColumn dataColumn)
      {
         return dimensionsHaveSharedUnits(_chart.AxisBy(AxisTypes.X), dataColumn.Dimension);
      }

      private bool dataDimensionMatchesYAxis(DataColumn dataColumn)
      {
         return dimensionsHaveSharedUnits(_chart.AxisBy(yAxisType), dataColumn.Dimension);
      }

      private bool dataDimensionMatchesYAxis(AxisTypes axisType)
      {
         return dimensionsHaveSharedUnits(_chart.AxisBy(axisType), Curve.yDimension);
      }

      private bool dimensionsHaveSharedUnits(Axis axis, IDimension dimension)
      {
         if (axis == null || dimension==null)
            return false;

         return dimension.HasSharedUnitNamesWith(axis.Dimension);
      }

      private bool canConvertYAxisUnits(AxisTypes axisType)
      {
         var axisY = _chart.AxisBy(axisType);
         if (axisY == null)
            return true;

         return Curve.yDimension.CanConvertToUnit(axisY.UnitName);
      }

      private static class AllRules
      {
         public static IEnumerable<IBusinessRule> All
         {
            get
            {
               yield return nameCannotBeEmpty;
               yield return dataDimensionMatchesXAxis;
               yield return dataDimensionMatchesYAxis;
               yield return dataDimensionMatchesYAxisType;
               yield return canConvertYAxisTypeToYAxisUnits;
            }
         }

         private static IBusinessRule nameCannotBeEmpty { get; } = GenericRules.NonEmptyRule<CurveDTO>(x => x.Name, Error.NameIsRequired);

         private static IBusinessRule dataDimensionMatchesXAxis { get; } = CreateRule.For<CurveDTO>()
            .Property(x => x.xData)
            .WithRule((curveDTO, column) => curveDTO.dataDimensionMatchesXAxis(column))
            .WithError(Error.DifferentXAxisDimension);

         private static IBusinessRule dataDimensionMatchesYAxis { get; } = CreateRule.For<CurveDTO>()
            .Property(x => x.yData)
            .WithRule((curveDTO, column) => curveDTO.dataDimensionMatchesYAxis(column))
            .WithError(Error.DifferentYAxisDimension);

         private static IBusinessRule canConvertYAxisTypeToYAxisUnits { get; } = CreateRule.For<CurveDTO>()
            .Property(x => x.yAxisType)
            .WithRule((curveDTO, axisType) => curveDTO.canConvertYAxisUnits(axisType))
            .WithError(Error.CannotConvertYAxisUnits);

         private static IBusinessRule dataDimensionMatchesYAxisType { get; } = CreateRule.For<CurveDTO>()
            .Property(x => x.yAxisType)
            .WithRule((curveDTO, axisType) => curveDTO.dataDimensionMatchesYAxis(axisType))
            .WithError(Error.DifferentYAxisDimension);
      }
   }
}