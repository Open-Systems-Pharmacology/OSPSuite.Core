using System.Drawing;
using FakeItEasy;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Chart;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Core.Extensions;
using OSPSuite.Helpers;

namespace OSPSuite.Core.Domain
{
   public abstract class concern_for_CurveChart : ContextSpecification<CurveChart>
   {
      protected Curve _curveOnYAxis;
      protected Curve _curveOnY2Axis;
      protected DataRepository _obsData1;
      protected DataRepository _obsData2;
      protected Axis _xAxis;
      protected Axis _yAxis;
      private IDimensionFactory _dimensionFactory;

      protected override void Context()
      {
         _dimensionFactory = A.Fake<IDimensionFactory>();
         sut = new CurveChart().WithAxes();

         _xAxis = sut.AxisBy(AxisTypes.X);
         _yAxis = sut.AxisBy(AxisTypes.Y);

         _obsData1 = DomainHelperForSpecs.ObservedData();
         _obsData2 = DomainHelperForSpecs.ObservedData();

         _curveOnYAxis = new Curve {yAxisType = AxisTypes.Y};
         _curveOnYAxis.SetxData(_obsData1.BaseGrid, _dimensionFactory);
         _curveOnYAxis.SetyData(_obsData1.FirstDataColumn(), _dimensionFactory);

         _curveOnY2Axis = new Curve {yAxisType = AxisTypes.Y2};
         _curveOnY2Axis.SetxData(_obsData2.BaseGrid, _dimensionFactory);
         _curveOnY2Axis.SetyData(_obsData2.FirstDataColumn(), _dimensionFactory);
      }
   }

   public class When_moving_series_in_legend : concern_for_CurveChart
   {
      private Curve _curveOne;
      private Curve _curveFour;

      protected override void Context()
      {
         base.Context();
         _curveOne = A.Fake<Curve>();
         _curveFour = A.Fake<Curve>();

         sut.AddCurve(_curveOne);
         sut.AddCurve(A.Fake<Curve>());
         sut.AddCurve(A.Fake<Curve>());
         sut.AddCurve(_curveFour);
      }

      protected override void Because()
      {
         sut.MoveCurvesInLegend(_curveFour, _curveOne);
      }

      [Observation]
      public void the_relative_legend_indicies_should_be_adjusted()
      {
         _curveFour.LegendIndex.HasValue.ShouldBeTrue();
         (_curveFour.LegendIndex < _curveOne.LegendIndex).ShouldBeTrue();
      }
   }

   public class When_adding_new_Y_axes : concern_for_CurveChart
   {
      private Axis _y2Axis;

      protected override void Context()
      {
         base.Context();
         sut.DefaultYAxisScaling = Scalings.Linear;
      }

      protected override void Because()
      {
         _y2Axis = sut.AddNewAxisFor(AxisTypes.Y2);
      }

      [Observation]
      public void the_default_y_axis_scaling_should_be_used_for_the_new_axis()
      {
         _y2Axis.Scaling.ShouldBeEqualTo(Scalings.Linear);
      }

      [Observation]
      public void the_axis_accessor_properties_should_return_the_expected_axis()
      {
         sut.AllUsedAxisTypes.ShouldContain(AxisTypes.X, AxisTypes.Y, AxisTypes.Y2);
         sut.AllUsedYAxisTypes.ShouldContain(AxisTypes.Y, AxisTypes.Y2);
         sut.AllUsedSecondaryAxisTypes.ShouldContain(AxisTypes.Y2);
      }
   }

   public class When_instantiating_a_new_CurveChart : concern_for_CurveChart
   {
      [Observation]
      public void the_default_Y_axis_scale_should_be_logarigmic()
      {
         sut.DefaultYAxisScaling.ShouldBeEqualTo(Scalings.Log);
      }

      [Observation]
      public void the_x_axis_property_should_return_the_axis_registered_for_axis_type_X()
      {
         sut.XAxis.ShouldBeEqualTo(_xAxis);
      }
   }

   public abstract class When_using_default_lineStyle_and_color : concern_for_CurveChart
   {
      protected override void Context()
      {
         base.Context();
         _curveOnYAxis.LineStyle = LineStyles.Dot;
         _curveOnYAxis.Color = Color.Red;
      }
   }

