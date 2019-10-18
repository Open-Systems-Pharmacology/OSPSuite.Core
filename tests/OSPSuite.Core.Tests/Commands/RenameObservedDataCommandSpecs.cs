using FakeItEasy;
using OSPSuite.Assets;
using OSPSuite.BDDHelper;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Core.Services;

namespace OSPSuite.Core.Commands
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
      protected IDataRepositoryNamer _dataRepositoryNamer;

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
         _dataRepositoryNamer = A.Fake<IDataRepositoryNamer>();
         A.CallTo(() => _executionContext.Resolve<IDataRepositoryNamer>()).Returns(_dataRepositoryNamer);
      }
   }

   public class When_renaming_observed_data : concern_for_RenameObservedDataCommand
   {
      protected override void Because()
      {
         sut.Execute(_executionContext);
      }

      [Observation]
      public void the_data_repository_renamer_is_used_to_rename()
      {
         A.CallTo(() => _dataRepositoryNamer.Rename(_dataRepository, _newName)).MustHaveHappened();
      }
   }

   public class When_reverting_an_observed_data_name_change : concern_for_RenameObservedDataCommand
   {
      protected override void Because()
      {
         sut.ExecuteAndInvokeInverse(_executionContext);
      }

      [Observation]
      public void the_data_repository_renamer_must_be_used_twice()
      {
         A.CallTo(() => _dataRepositoryNamer.Rename(_dataRepository, _newName)).MustHaveHappened();
         A.CallTo(() => _dataRepositoryNamer.Rename(_dataRepository, _oldName)).MustHaveHappened();
      }
   }
}