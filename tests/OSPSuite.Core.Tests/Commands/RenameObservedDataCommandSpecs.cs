using System.Linq;
using FakeItEasy;
using OSPSuite.Assets;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Commands;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Utility.Extensions;

namespace OSPSuite.Commands
{
   public abstract class concern_for_RenameObservedDataCommand : ContextSpecification<RenameObservedDataCommand>
   {
      protected DataRepository _dataRepository;
      protected string _newName;
      protected string _oldName;
      private IDimension _timeDimension;
      private IDimension _concDimension;
      private BaseGrid _baseGrid;
      protected DataColumn _column;
      protected IOSPSuiteExecutionContext _executionContext;

      protected override void Context()
      {
         _oldName = "oldName";
         _newName = "newName";
         _timeDimension = new Dimension(new BaseDimensionRepresentation(), "Time", "min");
         _concDimension = new Dimension(new BaseDimensionRepresentation(), "Conc", "mg/l");
         _dataRepository = new DataRepository {Name = _oldName};
         sut = new RenameObservedDataCommand(_dataRepository, _newName);
         _baseGrid = new BaseGrid("Time", _timeDimension);
         _column = new DataColumn("Col", _concDimension, _baseGrid);
         var quantityInfo = new QuantityInfo(_column.Name, new[] {_oldName, ObjectTypes.ObservedData, "Organ", "Compartment", "Drug", _column.Name}, QuantityType.Undefined);
         _baseGrid.QuantityInfo = new QuantityInfo("time", new[] {_oldName, ObjectTypes.ObservedData}, QuantityType.BaseGrid);
         _column.QuantityInfo = quantityInfo;
         _dataRepository.Add(_baseGrid);
         _dataRepository.Add(_column);
         _executionContext = A.Fake<IOSPSuiteExecutionContext>();

         A.CallTo(() => _executionContext.Project.ObservedDataBy(_dataRepository.Id)).Returns(_dataRepository);
      }
   }

   public class When_reverting_an_observed_data_name_change : concern_for_RenameObservedDataCommand
   {
      protected override void Because()
      {
         sut.ExecuteAndInvokeInverse(_executionContext);
      }

      [Observation]
      public void the_name_change_should_be_reversed_on_the_repository()
      {
         _dataRepository.Name.ShouldBeEqualTo(_oldName);
      }

      [Observation]
      public void should_also_revert_the_name_of_the_repository_in_the_columns()
      {
         _dataRepository.Each(column => column.QuantityInfo.Path.First().ShouldBeEqualTo(_oldName));
      }
   }

   public class When_renaming_an_observed_data : concern_for_RenameObservedDataCommand
   {
      protected override void Because()
      {
         sut.Execute(_executionContext);
      }

      [Observation]
      public void should_have_renamed_the_repository()
      {
         _dataRepository.Name.ShouldBeEqualTo(_newName);
      }

      [Observation]
      public void should_also_rename_the_name_of_the_repository_in_the_columns()
      {
         _dataRepository.Each(column => column.QuantityInfo.Path.First().ShouldBeEqualTo(_newName));
      }
   }
}