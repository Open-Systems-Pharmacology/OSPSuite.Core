using System.Collections.Generic;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using FakeItEasy;
using OSPSuite.Utility.Exceptions;
using SimModelNET;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Serialization.SimModel.Services;
using OSPSuite.Engine.Domain;
using ISimulation = SimModelNET.ISimulation;

namespace OSPSuite.Core
{
   public abstract class concern_for_SimModelBatch : ContextSpecification<ISimModelBatch>
   {
      protected IDataFactory _dataFactory;
      protected ISimModelExporter _simModelExporter;

      protected List<string> _variableParameterPaths;
      protected IModelCoreSimulation _modelCoreSimulation;
      protected ISimModelSimulationFactory _simModelSimulationFactory;
      protected ISimulation _simModelSimulation;
      protected string _simModelXmlString;
      protected List<IParameterProperties> _allSimModelParameters;
      protected IParameterProperties _parameterProperties1;
      protected IParameterProperties _parameterProperties2;
      protected IParameterProperties _parameterProperties3;

      protected override void Context()
      {
         _dataFactory = A.Fake<IDataFactory>();
         _simModelExporter = A.Fake<ISimModelExporter>();
         _simModelSimulationFactory = A.Fake<ISimModelSimulationFactory>();
         _simModelSimulation = A.Fake<ISimulation>();

         sut = new SimModelBatch(_simModelExporter, _simModelSimulationFactory, _dataFactory);

         A.CallTo(() => _simModelSimulationFactory.Create()).Returns(_simModelSimulation);
         _modelCoreSimulation = A.Fake<IModelCoreSimulation>();
         _simModelXmlString = "SimModelXml";
         A.CallTo(() => _simModelExporter.Export(_modelCoreSimulation, SimModelExportMode.Optimized)).Returns(_simModelXmlString);

         _parameterProperties1 = A.Fake<IParameterProperties>();
         _parameterProperties2 = A.Fake<IParameterProperties>();
         _parameterProperties3 = A.Fake<IParameterProperties>();

         A.CallTo(() => _parameterProperties1.Path).Returns("ParameterPath1");
         A.CallTo(() => _parameterProperties2.Path).Returns("ParameterPath2");
         A.CallTo(() => _parameterProperties3.Path).Returns("ParameterPath3");

         _allSimModelParameters = new List<IParameterProperties> {_parameterProperties1, _parameterProperties2, _parameterProperties3};
         A.CallTo(() => _simModelSimulation.ParameterProperties).Returns(_allSimModelParameters);

         _variableParameterPaths = new List<string> {_parameterProperties1.Path, _parameterProperties2.Path};
      }
   }

   public class When_initializing_the_sim_model_batch_with_a_simulaiton_and_a_list_of_variable_parameters : concern_for_SimModelBatch
   {
      protected override void Because()
      {
         sut.InitializeWith(_modelCoreSimulation, _variableParameterPaths);
      }

      [Observation]
      public void should_create_a_new_simulation_based_on_the_give_model_core_simulation()
      {
         A.CallTo(() => _simModelSimulation.LoadFromXMLString(_simModelXmlString)).MustHaveHappened();
      }

      [Observation]
      public void should_set_the_parameter_to_be_varied()
      {
         _simModelSimulation.VariableParameters.ShouldOnlyContain(_parameterProperties1, _parameterProperties2);
      }
   }

   public class When_running_the_simulation_defined_in_the_sim_model_batch : concern_for_SimModelBatch
   {
      private SimulationRunResults _simulationResult;
      private DataRepository _result;
      private string _simulationResultsName;

      protected override void Context()
      {
         base.Context();
         _simulationResultsName = "TTT";
         sut.InitializeWith(_modelCoreSimulation, _variableParameterPaths,false, _simulationResultsName);
         sut.UpdateParameterValue(_parameterProperties1.Path, 10);
         sut.UpdateParameterValue(_parameterProperties2.Path, 20);

         _result=new DataRepository("SIM");
         A.CallTo(() => _dataFactory.CreateRepository(_modelCoreSimulation, _simModelSimulation, _simulationResultsName)).Returns(_result);
      }

      protected override void Because()
      {
         _simulationResult = sut.RunSimulation();
      }

      [Observation]
      public void should_update_the_parameter_values()
      {
         _parameterProperties1.Value.ShouldBeEqualTo(10);
         _parameterProperties2.Value.ShouldBeEqualTo(20);
         A.CallTo(() => _simModelSimulation.SetParameterValues(_simModelSimulation.VariableParameters)).MustHaveHappened();
      }

      [Observation]
      public void should_run_the_simulation()
      {
         A.CallTo(() => _simModelSimulation.RunSimulation()).MustHaveHappened();
      }

      [Observation]
      public void should_return_a_simulation_result_containing_the_output_of_the_simulation_run()
      {
         _simulationResult.Success.ShouldBeTrue();
         _simulationResult.Results.ShouldBeEqualTo(_result);
      }
   }

   public class When_running_the_simulation_defined_in_the_sim_model_batch_and_the_run_throws_an_exception : concern_for_SimModelBatch
   {
      private SimulationRunResults _simulationResult;

      protected override void Context()
      {
         base.Context();
         _allSimModelParameters.Clear();
         _variableParameterPaths.Clear();
         sut.InitializeWith(_modelCoreSimulation, _variableParameterPaths);
         A.CallTo(() => _simModelSimulation.RunSimulation()).Throws(new OSPSuiteException("BLA"));
      }

      protected override void Because()
      {
         _simulationResult = sut.RunSimulation();
      }

      [Observation]
      public void should_return_an_unsuccessful_simulation_run_result()
      {
         _simulationResult.Success.ShouldBeFalse();
      }

      [Observation]
      public void should_update_the_error_message()
      {
         _simulationResult.Error.ShouldBeEqualTo("BLA");
      }
   }
}