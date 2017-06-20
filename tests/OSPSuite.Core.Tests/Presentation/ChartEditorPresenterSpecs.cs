using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using FakeItEasy;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Chart;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Helpers;
using OSPSuite.Presentation.Presenters.Charts;
using OSPSuite.Presentation.Views.Charts;
using OSPSuite.Utility.Events;

namespace OSPSuite.Presentation
{
   public abstract class concern_for_ChartEditorPresenter : ContextSpecification<IChartEditorPresenter>
   {
      protected IChartEditorView _view;
      protected IAxisSettingsPresenter _axisSettingsPresenter;
      protected IChartSettingsPresenter _chartSettingsPresenter;
      protected IChartExportSettingsPresenter _chartExportSettingsPresneter;
      protected ICurveSettingsPresenter _curveSettingsPresenter;
      protected IDataBrowserPresenter _dataBrowserPresenter;
      protected IChartTemplateMenuPresenter _chartTemplateMenuPresenter;
      private IChartUpdater _chartUpdater;
      private IEventPublisher _eventPublisher;
      private IDimensionFactory _dimensionFactory;
      protected CurveChart _chart;
      protected BaseGrid _baseGrid;
      protected DataColumn _standardColumn;

      protected override void Context()
      {
         _view = A.Fake<IChartEditorView>();
         _axisSettingsPresenter = A.Fake<IAxisSettingsPresenter>();
         _chartSettingsPresenter = A.Fake<IChartSettingsPresenter>();
         _chartExportSettingsPresneter = A.Fake<IChartExportSettingsPresenter>();
         _curveSettingsPresenter = A.Fake<ICurveSettingsPresenter>();
         _dataBrowserPresenter = A.Fake<IDataBrowserPresenter>();
         _chartTemplateMenuPresenter = A.Fake<IChartTemplateMenuPresenter>();
         _chartUpdater = A.Fake<IChartUpdater>();
         _eventPublisher = A.Fake<IEventPublisher>();
         _dimensionFactory = A.Fake<IDimensionFactory>();
         sut = new ChartEditorPresenter(_view, _axisSettingsPresenter, _chartSettingsPresenter, _chartExportSettingsPresneter, _curveSettingsPresenter, _dataBrowserPresenter, _chartTemplateMenuPresenter, _chartUpdater, _eventPublisher, _dimensionFactory);

         sut.SetCurveNameDefinition(x => x.QuantityInfo.PathAsString);

         _chart = new CurveChart().WithAxes();
         sut.Edit(_chart);

         _baseGrid = new BaseGrid("Time", DomainHelperForSpecs.TimeDimensionForSpecs());
         _standardColumn = new DataColumn("Standard", DomainHelperForSpecs.ConcentrationDimensionForSpecs(), _baseGrid)
         {
            DataInfo = new DataInfo(ColumnOrigins.Calculation),
         };

         A.CallTo(() => _dimensionFactory.GetMergedDimensionFor(_baseGrid)).Returns(_baseGrid.Dimension);
         A.CallTo(() => _dimensionFactory.GetMergedDimensionFor(_standardColumn)).Returns(_standardColumn.Dimension);
      }
   }

   public class When_adding_a_repository_to_the_chart_editor_presenter : concern_for_ChartEditorPresenter
   {
      private DataRepository _dataRepository;
      private DataColumn _hiddenColumn;
      private DataColumn _internalColumn;
      private DataColumn _auxiliaryObservedDataColumn;
      private List<DataColumn> _dataColumnsAdded;

      protected override void Context()
      {
         base.Context();
         _hiddenColumn = new DataColumn("hidden", DomainHelperForSpecs.ConcentrationDimensionForSpecs(), _baseGrid)
         {
            DataInfo = new DataInfo(ColumnOrigins.Calculation)
         };

         _internalColumn = new DataColumn("Internal", DomainHelperForSpecs.ConcentrationDimensionForSpecs(), _baseGrid)
         {
            DataInfo = new DataInfo(ColumnOrigins.Calculation),
            IsInternal = true
         };

         _auxiliaryObservedDataColumn = new DataColumn("Auxiliary", DomainHelperForSpecs.ConcentrationDimensionForSpecs(), _baseGrid)
         {
            DataInfo = new DataInfo(ColumnOrigins.ObservationAuxiliary),
         };


         _dataRepository = new DataRepository {_hiddenColumn, _internalColumn, _auxiliaryObservedDataColumn, _standardColumn};

         sut.SetShowDataColumnInDataBrowserDefinition(x => x.Name != _hiddenColumn.Name);

         A.CallTo(() => _dataBrowserPresenter.AddDataColumns(A<IEnumerable<DataColumn>>._))
            .Invokes(x => _dataColumnsAdded = x.GetArgument<IEnumerable<DataColumn>>(0).ToList());
      }

