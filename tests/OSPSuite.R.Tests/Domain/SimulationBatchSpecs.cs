using System;
using System.IO;
using System.Linq;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Extensions;
using OSPSuite.R.Services;
using OSPSuite.SimModel;
using OSPSuite.Utility.Exceptions;
using OSPSuite.Utility.Extensions;

namespace OSPSuite.R.Domain
{
   public abstract class concern_for_SimulationBatch : ContextForIntegration<SimulationBatch>
   {
      private string _simulationFile;
      private ISimulationPersister _simulationPersister;
      protected Simulation _simulation;
      protected ISimulationBatchFactory _simulationBatchFactory;

      public override void GlobalContext()
      {
         base.GlobalContext();
         _simulationFile = HelperForSpecs.DataFile("S1.pkml");
         _simulationPersister = Api.GetSimulationPersister();
         _simulationBatchFactory = Api.GetSimulationBatchFactory();

         _simulation = _simulationPersister.LoadSimulation(_simulationFile);
      }
   }

   public class When_running_a_batch_simulation_with_an_error : concern_for_SimulationBatch
   {
      private SimulationBatchOptions _simulationBatchOptions;
      private SimulationBatchRunValues _simulationBatchRunValues;

      public override void GlobalContext()
      {
         base.GlobalContext();

         // Force an error during simulation run
         _simulation.Settings.Solver.MxStep = 3;
         _simulationBatchOptions = new SimulationBatchOptions
         {
            VariableMolecules = new[]
            {
               new[] {"Organism", "Kidney", "Intracellular", "Caffeine"}.ToPathString()
            },

            VariableParameters = new[]
            {
               new[] {"Organism", "Liver", "Volume"}.ToPathString(),
               new[] {"Organism", "Hematocrit"}.ToPathString(),
            }
         };
         sut = _simulationBatchFactory.Create(_simulation, _simulationBatchOptions);
      }

      protected override void Because()
      {
         _simulationBatchRunValues = new SimulationBatchRunValues
         {
            InitialValues = new[] { 10.0 },
            ParameterValues = new[] { 3.0, 0.53 }
         };
      }

      [Observation]
      public void should_throw_an_exception_during_run()
      {
         The.Action(() => sut.Run(_simulationBatchRunValues)).ShouldThrowAn<OSPSuiteException>();
      }
   }

   public class When_running_a_batch_simulation_run : concern_for_SimulationBatch
   {
      private SimulationBatchOptions _simulationBatchOptions;
      private SimulationResults _results;

      public override void GlobalContext()
      {
         base.GlobalContext();
         _simulationBatchOptions = new SimulationBatchOptions
         {
            VariableMolecules = new[]
            {
               new[] {"Organism", "Kidney", "Intracellular", "Caffeine"}.ToPathString()
            },

            VariableParameters = new[]
            {
               new[] {"Organism", "Liver", "Volume"}.ToPathString(),
               new[] {"Organism", "Hematocrit"}.ToPathString(),
            }
         };
         sut = _simulationBatchFactory.Create(_simulation, _simulationBatchOptions);
      }

      protected override void Because()
      {
         var simulationBatchRunValues = new SimulationBatchRunValues
         {
            InitialValues = new[] {10.0},
            ParameterValues = new[] {3.0, 0.53}
         };
         _results = sut.Run(simulationBatchRunValues);
      }

      [Observation]
      public void should_be_able_to_simulate_the_simulation_for_multiple_runes()
      {
         _results.Count.ShouldBeEqualTo(1);
      }
   }

   public class When_running_a_batch_simulation_run_with_single_values : concern_for_SimulationBatch
   {
      private SimulationBatchOptions _simulationBatchOptions;
      private SimulationResults _results;

      public override void GlobalContext()
      {
         base.GlobalContext();
         _simulationBatchOptions = new SimulationBatchOptions
         {
            VariableMolecule= new[] {"Organism", "Kidney", "Intracellular", "Caffeine"}.ToPathString(),

            VariableParameter = new[] {"Organism", "Liver", "Volume"}.ToPathString(),

            CalculateSensitivity = true
         };
         sut = _simulationBatchFactory.Create(_simulation, _simulationBatchOptions);
      }

      protected override void Because()
      {
         var simulationBatchRunValues = new SimulationBatchRunValues
         {
            InitialValue = 10.0,
            ParameterValue = 3.0
         };
         _results = sut.Run(simulationBatchRunValues);
      }

      [Observation]
      public void should_be_able_to_simulate_the_simulation_for_multiple_runes()
      {
         _results.Count.ShouldBeEqualTo(1);
      }

