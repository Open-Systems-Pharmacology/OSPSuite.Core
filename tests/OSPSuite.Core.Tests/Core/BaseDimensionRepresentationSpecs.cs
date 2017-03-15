using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain.UnitSystem;

namespace OSPSuite.Core
{
   public abstract class concern_for_BaseDimensionRepresentation : ContextSpecification<BaseDimensionRepresentation>
   {
      protected double[] _exponents;

      protected override void Context()
      {
         _exponents = new double[] {1, 2, 3, 4, 5, 6, 7};

         sut = new BaseDimensionRepresentation(_exponents);
      }
   }

   public class When_comparing_equality_of_two_double_arrays_with_the_same_exponents : concern_for_BaseDimensionRepresentation
   {
      [Observation]
      public void should_return_that_they_are_equal()
      {
         sut.Equals(new BaseDimensionRepresentation(_exponents)).ShouldBeTrue();
         sut.Equals(new BaseDimensionRepresentation(sut)).ShouldBeTrue();
      }

      [Observation]
      public void should_return_that_they_are_not_equal()
      {
         sut.Equals(new BaseDimensionRepresentation(new double[] {1, 2, 3, 4, 5, 6, 7})).ShouldBeTrue();
      }
   }

   public class When_calculating_the_has_code_for_two_base_representation_with_the_same_exponents : concern_for_BaseDimensionRepresentation
   {
      [Observation]
      public void should_return_the_same_value()
      {
         sut.GetHashCode().ShouldBeEqualTo(new BaseDimensionRepresentation(_exponents).GetHashCode());
      }
   }
}