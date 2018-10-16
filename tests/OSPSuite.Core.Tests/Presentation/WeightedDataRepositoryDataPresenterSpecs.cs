using System.Data;
using FakeItEasy;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Domain.ParameterIdentifications;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Presentation.Mappers.ParameterIdentifications;
using OSPSuite.Presentation.Presenters.ParameterIdentifications;
using OSPSuite.Presentation.Views.ParameterIdentifications;
using DataColumn = OSPSuite.Core.Domain.Data.DataColumn;

namespace OSPSuite.Presentation
{
   public abstract class concern_for_WeightedDataRepositoryDataPresenter : ContextSpecification<WeightedDataRepositoryDataPresenter>
   {
      protected IWeightedDataRepositoryDataView _view;
      protected IWeightedDataRepositoryToDataTableMapper _weightedDataRepositoryToDataTableMapper;
      private DataRepository _dataRepository;
      protected WeightedObservedData _weightedObservedData;
      private IDimensionFactory _dimensionFactory;

      protected override void Context()
      {
         _view = A.Fake<IWeightedDataRepositoryDataView>();
         _dimensionFactory = A.Fake<IDimensionFactory>();
         _weightedDataRepositoryToDataTableMapper = A.Fake<IWeightedDataRepositoryToDataTableMapper>();
         var baseGrid = new BaseGrid("name", _dimensionFactory.NoDimension) {Values = new[] {0.0f}};
         var dataColumn = new DataColumn {Values = new[] {0.0f}};
         _dataRepository = new DataRepository {baseGrid, dataColumn};

         _weightedObservedData = new WeightedObservedData(_dataRepository);

         A.CallTo(() => _weightedDataRepositoryToDataTableMapper.MapFrom(_weightedObservedData)).Returns(new DataTable());

         sut = new WeightedDataRepositoryDataPresenter(_view, _weightedDataRepositoryToDataTableMapper);
      }
   }

   public class When_validating_a_weight_value_for_a_data_repository_point : concern_for_WeightedDataRepositoryDataPresenter
   {
      [Observation]
      public void zero_is_a_valid_weight()
      {
         sut.GetValidationMessagesForWeight("0").ShouldBeEmpty();
      }

      [Observation]
      public void positive_value_is_a_valid_weight()
      {
         sut.GetValidationMessagesForWeight("1").ShouldBeEmpty();
      }

      [Observation]
      public void negative_value_is_a_invalid_weight()
      {
         sut.GetValidationMessagesForWeight("-1").ShouldNotBeEmpty();
      }

      [Observation]
      public void empty_value_is_a_invalid_weight()
      {
         sut.GetValidationMessagesForWeight("").ShouldNotBeEmpty();
      }

      [Observation]
      public void invalid_value_is_a_invalid_weight()
      {
         sut.GetValidationMessagesForWeight("asdsadad").ShouldNotBeEmpty();
      }
   }

   public class When_changing_the_weight_of_a_observed_data_point : concern_for_WeightedDataRepositoryDataPresenter
   {
      protected override void Context()
      {
         base.Context();
         sut.EditObservedData(_weightedObservedData);
      }

      protected override void Because()
      {
         sut.ChangeWeight(0, 3.0f);
      }

      [Observation]
      public void the_weight_should_be_changed()
      {
         _weightedObservedData.Weights[0].ShouldBeEqualTo(3);
      }
   }
}