      protected override void Because()
      {
         sut.AddDataRepositories(new[] {_dataRepository});
      }

      [Observation]
      public void should_not_add_columns_that_are_marked_as_internal()
      {
         _dataColumnsAdded.ShouldNotContain(_internalColumn);
      }

      [Observation]
      public void should_not_add_columns_that_should_not_be_shown_in_the_data_browser()
      {
         _dataColumnsAdded.ShouldNotContain(_hiddenColumn);
      }

      [Observation]
      public void should_not_add_observed_data_auxiliary_column()
      {
         _dataColumnsAdded.ShouldNotContain(_auxiliaryObservedDataColumn);
      }

      [Observation]
      public void should_add_standard_column()
      {
         _dataColumnsAdded.ShouldContain(_standardColumn);
      }
   }

   internal class When_adding_a_new_curve_for_column_id_not_in_chart_and_default_settings_are_specified : concern_for_ChartEditorPresenter
   {
      private CurveOptions _defaultCurveOptions;
      private Curve _newCurve;

      protected override void Context()
      {
         base.Context();
         _defaultCurveOptions = new CurveOptions
         {
            Color = Color.Fuchsia,
            LineThickness = 2,
            VisibleInLegend = false,
            Symbol = Symbols.Diamond,
            LineStyle = LineStyles.DashDot
         };
      }

      protected override void Because()
      {
         _newCurve = sut.AddCurveForColumn(_standardColumn, _defaultCurveOptions);
      }

      [Observation]
      public void curve_options_should_be_updated_to_defaults()
      {
         _newCurve.CurveOptions.Color.ShouldBeEqualTo(Color.Fuchsia);
         _newCurve.CurveOptions.LineThickness.ShouldBeEqualTo(2);
         _newCurve.CurveOptions.VisibleInLegend.ShouldBeEqualTo(false);
         _newCurve.CurveOptions.Symbol.ShouldBeEqualTo(Symbols.Diamond);
         _newCurve.CurveOptions.LineStyle.ShouldBeEqualTo(LineStyles.DashDot);
      }
   }

   internal class When_adding_a_curve_for_a_column_already_in_chart_and_default_settings_are_specified : concern_for_ChartEditorPresenter
   {
      private CurveOptions _defaultCurveOptions;
      private CurveOptions _firstPlotCurveOptions;
      private Curve _curve;

      protected override void Context()
      {
         base.Context();
         _firstPlotCurveOptions = new CurveOptions
         {
            Color = Color.Black,
            LineThickness = 1,
            VisibleInLegend = true
         };

         _defaultCurveOptions = new CurveOptions
         {
            Color = Color.Fuchsia,
            LineThickness = 2,
            VisibleInLegend = false
         };

         sut.AddCurveForColumn(_standardColumn, _firstPlotCurveOptions);
      }

      protected override void Because()
      {
         _curve = sut.AddCurveForColumn(_standardColumn, _defaultCurveOptions);
      }

      [Observation]
      public void the_curve_options_should_not_have_been_updated_to_the_second_specified_values()
      {
         _curve.CurveOptions.Color.ShouldBeEqualTo(Color.Black);
         _curve.CurveOptions.LineThickness.ShouldBeEqualTo(1);
         _curve.CurveOptions.VisibleInLegend.ShouldBeEqualTo(true);
      }
   }

   public class When_refreshing_a_data_repository_used_in_the_chart_editor_presenter : concern_for_ChartEditorPresenter
   {
      private DataRepository _dataRepository;
      private DataColumn _columnThatWillBeInternal;
      private DataColumn _columnThatWillBeRemoved;
      private List<DataColumn> _dataColumnsRemoved;

