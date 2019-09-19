using System;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Core.Domain.UnitSystem;

namespace OSPSuite.Core.Domain
{
   public abstract class concern_for_WithDimensionExtensions : StaticContextSpecification
   {
      protected Dimension _timeDimension;
      protected Parameter _parameter;

      protected override void Context()
      {
         _timeDimension = new Dimension(new BaseDimensionRepresentation {TimeExponent = 1}, "Time", "min");
         _timeDimension.AddUnit("s", 1.0/60, 0);
         _timeDimension.AddUnit("h", 60, 0);

         _parameter = new Parameter().WithDimension(_timeDimension);
      }
   }

   public class When_converting_values_for_a_with_dimension_object_in_base_unit_to_a_given_unit : concern_for_WithDimensionExtensions
   {
      [Observation]
      public void should_return_the_given_value_in_base_unit_converted_to_the_unit()
      {
         _parameter.ConvertToUnit(60, "min").ShouldBeEqualTo(60);
         _parameter.ConvertToUnit(60, "s").ShouldBeEqualTo(3600);
         _parameter.ConvertToUnit(60, "h").ShouldBeEqualTo(1);
      }
   }

   public class When_converting_values_for_a_with_dimension_object_to_a_given_unit : concern_for_WithDimensionExtensions
   {
      [Observation]
      public void should_return_the_given_value_converted_in_the_unit()
      {
         _parameter.ConvertToBaseUnit(60, "min").ShouldBeEqualTo(60);
         _parameter.ConvertToBaseUnit(60, "s").ShouldBeEqualTo(1);
         _parameter.ConvertToBaseUnit(1, "h").ShouldBeEqualTo(60);
      }
   }


   public class When_converting_values_for_a_with_dimension_object_to_a_given_unit_that_is_not_defined_in_the_dimension : concern_for_WithDimensionExtensions
   {
      [Observation]
      public void should_return_the_default_value()
      {
         _parameter.ConvertToUnit(60, "toto").ShouldBeEqualTo(60);
      }
   }

   public class When_converting_values_for_a_quantity_object_to_base_unit : concern_for_WithDimensionExtensions
   {
      [Observation]
      public void should_return_the_expected_value()
      {
         _parameter.DisplayUnit = _timeDimension.Unit("h");
         _parameter.ConvertToBaseUnit(1).ShouldBeEqualTo(60);


         _parameter.DisplayUnit = _timeDimension.Unit("s");
         _parameter.ConvertToBaseUnit(60).ShouldBeEqualTo(1);
      }
   }

   public class When_converting_values_for_a_quantity_object_to_display_unit : concern_for_WithDimensionExtensions
   {
      [Observation]
      public void should_return_the_expected_value()
      {
         _parameter.DisplayUnit = _timeDimension.Unit("h");
         _parameter.ConvertToDisplayUnit(60).ShouldBeEqualTo(1);

         _parameter.DisplayUnit = _timeDimension.Unit("s");
         _parameter.ConvertToDisplayUnit(60).ShouldBeEqualTo(3600);
      }
   }

   public class When_converting_values_for_a_quantity_object_to_display_unit_that_does_not_exist : concern_for_WithDimensionExtensions
   {
      protected override void Context()
      {
         base.Context();
         _parameter.Formula = new ConstantFormula(60);
      }

      [Observation]
      public void should_return_the_default_value()
      {
         _parameter.ConvertToUnit("toto").ShouldBeEqualTo(60);
      }
   }

   public class When_converting_a_list_of_values_to_display_unit : concern_for_WithDimensionExtensions
   {
      [Observation]
      public void should_return_the_expected_value()
      {
         _parameter.DisplayUnit = _timeDimension.Unit("h");
         _parameter.ConvertToDisplayValues(new[] {60d, 120}).ShouldOnlyContainInOrder(1, 2);
      }
   }

   public class When_converting_a_list_of_values_to_base_unit : concern_for_WithDimensionExtensions
   {
      [Observation]
      public void should_return_the_expected_value()
      {
         _parameter.DisplayUnit = _timeDimension.Unit("h");
         _parameter.ConvertToBaseValues(new[] {1d, 2d}).ShouldOnlyContainInOrder(60, 120d);
      }
   }

   public class When_retrieving_the_dimension_name_and_base_unit_name_of_a_quantity : concern_for_WithDimensionExtensions
   {
      [Observation]
      public void should_return_the_expected_unit()
      {
         _parameter.DimensionName().ShouldBeEqualTo(_parameter.Dimension.Name);
         _parameter.BaseUnitName().ShouldBeEqualTo(_parameter.Dimension.BaseUnit.Name);
      }
   }

   public class When_retrieving_the_dimension_name_and_base_unit_name_of_a_quantity_with_an_undefined_dimension : concern_for_WithDimensionExtensions
   {
      [Observation]
      public void should_return_the_expected_unit()
      {
         _parameter.Dimension = null;
         _parameter.DimensionName().ShouldBeEqualTo(string.Empty);
         _parameter.BaseUnitName().ShouldBeEqualTo(string.Empty);
      }
   }
}