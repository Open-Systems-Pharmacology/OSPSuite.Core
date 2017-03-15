using OSPSuite.BDDHelper;
using FakeItEasy;
using OSPSuite.Core;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Helpers;
using OSPSuite.Presentation.Presenters.Charts;
using OSPSuite.Presentation.Views.Charts;

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

      protected override void Context()
      {
         _view = A.Fake<IChartEditorView>();
         _axisSettingsPresenter = A.Fake<IAxisSettingsPresenter>();
         _chartSettingsPresenter = A.Fake<IChartSettingsPresenter>();
         _chartExportSettingsPresneter = A.Fake<IChartExportSettingsPresenter>();
         _curveSettingsPresenter = A.Fake<ICurveSettingsPresenter>();
         _dataBrowserPresenter = A.Fake<IDataBrowserPresenter>();
         _chartTemplateMenuPresenter = A.Fake<IChartTemplateMenuPresenter>();
         sut = new ChartEditorPresenter(_view, _axisSettingsPresenter, _chartSettingsPresenter, _chartExportSettingsPresneter, _curveSettingsPresenter, _dataBrowserPresenter, _chartTemplateMenuPresenter);
      }
   }

   public class When_adding_a_repository_to_the_chart_editor_presenter : concern_for_ChartEditorPresenter
   {
      private DataRepository _dataRepository;
      private DataColumn _hiddenColumn;
      private DataColumn _internalColumn;
      private DataColumn _auxiliaryObservedDataColumn;
      private DataColumn _standardColumn;

      protected override void Context()
      {
         base.Context();
         var baseGrid = new BaseGrid("Time", DomainHelperForSpecs.TimeDimensionForSpecs());
         _hiddenColumn = new DataColumn("hidden", DomainHelperForSpecs.ConcentrationDimensionForSpecs(), baseGrid)
         {
            DataInfo = new DataInfo(ColumnOrigins.Calculation)
         };

         _internalColumn = new DataColumn("Internal", DomainHelperForSpecs.ConcentrationDimensionForSpecs(), baseGrid)
         {
            DataInfo = new DataInfo(ColumnOrigins.Calculation),
            IsInternal = true
         };

         _auxiliaryObservedDataColumn = new DataColumn("Auxiliary", DomainHelperForSpecs.ConcentrationDimensionForSpecs(), baseGrid)
         {
            DataInfo = new DataInfo(ColumnOrigins.ObservationAuxiliary),
         };

         _standardColumn = new DataColumn("Standard", DomainHelperForSpecs.ConcentrationDimensionForSpecs(), baseGrid)
         {
            DataInfo = new DataInfo(ColumnOrigins.Calculation),
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

   public class When_refreshing_a_data_repository_used_in_the_chart_editor_presenter : concern_for_ChartEditorPresenter
   {
      private DataRepository _dataRepository;
      private DataColumn _columnThatWillBeInternal;
      private DataColumn _standardColumn;
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
         sut.RefreshDataRepository(_dataRepository);
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
      public void should_udpate_the_column_that_were_already_existing()
      {
         A.CallTo(() => _dataBrowserPresenter.UpdateDataColumn(_standardColumn)).MustHaveHappened();
      }

      [Observation]
      public void should_add_the_new_columns()
      {
         A.CallTo(() => _dataBrowserPresenter.AddDataColumn(_newColumn)).MustHaveHappened();
      }
   }
}