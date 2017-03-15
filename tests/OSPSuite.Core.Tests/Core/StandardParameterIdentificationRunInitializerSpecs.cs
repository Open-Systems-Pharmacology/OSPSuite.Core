using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using FakeItEasy;
using OSPSuite.Core.Domain.ParameterIdentifications;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Domain.Services.ParameterIdentifications;

namespace OSPSuite.Core
{
   public abstract class concern_for_StandardParameterIdentificationRunInitializer : ContextSpecification<StandardParameterIdentificationRunInitializer>
   {
      protected ICloneManagerForModel _cloneManager;
      private IParameterIdentificationRun _parameterIdentificationRun;

      protected override void Context()
      {
         _cloneManager = A.Fake<ICloneManagerForModel>();
         _parameterIdentificationRun= A.Fake<IParameterIdentificationRun>();  
         sut = new StandardParameterIdentificationRunInitializer(_cloneManager,_parameterIdentificationRun);
      }
   }

   public class When_the_standard_parameter_identification_run_mode_creator_is_initializing_a_parameter_identification_run : concern_for_StandardParameterIdentificationRunInitializer
   {
      private ParameterIdentification _parameterIdentification;
      private ParameterIdentification _cloneParameterIdentification;
      private ParameterIdentification _result;

      protected override void Context()
      {
         base.Context();
         _parameterIdentification = A.Fake<ParameterIdentification>();
         _cloneParameterIdentification = A.Fake<ParameterIdentification>();
         A.CallTo(() => _cloneManager.Clone(_parameterIdentification)).Returns(_cloneParameterIdentification);

         sut.Initialize(_parameterIdentification, 0);
      }

      protected override void Because()
      {
         _result = sut.InitializeRun().Result;
      }

      [Observation]
      public void should_return_only_one_entry_being_a_clone_of_the_parameter_identification()
      {
         _result.ShouldBeEqualTo(_cloneParameterIdentification);
      }
   }
}	