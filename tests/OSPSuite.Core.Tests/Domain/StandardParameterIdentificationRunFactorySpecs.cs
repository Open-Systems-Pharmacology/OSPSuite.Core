using FakeItEasy;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain.ParameterIdentifications;
using OSPSuite.Core.Domain.Services.ParameterIdentifications;

namespace OSPSuite.Core.Domain
{
   public abstract class concern_for_StandardParameterIdentificationRunFactory : ContextSpecification<StandardParameterIdentificationRunFactory>
   {
      protected IParameterIdentificationRunInitializerFactory _runInitializerFactory;

      protected override void Context()
      {
         _runInitializerFactory = A.Fake<IParameterIdentificationRunInitializerFactory>();
         sut = new StandardParameterIdentificationRunFactory(_runInitializerFactory);
      }
   }

   public class When_the_standard_parameter_identification_run_mode_creator_is_asked_if_it_can_handle_a_given_parameter_identification_run_mode : concern_for_StandardParameterIdentificationRunFactory
   {
      [Observation]
      public void should_return_true_for_a_standard_run_mode()
      {
         sut.IsSatisfiedBy(new StandardParameterIdentificationRunMode()).ShouldBeTrue();
      }

      [Observation]
      public void should_return_false_otherwise()
      {
         sut.IsSatisfiedBy(new CategorialParameterIdentificationRunMode()).ShouldBeFalse();
         sut.IsSatisfiedBy(new MultipleParameterIdentificationRunMode()).ShouldBeFalse();
      }
   }
}