      [Observation]
      public void should_calculate_sensitivities()
      {
         _results.Each(r => r.Each(x => x.Sensitivities.Values.Each(s =>s.Count().ShouldBeEqualTo(x.Values.Count()))));
      }
   }

   public class When_running_a_batch_simulation_run_with_an_unexpected_number_of_parameters_or_initial_values : concern_for_SimulationBatch
   {
      private SimulationBatchOptions _simulationBatchOptions;

      public override void GlobalContext()
      {
         base.GlobalContext();
         _simulationBatchOptions = new SimulationBatchOptions
         {
            VariableMolecules = new[]
            {
               new[] {"Organism", "Kidney", "Intracellular", "Caffeine"}.ToPathString()
            },

            VariableParameters = new[]
            {
               new[] {"Organism", "Liver", "Volume"}.ToPathString(),
               new[] {"Organism", "Hematocrit"}.ToPathString(),
            }
         };
         sut = _simulationBatchFactory.Create(_simulation, _simulationBatchOptions);
      }

      [Observation]
      public void should_throw_an_error_if_the_number_of_initial_values_is_not_the_one_expected()
      {
         var simulationBatchRunValues = new SimulationBatchRunValues
         {
            InitialValues = new[] {10.0, 20},
            ParameterValues = new[] {3.0, 0.53}
         };
         The.Action(() => sut.Run(simulationBatchRunValues)).ShouldThrowAn<Exception>();
      }

      [Observation]
      public void should_throw_an_error_if_the_number_of_parameters_values_is_not_the_one_expected()
      {
         var simulationBatchRunValues = new SimulationBatchRunValues
         {
            InitialValues = new[] {10.0},
            ParameterValues = new[] {3.0}
         };
         The.Action(() => sut.Run(simulationBatchRunValues)).ShouldThrowAn<Exception>();
      }
   }

   public class When_creating_a_batch_simulation_with_parameter_that_do_not_exist_in_the_simulation : concern_for_SimulationBatch
   {
      private SimulationBatchOptions _simulationBatchOptions;

      public override void GlobalContext()
      {
         base.GlobalContext();
         _simulationBatchOptions = new SimulationBatchOptions
         {
            VariableParameters = new[]
            {
               new[] {"Organism", "NOT THERE", "Volume"}.ToPathString(),
               new[] {"Organism", "Hematocrit"}.ToPathString(),
            }
         };
      }

      [Observation]
      public void should_throw_an_error()
      {
         The.Action(() => _simulationBatchFactory.Create(_simulation, _simulationBatchOptions)).ShouldThrowAn<Exception>();
      }
   }

   public class When_creating_a_batch_simulation_with_molecules_that_do_not_exist_in_the_simulation : concern_for_SimulationBatch
   {
      private SimulationBatchOptions _simulationBatchOptions;

      public override void GlobalContext()
      {
         base.GlobalContext();
         _simulationBatchOptions = new SimulationBatchOptions
         {
            VariableMolecules = new[]
            {
               new[] {"Organism", "Kidney", "Intracellular", "Caffeine"}.ToPathString(),
               new[] {"Organism", "Kidney", "Intracellular", "NOT_THERE"}.ToPathString()
            }
         };
      }

      [Observation]
      public void should_throw_an_error()
      {
         The.Action(() => _simulationBatchFactory.Create(_simulation, _simulationBatchOptions)).ShouldThrowAn<Exception>();
      }
   }

   public class When_exporting_to_cpp_code : concern_for_SimulationBatch
   {
      private SimulationBatchOptions _simulationBatchOptions;
      private string _exportFolder;

      public override void GlobalContext()
      {
         base.GlobalContext();
         _simulationBatchOptions = new SimulationBatchOptions
         {
            VariableMolecules = new[]
            {
               new[] {"Organism", "Kidney", "Intracellular", "Caffeine"}.ToPathString()
            },

            VariableParameters = new[]
            {
               new[] {"Organism", "Liver", "Volume"}.ToPathString(),
               new[] {"Organism", "Hematocrit"}.ToPathString(),
            }
         };
         sut = _simulationBatchFactory.Create(_simulation, _simulationBatchOptions);
         _exportFolder = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
      }

      protected override void Because()
      {
         Directory.CreateDirectory(_exportFolder);
         sut.ExportToCPPCode(_exportFolder, fullMode: false, "TestModel_Values");
      }

      [Observation]
      public void should_export_cpp_code()
      {
         File.Exists(Path.Combine(_exportFolder, "TestModel_Values.cpp")).ShouldBeTrue();
      }
   }
}