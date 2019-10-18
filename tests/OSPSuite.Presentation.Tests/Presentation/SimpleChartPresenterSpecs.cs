using FakeItEasy;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Chart;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Core.Events;
using OSPSuite.Core.Extensions;
using OSPSuite.Helpers;
using OSPSuite.Presentation.Presenters.Charts;
using OSPSuite.Presentation.Views.Charts;
using OSPSuite.Utility.Events;

namespace OSPSuite.Presentation.Presentation
{
   public abstract class concern_for_SimpleChartPresenter : ContextSpecification<ISimpleChartPresenter>
   {
      protected ISimpleChartView _view;
      protected IChartDisplayPresenter _chartDisplayPresenter;
      protected IEventPublisher _eventPublisher;
      protected IPresentationUserSettings _presentationUserSettings;
      protected IChartFactory _chartFactory;
      private IDimensionFactory _dimensionFactory;

      protected override void Context()
      {
         _view = A.Fake<ISimpleChartView>();
         _chartFactory = A.Fake<IChartFactory>();
         _chartDisplayPresenter = A.Fake<IChartDisplayPresenter>();
         _eventPublisher = A.Fake<IEventPublisher>();
         _dimensionFactory= A.Fake<IDimensionFactory>();
         _presentationUserSettings = A.Fake<IPresentationUserSettings>();
         sut = new SimpleChartPresenter(_view, _chartDisplayPresenter, _chartFactory,_eventPublisher, _presentationUserSettings, _dimensionFactory);

         _presentationUserSettings.DefaultChartYScaling = Scalings.Log;
         A.CallTo(() => _chartFactory.Create<CurveChart>()).ReturnsLazily(() => new CurveChart {DefaultYAxisScaling = _presentationUserSettings.DefaultChartYScaling});
      }
   }

   public class When_creating_a_simple_chart_presenter : concern_for_SimpleChartPresenter
   {
      private DataRepository _dataRepository;

      protected override void Context()
      {
         base.Context();
         _dataRepository = A.Fake<DataRepository>();
         A.CallTo(() => _chartFactory.CreateChartFor(_dataRepository, _presentationUserSettings.DefaultChartYScaling))
            .Returns(new CurveChart { DefaultYAxisScaling = _presentationUserSettings.DefaultChartYScaling }.WithAxes());
      }

      protected override void Because()
      {
         sut.Plot(_dataRepository);
      }

      [Observation]
      public void log_lin_selector_should_be_hidden()
      {
         _view.LogLinSelectionEnabled.ShouldBeFalse();
      }

      [Observation]
      public void should_add_the_view_from_the_display_presenter_into_its_own_view()
      {
         A.CallTo(() => _view.AddView(_chartDisplayPresenter.View)).MustHaveHappened();
      }

      [Observation]
      public void chart_scale_should_be_using_the_default_chart_y_scaling()
      {
         sut.Chart.AxisBy(AxisTypes.Y).Scaling.ShouldBeEqualTo(_presentationUserSettings.DefaultChartYScaling);
      }
   }

   public class When_enabling_log_lin_selection : concern_for_SimpleChartPresenter
   {
      private DataRepository _dataRepository;

      protected override void Context()
      {
         base.Context();
         _dataRepository = A.Fake<DataRepository>();
         sut.Plot(_dataRepository);
      }

      protected override void Because()
      {
         sut.LogLinSelectionEnabled = true;
      }

      [Observation]
      public void view_must_be_changed_to_enable_selector()
      {
         _view.LogLinSelectionEnabled.ShouldBeTrue();
      }
   }

   public class When_setting_chart_to_log : concern_for_SimpleChartPresenter
   {
      private DataRepository _dataRepository;

      protected override void Context()
      {
         base.Context();
         _dataRepository = A.Fake<DataRepository>();
         A.CallTo(() => _chartFactory.CreateChartFor(_dataRepository, A<Scalings>._)).Returns(new CurveChart());
         sut.Plot(_dataRepository);
         sut.Chart.WithAxes();
      }
      
