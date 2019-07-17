using FakeItEasy;
using NUnit.Framework;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain.ParameterIdentifications;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Utility.Extensions;

namespace OSPSuite.Core.Domain
{
   public abstract class concern_for_ParameterIdentificationConfiguration : ContextSpecification<ParameterIdentificationConfiguration>
   {
      protected override void Context()
      {
         sut = new ParameterIdentificationConfiguration();
      }
   }

   public class When_updating_the_properties_from_a_source_parameter_identification_configuration : concern_for_ParameterIdentificationConfiguration
   {
      private ICloneManager _cloneManager;
      private ParameterIdentificationConfiguration _sourceParameterIdentificationConfiguration;

      protected override void Context()
      {
         base.Context();
         _cloneManager = A.Fake<ICloneManager>();
         _sourceParameterIdentificationConfiguration = new ParameterIdentificationConfiguration
         {
            LLOQMode = LLOQModes.SimulationOutputAsObservedDataLLOQ,
            RemoveLLOQMode = RemoveLLOQModes.NoTrailing,
            AlgorithmProperties = new OptimizationAlgorithmProperties("toto"),
            RunMode = new MultipleParameterIdentificationRunMode {NumberOfRuns = 10}
         };
      }

      protected override void Because()
      {
         sut.UpdatePropertiesFrom(_sourceParameterIdentificationConfiguration, _cloneManager);
      }

      [Observation]
      public void should_have_updated_all_properties()
      {
         sut.LLOQMode.ShouldBeEqualTo(_sourceParameterIdentificationConfiguration.LLOQMode);
         sut.RemoveLLOQMode.ShouldBeEqualTo(_sourceParameterIdentificationConfiguration.RemoveLLOQMode);
      }

      [Observation]
      public void should_have_created_a_clone_of_the_algorithm_used()
      {
         sut.AlgorithmProperties.Name.ShouldBeEqualTo(_sourceParameterIdentificationConfiguration.AlgorithmProperties.Name);
         Assert.AreNotSame(sut.AlgorithmProperties, _sourceParameterIdentificationConfiguration.AlgorithmProperties);
      }

      [Observation]
      public void should_have_created_a_clone_of_the_option_used()
      {
         sut.RunMode.ShouldNotBeEqualTo(_sourceParameterIdentificationConfiguration.RunMode);
         sut.RunMode.ShouldBeAnInstanceOf<MultipleParameterIdentificationRunMode>();
         sut.RunMode.DowncastTo<MultipleParameterIdentificationRunMode>().NumberOfRuns.ShouldBeEqualTo(10);
      }
   }

   public class When_creating_a_new_parameter_identificaiton_configuration : concern_for_ParameterIdentificationConfiguration
   {
      [Observation]
      public void should_use_observed_data_and_simulation_for_lloq_mode()
      {
         sut.LLOQMode.ShouldBeEqualTo(LLOQModes.SimulationOutputAsObservedDataLLOQ);
      }

      [Observation]
      public void should_use_never_for_remove_lloq_mode()
      {
         sut.RemoveLLOQMode.ShouldBeEqualTo(RemoveLLOQModes.Never);
      }
   }

   public class When_checking_if_the_algorithm_of_a_parameter_identification_configuration_is_defined : concern_for_ParameterIdentificationConfiguration
   {
      [Observation]
      public void should_return_true_if_the_algorithm_properties_are_defined()
      {
         sut.AlgorithmProperties = new OptimizationAlgorithmProperties("AA");
         sut.AlgorithmIsDefined.ShouldBeTrue();
      }

      [Observation]
      public void should_return_false_otherwise()
      {
         sut.AlgorithmProperties = null;
         sut.AlgorithmIsDefined.ShouldBeFalse();
      }
   }
}