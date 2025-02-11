using FakeItEasy;
using OSPSuite.Assets;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Commands;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Services;

namespace OSPSuite.Core.Domain
{
   public abstract class concern_for_RelatedItemTypeRetriever : ContextSpecification<RelatedItemTypeRetriever>
   {
      private IOSPSuiteExecutionContext _spSuiteExecutionContext;

      protected override void Context()
      {
         _spSuiteExecutionContext = A.Fake<IOSPSuiteExecutionContext>();
         A.CallTo(() => _spSuiteExecutionContext.TypeFor(A<ParameterValuesBuildingBlock>._)).Returns(ObjectTypes.ParameterValuesBuildingBlock);
         A.CallTo(() => _spSuiteExecutionContext.TypeFor(A<InitialConditionsBuildingBlock>._)).Returns(ObjectTypes.InitialConditionsBuildingBlock);
         A.CallTo(() => _spSuiteExecutionContext.TypeFor(A<ObserverBuildingBlock>._)).Returns(ObjectTypes.ObserverBuildingBlock);
         A.CallTo(() => _spSuiteExecutionContext.TypeFor(A<SpatialStructure>._)).Returns(ObjectTypes.SpatialStructure);
         sut = new RelatedItemTypeRetriever(_spSuiteExecutionContext);
      }
   }

   public class When_shortening_types : concern_for_RelatedItemTypeRetriever
   {
      [Observation]
      public void parameter_values_building_block_should_shorten_correctly()
      {
         sut.TypeFor(new ParameterValuesBuildingBlock()).ShouldBeEqualTo("PVs");
      }

      [Observation]
      public void initial_conditions_building_block_should_shorten_correctly()
      {
         sut.TypeFor(new InitialConditionsBuildingBlock()).ShouldBeEqualTo("ICs");
      }

      [Observation]
      public void observer_building_block_should_shorten_correctly()
      {
         sut.TypeFor(new ObserverBuildingBlock()).ShouldBeEqualTo("Observers");
      }

      [Observation]
      public void spatial_structure_building_block_should_shorten_correctly()
      {
         sut.TypeFor(new SpatialStructure()).ShouldBeEqualTo("Spatial Structure");
      }
   }
}
