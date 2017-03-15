using System.Linq;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain.Descriptors;

namespace OSPSuite.Core
{
   public abstract class concern_for_Tags : ContextSpecification<Tags>
   {
      protected Tag _tag1;
      private Tag _tag2;

      protected override void Context()
      {
         _tag1 = new Tag("TOTO");
         _tag2 = new Tag("TATA");
         sut = new Tags {_tag1, _tag2};
      }
   }

   public class When_removing_a_tag_by_string_that_does_not_exist : concern_for_Tags
   {
      protected override void Because()
      {
         sut.Remove("TITI");
      }

      [Observation]
      public void should_not_remove_anything()
      {
         sut.Count().ShouldBeEqualTo(2);
      }
   }

   public class When_removing_a_tag_by_string_that_does_exist : concern_for_Tags
   {
      protected override void Because()
      {
         sut.Remove("TOTO");
      }

      [Observation]
      public void should_remove_the_tag()
      {
         sut.Count().ShouldBeEqualTo(1);
      }
   }

   public class When_removing_a_tag_that_does_exist : concern_for_Tags
   {
      protected override void Because()
      {
         sut.Remove(_tag1);
      }

      [Observation]
      public void should_remove_the_tag()
      {
         sut.Count().ShouldBeEqualTo(1);
      }
   }

   public class When_removing_a_tag_that_does_not_exist : concern_for_Tags
   {
      protected override void Because()
      {
         sut.Remove(new Tag("TUTU"));
      }

      [Observation]
      public void should_remove_the_tag()
      {
         sut.Count().ShouldBeEqualTo(2);
      }
   }
}	