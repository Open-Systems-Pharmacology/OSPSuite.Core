using System;
using System.Collections.Generic;
using System.Linq;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Extensions;
using OSPSuite.R.Domain;
using OSPSuite.Utility.Extensions;
using SimulationRunOptions = OSPSuite.R.Domain.SimulationRunOptions;

namespace OSPSuite.R.Services
{
   public abstract class concern_for_ConcurrentSimulationRunner : ContextForIntegration<IConcurrentSimulationRunner>
   {
      protected ISimulationPersister _simulationPersister;
      protected IConcurrencyManager _concurrencyManager;
      protected ICoreUserSettings _coreUserSettings;
      protected ISimulationPersistableUpdater _simulationPersistableUpdater;

      public override void GlobalContext()
      {
         base.GlobalContext();
         _simulationPersister = Api.GetSimulationPersister();
         _concurrencyManager = Api.Container.Resolve<IConcurrencyManager>();
         _simulationPersistableUpdater = Api.Container.Resolve<ISimulationPersistableUpdater>();
         sut = new ConcurrentSimulationRunner(_concurrencyManager);
      }
   }

   public class When_using_time_points_instead_of_intervals : concern_for_ConcurrentSimulationRunner
   {
      private Simulation _sim;
      private ConcurrentRunSimulationBatch _concurrentRunSimulationBatch;
      private ConcurrencyManagerResult<SimulationResults>[] _results;

      protected override void Context()
      {
         base.Context();
         _sim = _simulationPersister.LoadSimulation(HelperForSpecs.DataFile("Aciclovir.pkml"));
         _sim.Settings.OutputSchema.Clear();
         _sim.Settings.OutputSchema.AddTimePoints(new[] { 1d, 2d, 3d });

         _concurrentRunSimulationBatch = new ConcurrentRunSimulationBatch(_sim, new SimulationBatchOptions { VariableParameters = new[] { "Aciclovir|Lipophilicity" } });
         _concurrentRunSimulationBatch.AddSimulationBatchRunValues(new SimulationBatchRunValues { ParameterValues = new[] { -0.097 } });
         sut.AddSimulationBatch(_concurrentRunSimulationBatch);
      }

      protected override void Because()
      {
         _results = sut.RunConcurrently();
      }

      [Observation]
      public void the_results_should_contain_run_results()
      {
         _results.First().ErrorMessage.ShouldBeEmpty();
      }
   }

   public class When_running_simulations_that_fail_sometimes : concern_for_ConcurrentSimulationRunner
   {
      private ConcurrencyManagerResult<SimulationResults>[] _results;
      private ConcurrentRunSimulationBatch _concurrentRunSimulationBatch;

