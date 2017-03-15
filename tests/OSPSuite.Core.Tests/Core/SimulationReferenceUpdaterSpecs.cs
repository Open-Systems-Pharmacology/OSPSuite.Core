using System.Collections.Generic;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using FakeItEasy;
using OSPSuite.Core.Commands;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.ParameterIdentifications;
using OSPSuite.Core.Domain.SensitivityAnalyses;
using OSPSuite.Core.Domain.Services;

namespace OSPSuite.Core
{
   public class concern_for_SimulationReferenceUpdater : ContextSpecification<SimulationReferenceUpdater>
   {
      protected IOSPSuiteExecutionContext _executionContext;

      protected override void Context()
      {
         _executionContext = A.Fake<IOSPSuiteExecutionContext>();
         sut = new SimulationReferenceUpdater(_executionContext);
      }
   }

   public class When_removing_references_to_a_simulation : concern_for_SimulationReferenceUpdater
   {
      private ISimulation _simulation;
      private IProject _project;
      private ParameterIdentification _parameterIdentification;
      private OutputMapping _outputMapping;
      private IdentificationParameter _identificationParameter;
      private ParameterSelection _linkedParameter;
      private ParameterSelection _secondLinkedParameter;
      private ISimulation _secondSimulation;
      private OutputMapping _secondOutputMapping;
      private SensitivityAnalysis _sensitivityAnalysis;

      protected override void Context()
      {
         base.Context();
         _project = A.Fake<IProject>();
         _parameterIdentification = new ParameterIdentification();

         A.CallTo(() => _executionContext.Project).Returns(_project);
         A.CallTo(() => _project.AllParameterIdentifications).Returns(new List<ParameterIdentification> { _parameterIdentification });
         
         _simulation = A.Fake<ISimulation>();

         _sensitivityAnalysis = new SensitivityAnalysis { Simulation = _simulation, IsLoaded = true};
         _sensitivityAnalysis.AddSensitivityParameter(new SensitivityParameter());
         A.CallTo(() => _project.AllSensitivityAnalyses).Returns(new List<SensitivityAnalysis> { _sensitivityAnalysis });
         _secondSimulation = A.Fake<ISimulation>();
         _parameterIdentification.AddSimulation(_simulation);
         _parameterIdentification.AddSimulation(_secondSimulation);

         _outputMapping = new OutputMapping { OutputSelection = new ParameterSelection(_simulation, "a|path") };
         _secondOutputMapping = new OutputMapping { OutputSelection = new ParameterSelection(_secondSimulation, "a|path") };
         _identificationParameter = new IdentificationParameter();
         _linkedParameter = new ParameterSelection(_simulation, "a|path");
         _secondLinkedParameter = new ParameterSelection(_secondSimulation, "a|path");

         _identificationParameter.AddLinkedParameter(_linkedParameter);
         _identificationParameter.AddLinkedParameter(_secondLinkedParameter);
         _parameterIdentification.AddIdentificationParameter(_identificationParameter);
         _parameterIdentification.AddOutputMapping(_outputMapping);
         _parameterIdentification.AddOutputMapping(_secondOutputMapping);
         _parameterIdentification.IsLoaded = true;
      }

      protected override void Because()
      {
         sut.RemoveSimulationFromParameterIdentificationsAndSensitivityAnalyses(_simulation);
      }

      [Observation]
      public void the_sensitivity_analsysis_should_have_no_parameters()
      {
         _sensitivityAnalysis.AllSensitivityParameters.ShouldBeEmpty();
      }

      [Observation]
      public void the_sensitivity_analysis_should_not_reference_the_simulation()
      {
         _sensitivityAnalysis.Simulation.ShouldNotBeEqualTo(_simulation);
      }

      [Observation]
      public void the_parameter_identification_should_not_reference_the_simulation()
      {
         _parameterIdentification.AllSimulations.ShouldNotContain(_simulation);
      }

      [Observation]
      public void the_second_identification_parameter_should_remain()
      {
         _identificationParameter.AllLinkedParameters.ShouldContain(_secondLinkedParameter);
      }

      [Observation]
      public void the_second_output_mapping_should_remain()
      {
         _parameterIdentification.AllOutputMappings.ShouldContain(_secondOutputMapping);
      }

      [Observation]
      public void the_identification_parameter_should_be_removed()
      {
         _identificationParameter.AllLinkedParameters.ShouldNotContain(_linkedParameter);
      }

      [Observation]
      public void the_output_mapping_should_be_removed()
      {
         _parameterIdentification.AllOutputMappings.ShouldNotContain(_outputMapping);
      }
   }
}