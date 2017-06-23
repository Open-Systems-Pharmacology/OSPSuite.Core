using System.Drawing;
using FakeItEasy;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Chart;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Helpers;
using OSPSuite.Presentation.DTO.Charts;
using OSPSuite.Utility.Reflection;
using OSPSuite.Utility.Validation;

namespace OSPSuite.Presentation
{
   public abstract class concern_for_CurveDTO : ContextSpecification<CurveDTO>
   {
      protected Curve _curve;
      protected CurveChart _chart;
      private IDimensionFactory _dimensionFactory;
      protected BaseGrid _xDataColumn;
      protected DataColumn _yDataColumn;

      protected override void Context()
      {
         _curve = new Curve { Name = "Curve Name" };

         _dimensionFactory = A.Fake<IDimensionFactory>();
         _xDataColumn = new BaseGrid("XData", DomainHelperForSpecs.TimeDimensionForSpecs());
         _yDataColumn = new DataColumn("YData", DomainHelperForSpecs.ConcentrationDimensionForSpecs(), _xDataColumn);

         A.CallTo(() => _dimensionFactory.MergedDimensionFor((DataColumn) _xDataColumn)).Returns(_xDataColumn.Dimension);
         A.CallTo(() => _dimensionFactory.MergedDimensionFor(_yDataColumn)).Returns(_yDataColumn.Dimension);

         _curve.SetxData(_xDataColumn, _dimensionFactory);
         _curve.SetyData(_yDataColumn, _dimensionFactory);

         _chart = new CurveChart();

         sut = new CurveDTO(_curve, _chart);

         InitializeChart();
      }

      protected virtual void InitializeChart()
      {
         _chart.WithAxes();
      }
   }

   public class When_initializing_the_curve_dto_with_a_chart_that_does_not_have_any_axis : concern_for_CurveDTO
   {
      protected override void InitializeChart()
      {
      }

      [Observation]
      public void should_return_an_valid_state()
      {
         sut.IsValid().ShouldBeFalse();
      }
   }

   public class When_initializing_the_curve_dto_with_axes_having_the_same_dimension_as_the_curve : concern_for_CurveDTO
   {
      protected override void InitializeChart()
      {
         base.InitializeChart();
         _chart.AxisBy(AxisTypes.X).Dimension = _xDataColumn.Dimension;
         _chart.AxisBy(AxisTypes.Y).Dimension = _yDataColumn.Dimension;
      }

      protected override void Context()
      {
         base.Context();
         _curve.yAxisType = AxisTypes.Y;
      }

      [Observation]
      public void should_return_a_valid_state()
      {
         sut.IsValid().ShouldBeTrue(sut.Validate().Message);
      }
   }

   public class When_initializing_the_curve_dto_with_x_axis_having_the_same_dimension_as_the_curve_but_y_axis_not_matching : concern_for_CurveDTO
   {
      protected override void InitializeChart()
      {
         base.InitializeChart();
         _chart.AxisBy(AxisTypes.X).Dimension = _xDataColumn.Dimension;
         _chart.AxisBy(AxisTypes.Y).Dimension = DomainHelperForSpecs.FractionDimensionForSpecs();
      }

      protected override void Context()
      {
         base.Context();
         _curve.yAxisType = AxisTypes.Y;
      }

      [Observation]
      public void should_return_a_valid_state_for_x_axis()
      {
         sut.Validate(x => x.xData).IsEmpty.ShouldBeTrue();
      }

      [Observation]
      public void should_return_an_invalid_state_for_y_axis()
      {
         sut.Validate(x => x.yAxisType).IsEmpty.ShouldBeFalse();
      }
   }

   public class When_initializing_the_curve_dto_with_y_axis_having_the_same_dimension_as_the_curve_but_x_axis_not_matching : concern_for_CurveDTO
   {
      protected override void InitializeChart()
      {
         base.InitializeChart();
         _chart.AxisBy(AxisTypes.X).Dimension = DomainHelperForSpecs.FractionDimensionForSpecs();
         _chart.AxisBy(AxisTypes.Y).Dimension = _yDataColumn.Dimension;
      }

      protected override void Context()
      {
         base.Context();
         _curve.yAxisType = AxisTypes.Y;
      }

      [Observation]
      public void should_return_a_valid_state_for_y_axis()
      {
         sut.Validate(x => x.yData).IsEmpty.ShouldBeTrue();
      }

      [Observation]
      public void should_return_an_invalid_state_for_x_axis()
      {
         sut.Validate(x => x.xData).IsEmpty.ShouldBeFalse();
      }
   }

   public class When_notified_that_a_property_of_the_underlying_curve_has_changed : concern_for_CurveDTO
   {
      private string _propertyName;

      protected override void Context()
      {
         base.Context();
         sut.PropertyChanged += (o, e) =>
         {
            _propertyName = e.PropertyName;
         };
      }

      protected override void Because()
      {
         _curve.Color = Color.Aqua;
      }

      [Observation]
      public void should_also_notify_the_property_changed()
      {
         _propertyName.ShouldBeEqualTo(ReflectionHelper.PropertyFor<CurveDTO, Color>(x=>x.Color).Name);   
      }
   }
}