using System.Collections.Generic;
using System.Data;
using System.Linq;
using FakeItEasy;
using OSPSuite.BDDHelper;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Domain.ParameterIdentifications;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Presentation.Mappers.ParameterIdentifications;
using DataColumn = OSPSuite.Core.Domain.Data.DataColumn;

namespace OSPSuite.Presentation.Presentation
{
   public abstract class concern_for_WeightedDataRepositoryToDataTableMapper : ContextSpecification<WeightedDataRepositoryToDataTableMapper>
   {
      protected IDataRepositoryExportTask _dataRepositoryTask;
      protected IDimensionFactory _dimensionFactory;
      protected DataRepository _dataRepository;
      protected WeightedObservedData _weightedObservedData;

      protected override void Context()
      {
         _dataRepositoryTask = A.Fake<IDataRepositoryExportTask>();
         _dimensionFactory = A.Fake<IDimensionFactory>();
         sut = new WeightedDataRepositoryToDataTableMapper(_dataRepositoryTask, _dimensionFactory);

         var baseGrid = new BaseGrid("name", _dimensionFactory.NoDimension) { Values = new[] { 0.0f } };
         var dataColumn = new DataColumn { Values = new[] { 0.0f } };
         _dataRepository = new DataRepository { baseGrid, dataColumn };

         _weightedObservedData = new WeightedObservedData(_dataRepository);

         A.CallTo(() => _dataRepositoryTask.ToDataTable(A<IEnumerable<DataColumn>>._, A<DataColumnExportOptions>._)).Returns(new[] {new DataTable() });
      }
   }

   public class When_mapping_a_weighted_data_repository : concern_for_WeightedDataRepositoryToDataTableMapper
   {
      protected override void Because()
      {
         sut.MapFrom(_weightedObservedData);
      }

      [Observation]
      public void the_output_table_must_include_columns_from_the_repository_and_additional_weight_column()
      {
         A.CallTo(() => _dataRepositoryTask.ToDataTable(A<IEnumerable<DataColumn>>.That.Matches(repo => repo.Count() == 3), A<DataColumnExportOptions>._)).MustHaveHappened();
      }

      [Observation]
      public void should_convert_the_observed_data_to_a_data_table()
      {
         A.CallTo(() => _dataRepositoryTask.ToDataTable(A<IEnumerable<DataColumn>>._, A<DataColumnExportOptions>._)).MustHaveHappened();
      }
   }
}
