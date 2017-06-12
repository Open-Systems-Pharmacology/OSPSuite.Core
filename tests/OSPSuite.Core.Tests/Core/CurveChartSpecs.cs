using System.Drawing;
using System.Linq;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using FakeItEasy;
using OSPSuite.Core.Chart;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Domain.UnitSystem;

namespace OSPSuite.Core
{
   public abstract class concern_for_CurveChart : ContextSpecification<CurveChart>
   {
      protected override void Context()
      {
         sut = new CurveChart();
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
         sut.MoveSeriesInLegend(_curveFour, _curveOne);
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
      private Curve _curve;

      protected override void Context()
      {
         base.Context();
         sut.DefaultYAxisScaling = Scalings.Linear;

         _curve = A.Fake<Curve>();

         sut.AddCurve(_curve);
      }

      protected override void Because()
      {
         sut.SetyData(_curve, A.Fake<DataColumn>(), A.Fake<IDimensionFactory>());
      }

      [Observation]
      public void the_default_y_axis_scaling_should_be_used_for_the_new_axis()
      {
         sut.Axes.First().Scaling.ShouldBeEqualTo(Scalings.Linear);
      }
   }

   public class When_instantiating_a_new_CurveChart : concern_for_CurveChart
   {
      [Observation]
      public void the_default_Y_axis_scale_should_be_logarigmic()
      {
         sut.DefaultYAxisScaling.ShouldBeEqualTo(Scalings.Log);
      }
   }

   public abstract class When_using_default_lineStyle_and_color : concern_for_CurveChart
   {
      protected Axis _yAxis;
      protected Curve _curve;

      protected override void Context()
      {
         base.Context();
         var xAxis = new Axis(AxisTypes.X);
         sut.AddAxis(xAxis);

         _yAxis = new Axis(AxisTypes.Y);
         sut.AddAxis(_yAxis);

         _curve = A.Fake<Curve>();
         _curve.LineStyle = LineStyles.Dot;
         _curve.Color = Color.Red;
      }
   }

   public class When_using_default_lineStyle_DashDot : When_using_default_lineStyle_and_color
   {
      protected override void Because()
      {
         _yAxis.DefaultLineStyle = LineStyles.DashDot;
         sut.AddCurve(_curve);
      }

      [Observation]
      public void after_adding_curve_the_curve_lineStyle_should_be_DashDot()
      {
         _curve.LineStyle.ShouldBeEqualTo(LineStyles.DashDot);
      }
   }

   public class When_using_default_lineStyle_None : When_using_default_lineStyle_and_color
   {
      protected override void Because()
      {
         _yAxis.DefaultLineStyle = LineStyles.None;
         sut.AddCurve(_curve);
      }

      [Observation]
      public void after_adding_curve_the_curve_lineStyle_should_be_unchanged()
      {
         _curve.LineStyle.ShouldBeEqualTo(LineStyles.Dot);
      }
   }

   public class When_using_default_color_Blue : When_using_default_lineStyle_and_color
   {
      protected override void Because()
      {
         _yAxis.DefaultColor = Color.Blue;
         sut.AddCurve(_curve);
      }

      [Observation]
      public void after_adding_curve_the_curve_color_should_be_Blue()
      {
         _curve.Color.ShouldBeEqualTo(Color.Blue);
      }
   }

   public class When_using_default_color_White : When_using_default_lineStyle_and_color
   {
      protected override void Because()
      {
         _yAxis.DefaultColor = Color.White;
         sut.AddCurve(_curve);
      }

      [Observation]
      public void after_adding_curve_the_curve_color_should_be_unchanged()
      {
         _curve.Color.ShouldBeEqualTo(Color.Red);
      }
   }
}
