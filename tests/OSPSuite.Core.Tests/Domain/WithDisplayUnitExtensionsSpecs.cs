using FakeItEasy;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain.UnitSystem;

namespace OSPSuite.Core.Domain
{
   public abstract class concern_for_WithDisplayUnitExtensions : StaticContextSpecification
   {
      protected Dimension _timeDimension;
      protected Parameter _parameter;
      protected IWithDisplayUnit _undefined = null;

      protected override void Context()
      {
         _timeDimension = new Dimension(new BaseDimensionRepresentation { TimeExponent = 1 }, "Time", "s");
         _timeDimension.AddUnit("min", 60, 0);
         _timeDimension.AddUnit("h", 3600, 0);

         _parameter = new Parameter().WithDimension(_timeDimension);
         _parameter.DisplayUnit = _timeDimension.Unit("h");

      }
   }

   public class When_returning_the_display_unit_name_of_an_undefined_entity : concern_for_WithDisplayUnitExtensions
   {
      [Observation]
      public void should_return_an_empty_string()
      {
         _undefined.DimensionName().ShouldBeEqualTo(string.Empty);
      }
   }


   public class When_returning_the_display_unit_name_of_an_quantity_with_a_defined_dimension : concern_for_WithDisplayUnitExtensions
   {
      [Observation]
      public void should_return_the_expected_unit()
      {
         _parameter.DisplayUnitName().ShouldBeEqualTo("h");
      }
   }
}