   public class When_using_default_lineStyle_DashDot : When_using_default_lineStyle_and_color
   {
      protected override void Because()
      {
         _yAxis.DefaultLineStyle = LineStyles.DashDot;
         sut.AddCurve(_curveOnYAxis);
      }

      [Observation]
      public void after_adding_curve_the_curve_lineStyle_should_be_DashDot()
      {
         _curveOnYAxis.LineStyle.ShouldBeEqualTo(LineStyles.DashDot);
      }
   }

   public class When_using_default_lineStyle_None : When_using_default_lineStyle_and_color
   {
      protected override void Because()
      {
         _yAxis.DefaultLineStyle = LineStyles.None;
         sut.AddCurve(_curveOnYAxis);
      }

      [Observation]
      public void after_adding_curve_the_curve_lineStyle_should_be_unchanged()
      {
         _curveOnYAxis.LineStyle.ShouldBeEqualTo(LineStyles.Dot);
      }
   }

   public class When_using_default_color_Blue : When_using_default_lineStyle_and_color
   {
      protected override void Because()
      {
         _yAxis.DefaultColor = Color.Blue;
         sut.AddCurve(_curveOnYAxis);
      }

      [Observation]
      public void after_adding_curve_the_curve_color_should_be_Blue()
      {
         _curveOnYAxis.Color.ShouldBeEqualTo(Color.Blue);
      }
   }

   public class When_using_default_color_White : When_using_default_lineStyle_and_color
   {
      protected override void Because()
      {
         _yAxis.DefaultColor = Color.White;
         sut.AddCurve(_curveOnYAxis);
      }

      [Observation]
      public void after_adding_curve_the_curve_color_should_be_unchanged()
      {
         _curveOnYAxis.Color.ShouldBeEqualTo(Color.Red);
      }
   }

   public class When_removing_a_curve_from_the_chart_resulting_in_a_y_axis_not_being_used_anymore : concern_for_CurveChart
   {
      private IDimension _dimension;
      private Axis _y2Axis;

      protected override void Context()
      {
         base.Context();
         _dimension = DomainHelperForSpecs.ConcentrationDimensionForSpecs();
         _y2Axis = sut.AddNewAxisFor(AxisTypes.Y2);
         _yAxis = sut.AxisBy(AxisTypes.Y);
         _yAxis.Dimension = _dimension;
         _yAxis.UnitName = _dimension.DefaultUnitName;
         _yAxis.SetRange(10, 20);

         _y2Axis.Dimension = _dimension;
         _y2Axis.UnitName = _dimension.DefaultUnitName;
         _y2Axis.SetRange(12, 17);

         sut.AddCurve(_curveOnYAxis, false);
         sut.AddCurve(_curveOnY2Axis, false);
      }

      protected override void Because()
      {
         sut.RemoveCurve(_curveOnYAxis);
      }

      [Observation]
      public void should_reset_the_y_axis()
      {
         _yAxis.Dimension.ShouldBeNull();
         _yAxis.UnitName.ShouldBeNull();
         _yAxis.Min.ShouldBeNull();
         _yAxis.Max.ShouldBeNull();
      }

      [Observation]
      public void should_not_reset_the_y2_axis_still_in_used()
      {
         _y2Axis.Dimension.ShouldBeEqualTo(_dimension);
         _y2Axis.UnitName.ShouldBeEqualTo(_dimension.DefaultUnitName);
         _y2Axis.Min.ShouldBeEqualTo(12f);
         _y2Axis.Max.ShouldBeEqualTo(17f);
      }
   }

   public class When_removing_an_axis_from_a_curve_chart : concern_for_CurveChart
   {
      private Axis _y2Axis;

      protected override void Context()
      {
         base.Context();
         _y2Axis = sut.AddNewAxisFor(AxisTypes.Y2);
         sut.AddCurve(_curveOnYAxis, false);
         sut.AddCurve(_curveOnY2Axis, false);
      }

      protected override void Because()
      {
         sut.RemoveAxis(_y2Axis);
      }

      [Observation]
      public void should_remove_the_axis_from_the_chart()
      {
         sut.HasAxis(AxisTypes.Y2).ShouldBeFalse();
      }

      [Observation]
      public void should_reset_the_y_axis_type_of_all_curves_using_this_axis_to_Y()
      {
         _curveOnY2Axis.yAxisType.ShouldBeEqualTo(AxisTypes.Y);
      }
   }
}