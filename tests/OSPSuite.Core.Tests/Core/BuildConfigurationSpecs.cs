using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain.Builder;

namespace OSPSuite.Core
{
   public abstract class concern_for_BuildConfiguration : ContextSpecification<IBuildConfiguration>
   {
      protected override void Context()
      {
         sut = new BuildConfiguration();
      }
   }

   public class When_returning_all_building_blocks_defined_in_a_build_configuration : concern_for_BuildConfiguration
   {
      protected override void Because()
      {
         sut.ParameterStartValues=new ParameterStartValuesBuildingBlock();
         sut.SpatialStructure=new SpatialStructure();
      }

      [Observation]
      public void should_not_return_the_undefined_building_blocks()
      {
         sut.AllBuildingBlocks.ShouldOnlyContain(sut.ParameterStartValues,sut.SpatialStructure);   
      }
   }
}	