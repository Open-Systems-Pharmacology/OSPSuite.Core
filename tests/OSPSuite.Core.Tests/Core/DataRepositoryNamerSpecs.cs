using System.Linq;
using OSPSuite.Assets;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Core.Services;
using OSPSuite.Utility.Extensions;

namespace OSPSuite.Core
{
   public abstract class concern_for_DataRepositoryNamer : ContextSpecification<DataRepositoryNamer>
   {
      protected override void Context()
      {
         sut = new DataRepositoryNamer();
      }
   }

   public class When_renaming_an_data_repository : concern_for_DataRepositoryNamer
   {
      private string _newName;
      private DataRepository _dataRepository;
      private Dimension _timeDimension;
      private string _oldName;
      private Dimension _concDimension;
      private BaseGrid _baseGrid;
      private DataColumn _column;

      protected override void Context()
      {
         base.Context();
         _oldName = "oldName";
         _newName = "newName";
         _dataRepository = new DataRepository {Name = _oldName};
         _timeDimension = new Dimension(new BaseDimensionRepresentation(), "Time", "min");
         _concDimension = new Dimension(new BaseDimensionRepresentation(), "Conc", "mg/l");
         _baseGrid = new BaseGrid("Time", _timeDimension);
         _column = new DataColumn("Col", _concDimension, _baseGrid);
         var quantityInfo = new QuantityInfo(_column.Name, new[] {_oldName, ObjectTypes.ObservedData, "Organ", "Compartment", "Drug", _column.Name}, QuantityType.Undefined);
         _baseGrid.QuantityInfo = new QuantityInfo("time", new[] {_oldName, ObjectTypes.ObservedData}, QuantityType.BaseGrid);
         _column.QuantityInfo = quantityInfo;
         _dataRepository.Add(_baseGrid);
         _dataRepository.Add(_column);
      }

      protected override void Because()
      {
         sut.Rename(_dataRepository, _newName);
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