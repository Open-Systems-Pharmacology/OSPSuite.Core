using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain;

namespace OSPSuite.Core
{
   public class When_checking_if_a_building_block_type_is_another_type : StaticContextSpecification
   {
      [Observation]
      public void should_return_true_if_the_types_are_equal()
      {
         PKSimBuildingBlockType.Compound.Is(PKSimBuildingBlockType.Compound).ShouldBeTrue();
      }

      [Observation]
      public void should_return_true_if_the_compared_type_contains_the_other_type()
      {
         PKSimBuildingBlockType.Compound.Is(PKSimBuildingBlockType.Compound | PKSimBuildingBlockType.Event).ShouldBeTrue();
      }

      [Observation]
      public void should_return_false_if_the_compared_type_does_not_contain_the_other_type()
      {
         PKSimBuildingBlockType.Compound.Is(PKSimBuildingBlockType.Formulation | PKSimBuildingBlockType.Event).ShouldBeFalse();
      }
   }
}