      protected override void Because()
      {
         sut.SetChartScale(Scalings.Log);
      }

      [Observation]
      public void chart_axis_set_to_log()
      {
         sut.Chart.AxisBy(AxisTypes.Y).Scaling.ShouldBeEqualTo(Scalings.Log);
      }

      [Observation]
      public void should_refresh_the_chart_display()
      {
         A.CallTo(() => _chartDisplayPresenter.Refresh()).MustHaveHappened();
      }
   }

   public class When_plotting_observed_data_that_doesnt_include_fraction_data : concern_for_SimpleChartPresenter
   {
      private DataRepository _dataRepository;

      protected override void Context()
      {
         base.Context();
         _dataRepository = DomainHelperForSpecs.ObservedData();
      }

      protected override void Because()
      {
         sut.PlotObservedData(_dataRepository);
      }

      [Observation]
      public void the_chart_should_have_axis_scaling_set_to_log()
      {
         sut.Chart.AxisBy(AxisTypes.Y).Scaling.ShouldBeEqualTo(Scalings.Log);
      }
   }

   public class When_plotting_observed_data_that_includes_fraction_data : concern_for_SimpleChartPresenter
   {
      private DataRepository _dataRepository;

      protected override void Context()
      {
         base.Context();
         _dataRepository = DomainHelperForSpecs.ObservedData();
         _dataRepository.FirstDataColumn().Dimension = DomainHelperForSpecs.FractionDimensionForSpecs();
      }

      protected override void Because()
      {
         sut.PlotObservedData(_dataRepository);
      }

      [Observation]
      public void the_chart_should_have_axis_scaling_set_to_linear()
      {
         sut.Chart.AxisBy(AxisTypes.Y).Scaling.ShouldBeEqualTo(Scalings.Linear);
      }
   }

   public class When_exporting_a_chart_to_pdf : concern_for_SimpleChartPresenter
   {
      private DataRepository _dataRepository;

      protected override void Context()
      {
         base.Context();
         _dataRepository= A.Fake<DataRepository>();
         sut.Plot(_dataRepository);
      }

      protected override void Because()
      {
         _chartDisplayPresenter.ExportToPDF();
      }

      [Observation]
      public void should_raise_the_export_to_pdf_event_with_the_chart()
      {
         A.CallTo(() => _eventPublisher.PublishEvent(A<ExportToPDFEvent>._)).MustHaveHappened();
      }
   }
   public class When_the_simple_chart_presenter_is_told_to_plot_the_chart_for_a_table_formula : concern_for_SimpleChartPresenter
   {
      private TableFormula _tableFormula;
      private CurveChart _chart;

      protected override void Context()
      {
         base.Context();
         _tableFormula = new TableFormula().WithDimension(DomainHelperForSpecs.LengthDimensionForSpecs());
         _tableFormula.AddPoint(1, 10);
         _tableFormula.AddPoint(2, 20);
         _tableFormula.AddPoint(3, 30);
         _chart = new CurveChart().WithAxes();
         A.CallTo(() => _chartFactory.CreateChartFor(_tableFormula)).Returns(_chart);
      }

      protected override void Because()
      {
         sut.Plot(_tableFormula);
      }

      [Observation]
      public void should_create_a_chart_based_on_the_given_table_formula()
      {
         A.CallTo(() => _chartFactory.CreateChartFor(_tableFormula)).MustHaveHappened();
      }

      [Observation]
      public void should_have_the_chart_display_presenter_to_edit_the_created_chart()
      {
         A.CallTo(() => _chartDisplayPresenter.Edit(_chart)).MustHaveHappened();
      }
   }

   public class When_the_simple_chart_presenter_is_refreshing_the_chart : concern_for_SimpleChartPresenter
   {
      protected override void Because()
      {
         sut.Refresh();
      }

      [Observation]
      public void should_refresh_the_display()
      {
         A.CallTo(() => _chartDisplayPresenter.Refresh()).MustHaveHappened();  
      }
   }
}