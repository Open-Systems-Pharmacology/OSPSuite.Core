using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using FakeItEasy;
using OSPSuite.Assets;
using OSPSuite.Core.Commands;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Events;

namespace OSPSuite.Core
{
   public abstract class concern_for_SetObservedDataValueCommand : ContextSpecification<SetObservedDataValueCommand>
   {
      protected CellValueChanged _cellValueChanged;
      private DataRepository _observedData;
      protected IOSPSuiteExecutionContext _context;
      protected DataColumn _column;
      protected BaseGrid _baseGrid;

      protected override void Context()
      {
         _context = A.Fake<IOSPSuiteExecutionContext>();
         _observedData = new DataRepository("data");
         _baseGrid = new BaseGrid("base", "Time", Constants.Dimension.NO_DIMENSION);
         _baseGrid.Values = new[] {1f, 2f, 3f};
         _column = new DataColumn("col", "Name", Constants.Dimension.NO_DIMENSION, _baseGrid);
         _column.Values = new[] {10f, 20f, 30f};
         _observedData.Add(_column);
         sut = new SetObservedDataValueCommand(_observedData, _cellValueChanged);
         A.CallTo(() => _context.TypeFor(_observedData)).Returns("TYPE");
      }
   }

   public class When_exectuting_the_set_observed_data_value_command_to_set_the_value_in_a_cell_for_a_column_that_is_not_a_base_grid : concern_for_SetObservedDataValueCommand
   {
      protected override void Context()
      {
         _cellValueChanged = new CellValueChanged();
         _cellValueChanged.ColumnId = "col";
         _cellValueChanged.RowIndex = 1;
         _cellValueChanged.OldValue = 20;
         _cellValueChanged.NewValue = 200;
         base.Context();
      }

      protected override void Because()
      {
         sut.Execute(_context);
      }

      [Observation]
      public void should_have_set_the_value_in_the_observed_data()
      {
         _column.Values.ShouldOnlyContainInOrder(10f, 200f, 30f);
      }

      [Observation]
      public void should_have_notified_an_observed_data_changed_event()
      {
         A.CallTo(() => _context.PublishEvent(A<ObservedDataValueChangedEvent>._)).MustHaveHappened();
      }

      [Observation]
      public void should_have_set_the_expected_description()
      {
         sut.BuildingBlockType.ShouldBeEqualTo("TYPE");
         sut.CommandType.ShouldBeEqualTo(Command.CommandTypeEdit);
      }
   }

   public class When_changing_the_time_value_to_a_value_that_did_not_exist_before_and_is_at_the_right_index : concern_for_SetObservedDataValueCommand
   {
      protected override void Context()
      {
         _cellValueChanged = new CellValueChanged();
         _cellValueChanged.ColumnId = "base";
         _cellValueChanged.RowIndex = 1;
         _cellValueChanged.OldValue = 2;
         _cellValueChanged.NewValue = 2.5f;
         base.Context();
      }

      protected override void Because()
      {
         sut.Execute(_context);
      }

      [Observation]
      public void should_replace_the_original_value_with_the_new_value()
      {
         _column.Values.ShouldOnlyContainInOrder(10f, 20f, 30f);
         _baseGrid.Values.ShouldOnlyContainInOrder(1f, 2.5f, 3f);
      }
   }

   public class When_changing_the_time_value_to_a_value_that_did_not_exist_before_but_required_a_shift_in_position_to_preserve_monotonie : concern_for_SetObservedDataValueCommand
   {
      protected override void Context()
      {
         _cellValueChanged = new CellValueChanged();
         _cellValueChanged.ColumnId = "base";
         _cellValueChanged.RowIndex = 1;
         _cellValueChanged.OldValue = 2;
         _cellValueChanged.NewValue = 4f;
         base.Context();
      }

      protected override void Because()
      {
         sut.Execute(_context);
      }

      [Observation]
      public void should_have_added_the_value_at_the_right_index_and_moved_the_column_values_accordingly()
      {
         _column.Values.ShouldOnlyContainInOrder(10f, 30f, 20f);
         _baseGrid.Values.ShouldOnlyContainInOrder(1f, 3f, 4f);
      }
   }
}