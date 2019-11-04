using System.Collections.Generic;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;

namespace OSPSuite.Core.Domain
{
   public abstract class concern_for_PathElement : ContextSpecification<PathElement>
   {
      protected override void Context()
      {
         sut = new PathElement();
      }
   }

   public class checking_the_equality_of_two_quantity_path_element_dto : concern_for_PathElement
   {
      [Observation]
      public void should_return_equals_if_they_are_both_without_display_name()
      {
         sut.Equals(new PathElement()).ShouldBeTrue();
         sut.CompareTo(new PathElement()).ShouldBeEqualTo(0);
      }

      [Observation]
      public void should_return_equals_if_they_have_the_same_display_name()
      {
         sut.DisplayName = "toto";
         sut.Equals(new PathElement {DisplayName = "toto"}).ShouldBeTrue();
      }

      [Observation]
      public void should_return_false_if_they_have_different_display_name()
      {
         sut.DisplayName = "toto";
         sut.Equals(new PathElement {DisplayName = "tata"}).ShouldBeFalse();
      }
   }

   public class adding_quantity_path_element_having_the_same_display_into_a_has_set : concern_for_PathElement
   {
      private HashSet<PathElement> _hash;

      protected override void Context()
      {
         base.Context();
         _hash = new HashSet<PathElement>();
      }

      [Observation]
      public void should_not_duplicate_values()
      {
         _hash.Add(new PathElement());
         _hash.Add(new PathElement());
         _hash.Count.ShouldBeEqualTo(1);
      }
   }
}