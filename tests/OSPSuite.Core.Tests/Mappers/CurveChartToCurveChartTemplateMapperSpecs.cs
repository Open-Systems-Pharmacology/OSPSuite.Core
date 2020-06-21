using FakeItEasy;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Chart;
using OSPSuite.Core.Chart.Mappers;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Domain.UnitSystem;

namespace OSPSuite.Core.Mappers
{
   public abstract class concern_for_CurveChartToCurveChartTemplateMapper : ContextSpecification<ICurveChartToCurveChartTemplateMapper>
   {
      protected ICloneManager _cloneManager;

      protected override void Context()
      {
         sut = new CurveChartToCurveChartTemplateMapper(_cloneManager);
      }
   }

   public class When_mapping_a_curve_chart_to_a_curve_chart_template : concern_for_CurveChartToCurveChartTemplateMapper
   {
      private CurveChart _curveChart;
      private IDimensionFactory _dimensionFactory;
      private BaseGrid _xData;
      private DataColumn _yData;
      private CurveChartTemplate _curveChartTemplate;
      private DataRepository _repository;

      protected override void Context()
      {
         base.Context();
         _repository = new DataRepository().WithName("REP");
         _xData = new BaseGrid("base", Constants.Dimension.NO_DIMENSION);
         _xData.QuantityInfo.Path = new[] {"A", "B"};
         _repository.Add(_xData);
         _yData = new DataColumn("Col", Constants.Dimension.NO_DIMENSION, _xData);
         _yData.QuantityInfo.Type = QuantityType.Enzyme;
         _xData.QuantityInfo.Path = new[] {"A", "B", "C"};
         _dimensionFactory = A.Fake<IDimensionFactory>();
         _curveChart = new CurveChart {Name = "This is a chart"};
         _curveChart.FontAndSize.ChartHeight = 500;
         _curveChart.AddAxis(new Axis(AxisTypes.X));
         _curveChart.AddAxis(new Axis(AxisTypes.Y));
         _curveChart.AddAxis(new Axis(AxisTypes.Y2));
         var curve = new Curve();
         curve.SetxData(_xData, _dimensionFactory);
         curve.SetyData(_yData, _dimensionFactory);
         _curveChart.AddCurve(curve);
      }

      protected override void Because()
      {
         _curveChartTemplate = sut.MapFrom(_curveChart);
      }

      [Observation]
      public void should_return_a_curve_template_containing_the_same_properties_as_the_given_curve()
      {
         _curveChartTemplate.IncludeOriginData.ShouldBeEqualTo(_curveChart.IncludeOriginData);
         _curveChartTemplate.AxisBy(AxisTypes.X).ShouldNotBeNull();
         _curveChartTemplate.AxisBy(AxisTypes.Y).ShouldNotBeNull();
         _curveChartTemplate.AxisBy(AxisTypes.Y3).ShouldBeNull();
      }

      [Observation]
      public void should_have_set_the_x_and_y_path_equal_to_the_quantity_path_of_the_used_data_column()
      {
         var curve = _curveChartTemplate.Curves[0];
         curve.ShouldNotBeNull();
         curve.xData.Path.ShouldBeEqualTo(_xData.QuantityInfo.PathAsString);
         curve.xData.QuantityType.ShouldBeEqualTo(_xData.QuantityInfo.Type);
         curve.xData.RepositoryName.ShouldBeEqualTo(_repository.Name);
         curve.yData.Path.ShouldBeEqualTo(_yData.QuantityInfo.PathAsString);
         curve.yData.QuantityType.ShouldBeEqualTo(_yData.QuantityInfo.Type);
         string.IsNullOrEmpty(curve.yData.RepositoryName).ShouldBeTrue();
      }

      [Observation]
      public void should_have_updated_the_font_and_size()
      {
         _curveChartTemplate.FontAndSize.ChartHeight.ShouldBeEqualTo(500);
      }
   }

   public class When_mapping_an_undefined_chart : concern_for_CurveChartToCurveChartTemplateMapper
   {
      [Observation]
      public void should_return_am_empty_template()
      {
         sut.MapFrom(null).ShouldNotBeNull();
      }
   }
}