using System;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Helpers;

namespace OSPSuite.R.Extensions
{
   public abstract class concern_for_WithDimensionExtensions : StaticContextSpecification
   {
      protected IDimension _dimension;
      protected Parameter _parameter;

      protected override void Context()
      {
         //min = 1, h = 2
         _dimension = DomainHelperForSpecs.TimeDimensionForSpecs();
         _parameter = new Parameter().WithDimension(_dimension);
      }
   }

   public class When_converting_a_value_to_base_unit_using_the_unit_index : concern_for_WithDimensionExtensions
   {
      [Observation]
      public void should_set_the_expected_value()
      {
         _parameter.ConvertToBaseUnit(2, 2).ShouldBeEqualTo(120);
      }

      [Observation]
      public void should_thrown_an_exception_if_the_index_is_out_of_range()
      {
         The.Action(() => _parameter.ConvertToBaseUnit(2, 3)).ShouldThrowAn<ArgumentOutOfRangeException>();
         The.Action(() => _parameter.ConvertToBaseUnit(2, 0)).ShouldThrowAn<ArgumentOutOfRangeException>();
         The.Action(() => _parameter.ConvertToBaseUnit(2, -1)).ShouldThrowAn<ArgumentOutOfRangeException>();
      }
   }


   public class When_converting_a_value_to_unit_using_the_unit_index : concern_for_WithDimensionExtensions
   {
      [Observation]
      public void should_set_the_expected_value()
      {
         _parameter.ConvertToUnit(120, 1).ShouldBeEqualTo(120);
         _parameter.ConvertToUnit(120, 2).ShouldBeEqualTo(2);
      }

      [Observation]
      public void should_thrown_an_exception_if_the_index_is_out_of_range()
      {
         The.Action(() => _parameter.ConvertToUnit(2, 3)).ShouldThrowAn<ArgumentOutOfRangeException>();
         The.Action(() => _parameter.ConvertToUnit(2, 0)).ShouldThrowAn<ArgumentOutOfRangeException>();
         The.Action(() => _parameter.ConvertToUnit(2, -1)).ShouldThrowAn<ArgumentOutOfRangeException>();
      }
   }
}