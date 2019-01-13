using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using FakeItEasy;
using OSPSuite.Core.Chart;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Events;
using OSPSuite.Presentation.Presenters.Charts;
using OSPSuite.Presentation.Presenters.ObservedData;
using OSPSuite.Presentation.Views.ObservedData;

namespace OSPSuite.Presentation
{
   public abstract class concern_for_DataRepositoryChartPresenter : ContextSpecification<DataRepositoryChartPresenter>
   {
      protected ISimpleChartPresenter _simpleChartPresenter = A.Fake<ISimpleChartPresenter>();
      protected IDataRepositoryChartView _view = A.Fake<IDataRepositoryChartView>();
      protected DataRepository _repository = new DataRepository();
      private CurveChart _chart;

      protected override void Context()
      {
         base.Context();
         _chart = new CurveChart().WithAxes();
         A.CallTo(() => _simpleChartPresenter.Chart).Returns(_chart);
         sut = new DataRepositoryChartPresenter(_view, _simpleChartPresenter);
         sut.EditObservedData(_repository);
      }
   }

   public class When_handling_observed_data_changed_event_on_other_repository : concern_for_DataRepositoryChartPresenter
   {
      [Observation]
      public void should_not_redraw_chart()
      {
         A.CallTo(() => _simpleChartPresenter.PlotObservedData(A<DataRepository>._)).MustHaveHappenedOnceExactly();
      }

      protected override void Because()
      {
         sut.Handle(new ObservedDataValueChangedEvent(new DataRepository()));
      }
   }

   public class When_handling_observed_data_changed_event : concern_for_DataRepositoryChartPresenter
   {
      [Observation]
      public void should_redraw_chart_with_new_data()
      {
         A.CallTo(() => _simpleChartPresenter.PlotObservedData(_repository)).MustHaveHappenedTwiceExactly();
      }

      protected override void Because()
      {
         sut.Handle(new ObservedDataValueChangedEvent(_repository));
      }
   }

   public class When_handling_table_observed_data_changed_event : concern_for_DataRepositoryChartPresenter
   {
      [Observation]
      public void should_redraw_chart_with_new_data()
      {
         A.CallTo(() => _simpleChartPresenter.PlotObservedData(_repository)).MustHaveHappenedTwiceExactly();
      }

      protected override void Because()
      {
         sut.Handle(new ObservedDataTableChangedEvent(_repository));
      }
   }
}