using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using FakeItEasy;
using OSPSuite.Core.Commands;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Core.Events;
using OSPSuite.Helpers;

namespace OSPSuite.Core
{
   public abstract class concern_for_SetObservedDataColumnUnitCommand : ContextSpecification<SetObservedDataColumnUnitCommand>
   {
      private DataRepository _observedData;
      private BaseGrid _baseGrid;
      protected DataColumn _conc;
      private Unit _cmUnit;
      protected Unit _mmUnit;

      protected override void Context()
      {
         _observedData = new DataRepository();
         _baseGrid = new BaseGrid("base", "base", DomainHelperForSpecs.TimeDimensionForSpecs());
         _baseGrid.Values = new[] { 1f, 2f, 3f };
         var lengthDimention = DomainHelperForSpecs.LengthDimensionForSpecs();
         _conc = new DataColumn("conc", "conc", lengthDimention, _baseGrid) {Values = new[] {0.01f, 0.02f, 0.03f}};
         _cmUnit = lengthDimention.Unit("cm");
         _conc.DisplayUnit = _cmUnit;
         _mmUnit = lengthDimention.Unit("mm");
         _observedData.Add(_conc);
         sut = new SetObservedDataColumnUnitCommand(_observedData, _conc.Id, _mmUnit);
      }
   }


   public class When_executing_the_set_observed_data_unit_command : concern_for_SetObservedDataColumnUnitCommand
   {
      private IOSPSuiteExecutionContext _context;

      protected override void Context()
      {
         base.Context();
         _context = A.Fake<IOSPSuiteExecutionContext>();
      }

      protected override void Because()
      {
         sut.Execute(_context);
      }

      [Observation]
      public void should_have_change_the_display_unit_in_the_column()
      {
         _conc.DisplayUnit.ShouldBeEqualTo(_mmUnit);
      }

      [Observation]
      public void should_have_updated_the_values_in_the_columns()
      {
         _conc.Values.ShouldOnlyContainInOrder(0.001f, 0.002f, 0.003f);
      }

      [Observation]
      public void should_notify_the_table_changed_event()
      {
         A.CallTo(() => _context.PublishEvent(A<ObservedDataTableChangedEvent>._)).MustHaveHappened();
      }
   }
}