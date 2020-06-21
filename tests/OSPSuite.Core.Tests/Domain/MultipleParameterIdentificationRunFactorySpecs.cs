using System.Collections.Generic;
using System.Linq;
using System.Threading;
using FakeItEasy;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain.ParameterIdentifications;
using OSPSuite.Core.Domain.Services.ParameterIdentifications;

namespace OSPSuite.Core.Domain
{
   public abstract class concern_for_MultipleParameterIdentificationRunFactory : ContextSpecification<MultipleParameterIdentificationRunFactory>
   {
      protected IParameterIdentificationRunInitializerFactory _runInitializerFactory;

      protected override void Context()
      {
         _runInitializerFactory = A.Fake<IParameterIdentificationRunInitializerFactory>(); 
         sut = new MultipleParameterIdentificationRunFactory(_runInitializerFactory);
      }
   }

   public class When_the_mupltiple_parameter_identification_run_mode_creator_is_asked_if_it_can_handle_a_given_parameter_identification_run_mode : concern_for_MultipleParameterIdentificationRunFactory
   {
      [Observation]
      public void should_return_true_for_a_standard_run_mode()
      {
         sut.IsSatisfiedBy(new MultipleParameterIdentificationRunMode()).ShouldBeTrue();
      }

      [Observation]
      public void should_return_false_otherwise()
      {
         sut.IsSatisfiedBy(new CategorialParameterIdentificationRunMode()).ShouldBeFalse();
         sut.IsSatisfiedBy(new StandardParameterIdentificationRunMode()).ShouldBeFalse();
      }
   }

   public class When_the_multiple_parameter_identification_run_mode_creator_is_creating_the_runnable_parameter_identifications : concern_for_MultipleParameterIdentificationRunFactory
   {
      private ParameterIdentification _parameterIdentification;
      private IReadOnlyList<IParameterIdentificationRun> _result;

      protected override void Context()
      {
         base.Context();
         _parameterIdentification = new ParameterIdentification();
         _parameterIdentification.Configuration.RunMode = new MultipleParameterIdentificationRunMode {NumberOfRuns = 3};
      }

      protected override void Because()
      {
         _result = sut.CreateFor(_parameterIdentification, new CancellationToken());
      }

      [Observation]
      public void should_return_only_one_entry_being_a_clone_of_the_parameter_identification()
      {
         _result.Count().ShouldBeEqualTo(3);
      }
   }
}