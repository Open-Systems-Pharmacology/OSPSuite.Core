using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using FakeItEasy;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Chart;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Helpers;
using OSPSuite.Presentation.Presenters.Charts;
using OSPSuite.Presentation.Views.Charts;
using OSPSuite.Utility.Collections;
using OSPSuite.Utility.Events;
using OSPSuite.Utility.Exceptions;
using OSPSuite.Utility.Extensions;

namespace OSPSuite.Presentation.Presentation
{
   public abstract class concern_for_ChartEditorPresenter : ContextSpecification<IChartEditorPresenter>
   {
      protected IChartEditorView _view;
      protected IAxisSettingsPresenter _axisSettingsPresenter;
      protected IChartSettingsPresenter _chartSettingsPresenter;
      protected IChartExportSettingsPresenter _chartExportSettingsPresenter;
      protected ICurveSettingsPresenter _curveSettingsPresenter;
      protected ICurveColorGroupingPresenter _curveColorGroupingPresenter;
      protected IDataBrowserPresenter _dataBrowserPresenter;
      protected IChartTemplateMenuPresenter _chartTemplateMenuPresenter;
      protected ICurveChartUpdater _chartUpdater;
      protected IEventPublisher _eventPublisher;
      private IDimensionFactory _dimensionFactory;
      protected CurveChart _chart;
      protected BaseGrid _baseGrid;
      protected DataColumn _standardColumn;
      protected DataColumn _standardColumn2;
      protected DataColumn _standardColumn3;
      protected List<DataRepository> _dataRepositoryList;
      protected BaseGrid _baseGrid2;
      protected BaseGrid _baseGrid3;

      protected override void Context()
      {
         _view = A.Fake<IChartEditorView>();
         _axisSettingsPresenter = A.Fake<IAxisSettingsPresenter>();
         _chartSettingsPresenter = A.Fake<IChartSettingsPresenter>();
         _chartExportSettingsPresenter = A.Fake<IChartExportSettingsPresenter>();
         _curveSettingsPresenter = A.Fake<ICurveSettingsPresenter>();
         _dataBrowserPresenter = A.Fake<IDataBrowserPresenter>();
         _chartTemplateMenuPresenter = A.Fake<IChartTemplateMenuPresenter>();
         _chartUpdater = A.Fake<ICurveChartUpdater>();
         _eventPublisher = A.Fake<IEventPublisher>();
         _dimensionFactory = A.Fake<IDimensionFactory>();
         _curveColorGroupingPresenter = A.Fake<ICurveColorGroupingPresenter>();
         sut = new ChartEditorPresenter(_view, _axisSettingsPresenter, _chartSettingsPresenter, _chartExportSettingsPresenter, _curveSettingsPresenter, _dataBrowserPresenter, _chartTemplateMenuPresenter, _chartUpdater, _eventPublisher, _dimensionFactory, _curveColorGroupingPresenter);

         sut.SetCurveNameDefinition(x => x.QuantityInfo.PathAsString);

         _chart = new CurveChart().WithAxes();
         sut.Edit(_chart);

         _baseGrid = new BaseGrid("Time", DomainHelperForSpecs.TimeDimensionForSpecs());
         _baseGrid2 = new BaseGrid("Time", DomainHelperForSpecs.TimeDimensionForSpecs());
         _baseGrid3 = new BaseGrid("Time", DomainHelperForSpecs.TimeDimensionForSpecs());
         _standardColumn = new DataColumn("Standard", DomainHelperForSpecs.ConcentrationDimensionForSpecs(), _baseGrid)
         {
            DataInfo = new DataInfo(ColumnOrigins.Observation),
         };
         _standardColumn2 = new DataColumn("Standard_2", DomainHelperForSpecs.ConcentrationDimensionForSpecs(), _baseGrid2)
         {
            DataInfo = new DataInfo(ColumnOrigins.Observation),
         };

         _standardColumn3 = new DataColumn("Standard_3", DomainHelperForSpecs.ConcentrationDimensionForSpecs(), _baseGrid3)
         {
            DataInfo = new DataInfo(ColumnOrigins.Observation),
         };

         A.CallTo(() => _dimensionFactory.MergedDimensionFor(_baseGrid)).Returns(_baseGrid.Dimension);
         A.CallTo(() => _dimensionFactory.MergedDimensionFor(_standardColumn)).Returns(_standardColumn.Dimension);
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


         _dataRepository = new DataRepository { _hiddenColumn, _internalColumn, _auxiliaryObservedDataColumn, _standardColumn };

         sut.SetShowDataColumnInDataBrowserDefinition(x => x.Name != _hiddenColumn.Name);

         A.CallTo(() => _dataBrowserPresenter.AddDataColumns(A<IReadOnlyList<DataColumn>>._))
            .Invokes(x => _dataColumnsAdded = x.GetArgument<IReadOnlyList<DataColumn>>(0).ToList());
      }

