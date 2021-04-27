using NUnit.Framework;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Extensions;
using OSPSuite.R.Domain;
using System.Collections.Generic;
using System.Linq;

namespace OSPSuite.R.Services
{
   class CoreUserSettings : ICoreUserSettings
   {
      public int MaximumNumberOfCoresToUse { get; set; } = 4;
      public int NumberOfBins { get; set; }
      public int NumberOfIndividualsPerBin { get; set; }
   }

   public class When_running_simulations_concurrently : ContextForIntegration<IConcurrentSimulationRunner>
   {
      private ISimulationPersister _simulationPersister;
      private ConcurrencyManagerResult<SimulationResults>[] _results;

      protected override void Context()
      {
         base.Context();

         _simulationPersister = Api.GetSimulationPersister();
         sut = Api.GetConcurrentSimulationRunner();
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
         Assert.IsNotNull(_results);
         Assert.IsTrue(_results.All(r => r.Result.ElementAt(0).AllValues.SelectMany(v => v.Values).Count() > 0));
      }
   }

   public class When_running_a_batch_simulation_run_concurrently : ContextForIntegration<IConcurrentSimulationRunner>
   {
      private SettingsForConcurrentRunSimulationBatch _simulationWithBatchOptions;
      private ISimulationPersister _simulationPersister;
      private ConcurrencyManagerResult<SimulationResults>[] _results;
      private IModelCoreSimulation _simulation;
      private List<string> _ids = new List<string>();
      private List<SimulationBatchRunValues> _simulationBatchRunValues = new List<SimulationBatchRunValues>();

      public override void GlobalContext()
      {
         base.GlobalContext();
         _simulationPersister = Api.GetSimulationPersister();
         _simulation = _simulationPersister.LoadSimulation(HelperForSpecs.DataFile("S1.pkml"));

         _simulationWithBatchOptions = new SettingsForConcurrentRunSimulationBatch()
         {
            Simulation = _simulation,
            SimulationBatchOptions = new SimulationBatchOptions
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
            }
         };

         sut = Api.GetConcurrentSimulationRunner();
         sut.AddSimulationBatchOption(_simulationWithBatchOptions);
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
         _ids.Add(_simulationWithBatchOptions.AddSimulationBatchRunValues(_simulationBatchRunValues[0]));
         _ids.Add(_simulationWithBatchOptions.AddSimulationBatchRunValues(_simulationBatchRunValues[1]));
         _ids.Add(_simulationWithBatchOptions.AddSimulationBatchRunValues(_simulationBatchRunValues[2]));

         _results = sut.RunConcurrently();
      }

      [Observation]
      public void should_be_able_to_simulate_the_simulation_for_multiple_runes()
      {
         foreach (var id in _ids)
         {
            var result = Api.GetSimulationBatchFactory().Create(_simulation, _simulationWithBatchOptions.SimulationBatchOptions).Run(_simulationBatchRunValues.FirstOrDefault(v => v.Id == id));
            var concurrentResult = _results.FirstOrDefault(r => r.Id == id);
            result.Time.Values.ShouldBeEqualTo(concurrentResult.Result.Time.Values);
            result.ResultsFor(0).ValuesAsArray().Select(qv => qv.Values).ShouldBeEqualTo(concurrentResult.Result.ResultsFor(0).ValuesAsArray().Select(qv => qv.Values));
         }
      }
   }
}
