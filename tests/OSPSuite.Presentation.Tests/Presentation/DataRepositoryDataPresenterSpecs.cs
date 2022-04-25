using System;
using System.Data;
using FakeItEasy;
using OSPSuite.Assets;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Commands.Core;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Core.Events;
using OSPSuite.Presentation.Presenters.ObservedData;
using OSPSuite.Presentation.Views.ObservedData;
using OSPSuite.Utility.Extensions;
using DataColumn = OSPSuite.Core.Domain.Data.DataColumn;

namespace OSPSuite.Presentation.Presentation
{
   public abstract class concern_for_DataRepositoryDataPresenter : ContextSpecification<DataRepositoryDataPresenter>
   {
      protected IDataRepositoryDataView _view;
      private IDataRepositoryExportTask _dataRepositoryTask;
      protected DataRepository _dataRepository;
      protected DataTable _dataTable;
      protected ICommandCollector _commandCollector;
      protected DataColumn _col;
      protected IDimension _dim;
      protected BaseGrid _baseGrid;

      protected override void Context()
      {
         _view = A.Fake<IDataRepositoryDataView>();
         _dataRepositoryTask = A.Fake<IDataRepositoryExportTask>();
         _commandCollector = A.Fake<ICommandCollector>();
         sut = new DataRepositoryDataPresenter(_view, _dataRepositoryTask);
         sut.InitializeWith(_commandCollector);

         //common setup
         _dataRepository = new DataRepository();
         _dataTable = new DataTable();
         A.CallTo(() => _dataRepositoryTask.ToDataTable(_dataRepository, A<DataColumnExportOptions>._)).Returns(new[] {_dataTable});

         var col = _dataTable.AddColumn<float>("test");
         col.ExtendedProperties.Add(Constants.DATA_REPOSITORY_COLUMN_ID, "col");
         _baseGrid = new BaseGrid("base", "base", Constants.Dimension.NO_DIMENSION) {Values = new ArraySegment<float>()};
         _dim = A.Fake<IDimension>();
         _col = new DataColumn("col", "col", _dim, _baseGrid) {Values = new ArraySegment<float>()};
         _dataRepository.Add(_baseGrid);
         _dataRepository.Add(_col);
      }
   }

   public class When_finding_values_in_repository_that_are_below_lloq_and_the_repository_doesnt_have_one : concern_for_DataRepositoryDataPresenter
   {
      protected override void Context()
      {
         base.Context();
         _col.DataInfo.LLOQ = 10;
         _col.InsertValueAt(0, 10.0f);
         _baseGrid.InsertValueAt(0, 0);
         _col.DataInfo.Origin = ColumnOrigins.Observation;
         sut.EditObservedData(_dataRepository);
      }

      [Observation]
      public void the_presenter_should_find_that_a_value_in_the_row_is_below()
      {
         sut.AnyObservationInThisRowIsBelowLLOQ(0).ShouldBeFalse();
      }

      [Observation]
      public void the_color_determined_by_the_presenter_should_be_default()
      {
         sut.BackgroundColorForRow(0).ShouldBeEqualTo(Colors.DefaultRowColor);
      }

      [Observation]
      public void tooltip_for_this_row_should_be_empty()
      {
         sut.ToolTipTextForRow(0).ShouldBeEqualTo(string.Empty);
      }
   }

   public class When_finding_values_in_repository_that_are_below_lloq : concern_for_DataRepositoryDataPresenter
   {
      protected override void Context()
      {
         base.Context();
         _col.DataInfo.LLOQ = 10;
         _col.InsertValueAt(0, 9.0f);
         _baseGrid.InsertValueAt(0, 0);
         _col.DataInfo.Origin = ColumnOrigins.Observation;
         sut.EditObservedData(_dataRepository);
      }

      [Observation]
      public void the_presenter_should_find_that_a_value_in_the_row_is_below()
      {
         sut.AnyObservationInThisRowIsBelowLLOQ(0).ShouldBeTrue();
      }

      [Observation]
      public void the_color_determined_by_the_presenter_should_not_be_default()
      {
         sut.BackgroundColorForRow(0).ShouldBeEqualTo(Colors.BelowLLOQ);
      }

      [Observation]
      public void tooltip_for_this_row_should_not_be_empty()
      {
         sut.ToolTipTextForRow(0).ShouldNotBeEmpty();
      }
   }

   public class When_editing_a_data_repository : concern_for_DataRepositoryDataPresenter
   {
      protected override void Because()
      {
         sut.EditObservedData(_dataRepository);
      }

      [Observation]
      public void should_create_a_data_table_containing_the_values_to_be_displayed_and_set_in_in_the_view()
      {
         A.CallTo(() => _view.BindTo(_dataTable)).MustHaveHappened();
      }
   }

   public class When_notified_that_a_value_was_changed_and_the_event_was_not_raised_from_the_presenter_itself : concern_for_DataRepositoryDataPresenter
   {
      protected override void Context()
      {
         base.Context();
         sut.EditObservedData(_dataRepository);
         sut.IsLatched = false;
      }

      protected override void Because()
      {
         sut.Handle(new ObservedDataValueChangedEvent(_dataRepository));
      }

      [Observation]
      public void should_rebind_the_values()
      {
         //once for the edit and one for the value changed
         A.CallTo(() => _view.BindTo(A<DataTable>._)).MustHaveHappenedTwiceExactly();
      }
   }
}