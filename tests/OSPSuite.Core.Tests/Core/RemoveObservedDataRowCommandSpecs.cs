using System;
using System.Linq;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using FakeItEasy;
using OSPSuite.Core.Commands;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Events;

namespace OSPSuite.Core
{
   public abstract class concern_for_RemoveObservedDataValueCommand : ContextSpecification<RemoveObservedDataRowCommand>
   {
      protected string _dataRepositoryId = "repositoryId";
      protected DataRepository _dataRepository;
      protected int _rowIndex;
      protected IOSPSuiteExecutionContext _executionContext = A.Fake<IOSPSuiteExecutionContext>();
      private BaseGrid _baseGrid;
      protected DataColumn _column;

      protected override void Context()
      {
         base.Context();
         _dataRepository = new DataRepository { Id = _dataRepositoryId };
         ConfigureColumns();
         sut = new RemoveObservedDataRowCommand(_dataRepository, _rowIndex);
      }

      protected void ConfigureColumns()
      {
         _baseGrid = new BaseGrid("base", "Time", Constants.Dimension.NO_DIMENSION) { Values = new[] { 1f, 2f, 3f } };
         _column = new DataColumn("col", "name", Constants.Dimension.NO_DIMENSION, _baseGrid) { Values = new[] { 10f, 20f, 30f } };
         _dataRepository.Add(_column);
      }

      protected int CountValuesWithData(float data)
      {
         // ReSharper disable once PossibleNullReferenceException - I'll take the exception in this case for unit testing
         return _dataRepository.Columns.FirstOrDefault(dataColumn => dataColumn.Id.Equals("col")).Values.Count(value => Math.Abs(value - data) < float.Epsilon);
      }
   }

   public class When_removing_row_index_thats_too_large : concern_for_RemoveObservedDataValueCommand
   {
      protected override void Context()
      {
         _rowIndex = 7;
         base.Context();
      }

      [Observation]
      public void throw_argument_out_of_range_exception()
      {
         The.Action(() => sut.Execute(_executionContext)).ShouldThrowAn<ArgumentOutOfRangeException>();
      }
   }

   public class When_calling_to_restore_execution_context : concern_for_RemoveObservedDataValueCommand
   {
      [Observation]
      public void context_must_be_called_to_get_repository()
      {
         A.CallTo(() => _executionContext.Get<DataRepository>(_dataRepositoryId)).MustHaveHappened();
      }

      protected override void Because()
      {
         sut.RestoreExecutionData(_executionContext);
      }
   }

   public class When_removing_row_from_data_repository : concern_for_RemoveObservedDataValueCommand
   {
      protected override void Context()
      {
         _rowIndex = 1;
         base.Context();
      }

      [Observation]
      public void row_must_be_removed()
      {
         _dataRepository.BaseGrid.Count.ShouldBeEqualTo(2);
         CountValuesWithData(20).ShouldBeEqualTo(0);
      }

      [Observation]
      public void other_rows_untouched()
      {
         CountValuesWithData(30).ShouldBeEqualTo(1);
         CountValuesWithData(10).ShouldBeEqualTo(1);
      }

      [Observation]
      public void event_is_raised()
      {
         A.CallTo(() => _executionContext.PublishEvent(A<ObservedDataTableChangedEvent>.Ignored)).MustHaveHappened();
      }

      protected override void Because()
      {
         sut.Execute(_executionContext);
      }
   }
}
