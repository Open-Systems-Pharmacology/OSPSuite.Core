using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Utility.Extensions;

namespace OSPSuite.Core.Domain
{
   public abstract class concern_for_WithDimensionAndDisplayUnitExtensions : StaticContextSpecification
   {
      protected Dimension _timeDimension;
      protected Parameter _withDimension;

      protected override void Context()
      {
         _timeDimension = new Dimension(new BaseDimensionRepresentation {TimeExponent = 1}, "Time", "s");
         _timeDimension.AddUnit("min", 60, 0);
         _timeDimension.AddUnit("h", 3600, 0);

         _withDimension = new Parameter().WithDimension(_timeDimension);
      }
   }

   public class When_converting_values_for_a_with_dimension_object_to_a_given_unit : concern_for_WithDimensionAndDisplayUnitExtensions
   {
      [Observation]
      public void should_return_the_given_value_converted_in_the_unit()
      {
         _withDimension.ConvertToUnit(60, "min").ShouldBeEqualTo(1);
         _withDimension.ConvertToUnit(3600, "min").ShouldBeEqualTo(60);
      }
   }

   public class When_converting_values_for_a_with_dimension_object_to_a_given_unit_that_is_not_defined_in_the_dimension : concern_for_WithDimensionAndDisplayUnitExtensions
   {
      [Observation]
      public void should_return_the_default_value()
      {
         _withDimension.ConvertToUnit(60, "toto").ShouldBeEqualTo(60);
      }
   }

   public class When_converting_values_for_a_quantity_object_to_base_unit : concern_for_WithDimensionAndDisplayUnitExtensions
   {
      private Parameter _parameter;

      protected override void Context()
      {
         base.Context();
         _parameter = _withDimension.DowncastTo<Parameter>();
      }

      [Observation]
      public void should_return_the_expected_value()
      {
         _parameter.DisplayUnit = _timeDimension.Unit("h");
         _parameter.ConvertToBaseUnit(1).ShouldBeEqualTo(3600);


         _parameter.DisplayUnit = _timeDimension.Unit("min");
         _parameter.ConvertToBaseUnit(1).ShouldBeEqualTo(60);
      }
   }

   public class When_converting_values_for_a_quantity_object_to_display_unit : concern_for_WithDimensionAndDisplayUnitExtensions
   {
      private Parameter _parameter;

      protected override void Context()
      {
         base.Context();
         _parameter = _withDimension.DowncastTo<Parameter>();
      }

      [Observation]
      public void should_return_the_expected_value()
      {
         _parameter.DisplayUnit = _timeDimension.Unit("h");
         _parameter.ConvertToDisplayUnit(3600).ShouldBeEqualTo(1);

         _parameter.DisplayUnit = _timeDimension.Unit("min");
         _parameter.ConvertToDisplayUnit(60).ShouldBeEqualTo(1);
      }
   }

   public class When_converting_values_for_a_quantity_object_to_display_unit_that_does_not_exist : concern_for_WithDimensionAndDisplayUnitExtensions
   {
      private Parameter _parameter;

      protected override void Context()
      {
         base.Context();
         _parameter = _withDimension.DowncastTo<Parameter>();
         _parameter.Formula = new ConstantFormula(60);
      }

      [Observation]
      public void should_return_the_default_value()
      {
         _parameter.ConvertToUnit("toto").ShouldBeEqualTo(60);
      }
   }

   public class When_converting_a_list_of_values_to_display_unit : concern_for_WithDimensionAndDisplayUnitExtensions
   {
      private Parameter _parameter;

      protected override void Context()
      {
         base.Context();
         _parameter = _withDimension.DowncastTo<Parameter>();
      }

      [Observation]
      public void should_return_the_expected_value()
      {
         _parameter.DisplayUnit = _timeDimension.Unit("h");
         _parameter.ConvertToDisplayValues(new[] {3600d, 7200}).ShouldOnlyContainInOrder(1, 2);
      }
   }

   public class When_converting_a_list_of_values_to_base_unit : concern_for_WithDimensionAndDisplayUnitExtensions
   {
      private Parameter _parameter;

      protected override void Context()
      {
         base.Context();
         _parameter = _withDimension.DowncastTo<Parameter>();
      }

      [Observation]
      public void should_return_the_expected_value()
      {
         _parameter.DisplayUnit = _timeDimension.Unit("h");
         _parameter.ConvertToBaseValues(new[] {1d, 2d}).ShouldOnlyContainInOrder(3600d, 7200d);
      }
   }
}