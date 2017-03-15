using System.Windows.Forms;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Utility.Events;
using FakeItEasy;
using OSPSuite.Core;
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

namespace OSPSuite.Presentation
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
         A.CallTo(() => _chartDisplayPresenter.Control).Returns(new Control());
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
      public void chart_scale_should_be_linear()
      {
         sut.Chart.Axes[AxisTypes.Y].Scaling.ShouldBeEqualTo(Scalings.Linear);
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
         sut.Chart.Axes[AxisTypes.Y].Scaling.ShouldBeEqualTo(Scalings.Log);
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
         sut.Chart.Axes[AxisTypes.Y].Scaling.ShouldBeEqualTo(Scalings.Log);
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
         sut.Chart.Axes[AxisTypes.Y].Scaling.ShouldBeEqualTo(Scalings.Linear);
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
      private ICurveChart _chart;
      private IItemNotifyCache<AxisTypes, IAxis> _cache;

      protected override void Context()
      {
         base.Context();
         _tableFormula = new TableFormula().WithDimension(DomainHelperForSpecs.LengthDimensionForSpecs());
         _tableFormula.AddPoint(1, 10);
         _tableFormula.AddPoint(2, 20);
         _tableFormula.AddPoint(3, 30);
         _cache = new ItemNotifyCache<AxisTypes, IAxis>(x => x.AxisType) { new Axis(AxisTypes.X), new Axis(AxisTypes.Y) };
         _chart = A.Fake<ICurveChart>();
         A.CallTo(() => _chart.Axes).Returns(_cache);
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
      public void should_set_the_data_source_of_the_display_presenter_to_the_created_chart()
      {
         _chartDisplayPresenter.DataSource.ShouldBeEqualTo(_chart);
      }
   }
}