      protected override void Context()
      {
         base.Context();
         sut.SimulationRunOptions = new SimulationRunOptions { NumberOfCoresToUse = 19 };
         var modelCoreSimulation = _simulationPersister.LoadSimulation(HelperForSpecs.DataFile("Aciclovir.pkml"));

         _concurrentRunSimulationBatch = new ConcurrentRunSimulationBatch(modelCoreSimulation, new SimulationBatchOptions { VariableParameters = new[] { "Aciclovir|Lipophilicity" } });
         _concurrentRunSimulationBatch.AddSimulationBatchRunValues(new SimulationBatchRunValues { ParameterValues = new[] { 0.1 } });
         _concurrentRunSimulationBatch.AddSimulationBatchRunValues(new SimulationBatchRunValues { ParameterValues = new[] { 0.1 } });
         _concurrentRunSimulationBatch.AddSimulationBatchRunValues(new SimulationBatchRunValues { ParameterValues = new[] { 0.1 } });
         _concurrentRunSimulationBatch.AddSimulationBatchRunValues(new SimulationBatchRunValues { ParameterValues = new[] { 0.1 } });
         _concurrentRunSimulationBatch.AddSimulationBatchRunValues(new SimulationBatchRunValues { ParameterValues = new[] { 0.1 } });
         _concurrentRunSimulationBatch.AddSimulationBatchRunValues(new SimulationBatchRunValues { ParameterValues = new[] { 0.1 } });
         _concurrentRunSimulationBatch.AddSimulationBatchRunValues(new SimulationBatchRunValues { ParameterValues = new[] { 0.1 } });
         _concurrentRunSimulationBatch.AddSimulationBatchRunValues(new SimulationBatchRunValues { ParameterValues = new[] { 0.1 } });
         _concurrentRunSimulationBatch.AddSimulationBatchRunValues(new SimulationBatchRunValues { ParameterValues = new[] { 0.1 } });
         _concurrentRunSimulationBatch.AddSimulationBatchRunValues(new SimulationBatchRunValues { ParameterValues = new[] { 0.1 } });
         _concurrentRunSimulationBatch.AddSimulationBatchRunValues(new SimulationBatchRunValues { ParameterValues = new[] { 0.1 } });
         _concurrentRunSimulationBatch.AddSimulationBatchRunValues(new SimulationBatchRunValues { ParameterValues = new[] { 0.1 } });
         _concurrentRunSimulationBatch.AddSimulationBatchRunValues(new SimulationBatchRunValues { ParameterValues = new[] { 0.1 } });
         _concurrentRunSimulationBatch.AddSimulationBatchRunValues(new SimulationBatchRunValues { ParameterValues = new[] { 0.1 } });
         _concurrentRunSimulationBatch.AddSimulationBatchRunValues(new SimulationBatchRunValues { ParameterValues = new[] { 0.1 } });
         _concurrentRunSimulationBatch.AddSimulationBatchRunValues(new SimulationBatchRunValues { ParameterValues = new[] { 0.1 } });
         _concurrentRunSimulationBatch.AddSimulationBatchRunValues(new SimulationBatchRunValues { ParameterValues = new[] { 0.1 } });
         _concurrentRunSimulationBatch.AddSimulationBatchRunValues(new SimulationBatchRunValues { ParameterValues = new[] { 0.1 } });
         _concurrentRunSimulationBatch.AddSimulationBatchRunValues(new SimulationBatchRunValues { ParameterValues = new[] { 0.1 } });
         _concurrentRunSimulationBatch.AddSimulationBatchRunValues(new SimulationBatchRunValues { ParameterValues = new[] { 0.1 } });
         _concurrentRunSimulationBatch.AddSimulationBatchRunValues(new SimulationBatchRunValues { ParameterValues = new[] { 0.1 } });
         _concurrentRunSimulationBatch.AddSimulationBatchRunValues(new SimulationBatchRunValues { ParameterValues = new[] { 0.1 } });
         _concurrentRunSimulationBatch.AddSimulationBatchRunValues(new SimulationBatchRunValues { ParameterValues = new[] { 0.1 } });
         _concurrentRunSimulationBatch.AddSimulationBatchRunValues(new SimulationBatchRunValues { ParameterValues = new[] { 0.1 } });
         _concurrentRunSimulationBatch.AddSimulationBatchRunValues(new SimulationBatchRunValues { ParameterValues = new[] { 0.1 } });
         _concurrentRunSimulationBatch.AddSimulationBatchRunValues(new SimulationBatchRunValues { ParameterValues = new[] { 0.1 } });
         _concurrentRunSimulationBatch.AddSimulationBatchRunValues(new SimulationBatchRunValues { ParameterValues = new[] { 0.1 } });
         _concurrentRunSimulationBatch.AddSimulationBatchRunValues(new SimulationBatchRunValues { ParameterValues = new[] { 0.1 } });
         _concurrentRunSimulationBatch.AddSimulationBatchRunValues(new SimulationBatchRunValues { ParameterValues = new[] { 0.1 } });


         sut.AddSimulationBatch(_concurrentRunSimulationBatch);
      }

      protected override void Because()
      {
         _results = sut.RunConcurrently();
      }

      [Observation]
      public void the_test_might_not_always_pass()
      {
         var individualResults = _results.Where(x => x.Succeeded).SelectMany(x => x.Result.AllIndividualResults).ToList();

         var golden = individualResults.First().AllValues.First().Values;

         individualResults.All(x => golden.SequenceEqual(x.AllValues.First().Values)).ShouldBeTrue();
         _results.Count(x => x.Succeeded == false).ShouldBeEqualTo(0);
      }
   }

   public class When_running_simulations_concurrently : concern_for_ConcurrentSimulationRunner
   {
      private ConcurrencyManagerResult<SimulationResults>[] _results;

