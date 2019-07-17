using FakeItEasy;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Presentation.DTO;
using OSPSuite.Utility.Validation;

namespace OSPSuite.Presentation.Presentation
{
   public abstract class concern_for_RenameObjectDTO : ContextSpecification<RenameObjectDTO>
   {
      protected IBuildingBlock _buildingBlock;
      private IBuildingBlock _existingBuildingBlock;

      protected override void Context()
      {
         _buildingBlock = A.Fake<IBuildingBlock>();
         _existingBuildingBlock = A.Fake<IBuildingBlock>();
         _buildingBlock.Name = "TOTO";
         _existingBuildingBlock.Name = "existing";
         sut = new RenameObjectDTO(_buildingBlock.Name);
         sut.AddUsedNames(new[] {_buildingBlock.Name});
      }
   }

   public class when_setting_the_name_for_a_rename_object_base_dto : concern_for_RenameObjectDTO
   {
      [Observation]
      public void the_entity_dto_should_not_be_valid_if_the_name_did_not_change()
      {
         sut.Validate(x => x.Name, _buildingBlock.Name).IsEmpty.ShouldBeFalse();
      }
   }

   public class when_setting_the_name_for_a_rename_object_base_dto_to_the_same_name_with_different_case : concern_for_RenameObjectDTO
   {
      [Observation]
      public void the_entity_dto_should_be_valid()
      {
         sut.Validate(x => x.Name, "Toto").IsEmpty.ShouldBeTrue();
      }
   }

   public class when_setting_the_name_for_a_rename_object_base_dto_to_the_same_name_with_different_case_in_a_cloning_scenarion : concern_for_RenameObjectDTO
   {
      protected override void Context()
      {
         base.Context();
         sut.AllowSameNameAsOriginalInDifferentCase = false;
      }

      [Observation]
      public void the_entity_dto_should_not_be_valid()
      {
         sut.Validate(x => x.Name, "Toto").IsEmpty.ShouldBeFalse();
      }
   }
}