using System.Collections.Generic;
using System.IO;
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

      protected string[] _variableParameterPaths;
      protected string[] _variableSpeciesPath;
      protected Simulation _simModelSimulation;
      protected IModelCoreSimulation _modelCoreSimulation;
      private SimModelManagerForSpecs _simModelManagerForSpecs;

      public override void GlobalContext()
      {
         base.GlobalContext();

         _modelCoreSimulation = IoC.Resolve<SimulationHelperForSpecs>().CreateSimulation();
         var simModelExporter = IoC.Resolve<ISimModelExporter>();
         var simModelSimulationFactory = A.Fake<ISimModelSimulationFactory>();
         A.CallTo(() => simModelSimulationFactory.Create()).Returns(new Simulation());
         _simModelManagerForSpecs = new SimModelManagerForSpecs(simModelExporter, simModelSimulationFactory);

         _simModelSimulation = _simModelManagerForSpecs.CreateSimulation(_modelCoreSimulation);
         _dataFactory = A.Fake<IDataFactory>();
         _variableParameterPaths = new[]
         {
            new ObjectPath(Constants.ORGANISM, ConstantsForSpecs.BW).PathAsString,
            new ObjectPath(Constants.ORGANISM, ConstantsForSpecs.TableParameter1).PathAsString,
         };

         _variableSpeciesPath = new[]
         {
            new ObjectPath(Constants.ORGANISM, ConstantsForSpecs.ArterialBlood, ConstantsForSpecs.Plasma, "A").PathAsString,
            new ObjectPath(Constants.ORGANISM, ConstantsForSpecs.VenousBlood, ConstantsForSpecs.Plasma, "B").PathAsString,
         };

         sut = new SimModelBatch(simModelExporter, simModelSimulationFactory, _dataFactory);
      }
   }

   public class When_initializing_the_sim_model_batch_with_a_simulation_and_a_list_of_variable_parameters : concern_for_SimModelBatch
   {
      protected override void Because()
      {
         sut.InitializeWith(_modelCoreSimulation, _variableParameterPaths, _variableSpeciesPath);
      }

      [Observation]
      public void should_set_the_parameter_to_be_varied()
      {
         _simModelSimulation.VariableParameters.Count().ShouldBeEqualTo(2);
      }

      [Observation]
      public void should_set_the_species_to_be_varied()
      {
         _simModelSimulation.VariableSpecies.Count().ShouldBeEqualTo(2);
      }
   }

   public class When_running_the_simulation_defined_in_the_sim_model_batch : concern_for_SimModelBatch
   {
      private SimulationRunResults _simulationResult;
      private DataRepository _result;
      private string _simulationResultsName;
      private IEnumerable<ParameterProperties> _variableParameters;
      private IEnumerable<SpeciesProperties> _variableSpecies;

      protected override void Context()
      {
         base.Context();
         _simulationResultsName = "TTT";
         sut.InitializeWith(_modelCoreSimulation, _variableParameterPaths, _variableSpeciesPath, false, _simulationResultsName);

         _variableParameters = _simModelSimulation.VariableParameters;
         sut.UpdateParameterValue(_variableParameterPaths[0], 10);
         sut.UpdateParameterValue(_variableParameterPaths[1], 20);

         _variableSpecies = _simModelSimulation.VariableSpecies;
         sut.UpdateInitialValue(_variableSpeciesPath[0], 5);
         sut.UpdateInitialValue(_variableSpeciesPath[1], 6);

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
      public void should_update_the_initial_values()
      {
         _variableSpecies.ElementAt(0).InitialValue.ShouldBeEqualTo(5);
         _variableSpecies.ElementAt(1).InitialValue.ShouldBeEqualTo(6);
      }

      [Observation]
      public void should_return_a_simulation_result_containing_the_output_of_the_simulation_run()
      {
         _simulationResult.Success.ShouldBeTrue(_simulationResult.Error);
         _simulationResult.Results.ShouldBeEqualTo(_result);
      }
   }

   public class When_exporting_the_sim_model_simulation_to_c_plusplus_code : concern_for_SimModelBatch
   {
      private string _exportFolder;

      protected override void Context()
      {
         base.Context();
         sut.KeepXMLNodeInSimModelSimulation = true;
         sut.InitializeWith(_modelCoreSimulation, _variableParameterPaths, _variableSpeciesPath, false);
         _exportFolder = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
      }

      protected override void Because()
      {
         Directory.CreateDirectory(_exportFolder);
         sut.ExportToCPPCode(_exportFolder, CodeExportMode.Values);
      }

      [Observation]
      public void should_export_cpp_code()
      {
         File.Exists(Path.Combine(_exportFolder, "Standard.cpp")).ShouldBeTrue();
      }
   }
}