using FakeItEasy;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Mappers;
using OSPSuite.Core.Domain.Services;
using System;

namespace OSPSuite.Core.Mappers
{
   internal abstract class concern_for_ContainerBuilderToContainerMapper : ContextSpecification<IContainerBuilderToContainerMapper>
   {
      protected ICloneManagerForModel _cloneManagerForModel;
      protected IEntityTracker _entityTracker;

      protected override void Context()
      {
         _cloneManagerForModel = A.Fake<ICloneManagerForModel>();
         _entityTracker= A.Fake<IEntityTracker>();
         sut = new ContainerBuilderToContainerMapper(_cloneManagerForModel, _entityTracker);
      }
   }

   internal class When_mapping_a_container_from_a_container_builder : concern_for_ContainerBuilderToContainerMapper
   {
      private IContainer _containerBuilder;
      private IContainer _clonedContainer;
      private SimulationConfiguration _simulationConfiguration;
      private IParameter _parameterBuilder;
      private IParameter _clonedParameter;
      private IContainer _result;
      private SimulationBuilder _simulationBuilder;
      private IReactionMerger _reactionMerger;
      protected override void Context()
      {
         base.Context();
         _reactionMerger = new ReactionMerger();
         _containerBuilder = new Container();
         _parameterBuilder = new Parameter().WithName("toto");
         _containerBuilder.Add(_parameterBuilder);
         _clonedContainer = new Container();
         _clonedParameter = new Parameter().WithName("toto");
         _clonedContainer.Add(_clonedParameter);
         _simulationConfiguration = new SimulationConfiguration();
         _simulationBuilder = new SimulationBuilder(_simulationConfiguration, _reactionMerger);
         A.CallTo(() => _cloneManagerForModel.Clone(_containerBuilder)).Returns(_clonedContainer);
      }

      protected override void Because()
      {
         _result = sut.MapFrom(_containerBuilder, _simulationBuilder);
      }

      [Observation]
      public void should_return_a_clone_from_the_given_container_ready_to_be_used_in_the_model()
      {
         _result.ShouldBeEqualTo(_clonedContainer);
      }

      [Observation]
      public void should_have_referenced_the_parameter_with_the_parameter_builder()
      {
         A.CallTo(() => _entityTracker.Track(_clonedParameter, _parameterBuilder, _simulationBuilder)).MustHaveHappened();
      }
   }
}