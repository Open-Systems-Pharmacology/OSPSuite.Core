using System.Collections.Generic;
using System.Linq;
using FakeItEasy;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Serialization.SimModel.Services;
using OSPSuite.Helpers;
using OSPSuite.SimModel;
using OSPSuite.Utility.Container;

namespace OSPSuite.Core
{
   public abstract class concern_for_SimModelBatch : ContextForIntegration<ISimModelBatch>
   {
      protected IDataFactory _dataFactory;
      protected ISimModelExporter _simModelExporter;

      protected string[] _variableParameterPaths;
      protected ISimModelSimulationFactory _simModelSimulationFactory;
      protected Simulation _simModelSimulation;
      protected string _simModelXmlString;
      private IObjectPathFactory _objectPathFactory;
      protected IModelCoreSimulation _modelCoreSimulation;
      private SimModelManagerForSpecs _simModelManagerForSpecs;
      protected IEnumerable<ParameterProperties> _variableParameters;

      public override void GlobalContext()
      {
         base.GlobalContext();

         _objectPathFactory = IoC.Resolve<IObjectPathFactory>();
         _modelCoreSimulation = IoC.Resolve<SimulationHelperForSpecs>().CreateSimulation();
         var simModelExporter = IoC.Resolve<ISimModelExporter>();
         var simModelSimulationFactory = A.Fake<ISimModelSimulationFactory>();
         A.CallTo(() => simModelSimulationFactory.Create()).Returns(new Simulation());
         _simModelManagerForSpecs = new SimModelManagerForSpecs(simModelExporter, simModelSimulationFactory);

         _simModelSimulation = _simModelManagerForSpecs.CreateSimulation(_modelCoreSimulation);
         _dataFactory = A.Fake<IDataFactory>();
         _variableParameterPaths = new[]
         {
            _objectPathFactory.CreateObjectPathFrom(ConstantsForSpecs.Organism, ConstantsForSpecs.BW).PathAsString,
            _objectPathFactory.CreateObjectPathFrom(ConstantsForSpecs.Organism, ConstantsForSpecs.TableParameter1).PathAsString,
         };


         sut = new SimModelBatch(simModelExporter, simModelSimulationFactory, _dataFactory);
      }
   }

   public class When_initializing_the_sim_model_batch_with_a_simulation_and_a_list_of_variable_parameters : concern_for_SimModelBatch
   {
      protected override void Because()
      {
         sut.InitializeWith(_modelCoreSimulation, _variableParameterPaths);
      }

      [Observation]
      public void should_set_the_parameter_to_be_varied()
      {
         _simModelSimulation.VariableParameters.Count().ShouldBeEqualTo(2);
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
         sut.InitializeWith(_modelCoreSimulation, _variableParameterPaths, false, _simulationResultsName);

         _variableParameters = _simModelSimulation.VariableParameters;
         sut.UpdateParameterValue(_variableParameterPaths[0], 10);
         sut.UpdateParameterValue(_variableParameterPaths[1], 20);

         _result = new DataRepository("SIM");
         A.CallTo(() => _dataFactory.CreateRepository(_modelCoreSimulation, _simModelSimulation, _simulationResultsName)).Returns(_result);
      }

      protected override void Because()
      {
         _simulationResult = sut.RunSimulation();
      }

      [Observation]
      public void should_update_the_parameter_values()
      {
         _variableParameters.ElementAt(0).Value.ShouldBeEqualTo(10);
         _variableParameters.ElementAt(1).Value.ShouldBeEqualTo(20);
      }

      [Observation]
      public void should_return_a_simulation_result_containing_the_output_of_the_simulation_run()
      {
         _simulationResult.Success.ShouldBeTrue();
         _simulationResult.Results.ShouldBeEqualTo(_result);
      }
   }
}