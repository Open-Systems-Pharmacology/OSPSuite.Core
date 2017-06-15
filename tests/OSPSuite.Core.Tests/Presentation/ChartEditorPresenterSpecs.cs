using System.Drawing;
using OSPSuite.BDDHelper;
using FakeItEasy;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core;
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
         _chartUpdater= A.Fake<IChartUpdater>();
         _eventPublisher= A.Fake<IEventPublisher>();
         _dimensionFactory= A.Fake<IDimensionFactory>();
         sut = new ChartEditorPresenter(_view, _axisSettingsPresenter, _chartSettingsPresenter, _chartExportSettingsPresneter, _curveSettingsPresenter, _dataBrowserPresenter, _chartTemplateMenuPresenter,_chartUpdater, _eventPublisher, _dimensionFactory);

         sut.SetCurveNameDefinition(x=>x.QuantityInfo.PathAsString);

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
      }

      protected override void Because()
      {
         sut.AddDataRepository(_dataRepository);
      }

      [Observation]
      public void should_not_add_columns_that_are_marked_as_internal()
      {
         A.CallTo(() => _dataBrowserPresenter.AddDataColumn(_internalColumn)).MustNotHaveHappened();
      }

      [Observation]
      public void should_not_add_columns_that_should_not_be_shown_in_the_data_browser()
      {
         A.CallTo(() => _dataBrowserPresenter.AddDataColumn(_hiddenColumn)).MustNotHaveHappened();
      }

      [Observation]
      public void should_not_add_observed_data_auxiliary_column()
      {
         A.CallTo(() => _dataBrowserPresenter.AddDataColumn(_auxiliaryObservedDataColumn)).MustNotHaveHappened();
      }

      [Observation]
      public void should_add_standard_column()
      {
         A.CallTo(() => _dataBrowserPresenter.AddDataColumn(_standardColumn)).MustHaveHappened();
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
      private DataColumn _newColumn;
      private DataColumn _columnThatWillBeRemoved;

      protected override void Context()
      {
         base.Context();
         var baseGrid = new BaseGrid("Time", DomainHelperForSpecs.TimeDimensionForSpecs());
         _newColumn = new DataColumn("New", DomainHelperForSpecs.ConcentrationDimensionForSpecs(), baseGrid)
         {
            DataInfo = new DataInfo(ColumnOrigins.Calculation)
         };

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
         sut.AddDataRepository(_dataRepository);

         _columnThatWillBeInternal.IsInternal = true;
         _dataRepository.Remove(_columnThatWillBeRemoved);
         _dataRepository.Add(_newColumn);
      }

      protected override void Because()
      {
         sut.RemoveUnusedColumnsAndAdd(_dataRepository);
      }

      [Observation]
      public void should_remove_the_column_that_were_removed()
      {
         A.CallTo(() => _dataBrowserPresenter.RemoveDataColumn(_columnThatWillBeRemoved)).MustHaveHappened();
      }

      [Observation]
      public void should_remove_the_column_that_became_internal()
      {
         A.CallTo(() => _dataBrowserPresenter.RemoveDataColumn(_columnThatWillBeInternal)).MustHaveHappened();
      }

   

      [Observation]
      public void should_add_the_new_columns()
      {
         A.CallTo(() => _dataBrowserPresenter.AddDataColumn(_newColumn)).MustHaveHappened();
      }
   }
}