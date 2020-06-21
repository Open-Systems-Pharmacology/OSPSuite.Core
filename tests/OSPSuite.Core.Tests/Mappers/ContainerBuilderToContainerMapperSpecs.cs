using FakeItEasy;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Mappers;
using OSPSuite.Core.Domain.Services;

namespace OSPSuite.Core.Mappers
{
   public abstract class concern_for_container_builder_to_container : ContextSpecification<IContainerBuilderToContainerMapper>
   {
      protected ICloneManagerForModel _cloneManagerForModel;

      protected override void Context()
      {
         _cloneManagerForModel = A.Fake<ICloneManagerForModel>();
         sut = new ContainerBuilderToContainerMapper(_cloneManagerForModel);
      }
   }

   
   public class When_mapping_a_container_from_a_container_builder : concern_for_container_builder_to_container
   {
      private IContainer _containerBuilder;
      private IContainer _clonedContainer;
      private IBuildConfiguration _buildConfiguration;
      private IParameter _parameterBuilder;
      private IParameter _clonedParameter;
      private IContainer _result;

      protected override void Context()
      {
         base.Context();
         _containerBuilder = new Container();
         _parameterBuilder = new Parameter().WithName("toto");
         _containerBuilder.Add(_parameterBuilder);
         _clonedContainer = new Container();
         _clonedParameter = new Parameter().WithName("toto");
         _clonedContainer.Add(_clonedParameter);
         _buildConfiguration = A.Fake<IBuildConfiguration>();
         A.CallTo(() => _cloneManagerForModel.Clone(_containerBuilder)).Returns(_clonedContainer);
      }

      protected override void Because()
      {
         _result = sut.MapFrom(_containerBuilder, _buildConfiguration);
      }

      [Observation]
      public void should_return_a_clone_from_the_given_container_ready_to_be_used_in_the_model()
      {
         _result.ShouldBeEqualTo(_clonedContainer);
      }

      [Observation]
      public void should_have_referenced_the_parameter_with_the_parmaeter_builder()
      {
         A.CallTo(() => _buildConfiguration.AddBuilderReference(_clonedParameter,_parameterBuilder)).MustHaveHappened();
      }
   }
}	