      protected override void Because()
      {
         sut.AddDataRepositories(new[] { _dataRepository });
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

   public class When_adding_a_curve_for_a_columns_grouped_for_the_same_color : concern_for_ChartEditorPresenter
   {
      protected override void Context()
      {
         base.Context();
         sut.AddCurvesWithSameColorForColumn(new List<DataColumn>() { _standardColumn, _standardColumn2 });
      }

      [Observation]
      public void both_curves_should_have_the_same_value()
      {
         sut.Chart.Curves.Count.ShouldBeEqualTo(2);
         sut.Chart.Curves.Each(elem => elem.Color.ShouldBeEqualTo(sut.Chart.Curves.First().Color));
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

         _dataRepository = new DataRepository { _columnThatWillBeInternal, _columnThatWillBeRemoved, _standardColumn };

         sut.SetShowDataColumnInDataBrowserDefinition(x => true);
         sut.AddDataRepositories(new[] { _dataRepository });

         _columnThatWillBeInternal.IsInternal = true;
         _dataRepository.Remove(_columnThatWillBeRemoved);

         A.CallTo(() => _dataBrowserPresenter.AllDataColumns).Returns(new[] { _columnThatWillBeInternal, _columnThatWillBeRemoved, _standardColumn });

         A.CallTo(() => _dataBrowserPresenter.RemoveDataColumns(A<IReadOnlyList<DataColumn>>._))
            .Invokes(x => _dataColumnsRemoved = x.GetArgument<IReadOnlyList<DataColumn>>(0).ToList());
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
      private CurveChart _anotherCurveChart;
      private CurveChart _curveChart;

      protected override void Context()
      {
         base.Context();
         _curveChart = new CurveChart { Id = "TOTO" };
         _anotherCurveChart = new CurveChart { Id = "HELLO" };
         sut.Edit(_curveChart);
      }

      protected override void Because()
      {
         sut.Handle(new CurveChartUpdatedEvent(_anotherCurveChart, null, refreshCurveData: true, propagateChartChangeEvent: true));
      }

      [Observation]
      public void should_not_refresh_the_chart()
      {
         A.CallTo(() => _axisSettingsPresenter.Refresh()).MustNotHaveHappened();
      }
   }

   public class When_the_chart_editor_presenter_is_notified_of_a_chart_updated_event_for_a_chart_that_is_being_edited : concern_for_ChartEditorPresenter
   {
      private CurveChart _curveChart;
      private bool _notified;

      protected override void Context()
      {
         base.Context();
         _curveChart = new CurveChart { Id = "TOTO" };
         sut.Edit(_curveChart);
         sut.ChartChanged += () => _notified = true;
      }

      protected override void Because()
      {
         sut.Handle(new CurveChartUpdatedEvent(_curveChart, null, refreshCurveData: true, propagateChartChangeEvent: true));
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

   public class When_the_chart_editor_presenter_is_notified_of_a_chart_updated_event_for_a_chart_that_is_being_edited_and_no_chart_changed_event_should_be_propagated : concern_for_ChartEditorPresenter
   {
      private CurveChart _curveChart;
      private bool _notified;

      protected override void Context()
      {
         base.Context();
         _curveChart = new CurveChart { Id = "TOTO" };
         sut.Edit(_curveChart);
         sut.ChartChanged += () => _notified = true;
      }

      protected override void Because()
      {
         sut.Handle(new CurveChartUpdatedEvent(_curveChart, null, refreshCurveData: true, propagateChartChangeEvent: false));
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

   public class When_chart_editor_presenter_is_clearing_its_content : concern_for_ChartEditorPresenter
   {
      protected override void Because()
      {
         sut.Clear();
      }

      [Observation]
      public void should_clear_all_sub_presenters()
      {
         A.CallTo(() => _dataBrowserPresenter.Clear()).MustHaveHappened();
         A.CallTo(() => _curveSettingsPresenter.Clear()).MustHaveHappened();
         A.CallTo(() => _chartSettingsPresenter.Clear()).MustHaveHappened();
         A.CallTo(() => _chartExportSettingsPresenter.Clear()).MustHaveHappened();
         A.CallTo(() => _axisSettingsPresenter.Clear()).MustHaveHappened();
      }
   }

   public class When_the_chart_editor_presenter_is_notified_that_the_used_state_of_some_columns_was_changed_to_used : concern_for_ChartEditorPresenter
   {
      protected override void Because()
      {
         _dataBrowserPresenter.UsedChanged += Raise.With(new UsedColumnsEventArgs(new[] { _standardColumn, }, true, false));
      }

      [Observation]
      public void should_add_columns_as_curve_to_the_chart()
      {
         _chart.FindCurveWithSameData(_standardColumn.BaseGrid, _standardColumn).ShouldNotBeNull();
      }

      [Observation]
      public void should_update_the_chart()
      {
         A.CallTo(() => _chartUpdater.UpdateTransaction(_chart, CurveChartUpdateModes.Add, true)).MustHaveHappened();
      }
   }

   public class When_the_chart_editor_presenter_is_notified_that_the_selected_columns_have_changed_in_the_data_browser_and_all_selected_columns_are_used : concern_for_ChartEditorPresenter
   {
      protected override void Context()
      {
         base.Context();
         A.CallTo(() => _dataBrowserPresenter.IsUsed(_standardColumn)).Returns(true);
         A.CallTo(() => _dataBrowserPresenter.IsUsed(_baseGrid)).Returns(true);
      }

      protected override void Because()
      {
         _dataBrowserPresenter.SelectionChanged += Raise.With(new ColumnsEventArgs(new[] { _standardColumn, _baseGrid, }));
      }

      [Observation]
      public void should_update_the_state_of_the_used_check_box_in_the_view_to_selected()
      {
         A.CallTo(() => _view.SetSelectAllCheckBox(true)).MustHaveHappened();
      }
   }

   public class When_the_chart_editor_presenter_is_notified_that_the_selected_columns_have_changed_in_the_data_browser_and_all_selected_columns_are_unused : concern_for_ChartEditorPresenter
   {
      protected override void Context()
      {
         base.Context();
         A.CallTo(() => _dataBrowserPresenter.IsUsed(_standardColumn)).Returns(false);
         A.CallTo(() => _dataBrowserPresenter.IsUsed(_baseGrid)).Returns(false);
      }

      protected override void Because()
      {
         _dataBrowserPresenter.SelectionChanged += Raise.With(new ColumnsEventArgs(new[] { _standardColumn, _baseGrid, }));
      }

      [Observation]
      public void should_update_the_state_of_the_used_check_box_in_the_view_to_unselected()
      {
         A.CallTo(() => _view.SetSelectAllCheckBox(false)).MustHaveHappened();
      }
   }

   public class When_the_chart_editor_presenter_is_notified_that_the_selected_columns_have_changed_in_the_data_browser_and_some__columns_are_used_and_other_unused : concern_for_ChartEditorPresenter
   {
      protected override void Context()
      {
         base.Context();
         A.CallTo(() => _dataBrowserPresenter.IsUsed(_standardColumn)).Returns(false);
         A.CallTo(() => _dataBrowserPresenter.IsUsed(_baseGrid)).Returns(true);
      }

      protected override void Because()
      {
         _dataBrowserPresenter.SelectionChanged += Raise.With(new ColumnsEventArgs(new[] { _standardColumn, _baseGrid, }));
      }

      [Observation]
      public void should_update_the_state_of_the_used_check_box_in_the_view_to_undefined()
      {
         A.CallTo(() => _view.SetSelectAllCheckBox(null)).MustHaveHappened();
      }
   }

   public class When_the_chart_editor_presenter_is_notified_that_the_used_state_of_some_columns_was_changed_to_unused : concern_for_ChartEditorPresenter
   {
      protected override void Context()
      {
         base.Context();
         sut.AddCurveForColumn(_standardColumn);
      }

      protected override void Because()
      {
         _dataBrowserPresenter.UsedChanged += Raise.With(new UsedColumnsEventArgs(new[] { _standardColumn, }, false, false));
      }

      [Observation]
      public void should_add_columns_as_curve_to_the_chart()
      {
         _chart.FindCurveWithSameData(_standardColumn.BaseGrid, _standardColumn).ShouldBeNull();
      }

      [Observation]
      public void should_update_the_chart()
      {
         A.CallTo(() => _chartUpdater.UpdateTransaction(_chart, CurveChartUpdateModes.Remove, true)).MustHaveHappened();
      }
   }

   public class When_the_chart_editor_presenter_is_notified_that_the_data_is_linked_to_simulation : concern_for_ChartEditorPresenter
   {
      protected override void Context()
      {
         base.Context();
         sut.AddCurveForColumn(_standardColumn, isLinkedDataToSimulation: false);
      }

      protected override void Because()
      {
         sut.AddCurveForColumn(_standardColumn2, isLinkedDataToSimulation: true);
      }

      [Observation]
      public void should_add_curve_with_same_color()
      {
         sut.Chart.Curves.Count().ShouldBeEqualTo(2);
         sut.Chart.Curves.First().Color.ShouldBeEqualTo(sut.Chart.Curves.ToList()[1].Color); 
      }
   }

   public class When_the_chart_editor_presenter_is_notified_that_an_axis_should_be_removed : concern_for_ChartEditorPresenter
   {
      private Axis _axisY2;

      protected override void Context()
      {
         base.Context();
         sut.AddCurveForColumn(_standardColumn);
         _axisY2 = new Axis(AxisTypes.Y2);
         _chart.AddAxis(_axisY2);
      }

      protected override void Because()
      {
         _axisSettingsPresenter.AxisRemoved += Raise.With(new AxisEventArgs(_axisY2));
      }

      [Observation]
      public void should_remove_the_axis()
      {
         _chart.HasAxis(AxisTypes.Y2).ShouldBeFalse();
      }

      [Observation]
      public void should_update_the_chart()
      {
         A.CallTo(() => _chartUpdater.UpdateTransaction(_chart, CurveChartUpdateModes.All, true)).MustHaveHappened();
      }
   }

   public class When_the_chart_editor_presenter_is_notified_that_an_axis_should_be_removed_that_is_not_the_higher_ranked_axis : concern_for_ChartEditorPresenter
   {
      private Axis _axisY2;

      protected override void Context()
      {
         base.Context();
         sut.AddCurveForColumn(_standardColumn);
         _axisY2 = new Axis(AxisTypes.Y2);
         _chart.AddAxis(_axisY2);
         _chart.AddAxis(new Axis(AxisTypes.Y3));
      }

      [Observation]
      public void should_thrown_an_exception_explaining_that_the_axis_cannot_be_removed()
      {
         The.Action(() => { _axisSettingsPresenter.AxisRemoved += Raise.With(new AxisEventArgs(_axisY2)); }).ShouldThrowAn<OSPSuiteException>();
      }
   }

   public class When_the_chart_editor_presenter_is_notified_that_some_properties_of_an_axis_were_changed : concern_for_ChartEditorPresenter
   {
      protected override void Because()
      {
         _axisSettingsPresenter.AxisPropertyChanged += Raise.With(new AxisEventArgs(_chart.XAxis));
      }

      [Observation]
      public void should_update_the_chart()
      {
         A.CallTo(() => _chartUpdater.Update(_chart, CurveChartUpdateModes.All)).MustHaveHappened();
      }
   }

   public class When_the_chart_editor_presenter_is_notified_that_an_axis_should_be_added : concern_for_ChartEditorPresenter
   {
      protected override void Context()
      {
         base.Context();
         _chart.HasAxis(AxisTypes.Y2).ShouldBeFalse();
      }

      protected override void Because()
      {
         _axisSettingsPresenter.AxisAdded += Raise.WithEmpty();
      }

      [Observation]
      public void should_let_the_chart_add_a_new_axis()
      {
         _chart.HasAxis(AxisTypes.Y2).ShouldBeTrue();
      }

      [Observation]
      public void should_update_the_chart()
      {
         A.CallTo(() => _chartUpdater.UpdateTransaction(_chart, CurveChartUpdateModes.All, true)).MustHaveHappened();
      }
   }

   public class When_the_chart_editor_presenter_is_being_notified_that_the_chart_export_settings_were_changed : concern_for_ChartEditorPresenter
   {
      protected override void Because()
      {
         _chartExportSettingsPresenter.ChartExportSettingsChanged += Raise.WithEmpty();
      }

      [Observation]
      public void should_notify_a_chart_property_changed_event()
      {
         A.CallTo(() => _eventPublisher.PublishEvent(A<ChartPropertiesChangedEvent>.That.Matches(x => x.Chart == _chart))).MustHaveHappened();
      }
   }

   public class When_the_chart_editor_presenter_is_notified_that_a_curve_should_be_removed_from_the_chart : concern_for_ChartEditorPresenter
   {
      private Curve _curve;

      protected override void Context()
      {
         base.Context();
         _curve = sut.AddCurveForColumn(_standardColumn);
      }

      protected override void Because()
      {
         _curveSettingsPresenter.RemoveCurve += Raise.With(new CurveEventArgs(_curve));
      }

      [Observation]
      public void should_remove_the_curve_from_the_chart()
      {
         _chart.HasCurve(_curve.Id).ShouldBeFalse();
      }

      [Observation]
      public void should_update_the_chart()
      {
         A.CallTo(() => _chartUpdater.UpdateTransaction(_chart, CurveChartUpdateModes.Remove, true)).MustHaveHappened();
      }
   }

   public class When_the_chart_editor_presenter_is_notified_that_a_curve_should_be_added_for_some_columns : concern_for_ChartEditorPresenter
   {
      protected override void Because()
      {
         _curveSettingsPresenter.AddCurves += Raise.With(new ColumnsEventArgs(new[] { _standardColumn, }));
      }

      [Observation]
      public void should_have_added_a_curve_for_the_column_to_the_chart()
      {
         _chart.FindCurveWithSameData(_standardColumn.BaseGrid, _standardColumn).ShouldNotBeNull();
      }

      [Observation]
      public void should_update_the_chart()
      {
         A.CallTo(() => _chartUpdater.UpdateTransaction(_chart, CurveChartUpdateModes.Add, true)).MustHaveHappened();
      }
   }

   public class When_the_chart_editor_presenter_is_notified_that_the_properties_of_a_curve_were_changed : concern_for_ChartEditorPresenter
   {
      private Curve _curve;

      protected override void Context()
      {
         base.Context();
         _curve = sut.AddCurveForColumn(_standardColumn);
      }

      protected override void Because()
      {
         _curveSettingsPresenter.CurvePropertyChanged += Raise.With(new CurveEventArgs(_curve));
      }

      [Observation]
      public void should_update_the_chart()
      {
         A.CallTo(() => _chartUpdater.Update(_chart, A<IReadOnlyList<Curve>>._, CurveChartUpdateModes.Property)).MustHaveHappened();
      }
   }

   public class When_the_chart_editor_presenter_is_notified_that_the_chart_settings_were_changed : concern_for_ChartEditorPresenter
   {
      protected override void Because()
      {
         _chartSettingsPresenter.ChartSettingsChanged += Raise.WithEmpty();
      }

      [Observation]
      public void should_update_the_chart()
      {
         A.CallTo(() => _chartUpdater.Update(_chart, CurveChartUpdateModes.All)).MustHaveHappened();
      }
   }

   public class When_setting_metadata_for_color_grouping : concern_for_ChartEditorPresenter
   {
      private IReadOnlyList<string> _commonMetaData;
      private IReadOnlyList<string> _calculatedCommonMetaData;
      private DataRepository _dataRepository1;
      private DataRepository _dataRepository2;
      private DataRepository _dataRepository3;

      protected override void Context()
      {
         base.Context();
         _dataRepository1 = new DataRepository { _standardColumn };
         _dataRepository2 = new DataRepository { _standardColumn2 };
         _dataRepository3 = new DataRepository { _standardColumn3 };
         _dataRepository1.ExtendedProperties.Add(new ExtendedProperty<int>() { Name = "ID", Value = 1 });
         _dataRepository2.ExtendedProperties.Add(new ExtendedProperty<int>() { Name = "ID", Value = 2 });
         _dataRepository3.ExtendedProperties.Add(new ExtendedProperty<int>() { Name = "ID", Value = 3 });

         _dataRepository1.ExtendedProperties.Add(new ExtendedProperty<string>() { Name = "Species", Value = "Human" });
         _dataRepository2.ExtendedProperties.Add(new ExtendedProperty<string>() { Name = "Species", Value = "Human" });
         _dataRepository2.ExtendedProperties.Add(new ExtendedProperty<string>() { Name = "NotCommonMetaData", Value = "test" });
         _dataRepository3.ExtendedProperties.Add(new ExtendedProperty<string>() { Name = "Species", Value = "Dog" });
         _dataRepositoryList = new List<DataRepository>() { _dataRepository1, _dataRepository2, _dataRepository3 };

         sut.AddDataRepositories(_dataRepositoryList);

         _commonMetaData = new List<string> { "ID", "Species" };
         ;

         A.CallTo(() => _dataBrowserPresenter.GetAllUsedDataColumns()).Returns(new[] { _standardColumn, _standardColumn2, _standardColumn3 });


         A.CallTo(() => _curveColorGroupingPresenter.SetMetadata(A<IReadOnlyList<string>>._))
            .Invokes(x => { _calculatedCommonMetaData = x.GetArgument<IReadOnlyList<string>>(0); });
      }

      protected override void Because()
      {
         sut.Handle(new CurveChartUpdatedEvent(_chart, null, true, true));
      }

      [Observation]
      public void should_set_common_meta_data_correctly()
      {
         _calculatedCommonMetaData.ShouldBeEqualTo(_commonMetaData);
      }
   }

   public class When_the_chart_editor_presenter_is_notified_that_color_grouping_should_be_applied : concern_for_ChartEditorPresenter
   {
      private readonly Cache<string, Color> _colorsForCurveName = new Cache<string, Color>();
      private List<string> _groupingCriteria;
      private DataRepository _dataRepository1;
      private DataRepository _dataRepository2;
      private DataRepository _dataRepository3;
      private Curve _curve;
      private Curve _curve2;
      private Curve _curve3;

      protected override void Context()
      {
         base.Context();
         _dataRepository1 = new DataRepository { _standardColumn };
         _dataRepository2 = new DataRepository { _standardColumn2 };
         _dataRepository3 = new DataRepository { _standardColumn3 };
         _dataRepository1.ExtendedProperties.Add(new ExtendedProperty<int>() { Name = "ID", Value = 1 });
         _dataRepository2.ExtendedProperties.Add(new ExtendedProperty<int>() { Name = "ID", Value = 2 });
         _dataRepository3.ExtendedProperties.Add(new ExtendedProperty<int>() { Name = "ID", Value = 3 });

         _dataRepository1.ExtendedProperties.Add(new ExtendedProperty<string>() { Name = "Species", Value = "Human" });
         _dataRepository2.ExtendedProperties.Add(new ExtendedProperty<string>() { Name = "Species", Value = "Human" });
         _dataRepository2.ExtendedProperties.Add(new ExtendedProperty<string>() { Name = "NotCommonMetaData", Value = "test" });
         _dataRepository3.ExtendedProperties.Add(new ExtendedProperty<string>() { Name = "Species", Value = "Dog" });
         _dataRepositoryList = new List<DataRepository>() { _dataRepository1, _dataRepository2, _dataRepository3 };

         _curve = sut.AddCurveForColumn(_standardColumn);
         _curve2 = sut.AddCurveForColumn(_standardColumn2);
         _curve3 = sut.AddCurveForColumn(_standardColumn3);

         A.CallTo(() => _dataBrowserPresenter.GetAllUsedDataColumns()).Returns(new[] { _standardColumn, _standardColumn2, _standardColumn3 });
         sut.AddDataRepositories(_dataRepositoryList);


         A.CallTo(() => _curveSettingsPresenter.UpdateColorForCurve(A<Curve>._, A<Color>._))
            .Invokes(x =>
            {
               //since curveSettingsPresenter is a Fake, we need to do this to get the list of usedColors updated.
               //otherwise we get the same color assigned all the time
               x.GetArgument<Curve>(0).Color = x.GetArgument<Color>(1);
               _colorsForCurveName.Add(x.GetArgument<Curve>(0).Id, x.GetArgument<Color>(1));
            });
      }

      protected override void Because()
      {
         sut.Handle(new CurveChartUpdatedEvent(_chart, null, true, true));
      }

      [Observation]
      public void should_group_on_single_criterion()
      {
         _groupingCriteria = new List<string> { "Species" };
         _curveColorGroupingPresenter.ApplySelectedColorGrouping += Raise.With(new CurveColorGroupingEventArgs(_groupingCriteria));

         _colorsForCurveName[_curve.Id].Equals(_colorsForCurveName[_curve2.Id]).ShouldBeTrue();
         _colorsForCurveName[_curve3.Id].Equals(_colorsForCurveName[_curve.Id]).ShouldBeFalse();
         _colorsForCurveName[_curve3.Id].Equals(_colorsForCurveName[_curve2.Id]).ShouldBeFalse();
      }

      [Observation]
      public void should_group_on_multiple_criteria()
      {
         _groupingCriteria = new List<string> { "Species", "ID" };
         _curveColorGroupingPresenter.ApplySelectedColorGrouping += Raise.With(new CurveColorGroupingEventArgs(_groupingCriteria));

         _colorsForCurveName[_curve.Id].Equals(_colorsForCurveName[_curve2.Id]).ShouldBeFalse();
         _colorsForCurveName[_curve3.Id].Equals(_colorsForCurveName[_curve.Id]).ShouldBeFalse();
         _colorsForCurveName[_curve3.Id].Equals(_colorsForCurveName[_curve2.Id]).ShouldBeFalse();
      }
   }
}