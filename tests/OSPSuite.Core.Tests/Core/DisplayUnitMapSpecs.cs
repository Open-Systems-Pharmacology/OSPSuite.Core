using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Utility.Validation;
using FakeItEasy;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.UnitSystem;

namespace OSPSuite.Core
{
   public abstract class concern_for_DisplayUnitMap : ContextSpecification<DisplayUnitMap>
   {
      protected override void Context()
      {
         sut = new DisplayUnitMap();
      }
   }

   public class a_display_unit_map_with_an_undefined_dimension : concern_for_DisplayUnitMap
   {
      protected override void Context()
      {
         base.Context();
         sut.DisplayUnit = A.Fake<Unit>();
      }

      [Observation]
      public void should_not_be_valid()
      {
         sut.IsValid().ShouldBeFalse();
      }
   }


   public class a_display_unit_map_with_an_undefined_unit : concern_for_DisplayUnitMap
   {
      protected override void Context()
      {
         base.Context();
         sut.Dimension = A.Fake<IDimension>();
         sut.DisplayUnit = null;
      }

      [Observation]
      public void should_not_be_valid()
      {
         sut.IsValid().ShouldBeFalse();
      }
   }

   public class a_display_unit_map_with_an_defined_unit_and_dimension : concern_for_DisplayUnitMap
   {
      protected override void Context()
      {
         base.Context();
         sut.Dimension = A.Fake<IDimension>();
         sut.DisplayUnit = A.Fake<Unit>();
      }

      [Observation]
      public void should_be_valid()
      {
         sut.IsValid().ShouldBeTrue();
      }
   }

   public class When_changing_the_dimension_of_a_given_mapping : concern_for_DisplayUnitMap
   {
      private IDimension _dimension;

      protected override void Context()
      {
         base.Context();
         _dimension = A.Fake<IDimension>();
         _dimension.DefaultUnit = A.Fake<Unit>();
      }
      protected override void Because()
      {
         sut.Dimension = _dimension;
      }

      [Observation]
      public void should_set_the_display_unit_to_be_the_default_unit_for_that_dimension()
      {
         sut.DisplayUnit.ShouldBeEqualTo(_dimension.DefaultUnit);
      }
   }
}