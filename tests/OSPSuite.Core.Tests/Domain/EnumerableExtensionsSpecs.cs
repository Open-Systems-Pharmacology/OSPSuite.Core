using System.Collections.Generic;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Extensions;

namespace OSPSuite.Core.Domain
{
   public abstract class concern_for_EnumerableExtensions : StaticContextSpecification
   {
      protected List<string> _enumeration;

      protected override void Because()
      {
         _enumeration = new List<string> {"A", "B", "C"};
      }
   }

   public class When_checking_if_an_enumeration_contains_all_item_of_another_enumeration : concern_for_EnumerableExtensions
   {
      [Observation]
      public void should_return_true_if_all_items_are_contained()
      {
         _enumeration.ContainsAll(new[] {"A", "B"}).ShouldBeTrue();
         _enumeration.ContainsAll(new string[] {}).ShouldBeTrue();
      }

      [Observation]
      public void should_return_false_otherwise()
      {
         _enumeration.ContainsAll(new[] { "A", "D" }).ShouldBeFalse();
      }
   }

   public class When_checking_if_an_enumeration_contains_any_item_of_another_enumeration : concern_for_EnumerableExtensions
   {
      [Observation]
      public void should_return_true_if_at_least_one_item_is_contained()
      {
         _enumeration.ContainsAny(new[] { "A", "B" }).ShouldBeTrue();
         _enumeration.ContainsAny(new[] { "C", "D"}).ShouldBeTrue();
      }

      [Observation]
      public void should_return_false_otherwise()
      {
         _enumeration.ContainsAny(new[] { "D", "E" }).ShouldBeFalse();
      }
   }
}	