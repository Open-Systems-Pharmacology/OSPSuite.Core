using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using FakeItEasy;
using OSPSuite.Assets;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Commands;
using OSPSuite.Core.Commands.Core;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Core.Events;
using OSPSuite.Presentation.DTO;
using OSPSuite.Presentation.Presenters.ObservedData;
using OSPSuite.Presentation.Views.ObservedData;
using OSPSuite.Utility.Extensions;
using DataColumn = OSPSuite.Core.Domain.Data.DataColumn;

namespace OSPSuite.Presentation.Presentation
{
   public abstract class concern_for_DataRepositoryDataPresenter : ContextSpecification<DataRepositoryDataPresenter>
   {
      protected IEditObservedDataTask _editObservedDataTask;
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
         _editObservedDataTask = A.Fake<IEditObservedDataTask>();
         _view = A.Fake<IDataRepositoryDataView>();
         _dataRepositoryTask = A.Fake<IDataRepositoryExportTask>();
         _commandCollector = A.Fake<ICommandCollector>();
         sut = new DataRepositoryDataPresenter(_view, _dataRepositoryTask, _editObservedDataTask);
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

   public class When_getting_validation_messages_for_unchanging_basegrid_values : concern_for_DataRepositoryDataPresenter
   {
      private List<string> _result;

      protected override void Context()
      {
         base.Context();

         var col = _dataTable.AddColumn<float>("base");
         col.ExtendedProperties.Add(Constants.DATA_REPOSITORY_COLUMN_ID, "base");
         _dataRepository.Columns.Each(column => column.InsertValueAt(0, 0.0f));
      }

      protected override void Because()
      {
         sut.EditObservedData(_dataRepository);
         _result = sut.GetCellValidationErrorMessages(0, 1, "0").ToList();
      }

      [Observation]
      public void should_not_return_validation_messages()
      {
         _result.Count.ShouldBeEqualTo(0);
      }
   }

   public class When_getting_validation_messages_for_an_empty_value : concern_for_DataRepositoryDataPresenter
   {
      private List<string> _result;

      protected override void Because()
      {
         sut.EditObservedData(_dataRepository);
         _result = sut.GetCellValidationErrorMessages(0, 1, "    ").ToList();
      }

      [Observation]
      public void should_return_validation_messages()
      {
         _result.Count.ShouldBeEqualTo(1);
         _result[0].ShouldBeEqualTo(Error.ValueIsRequired);
      }
   }

   public class When_getting_validation_messages_for_repeated_basegrid_values : concern_for_DataRepositoryDataPresenter
   {
      private List<string> _result;

      protected override void Context()
      {
         base.Context();
         var col = _dataTable.AddColumn<float>("base");
         col.ExtendedProperties.Add(Constants.DATA_REPOSITORY_COLUMN_ID, "base");
         _dataRepository.Columns.Each(column => column.InsertValueAt(0, 0.0f));
      }

      protected override void Because()
      {
         sut.EditObservedData(_dataRepository);
         _result = sut.GetCellValidationErrorMessages(1, 1, "0").ToList();
      }

      [Observation]
      public void validation_messages_should_contain_a_message()
      {
         _result.Count.ShouldBeGreaterThan(0);
      }

      [Observation]
      public void validation_message_should_be_correct()
      {
         _result.FirstOrDefault().ShouldBeEqualTo(Error.ExistingValueInDataRepository("base", 0f, _dataRepository.BaseGrid.DisplayUnit.ToString()));
      }
   }

   public class When_adding_row_to_data_repository : concern_for_DataRepositoryDataPresenter
   {
      protected override void Context()
      {
         base.Context();
         _dataTable.AddColumn<float>("base");
         _dataTable.Columns[1].ExtendedProperties.Add(Constants.DATA_REPOSITORY_COLUMN_ID, _baseGrid.Id);
         sut.EditObservedData(_dataRepository);
         sut.AddRow();
      }

