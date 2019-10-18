using FakeItEasy;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Helpers;

namespace OSPSuite.Core.Commands
{
   public abstract class concern_for_UpdateObservedDataMolWeightCommand : ContextSpecification<UpdateObservedDataMolWeightCommand>
   {
      protected double _newValue;
      protected DataRepository _observedData;
      private IDimension _molWeightDimension;
      protected double _oldValue;
      private BaseGrid _baseGrid;
      protected DataColumn _column;
      protected string _id;
      protected IOSPSuiteExecutionContext _executionContext;

      protected override void Context()
      {
         _baseGrid = new BaseGrid("base", "Time", DomainHelperForSpecs.TimeDimensionForSpecs());
         _column = new DataColumn("col", "name", DomainHelperForSpecs.ConcentrationDimensionForSpecs(), _baseGrid);
         _molWeightDimension = DomainHelperForSpecs.LengthDimensionForSpecs();
         _observedData = new DataRepository(_id) {_column};
         _newValue = 100;
         _oldValue = 50;
         sut = new UpdateObservedDataMolWeightCommand(_observedData, _molWeightDimension, _oldValue, _newValue);

         _executionContext = A.Fake<IOSPSuiteExecutionContext>();
      }
   }

   public class When_executing_the_update_observed_data_mol_weight_command : concern_for_UpdateObservedDataMolWeightCommand
   {
      protected override void Because()
      {
         sut.Execute(_executionContext);
      }

      [Observation]
      public void should_update_the_mol_weight_in_all_columnds_defined_in_the_data_repository()
      {
         _column.DataInfo.MolWeight.ShouldBeEqualTo(_newValue);
      }
   }

   public class When_reverting_the_execution_of_the_update_observed_data_mol_weight_command : concern_for_UpdateObservedDataMolWeightCommand
   {
      protected override void Context()
      {
         base.Context();
         A.CallTo(() => _executionContext.Get<DataRepository>(_id)).Returns(_observedData);
      }
      protected override void Because()
      {
         sut.ExecuteAndInvokeInverse(_executionContext);
      }

      [Observation]
      public void should_update_the_mol_weight_in_all_columnds_defined_in_the_data_repository()
      {
         _column.DataInfo.MolWeight.ShouldBeEqualTo(_oldValue);
      }
   }
}