      protected override void Context()
      {
         base.Context();
         var baseGrid = new BaseGrid("Time", DomainHelperForSpecs.TimeDimensionForSpecs());

         _columnThatWillBeInternal = new DataColumn("Column that wil be internal", DomainHelperForSpecs.ConcentrationDimensionForSpecs(), baseGrid)
         {
            DataInfo = new DataInfo(ColumnOrigins.Calculation),
            IsInternal = false
         };

         _columnThatWillBeRemoved = new DataColumn("Column That will be removed", DomainHelperForSpecs.ConcentrationDimensionForSpecs(), baseGrid)
         {
            DataInfo = new DataInfo(ColumnOrigins.Calculation),
         };

         _standardColumn = new DataColumn("Standard", DomainHelperForSpecs.ConcentrationDimensionForSpecs(), baseGrid)
         {
            DataInfo = new DataInfo(ColumnOrigins.Calculation),
         };

         _dataRepository = new DataRepository {_columnThatWillBeInternal, _columnThatWillBeRemoved, _standardColumn};

         sut.SetShowDataColumnInDataBrowserDefinition(x => true);
         sut.AddDataRepositories(new[] {_dataRepository});

         _columnThatWillBeInternal.IsInternal = true;
         _dataRepository.Remove(_columnThatWillBeRemoved);

         A.CallTo(() => _dataBrowserPresenter.AllDataColumns).Returns(new[] {_columnThatWillBeInternal, _columnThatWillBeRemoved, _standardColumn});

         A.CallTo(() => _dataBrowserPresenter.RemoveDataColumns(A<IEnumerable<DataColumn>>._))
            .Invokes(x => _dataColumnsRemoved = x.GetArgument<IEnumerable<DataColumn>>(0).ToList());
      }

      protected override void Because()
      {
         sut.RemoveUnusedColumns();
      }

      [Observation]
      public void should_remove_the_column_that_were_removed()
      {
         _dataColumnsRemoved.ShouldContain(_columnThatWillBeRemoved);
      }

      [Observation]
      public void should_remove_the_column_that_became_internal()
      {
         _dataColumnsRemoved.ShouldContain(_columnThatWillBeInternal);
      }

      [Observation]
      public void should_not_remove_the_existing_columns()
      {
         _dataColumnsRemoved.ShouldNotContain(_standardColumn);
      }
   }

   public class When_the_chart_editor_presenter_is_notififed_of_a_chart_updated_event_for_a_chart_that_is_not_edited : concern_for_ChartEditorPresenter
   {
      private IChart _anotherCurveChart;
      private CurveChart _curveChart;

      protected override void Context()
      {
         base.Context();
         _curveChart = new CurveChart {Id = "TOTO"};
         _anotherCurveChart = new CurveChart {Id = "HELLO"};
         sut.Edit(_curveChart);
      }

      protected override void Because()
      {
         sut.Handle(new ChartUpdatedEvent(_anotherCurveChart, true));
      }

      [Observation]
      public void should_not_refresh_the_chart()
      {
         A.CallTo(() => _axisSettingsPresenter.Refresh()).MustNotHaveHappened();
      }
   }

   public class When_the_chart_editor_presenter_is_notififed_of_a_chart_updated_event_for_a_chart_that_is_being_edited : concern_for_ChartEditorPresenter
   {
      private CurveChart _curveChart;
      private bool _notified;

      protected override void Context()
      {
         base.Context();
         _curveChart = new CurveChart {Id = "TOTO"};
         sut.Edit(_curveChart);
         sut.ChartChanged += () => _notified = true;
      }

      protected override void Because()
      {
         sut.Handle(new ChartUpdatedEvent(_curveChart, propogateChartChangeEvent: true));
      }

      [Observation]
      public void should_refresh_the_chart()
      {
         A.CallTo(() => _axisSettingsPresenter.Refresh()).MustHaveHappened();
      }

      [Observation]
      public void should_notify_the_chart_changed_event()
      {
         _notified.ShouldBeTrue();
      }
   }

   public class When_the_chart_editor_presenter_is_notififed_of_a_chart_updated_event_for_a_chart_that_is_being_edited_and_no_chart_changed_event_should_be_propagated : concern_for_ChartEditorPresenter
   {
      private CurveChart _curveChart;
      private bool _notified;

      protected override void Context()
      {
         base.Context();
         _curveChart = new CurveChart {Id = "TOTO"};
         sut.Edit(_curveChart);
         sut.ChartChanged += () => _notified = true;
      }

      protected override void Because()
      {
         sut.Handle(new ChartUpdatedEvent(_curveChart, propogateChartChangeEvent: false));
      }

      [Observation]
      public void should_refresh_the_chart()
      {
         A.CallTo(() => _axisSettingsPresenter.Refresh()).MustHaveHappened();
      }

      [Observation]
      public void should_not_notify_the_chart_changed_event()
      {
         _notified.ShouldBeFalse();
      }
   }
}