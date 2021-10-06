using System.Collections.Generic;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain.UnitSystem;

namespace OSPSuite.Core.Domain
{
   public abstract class concern_for_DimensionFactory : ContextSpecification<IDimensionFactory>
   {
      protected Dimension _drugMassDimension;
      protected Dimension _volumeDimension;
      protected Dimension _flowDimension;
      protected Dimension _timeDimension;
      protected Dimension _inversedTimeDimension;
      protected Dimension _anotherDimensionThatLooksLikeVolumeWithADifferentUnit;

      protected override void Context()
      {
         sut = new DimensionFactory();

         _drugMassDimension = new Dimension(new BaseDimensionRepresentation(), "DrugMass", "g");
         _volumeDimension = new Dimension(new BaseDimensionRepresentation {MassExponent = 3}, "Volume", "l");
         _anotherDimensionThatLooksLikeVolumeWithADifferentUnit = new Dimension(new BaseDimensionRepresentation {MassExponent = 3, TimeExponent = -1}, "01_OTHER", "");
         _flowDimension = new Dimension(new BaseDimensionRepresentation {MassExponent = 3, TimeExponent = -1}, "flow", "l/min");
         _timeDimension = new Dimension(new BaseDimensionRepresentation {TimeExponent = 1}, "Time", "min");
         _inversedTimeDimension = new Dimension(new BaseDimensionRepresentation {TimeExponent = -1}, "InversedTime", "1/min");

         sut.AddDimension(_drugMassDimension);
         sut.AddDimension(_volumeDimension);
         sut.AddDimension(_flowDimension);
         sut.AddDimension(_anotherDimensionThatLooksLikeVolumeWithADifferentUnit);
         sut.AddDimension(_timeDimension);
         sut.AddDimension(_inversedTimeDimension);
         sut.AddDimension(Constants.Dimension.NO_DIMENSION);
      }
   }

   public class When_retrieving_the_dimension_and_unit_for_an_existing_unit : concern_for_DimensionFactory
   {
      [Observation]
      public void should_return_the_expected_dimension_for_a_real_match()
      {
         var (dim, unit) = sut.FindUnit("g");
         dim.ShouldBeEqualTo(_drugMassDimension);
         unit.ShouldBeEqualTo(_drugMassDimension.Unit("g"));
      }

      [Observation]
      public void should_return_the_expected_dimension_for_a_case_match()
      {
         var (dim, unit) = sut.FindUnit("G");
         dim.ShouldBeEqualTo(_drugMassDimension);
         unit.ShouldBeEqualTo(_drugMassDimension.Unit("g"));
      }
   }

   public class When_retrieving_the_dimension_and_unit_for_a_unit_that_does_not_exist : concern_for_DimensionFactory
   {
      [Observation]
      public void should_return_the_expected_dimension()
      {
         var (dim, unit) = sut.FindUnit("nope");
         dim.ShouldBeNull();
         unit.ShouldBeNull();
      }
   }

   public abstract class When_retrieving_dimension_from_unit_with_multiple_matching_units : concern_for_DimensionFactory
   {
      protected IDimension _result;
      protected Dimension _accelerationDimension;
      protected abstract string ConvertUnitCase(string unit);

      protected override void Context()
      {
         base.Context();
         _accelerationDimension = new Dimension(new BaseDimensionRepresentation(), "Acceleration", "G");
         sut.AddDimension(_accelerationDimension);
      }

      protected override void Because()
      {
         _result = sut.DimensionForUnit(ConvertUnitCase("g"));
      }
   }

   public class When_retrieving_upper_case_unit_from_multiple_matching_units : When_retrieving_dimension_from_unit_with_multiple_matching_units
   {
      protected override string ConvertUnitCase(string unit)
      {
         return unit.ToUpper();
      }

      [Observation]
      public void results_in_lower_case_match()
      {
         _result.ShouldBeEqualTo(_accelerationDimension);
      }
   }

   public class When_retrieving_the_dimensions_sorted_by_name : concern_for_DimensionFactory
   {
      [Observation]
      public void should_return_the_dimension_in_the_expected_order()
      {
         sut.DimensionsSortedByName.ShouldOnlyContainInOrder(_anotherDimensionThatLooksLikeVolumeWithADifferentUnit, Constants.Dimension.NO_DIMENSION, _drugMassDimension, _flowDimension, _inversedTimeDimension, _timeDimension, _volumeDimension);
      }
   }

   public class When_retrieving_lower_case_unit_from_multiple_matching_units : When_retrieving_dimension_from_unit_with_multiple_matching_units
   {
      protected override string ConvertUnitCase(string unit)
      {
         return unit.ToLower();
      }

      [Observation]
      public void results_in_lower_case_match()
      {
         _result.ShouldBeEqualTo(_drugMassDimension);
      }
   }

   public class When_told_to_retrieve_a_dimension_by_name_that_does_not_exist_and_that_is_not_an_RHS_dimension : concern_for_DimensionFactory
   {
      [Observation]
      public void should_throw_an_exception()
      {
         The.Action(() => sut.Dimension("TOTO")).ShouldThrowAn<KeyNotFoundException>();
      }
   }

   public class When_told_to_retrieve_the_RHS_dimension_for_the_dimensionless_dimension : concern_for_DimensionFactory
   {
      [Observation]
      public void should_return_the_per_time_dimension()
      {
         var rhsDimension = sut.GetOrAddRHSDimensionFor(Constants.Dimension.NO_DIMENSION);
         rhsDimension.ShouldBeEqualTo(_inversedTimeDimension);
      }
   }

   public class When_told_to_retrieve_the_RHS_dimension_for_the_time_dimension : concern_for_DimensionFactory
   {
      [Observation]
      public void should_return_the_fraction_dimension()
      {
         var rhsDimension = sut.GetOrAddRHSDimensionFor(_timeDimension);
         rhsDimension.ShouldBeEqualTo(Constants.Dimension.NO_DIMENSION);
      }
   }

   public class When_told_to_retrieve_a_rhs_dimension_for_a_dimension : concern_for_DimensionFactory
   {
      [Observation]
      public void should_return_the_equivalent_dimension_if_it_exists_instead_of_creating_a_new_one()
      {
         sut.GetOrAddRHSDimensionFor(_volumeDimension).ShouldBeEqualTo(_flowDimension);
      }
   }

   public class When_told_to_try_to_retrieve_a_dimension_by_name_that_does_exist_and_that_is_a_possible_RHS_dimension : concern_for_DimensionFactory
   {
      [Observation]
      public void should_return_false()
      {
         var res = sut.TryGetDimension(_volumeDimension.Name + Constants.Dimension.RHS_DIMENSION_SUFFIX, out var dimension);
         res.ShouldBeTrue();
         dimension.ShouldBeEqualTo(_flowDimension);
      }
   }

   public class When_told_to_try_to_retrieve_a_dimension_by_name_that_does_not_exist_and_that_is_not_a_possible_RHS_dimension : concern_for_DimensionFactory
   {
      [Observation]
      public void should_return_false()
      {
         var res = sut.TryGetDimension("TOTO" + Constants.Dimension.RHS_DIMENSION_SUFFIX, out var dimension);
         res.ShouldBeFalse();
         dimension.ShouldBeNull();
      }
   }
}