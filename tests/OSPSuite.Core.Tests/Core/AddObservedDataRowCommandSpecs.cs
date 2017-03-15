using System;
using System.Linq;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Utility.Collections;
using OSPSuite.Utility.Extensions;
using FakeItEasy;
using OSPSuite.Core.Commands;
using OSPSuite.Core.Commands.Core;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Core.Events;

namespace OSPSuite.Core
{
   public abstract class concern_for_AddObservedDataRowCommand : ContextSpecification<AddObservedDataRowCommand>
   {
      protected DataRepository _observedData;
      protected DataRowData _dataRowData;
      protected BaseGrid _baseGrid;
      protected IOSPSuiteExecutionContext _executionContext;

      protected override void Context()
      {
         _executionContext = A.Fake<IOSPSuiteExecutionContext>();
         _observedData = new DataRepository();
         _baseGrid = new BaseGrid("nameBaseGrid", new Dimension(new BaseDimensionRepresentation(), "Time", "min"));
         var column = new DataColumn("columnId", "name1", new Dimension(new BaseDimensionRepresentation(), "Conc", "mg/l"), _baseGrid) {Values = new ArraySegment<float>()};
         _baseGrid.Values = new ArraySegment<float>();
         _observedData.Add(column);
         _observedData.Add(_baseGrid);

         _observedData.InsertValues(1, new Cache<string, float> {{"columnId", 1f}});
         _observedData.InsertValues(2, new Cache<string, float> {{"columnId", 2f}});

         _dataRowData = new DataRowData();

         sut = new AddObservedDataRowCommand(_observedData, _dataRowData);
      }
   }

   public class When_retrieving_inverse_command_for_add : concern_for_AddObservedDataRowCommand
   {
      private IReversibleCommand<IOSPSuiteExecutionContext> _result;

      [Observation]
      public void must_be_of_correct_type()
      {
         _result.ShouldBeAnInstanceOf<RemoveObservedDataRowCommand>();
      }

      protected override void Because()
      {
         _result = sut.InverseCommand(_executionContext);
      }
   }

   public class When_adding_new_data_to_observed_data : concern_for_AddObservedDataRowCommand
   {
      protected override void Context()
      {
         base.Context();
         _dataRowData.Data.Add("columnId", _observedData.ConvertBaseValueForColumn("columnId", 1.2f));
         _dataRowData.BaseGridValue = _observedData.ConvertBaseValueForColumn(_baseGrid.Id, 1.2f);
      }

      protected override void Because()
      {
         sut.Execute(_executionContext);
      }

      [Observation]
      public void should_have_new_data_inserted_to_correct_index()
      {
         _observedData.BaseGrid.IndexOf(1.2f).ShouldBeEqualTo(1);
         _observedData.Columns.First(x => x.Id.Equals("columnId")).Values[1].ShouldBeEqualTo(1.2f);
      }

      [Observation]
      public void must_have_new_data_inserted()
      {
         _observedData.Columns.Each(x => x.Values.Count.ShouldBeEqualTo(3));
      }

      [Observation]
      public void A_call_to_public_event_should_have_happened()
      {
         A.CallTo(() => _executionContext.PublishEvent(A<ObservedDataTableChangedEvent>.That.Matches(g => g.ObservedData.Equals(_observedData)))).MustHaveHappened();
      }
   }
}