      protected override void Context()
      {
         base.Context();

         sut.AddSimulation(_simulationPersister.LoadSimulation(HelperForSpecs.DataFile("S1.pkml")));
         sut.AddSimulation(_simulationPersister.LoadSimulation(HelperForSpecs.DataFile("simple.pkml")));
         sut.AddSimulation(_simulationPersister.LoadSimulation(HelperForSpecs.DataFile("simple.pkml")));
         sut.AddSimulation(_simulationPersister.LoadSimulation(HelperForSpecs.DataFile("multiple_dosing.pkml")));
      }

      protected override void Because()
      {
         _results = sut.RunConcurrently();
      }

      [Observation]
      public void should_run_the_simulations()
      {
         _results.ShouldNotBeNull();
         _results.All(r => r.Result.ElementAt(0).AllValues.SelectMany(v => v.Values).Any()).ShouldBeTrue();
      }
   }

   public class When_applying_start_values_for_all_molecule_paths : concern_for_ConcurrentSimulationRunner
   {
      private Simulation _simulation;
      private IContainerTask _containerTask;
      private string[] _allMoleculePaths;
      private IEnumerable<double> _moleculesStartValues;
      private ConcurrentRunSimulationBatch _concurrentRunSimulationBatch;
      private ConcurrencyManagerResult<SimulationResults>[] _result;

      protected override void Context()
      {
         base.Context();
         _simulation = _simulationPersister.LoadSimulation(HelperForSpecs.DataFile("S1.pkml"));
         _containerTask = Api.GetContainerTask();
         _allMoleculePaths = _containerTask.AllMoleculesPathsIn(_simulation);
         _moleculesStartValues = _allMoleculePaths.Select(x => _containerTask.GetValueByPath(_simulation, x, true));

         _concurrentRunSimulationBatch = new ConcurrentRunSimulationBatch(_simulation, new SimulationBatchOptions { VariableMolecules = _allMoleculePaths });
         _concurrentRunSimulationBatch.AddSimulationBatchRunValues(new SimulationBatchRunValues { InitialValues = _moleculesStartValues.ToArray() });
         sut.AddSimulationBatch(_concurrentRunSimulationBatch);
      }

      protected override void Because()
      {
         _result = sut.RunConcurrently();
      }

      [Observation]
      public void should_complete_without_error()
      {
         _result.Length.ShouldBeEqualTo(1);
      }
   }

   public class When_running_a_batch_simulation_with_exception : concern_for_ConcurrentSimulationRunner
   {
      private ConcurrentRunSimulationBatch _concurrentRunSimulationBatch;
      private ConcurrencyManagerResult<SimulationResults>[] _results;
      private IModelCoreSimulation _simulation;
      private readonly List<string> _ids = new List<string>();
      private readonly List<SimulationBatchRunValues> _simulationBatchRunValues = new List<SimulationBatchRunValues>();

      public override void GlobalContext()
      {
         base.GlobalContext();
         _simulation = _simulationPersister.LoadSimulation(HelperForSpecs.DataFile("S1.pkml"));

         _concurrentRunSimulationBatch = new ConcurrentRunSimulationBatch
         (
            _simulation,
            new SimulationBatchOptions
            {
               VariableMolecules = new[]
               {
                  new[] { "Organism", "Kidney", "Intracellular", "Caffeine" }.ToPathString()
               },

               VariableParameters = new[]
               {
                  new[] { "Organism", "Liver", "Volume" }.ToPathString(),
                  new[] { "Organism", "Hematocrit" }.ToPathString(),
               }
            }
         );

         sut.AddSimulationBatch(_concurrentRunSimulationBatch);
      }

      protected override void Because()
      {
         _simulationBatchRunValues.Add(new SimulationBatchRunValues
         {
            InitialValues = new[] { 10.0 },
            ParameterValues = new[] { 3.5, 0.53 }
         });

         // Force an error during simulation run because ParameterValues vector does not have enough values
         _simulationBatchRunValues.Add(new SimulationBatchRunValues
         {
            InitialValues = new[] { 10.0 },
            ParameterValues = new[] { 3.5 }
         });

         _concurrentRunSimulationBatch.AddSimulationBatchRunValues(_simulationBatchRunValues[0]);
         _concurrentRunSimulationBatch.AddSimulationBatchRunValues(_simulationBatchRunValues[1]);

         _ids.AddRange(_concurrentRunSimulationBatch.SimulationBatchRunValues.Select(x => x.Id));
         _results = sut.RunConcurrently();
      }

