using NUnit.Framework;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;

namespace OSPSuite.Core.Extensions
{
   public abstract class concern_for_ModuleExtensions : StaticContextSpecification
   {
      protected Module _module;
      protected MoleculeBuildingBlock _moleculeBuildingBlock;
      protected PassiveTransportBuildingBlock _passiveTransportBuildingBlock;

      public override void GlobalContext()
      {
         base.GlobalContext();
         _module = new Module();
         _moleculeBuildingBlock = new MoleculeBuildingBlock();
         _passiveTransportBuildingBlock = new PassiveTransportBuildingBlock();
      }
   }


   public class When_building_block_type_is_not_in_unique_types : concern_for_ModuleExtensions
   {
      private class CustomBuildingBlock : PassiveTransportBuildingBlock { }
      [Observation]
      public void can_add_should_return_true()
      {
         var customBuildingBlock = new CustomBuildingBlock();
         _module.CanAdd(customBuildingBlock).ShouldBeTrue();
      }
   }

   public class When_no_same_type_building_block_exists : concern_for_ModuleExtensions
   {
      [Observation]
      public void can_add_should_return_true() =>
         _module.CanAdd(_moleculeBuildingBlock).ShouldBeTrue();
   }

   public class When_same_type_building_block_already_exists : concern_for_ModuleExtensions
   {
      [Observation]
      public void can_add_should_return_false()
      {
         _module.Add(_moleculeBuildingBlock);
         var anotherMoleculeBuildingBlock = new MoleculeBuildingBlock();
         _module.CanAdd(anotherMoleculeBuildingBlock).ShouldBeFalse();
      }
   }

   public class When_subtype_building_block_already_exists : concern_for_ModuleExtensions
   {
      private class AdvancedMoleculeBuildingBlock : MoleculeBuildingBlock { }
      [Observation]
      public void can_add_should_return_false()
      {
         _module.Add(_moleculeBuildingBlock);
         var anotherMoleculeBuildingBlock = new AdvancedMoleculeBuildingBlock();
         _module.CanAdd(anotherMoleculeBuildingBlock).ShouldBeFalse();
      }
   }

   public class When_different_type_building_block_exists : concern_for_ModuleExtensions
   {

      [Observation]
      public void can_add_should_return_true()
      {
         _module.Add(_moleculeBuildingBlock);
         _module.CanAdd(_passiveTransportBuildingBlock).ShouldBeTrue();
      }
   }
}
