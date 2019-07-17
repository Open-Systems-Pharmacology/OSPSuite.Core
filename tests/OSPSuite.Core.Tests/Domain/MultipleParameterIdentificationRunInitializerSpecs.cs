using FakeItEasy;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain.ParameterIdentifications;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Domain.Services.ParameterIdentifications;
using OSPSuite.Core.Maths.Random;
using OSPSuite.Helpers;

namespace OSPSuite.Core.Domain
{
   public abstract class concern_for_MultipleParameterIdentificationRunInitializer : ContextSpecification<MultipleParameterIdentificationRunInitializer>
   {
      protected ICloneManagerForModel _cloneManager;
      protected IParameterIdentificationRun _parameterIdentificationRun;

      protected override void Context()
      {
         _cloneManager = A.Fake<ICloneManagerForModel>();
         _parameterIdentificationRun = A.Fake<IParameterIdentificationRun>();
         sut = new MultipleParameterIdentificationRunInitializer(_cloneManager, _parameterIdentificationRun);
      }
   }

   public class When_the_multiple_parameter_identification_run_mode_creator_is_initializing_a_parameter_identification_run : concern_for_MultipleParameterIdentificationRunInitializer
   {
      private ParameterIdentification _parameterIdentification;
      private ParameterIdentification _result;

      protected override void Context()
      {
         base.Context();
         _parameterIdentification = new ParameterIdentification();
         _parameterIdentification.Configuration.RunMode = new MultipleParameterIdentificationRunMode {NumberOfRuns = 3};
         A.CallTo(() => _cloneManager.Clone(_parameterIdentification)).ReturnsLazily(x =>
         {
            var clone = new ParameterIdentification();
            var ip = DomainHelperForSpecs.IdentificationParameter(min: 10, max: 20, startValue: 15).WithName("PARA1");
            clone.AddIdentificationParameter(ip);

            var fixedIp = DomainHelperForSpecs.IdentificationParameter(min: 20, max: 40, startValue: 30, isFixed: true).WithName("PARA2");
            clone.AddIdentificationParameter(fixedIp);
            return clone;
         });

         sut.Initialize(_parameterIdentification, 2, new RandomGenerator());
      }

      protected override void Because()
      {
         _result = sut.InitializeRun().Result;
      }

      [Observation]
      public void should_have_randomized_the_start_values_of_variable_parameters()
      {
         var ip = _result.AllIdentificationParameters[0];
         ip.StartValueParameter.Value.ShouldBeGreaterThanOrEqualTo(10);
         ip.StartValueParameter.Value.ShouldBeSmallerThanOrEqualTo(20);
      }

      [Observation]
      public void should_not_have_randomized_the_start_values_of_fixed_paramters()
      {
         var ip = _result.AllIdentificationParameters[1];
         ip.StartValue.ShouldBeEqualTo(30);
      }
   }
}