      [Observation]
      public void should_return_a_result_with_success_set_to_false()
      {
         _results.Count(x => x.Succeeded).ShouldBeEqualTo(1);
         _results.Count(x => !x.Succeeded).ShouldBeEqualTo(1);
      }
   }

   public class When_running_a_batch_simulation_run_concurrently : concern_for_ConcurrentSimulationRunner
   {
      private ConcurrentRunSimulationBatch _concurrentRunSimulationBatch;
      private ConcurrencyManagerResult<SimulationResults>[] _results;
      private IModelCoreSimulation _simulation;
      private readonly List<string> _ids = new List<string>();
      private readonly List<SimulationBatchRunValues> _simulationBatchRunValues = new List<SimulationBatchRunValues>();

      public override void GlobalContext()
      {
         base.GlobalContext();
         _simulation = _simulationPersister.LoadSimulation(HelperForSpecs.DataFile("S1.pkml"));
         _simulationPersistableUpdater.UpdateSimulationPersistable(_simulation);

         _concurrentRunSimulationBatch = new ConcurrentRunSimulationBatch
         (
            _simulation,
            new SimulationBatchOptions
            {
               VariableMolecules = new[]
               {
                  new[] { "Organism", "Kidney", "Intracellular", "Caffeine" }.ToPathString()
               },

               VariableParameters = new[]
               {
                  new[] { "Organism", "Liver", "Volume" }.ToPathString(),
                  new[] { "Organism", "Hematocrit" }.ToPathString(),
               }
            }
         );

         sut.AddSimulationBatch(_concurrentRunSimulationBatch);
      }

      protected override void Because()
      {
         _simulationBatchRunValues.Add(new SimulationBatchRunValues
         {
            InitialValues = new[] { 10.0 },
            ParameterValues = new[] { 3.5, 0.53 }
         });
         _simulationBatchRunValues.Add(new SimulationBatchRunValues
         {
            InitialValues = new[] { 9.0 },
            ParameterValues = new[] { 3.4, 0.50 }
         });
         _simulationBatchRunValues.Add(new SimulationBatchRunValues
         {
            InitialValues = new[] { 10.5 },
            ParameterValues = new[] { 3.6, 0.55 }
         });
         _concurrentRunSimulationBatch.AddSimulationBatchRunValues(_simulationBatchRunValues[0]);
         _concurrentRunSimulationBatch.AddSimulationBatchRunValues(_simulationBatchRunValues[1]);
         _concurrentRunSimulationBatch.AddSimulationBatchRunValues(_simulationBatchRunValues[2]);

         _ids.AddRange(_concurrentRunSimulationBatch.SimulationBatchRunValues.Select(x => x.Id));
         _results = sut.RunConcurrently();
      }

      [Observation]
      public void should_be_able_to_simulate_the_simulation_for_multiple_runs()
      {
         foreach (var id in _ids)
         {
            var result = Api.GetSimulationBatchFactory().Create(_simulation, _concurrentRunSimulationBatch.SimulationBatchOptions).Run(_simulationBatchRunValues.FirstOrDefault(v => v.Id == id));
            var concurrentResult = _results.First(r => r.Id == id);
            result.Time.Values.ShouldBeEqualTo(concurrentResult.Result.Time.Values);
            result.ResultsFor(0).ValuesAsArray().Select(qv => qv.Values).ShouldBeEqualTo(concurrentResult.Result.ResultsFor(0).ValuesAsArray().Select(qv => qv.Values));
         }
      }
   }

   public class When_running_some_simulation_concurrently_and_then_adding_some_new_parameters : concern_for_ConcurrentSimulationRunner
   {
      private Simulation _simulation;
      private ConcurrentRunSimulationBatch _simulationBatch;
      private SimulationBatchRunValues _parValues1;
      private SimulationBatchRunValues _parValues2;

