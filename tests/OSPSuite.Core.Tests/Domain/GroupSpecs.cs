using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;

namespace OSPSuite.Core.Domain
{
   public abstract class concern_for_Group : ContextSpecification<IGroup>
   {
      protected override void Context()
      {
         sut = new Group{Name="Level1"};
         sut.AddChild(new Group{Name="Level11"});
         var subGroup = new Group { Name = "Level12" };
         subGroup.AddChild(new Group { Name = "Level121" });
         sut.AddChild(subGroup);
      }
   }

   public class When_asking_if_a_parameter_group_contains_a_sub_group_by_name : concern_for_Group
   {
      [Observation]
      public void should_return_true_if_the_parameter_group_contains_a_sub_group_with_this_name()
      {
         sut.ContainsGroup("Level1").ShouldBeTrue();
         sut.ContainsGroup("Level11").ShouldBeTrue();
         sut.ContainsGroup("Level121").ShouldBeTrue();
      }

      [Observation]
      public void should_return_false_if_the_parameter_group_does_not_contain_a_sub_group_with_this_name()
      {
         sut.ContainsGroup("Level132").ShouldBeFalse();
      }
   }
}	