using FakeItEasy;
using NUnit.Framework;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Chart;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Domain.Mappers;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Presentation.Services;
using IContainer = OSPSuite.Utility.Container.IContainer;

namespace OSPSuite.Presentation.Presentation
{
   public abstract class concern_for_ChartFactory : ContextSpecification<ChartFactory>
   {
      private IContainer _container;
      protected IPresentationUserSettings _userSettings;
      private ITableFormulaToDataRepositoryMapper _tableFormulaToDataRepositoryMapper;
      private IDimensionFactory _dimensionFactory;

      protected override void Context()
      {
         _container = A.Fake<IContainer>();
         _userSettings = A.Fake<IPresentationUserSettings>();
         _userSettings.DefaultChartYScaling = Scalings.Linear;
         _tableFormulaToDataRepositoryMapper = A.Fake<ITableFormulaToDataRepositoryMapper>();
         _dimensionFactory = A.Fake<IDimensionFactory>();
         sut = new ChartFactory(_container, new IdGenerator(), _userSettings, _dimensionFactory, _tableFormulaToDataRepositoryMapper);
      }
   }

   public abstract class When_creating_a_chart : concern_for_ChartFactory
   {
      protected DataRepository _repository;
      protected CurveChart _result;
      protected override void Context()
      {
         base.Context();
         _repository = new DataRepository("id");
         var baseGrid = new BaseGrid("baseGridId", new Dimension(new BaseDimensionRepresentation(), "dimName", "baseUnit"));
         _repository.Add(baseGrid);
         _repository.Add(new DataColumn("column", new Dimension(new BaseDimensionRepresentation(), "dimName", "baseUnit"), baseGrid));
      }
   }

   public class When_creating_a_chart_and_setting_the_Y_scale : When_creating_a_chart
   {
      [TestCase(Scalings.Linear)]
      [TestCase(Scalings.Log)]
      public void should_use_the_default_Y_scale_from_user_settings(Scalings scale)
      {
         _userSettings.DefaultChartYScaling = scale;
         _result = sut.CreateChartFor(_repository, Scalings.Log);
         _result.AxisBy(AxisTypes.Y).Scaling.ShouldBeEqualTo(Scalings.Log);
      }
   }

   public class When_creating_a_chart_without_setting_the_Y_scale : When_creating_a_chart
   {
      [TestCase(Scalings.Linear)]
      [TestCase(Scalings.Log)]
      public void should_use_the_default_Y_scale_from_user_settings(Scalings scale)
      {
         _userSettings.DefaultChartYScaling = scale;
         _result = sut.CreateChartFor(_repository);
         _result.AxisBy(AxisTypes.Y).Scaling.ShouldBeEqualTo(_userSettings.DefaultChartYScaling);
      }
   }

   public class When_creating_new_chart : concern_for_ChartFactory
   {
      private ChartWithObservedData _result;

      protected override void Because()
      {
         _result = sut.Create<ChartWithObservedData>();
      }

      [Observation]
      public void must_have_set_the_default_axis_type_for_the_chart_created()
      {
         _result.DefaultYAxisScaling.ShouldBeEqualTo(_userSettings.DefaultChartYScaling);
      }
   }
}
