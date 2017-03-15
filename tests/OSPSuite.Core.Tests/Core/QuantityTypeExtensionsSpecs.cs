using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain;

namespace OSPSuite.Core
{
   internal class When_testing_equal_types : StaticContextSpecification
   {
      [Observation]
      public void should_return_true()
      {
         QuantityType.Drug.Is(QuantityType.Drug).ShouldBeTrue();
      }
   }

   internal class When_testing_unequal_types : StaticContextSpecification
   {
      [Observation]
      public void should_return_false()
      {
         QuantityType.Drug.Is(QuantityType.Protein).ShouldBeFalse();
      }
   }

   internal class When_testing_type_containing_tested_type : StaticContextSpecification
   {
      [Observation]
      public void should_return_true()
      {
         (QuantityType.Drug | QuantityType.Parameter).Is(QuantityType.Drug).ShouldBeTrue();
         (QuantityType.Drug).Is(QuantityType.Drug | QuantityType.Parameter).ShouldBeTrue();
         (QuantityType.Drug|QuantityType.Protein).Is(QuantityType.Drug | QuantityType.Parameter).ShouldBeTrue();
         (QuantityType.Drug | QuantityType.Observer).Is(QuantityType.Molecule).ShouldBeTrue();
         (QuantityType.Drug | QuantityType.Observer).Is(QuantityType.Enzyme | QuantityType.Observer).ShouldBeTrue();
      }
   }
}