      protected override void Because()
      {
         sut.AddData(0);
      }

      [Observation]
      public void a_call_to_add_data_task_must_occur()
      {
         A.CallTo(() => _editObservedDataTask.AddValue(_dataRepository, A<DataRowData>.Ignored)).MustHaveHappened();
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

   public class When_notify_that_a_value_was_edited_by_the_user : concern_for_DataRepositoryDataPresenter
   {
      private CellValueChangedDTO _dto;
      private CellValueChanged _cellValueChanged;
      private ICommand _command;

      protected override void Context()
      {
         base.Context();
         sut.EditObservedData(_dataRepository);
         _dto = new CellValueChangedDTO {ColumnIndex = 0, OldDisplayValue = 1, NewDisplayValue = 2, RowIndex = 3};
         _command = A.Fake<ICommand>();

         A.CallTo(() => _dim.UnitValueToBaseUnitValue(A<Unit>._, 1)).Returns(10);
         A.CallTo(() => _dim.UnitValueToBaseUnitValue(A<Unit>._, 2)).Returns(20);
         A.CallTo(() => _editObservedDataTask.SetValue(A<DataRepository>._, A<CellValueChanged>._))
            .Invokes(x => _cellValueChanged = x.GetArgument<CellValueChanged>(1)).Returns(_command);
      }

      protected override void Because()
      {
         sut.ValueIsSet(_dto);
      }

      [Observation]
      public void should_have_created_a_command_that_will_update_the_value()
      {
         _cellValueChanged.RowIndex.ShouldBeEqualTo(_dto.RowIndex);
         _cellValueChanged.ColumnId.ShouldBeEqualTo("col");
         _cellValueChanged.NewValue.ShouldBeEqualTo(20);
         _cellValueChanged.OldValue.ShouldBeEqualTo(10);
      }

      [Observation]
      public void should_not_rebind_the_data_to_the_view()
      {
         A.CallTo(() => _view.BindTo(_dataTable)).MustHaveHappenedOnceExactly();
      }

      [Observation]
      public void should_have_added_the_command_to_the_command_collector()
      {
         A.CallTo(() => _commandCollector.AddCommand(_command)).MustHaveHappened();
      }
   }

   public class When_handling_event_for_removed_data : concern_for_DataRepositoryDataPresenter
   {
      protected override void Because()
      {
         sut.EditObservedData(_dataRepository);
         sut.Handle(new ObservedDataTableChangedEvent(_dataRepository));
      }

      [Observation]
      public void view_must_have_been_bound_to_table()
      {
         A.CallTo(() => _view.BindTo(A<DataTable>.Ignored)).MustHaveHappenedTwiceExactly();
      }
   }

   public class When_removing_value_not_in_repository : concern_for_DataRepositoryDataPresenter
   {
      protected override void Context()
      {
         base.Context();
         _dataRepository.Columns.Each(column => column.InsertValueAt(0, 0.1f));
         _dataRepository.Columns.Each(column => column.InsertValueAt(1, 0.2f));
         _dataTable.Rows.Add(_dataTable.NewRow());
         _dataTable.Rows.Add(_dataTable.NewRow());
         _dataTable.Rows.Add(_dataTable.NewRow());
         sut.EditObservedData(_dataRepository);
      }

      protected override void Because()
      {
         sut.RemoveData(2);
      }

      [Observation]
      public void must_remove_row_from_table_instead()
      {
         _dataTable.Rows.Count.ShouldBeEqualTo(2);
      }
   }

   public class When_removing_value_from_repository : concern_for_DataRepositoryDataPresenter
   {
      protected override void Context()
      {
         base.Context();
         _dataRepository.Columns.Each(column => column.InsertValueAt(0, 0.1f));
         _dataRepository.Columns.Each(column => column.InsertValueAt(1, 0.2f));
         _dataTable.Rows.Add(_dataTable.NewRow());
         _dataTable.Rows.Add(_dataTable.NewRow());

         sut.EditObservedData(_dataRepository);
      }

      protected override void Because()
      {
         sut.RemoveData(1);
      }

      [Observation]
      public void command_must_have_been_called_to_remove_data()
      {
         A.CallTo(() => _editObservedDataTask.RemoveValue(A<DataRepository>._, 1)).MustHaveHappened();
      }
   }

   public class When_notified_that_a_value_was_changed_and_the_event_was_raised_from_the_presenter_itself : concern_for_DataRepositoryDataPresenter
   {
      protected override void Context()
      {
         base.Context();
         sut.EditObservedData(_dataRepository);
         sut.IsLatched = true;
      }

      protected override void Because()
      {
         sut.Handle(new ObservedDataValueChangedEvent(_dataRepository));
      }

      [Observation]
      public void should_not_rebind_the_values()
      {
         //once for the edit
         A.CallTo(() => _view.BindTo(A<DataTable>._)).MustHaveHappenedOnceExactly();
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

   public class When_notified_that_a_table_was_changed_and_the_event_was_raised_from_the_presenter_itself : concern_for_DataRepositoryDataPresenter
   {
      protected override void Context()
      {
         base.Context();
         sut.EditObservedData(_dataRepository);
         sut.IsLatched = true;
      }

      protected override void Because()
      {
         sut.Handle(new ObservedDataTableChangedEvent(_dataRepository));
      }

      [Observation]
      public void should_rebind_the_values_nonetheless()
      {
         //once for the edit
         A.CallTo(() => _view.BindTo(A<DataTable>._)).MustHaveHappenedTwiceExactly();
      }
   }

   public class When_the_presenter_is_notified_that_the_unit_for_a_given_column_was_changed : concern_for_DataRepositoryDataPresenter
   {
      private ICommand _command;
      private Unit _newUnit;

      protected override void Context()
      {
         base.Context();
         _command = A.Fake<ICommand>();
         _newUnit = A.Fake<Unit>();
         A.CallTo(() => _editObservedDataTask.SetUnit(_dataRepository, _col.Id, _newUnit)).Returns(_command);
         sut.EditObservedData(_dataRepository);
      }

      protected override void Because()
      {
         sut.ChangeUnit(0, _newUnit);
      }

      [Observation]
      public void should_leverage_the_observed_data_task_to_create_a_command_changing_the_unit()
      {
         A.CallTo(() => _editObservedDataTask.SetUnit(_dataRepository, _col.Id, _newUnit)).MustHaveHappened();
      }

      [Observation]
      public void should_have_added_the_command_to_the_command_collector()
      {
         A.CallTo(() => _commandCollector.AddCommand(_command)).MustHaveHappened();
      }
   }

   public class When_retrieving_all_the_units_available_for_a_predefined_column : concern_for_DataRepositoryDataPresenter
   {
      private Unit _unit1;
      private Unit _unit2;

      protected override void Context()
      {
         base.Context();
         _unit1 = A.Fake<Unit>();
         _unit2 = A.Fake<Unit>();
         A.CallTo(() => _dim.Units).Returns(new[] {_unit1, _unit2});
         sut.EditObservedData(_dataRepository);
      }

      [Observation]
      public void should_return_the_list_of_all_available_units_for_the_underlying_column()
      {
         sut.AvailableUnitsFor(0).ShouldOnlyContain(_unit1, _unit2);
      }
   }

   public class When_retrieving_all_the_units_available_for_an_unbound_column_ : concern_for_DataRepositoryDataPresenter
   {
      protected override void Context()
      {
         base.Context();
         _dataTable.AddColumn<float>("unbound");
         sut.EditObservedData(_dataRepository);
      }

      [Observation]
      public void should_return_an_empty_list()
      {
         sut.AvailableUnitsFor(1).ShouldBeEmpty();
      }
   }
}