      public override void GlobalContext()
      {
         base.GlobalContext();
         _simulation = _simulationPersister.LoadSimulation(HelperForSpecs.DataFile("S1.pkml"));

         _simulationBatch = new ConcurrentRunSimulationBatch
         (
            _simulation,
            new SimulationBatchOptions
            {
               VariableParameters = new[]
               {
                  new[] { "Organism", "Liver", "Volume" }.ToPathString(),
                  new[] { "Organism", "Hematocrit" }.ToPathString(),
               }
            }
         );

         _parValues1 = new SimulationBatchRunValues
         {
            ParameterValues = new[] { 3.5, 0.53 }
         };
         _parValues2 = new SimulationBatchRunValues
         {
            ParameterValues = new[] { 3.4, 0.50 }
         };

         _simulationBatch.AddSimulationBatchRunValues(_parValues1);
         sut.AddSimulationBatch(_simulationBatch);
         sut.RunConcurrently();
         sut.Dispose();
      }

      protected override void Because()
      {
         _simulationBatch.AddSimulationBatchRunValues(_parValues2);
      }

      [Observation]
      public void should_not_crash()
      {
         sut.AddSimulationBatch(_simulationBatch);
         var res = sut.RunConcurrently();
         res[0].Succeeded.ShouldBeTrue();
      }
   }

   public class When_running_some_simulation_concurrently : concern_for_ConcurrentSimulationRunner
   {
      private Simulation _simulation;
      private ConcurrentRunSimulationBatch _simulationBatch1;
      private SimulationBatchRunValues _parValues1;
      private SimulationBatchRunValues _parValues2;
      private SimulationBatchRunValues _parValues3;
      private ConcurrentRunSimulationBatch _simulationBatch2;
      private ConcurrentRunSimulationBatch _simulationBatch3;

      public override void GlobalContext()
      {
         base.GlobalContext();
         _simulation = _simulationPersister.LoadSimulation(HelperForSpecs.DataFile("S1.pkml"));

         _simulationBatch1 = new ConcurrentRunSimulationBatch
         (
            _simulation,
            new SimulationBatchOptions
            {
               VariableParameters = new[]
               {
                  new[] { "Organism", "Liver", "Volume" }.ToPathString(),
                  new[] { "Organism", "Hematocrit" }.ToPathString(),
               }
            }
         );

         _simulationBatch2 = new ConcurrentRunSimulationBatch
         (
            _simulation,
            new SimulationBatchOptions
            {
               VariableParameters = new[]
               {
                  new[] { "Organism", "Liver", "Volume" }.ToPathString(),
                  new[] { "Organism", "Hematocrit" }.ToPathString(),
               }
            }
         );

         _simulationBatch3 = new ConcurrentRunSimulationBatch
         (
            _simulation,
            new SimulationBatchOptions
            {
               VariableParameters = new[]
               {
                  new[] { "Organism", "Liver", "Volume" }.ToPathString(),
                  new[] { "Organism", "Hematocrit" }.ToPathString(),
               }
            }
         );


         _parValues1 = new SimulationBatchRunValues
         {
            ParameterValues = new[] { 3.5, 0.53 }
         };
         _parValues2 = new SimulationBatchRunValues
         {
            ParameterValues = new[] { 3.5, 0.53 }
         };
         _parValues3 = new SimulationBatchRunValues
         {
            ParameterValues = new[] { 3.5, 0.53 }
         };

         _simulationBatch1.AddSimulationBatchRunValues(_parValues1);
         _simulationBatch2.AddSimulationBatchRunValues(_parValues2);
         _simulationBatch3.AddSimulationBatchRunValues(_parValues3);
         sut.AddSimulationBatch(_simulationBatch1);
         sut.AddSimulationBatch(_simulationBatch2);
         sut.AddSimulationBatch(_simulationBatch3);
      }

      [Observation]
      public void should_initialize_the_batch_in_parallel()
      {
         var res = sut.RunConcurrently();
         res[0].Succeeded.ShouldBeTrue();
      }
   }

   public class When_running_a_simulation_that_crashes : concern_for_ConcurrentSimulationRunner
   {
      private Simulation _simulation;
      private IContainerTask _containerTask;
      private string[] _allMoleculePaths;
      private IEnumerable<double> _moleculesStartValues;
      private ConcurrentRunSimulationBatch _concurrentRunSimulationBatch;
      private ConcurrencyManagerResult<SimulationResults>[] _results;
      private string _fullMessage;

