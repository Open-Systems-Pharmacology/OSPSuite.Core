using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using FakeItEasy;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Domain.ParameterIdentifications;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Core.Extensions;
using OSPSuite.Helpers;

namespace OSPSuite.Core
{
   public abstract class concern_for_OutputMapping : ContextSpecification<OutputMapping>
   {
      protected DataRepository _dataRepository;
      protected SimulationQuantitySelection _simulationQuantitySelection;
      protected IQuantity _quantity;
      protected DataColumn _firstDimension;

      protected override void Context()
      {
         sut = new OutputMapping();
         _quantity = A.Fake<IQuantity>();
         _simulationQuantitySelection = A.Fake<SimulationQuantitySelection>();
         A.CallTo(() => _simulationQuantitySelection.Quantity).Returns(_quantity);
         sut.OutputSelection = _simulationQuantitySelection;
         _dataRepository = DomainHelperForSpecs.ObservedData();
         sut.WeightedObservedData = new WeightedObservedData(_dataRepository);
         _firstDimension = _dataRepository.FirstDataColumn();
      }
   }

   public class When_updating_simulation_reference : concern_for_OutputMapping
   {
      private ISimulation _newSimulation;
      private ISimulation _oldSimulation;

      protected override void Context()
      {
         base.Context();
         _newSimulation = A.Fake<ISimulation>();
         _oldSimulation = A.Fake<ISimulation>();
         A.CallTo(() => _quantity.Dimension).Returns(_firstDimension.Dimension);
         sut.OutputSelection = new SimulationQuantitySelection(_oldSimulation, A.Fake<QuantitySelection>());
      }

      protected override void Because()
      {
         sut.UpdateSimulation(_newSimulation);
      }

      [Observation]
      public void the_referenced_simulation_should_be_updated()
      {
         sut.Simulation.ShouldBeEqualTo(_newSimulation);
      }
   }

   public class When_retrieving_the_full_output_path_of_an_output_mapping_for_which_the_output_is_not_defined : concern_for_OutputMapping
   {
      [Observation]
      public void should_return_an_empty_string()
      {
         string.IsNullOrEmpty(sut.FullOutputPath).ShouldBeTrue();
      }
   }

   public class When_checking_if_the_output_dimension_matches_the_observed_data_dimension : concern_for_OutputMapping
   {
      [Observation]
      public void should_return_valid_if_both_dimensions_are_concentration()
      {
         var dimension = A.Fake<IDimension>();
         A.CallTo(() => dimension.Name).Returns(Constants.Dimension.MASS_CONCENTRATION);
         _firstDimension.Dimension = dimension;

         var dimension2 = A.Fake<IDimension>();
         A.CallTo(() => dimension2.Name).Returns(Constants.Dimension.MOLAR_CONCENTRATION);
         A.CallTo(() => _quantity.Dimension).Returns(dimension2);
         sut.DimensionsAreConsistent().ShouldBeTrue();
      }

      [Observation]
      public void should_return_valid_if_both_dimensions_are_amount()
      {
         var dimension = A.Fake<IDimension>();
         A.CallTo(() => dimension.Name).Returns(Constants.Dimension.AMOUNT);
         _firstDimension.Dimension = dimension;

         var dimension2 = A.Fake<IDimension>();
         A.CallTo(() => dimension2.Name).Returns(Constants.Dimension.MASS_AMOUNT);
         A.CallTo(() => _quantity.Dimension).Returns(dimension2);
         sut.DimensionsAreConsistent().ShouldBeTrue();
      }

      [Observation]
      public void should_return_valid_if_both_dimensions_share_the_same_base_unit()
      {
         var dimension = A.Fake<IDimension>();
         A.CallTo(() => dimension.BaseUnit).Returns(new Unit("base", 1, 0));
         _firstDimension.Dimension = dimension;

         var dimension2 = A.Fake<IDimension>();
         A.CallTo(() => dimension2.BaseUnit).Returns(new Unit("base", 1, 0));
         A.CallTo(() => _quantity.Dimension).Returns(dimension2);
         sut.DimensionsAreConsistent().ShouldBeTrue();
      }

      [Observation]
      public void should_return_invalid_if_at_least_one_dimension_is_null()
      {
         A.CallTo(() => _quantity.Dimension).Returns(null);
         sut.DimensionsAreConsistent().ShouldBeFalse();
      }

      [Observation]
      public void should_return_invalid_otherwise()
      {
         A.CallTo(() => _quantity.Dimension).Returns(A.Fake<IDimension>());
         sut.DimensionsAreConsistent().ShouldBeFalse();
      }
   }
}