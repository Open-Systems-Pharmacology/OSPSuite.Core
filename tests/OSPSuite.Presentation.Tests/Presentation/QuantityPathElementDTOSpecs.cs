using System.Collections.Generic;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Presentation.DTO;

namespace OSPSuite.Presentation.Presentation
{
   public abstract class concern_for_QuantityPathElementDTO : ContextSpecification<PathElementDTO>
   {
      protected override void Context()
      {
         sut = new PathElementDTO();
      }
   }

   public class checking_the_equality_of_two_quantity_path_element_dto : concern_for_QuantityPathElementDTO
   {
      [Observation]
      public void should_return_equals_if_they_are_both_without_display_name()
      {
         sut.Equals(new PathElementDTO()).ShouldBeTrue();
         sut.CompareTo(new PathElementDTO()).ShouldBeEqualTo(0);
      }

      [Observation]
      public void should_return_equals_if_they_have_the_same_display_name()
      {
         sut.DisplayName = "toto";
         sut.Equals(new PathElementDTO {DisplayName = "toto"}).ShouldBeTrue();
      }

      [Observation]
      public void should_return_false_if_they_have_different_display_name()
      {
         sut.DisplayName = "toto";
         sut.Equals(new PathElementDTO {DisplayName = "tata"}).ShouldBeFalse();
      }
   }

   public class adding_quantity_path_element_having_the_same_display_into_a_has_set : concern_for_QuantityPathElementDTO
   {
      private HashSet<PathElementDTO> _hash;

      protected override void Context()
      {
         base.Context();
         _hash = new HashSet<PathElementDTO>();
      }

      [Observation]
      public void should_not_duplicate_values()
      {
         _hash.Add(new PathElementDTO());
         _hash.Add(new PathElementDTO());
         _hash.Count.ShouldBeEqualTo(1);
      }
   }
}