      protected override void Context()
      {
         base.Context();
         _simulation = _simulationPersister.LoadSimulation(HelperForSpecs.DataFile("ErrorSim.pkml"));

         _containerTask = Api.GetContainerTask();
         _allMoleculePaths = _containerTask.AllMoleculesPathsIn(_simulation);
         _moleculesStartValues = _allMoleculePaths.Select(x => _containerTask.GetValueByPath(_simulation, x, true));

         _concurrentRunSimulationBatch = new ConcurrentRunSimulationBatch(_simulation, new SimulationBatchOptions { VariableMolecules = _allMoleculePaths });
         _concurrentRunSimulationBatch.AddSimulationBatchRunValues(new SimulationBatchRunValues { InitialValues = _moleculesStartValues.ToArray() });
         sut.AddSimulationBatch(_concurrentRunSimulationBatch);

         var simulationRunner = Api.GetSimulationRunner();
         try
         {
            simulationRunner.Run(new SimulationRunArgs { Simulation = _simulation });
         }
         catch (Exception exception)
         {
            _fullMessage = exception.FullMessage();
         }
      }

      protected override void Because()
      {
         _results = sut.RunConcurrently();
      }

      [Observation]
      public void an_error_message_should_result()
      {
         _results[0].ErrorMessage.ShouldBeEqualTo(_fullMessage);
      }
   }

   public class When_running_simulation_crashing_in_R_concurrently : concern_for_ConcurrentSimulationRunner
   {
      private Simulation _simulation;
      private ConcurrentRunSimulationBatch _simulationBatch;
      private SimulationBatchRunValues _parValues;

      public override void GlobalContext()
      {
         base.GlobalContext();
         _simulation = _simulationPersister.LoadSimulation(HelperForSpecs.DataFile("not_ok_sim.pkml"));

         var containerTask = Api.GetContainerTask();
         containerTask.AddQuantitiesToSimulationOutputByPath(_simulation, "DERMAL_APPLICATION_AREA|skin_compartment|SC_skin_sublayer|comp10_1|layer1|permeant");

         _simulationBatch = new ConcurrentRunSimulationBatch
         (
            _simulation,
            new SimulationBatchOptions
            {
               VariableParameters = new[]
               {
                  "DERMAL_APPLICATION_AREA|skin_compartment|SC_skin_sublayer|SC_total_thickness",
               }
            }
         );


         _parValues = new SimulationBatchRunValues
         {
            ParameterValues = new[] { 0.0002 }
         };


         _simulationBatch.AddSimulationBatchRunValues(_parValues);
         sut.AddSimulationBatch(_simulationBatch);
      }

      [Observation]
      public void should_initialize_the_batch_in_parallel()
      {
         var res = sut.RunConcurrently();
         res[0].Succeeded.ShouldBeTrue();
         res[0].Result.Count.ShouldBeEqualTo(1);
      }
   }

   public class When_running_simulation_giving_different_results_in_R : concern_for_ConcurrentSimulationRunner
   {
      private Simulation _simulation;
      private ConcurrentRunSimulationBatch _simulationBatch;
      private SimulationBatchRunValues _parValues;
      private ISimulationRunner _simulationRunner;
      private IContainerTask _containerTask;

      public override void GlobalContext()
      {
         base.GlobalContext();
         _simulation = _simulationPersister.LoadSimulation(HelperForSpecs.DataFile("sc_model_2c.pkml"));
         _simulationRunner = Api.GetSimulationRunner();

         _containerTask = Api.GetContainerTask();
         _containerTask.AddQuantitiesToSimulationOutputByPath(_simulation, "DERMAL_APPLICATION_AREA|permeant|Mass_balance_observer");
         _containerTask.AddQuantitiesToSimulationOutputByPath(_simulation, "DERMAL_APPLICATION_AREA|permeant|Stratum_corneum_observer");
         _containerTask.AddQuantitiesToSimulationOutputByPath(_simulation, "DERMAL_APPLICATION_AREA|permeant|Vehicle_observer");

         _simulationBatch = new ConcurrentRunSimulationBatch
         (
            _simulation,
            new SimulationBatchOptions
            {
               VariableParameters = new[]
               {
                  "DERMAL_APPLICATION_AREA|skin_compartment|Hydrated SC",
                  "DERMAL_APPLICATION_AREA|skin_compartment|SC_skin_sublayer|SC_total_thickness",
               }
            }
         );


         _parValues = new SimulationBatchRunValues
         {
            ParameterValues = new[]
            {
               1, 0.0002
            }
         };


         _simulationBatch.AddSimulationBatchRunValues(_parValues);
         sut.AddSimulationBatch(_simulationBatch);
      }

