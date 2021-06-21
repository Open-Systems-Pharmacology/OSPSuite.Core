using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using FakeItEasy;
using NUnit.Framework;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Serialization.SimModel.Services;
using OSPSuite.Helpers;
using OSPSuite.SimModel;
using OSPSuite.Utility.Container;
using OSPSuite.Utility.Exceptions;

namespace OSPSuite.Core
{
   public abstract class concern_for_SimModelBatch : ContextForIntegration<ISimModelBatch>
   {
      protected IDataFactory _dataFactory;
      protected ISimModelExporter _simModelExporter;

      protected string[] _variableParameterPaths;
      protected string[]_variableSpeciesPath;

      protected ISimModelSimulationFactory _simModelSimulationFactory;
      protected Simulation _simModelSimulation;
      protected string _simModelXmlString;
      private IObjectPathFactory _objectPathFactory;
      protected IModelCoreSimulation _modelCoreSimulation;
      private SimModelManagerForSpecs _simModelManagerForSpecs;

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

         _variableSpeciesPath = new []
         {
            _objectPathFactory.CreateObjectPathFrom(ConstantsForSpecs.Organism, ConstantsForSpecs.ArterialBlood, ConstantsForSpecs.Plasma, "A").PathAsString,
            _objectPathFactory.CreateObjectPathFrom(ConstantsForSpecs.Organism, ConstantsForSpecs.VenousBlood, ConstantsForSpecs.Plasma, "B").PathAsString,
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
         sut.InitializeWith(_modelCoreSimulation, _variableParameterPaths,_variableSpeciesPath,  false, _simulationResultsName);

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
         sut.InitializeWith(_modelCoreSimulation, _variableParameterPaths, _variableSpeciesPath, false);
         _exportFolder = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
      }

      protected override void Because()
      {
         //Directory.CreateDirectory(_exportFolder);
         //try
         //{
         //   Debug.WriteLine("Debug: Before export");
         //   sut.ExportToCPPCode(_exportFolder, CodeExportMode.Values);
         //   Debug.WriteLine("Debug: After export");
         //}
         //catch (OSPSuiteException ex)
         //{
         //   Debug.WriteLine(ex.ToString());
         //}
         //catch (Exception e)
         //{
         //   Debug.WriteLine(e);
         //}
      }

      [Observation]
      public void should_export_cpp_code()
      {
         Directory.CreateDirectory(_exportFolder);
         try
         {
            Console.SetOut(TestContext.Progress);
            Debug.WriteLine("Debug: Before export");
            Console.WriteLine("Console: Before export");
            sut.ExportToCPPCode(_exportFolder, CodeExportMode.Values);
            Debug.WriteLine("Debug: After export");
         }
         catch (OSPSuiteException ex)
         {
            Debug.WriteLine(ex.ToString());
         }
         catch (Exception e)
         {
            Debug.WriteLine(e);
         }
      }

   }

}