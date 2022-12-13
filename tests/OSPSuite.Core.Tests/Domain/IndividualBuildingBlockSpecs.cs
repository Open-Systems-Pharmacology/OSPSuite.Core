using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain.Builder;

namespace OSPSuite.Core.Domain
{
   public class concern_for_IndividualBuildingBlock : ContextSpecification<IndividualBuildingBlock>
   {
      protected override void Context()
      {
         sut = new IndividualBuildingBlock
         {
            OriginData = new CoreOriginData()
         };
      }
   }

   public class When_the_data_parameters_are_not_set : concern_for_IndividualBuildingBlock
   {
      [Observation]
      public void the_default_data_should_be_zero()
      {
         sut.Species.ShouldBeNullOrEmpty();
         sut.Age.ShouldBeEqualTo(0);
         sut.Comment.ShouldBeNullOrEmpty();
         sut.Height.ShouldBeEqualTo(0);
         sut.Weight.ShouldBeEqualTo(0);
         sut.ValueOrigin.ShouldNotBeNull();
      }
   }

   public class When_updating_properties_of_a_building_block : concern_for_IndividualBuildingBlock
   {
      private IndividualBuildingBlock _sourceBB;

      protected override void Context()
      {
         base.Context();
         _sourceBB = new IndividualBuildingBlock
         {
            OriginData = new CoreOriginData
            {
               Species = "species",
               Age = new OriginDataParameter(4, "unit", "age"),
               BMI = new OriginDataParameter(3),
               Comment = "a comment",
               Height = new OriginDataParameter(2),
               Weight = new OriginDataParameter(22),
               ValueOrigin = { Description = "the new origin" }
            }
         };
      }

      protected override void Because()
      {
         sut.UpdatePropertiesFrom(_sourceBB, null);
      }

      [Observation]
      public void the_updated_building_block_properties_should_match()
      {
         sut.Species.ShouldBeEqualTo(_sourceBB.Species);
         sut.Age.ShouldBeEqualTo(_sourceBB.Age);
         sut.Comment.ShouldBeEqualTo(_sourceBB.Comment);
         sut.Height.ShouldBeEqualTo(_sourceBB.Height);
         sut.Weight.ShouldBeEqualTo(_sourceBB.Weight);
         sut.ValueOrigin.Description.ShouldBeEqualTo(_sourceBB.ValueOrigin.Description);
      }
   }
}