      [Observation]
      public void should_calculate_the_value_properly()
      {
         var res = sut.RunConcurrently();
         var asyncRes = res[0].Result;
         _containerTask.SetValueByPath(_simulation, "DERMAL_APPLICATION_AREA|skin_compartment|SC_skin_sublayer|SC_total_thickness", 0.0002, throwIfNotFound: true);
         _containerTask.SetValueByPath(_simulation, "DERMAL_APPLICATION_AREA|skin_compartment|Hydrated SC", 1, throwIfNotFound: true);
         var expectedResults = _simulationRunner.Run(new SimulationRunArgs { Simulation = _simulation });
         res[0].Succeeded.ShouldBeTrue();
         asyncRes.Count.ShouldBeEqualTo(1);
         asyncRes.AllValuesFor("DERMAL_APPLICATION_AREA|permeant|Vehicle_observer").Last().ShouldBeEqualTo(
            expectedResults.AllValuesFor("DERMAL_APPLICATION_AREA|permeant|Vehicle_observer").Last());
      }
   }

   public class When_running_simulation_giving_different_results_in_R_with_another_order : concern_for_ConcurrentSimulationRunner
   {
      private Simulation _simulation;
      private ConcurrentRunSimulationBatch _simulationBatch;
      private SimulationBatchRunValues _parValues;
      private ISimulationRunner _simulationRunner;
      private IContainerTask _containerTask;

      public override void GlobalContext()
      {
         base.GlobalContext();
         _simulation = _simulationPersister.LoadSimulation(HelperForSpecs.DataFile("sc_model_2c.pkml"));
         _simulationRunner = Api.GetSimulationRunner();

         _containerTask = Api.GetContainerTask();
         _containerTask.AddQuantitiesToSimulationOutputByPath(_simulation, "DERMAL_APPLICATION_AREA|permeant|Mass_balance_observer");
         _containerTask.AddQuantitiesToSimulationOutputByPath(_simulation, "DERMAL_APPLICATION_AREA|permeant|Stratum_corneum_observer");
         _containerTask.AddQuantitiesToSimulationOutputByPath(_simulation, "DERMAL_APPLICATION_AREA|permeant|Vehicle_observer");

         _simulationBatch = new ConcurrentRunSimulationBatch
         (
            _simulation,
            new SimulationBatchOptions
            {
               VariableParameters = new[]
               {
                  "DERMAL_APPLICATION_AREA|skin_compartment|SC_skin_sublayer|SC_total_thickness",
                  "DERMAL_APPLICATION_AREA|skin_compartment|Hydrated SC",
               }
            }
         );


         _parValues = new SimulationBatchRunValues
         {
            ParameterValues = new[]
            {
               0.0002, 1
            }
         };


         _simulationBatch.AddSimulationBatchRunValues(_parValues);
         sut.AddSimulationBatch(_simulationBatch);
      }

      [Observation]
      public void should_calculate_the_value_properly()
      {
         var res = sut.RunConcurrently();
         var asyncRes = res[0].Result;
         _containerTask.SetValueByPath(_simulation, "DERMAL_APPLICATION_AREA|skin_compartment|SC_skin_sublayer|SC_total_thickness", 0.0002, throwIfNotFound: true);
         _containerTask.SetValueByPath(_simulation, "DERMAL_APPLICATION_AREA|skin_compartment|Hydrated SC", 1, throwIfNotFound: true);
         var expectedResults = _simulationRunner.Run(new SimulationRunArgs { Simulation = _simulation });
         res[0].Succeeded.ShouldBeTrue();
         asyncRes.Count.ShouldBeEqualTo(1);
         asyncRes.AllValuesFor("DERMAL_APPLICATION_AREA|permeant|Vehicle_observer").Last().ShouldBeEqualTo(
            expectedResults.AllValuesFor("DERMAL_APPLICATION_AREA|permeant|Vehicle_observer").Last());
      }
   }
}