using NUnit.Framework;
using OSPSuite.BDDHelper;
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

   [TestFixture]
   public class ModuleNodeExtensionsTests : concern_for_ModuleExtensions
   {

      [Observation]
      public void CanAdd_ShouldReturnTrue_WhenBuildingBlockTypeIsNotInUniqueTypes()
      {
         var customBuildingBlock = new CustomBuildingBlock();

         var result = _module.CanAdd(customBuildingBlock);

         Assert.IsTrue(result);
      }

      [Observation]
      public void CanAdd_ShouldReturnTrue_WhenNoSameTypeBuildingBlockExists()
      {
         var result = _module.CanAdd(_moleculeBuildingBlock);

         Assert.IsTrue(result);
      }

      [Observation]
      public void CanAdd_ShouldReturnFalse_WhenSameTypeBuildingBlockAlreadyExists()
      {
         _module.Add(_moleculeBuildingBlock);

         var anotherMoleculeBuildingBlock = new MoleculeBuildingBlock();
         var result = _module.CanAdd(anotherMoleculeBuildingBlock);

         Assert.IsFalse(result);
      }

      [Observation]
      public void CanAdd_ShouldReturnFalse_WhenSubtypeBuildingBlockAlreadyExists()
      {
         _module.Add(_moleculeBuildingBlock);

         var anotherMoleculeBuildingBlock = new AdvancedMoleculeBuildingBlock(); // Assume this is a subtype
         var result = _module.CanAdd(anotherMoleculeBuildingBlock);

         Assert.IsFalse(result);
      }

      [Test]
      public void CanAdd_ShouldReturnFalse_WhenSupertypeBuildingBlockAlreadyExists()
      {
         _module.Add(_passiveTransportBuildingBlock);

         var baseBuildingBlock = new CustomBuildingBlock(); // Assume this is a supertype
         var result = _module.CanAdd(baseBuildingBlock);

         Assert.IsFalse(result);
      }

      [Observation]
      public void CanAdd_ShouldReturnTrue_WhenDifferentTypeBuildingBlockExists()
      {
         _module.Add(_moleculeBuildingBlock);

         var result = _module.CanAdd(_passiveTransportBuildingBlock);

         Assert.IsTrue(result);
      }

      private class CustomBuildingBlock : PassiveTransportBuildingBlock { }

      private class AdvancedMoleculeBuildingBlock : MoleculeBuildingBlock { }
   }
}
