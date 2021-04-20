using NUnit.Framework;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Extensions;
using OSPSuite.R.Domain;
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
      private ConcurrentSimulationResults[] _results;

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
         Assert.IsTrue(_results.All(r => r.SimulationResults.ElementAt(0).AllValues.SelectMany(v => v.Values).Count() > 0));
      }
   }

   public class When_running_a_batch_simulation_run_concurrently : ContextForIntegration<IConcurrentSimulationRunner>
   {
      private SimulationWithBatchOptions _simulationWithBatchOptions;
      private ISimulationPersister _simulationPersister;
      private ConcurrentSimulationResults[] _results;
      private IModelCoreSimulation _simulation;

      public override void GlobalContext()
      {
         base.GlobalContext();
         _simulationPersister = Api.GetSimulationPersister();
         _simulation = _simulationPersister.LoadSimulation(HelperForSpecs.DataFile("S1.pkml"));

         _simulationWithBatchOptions = new SimulationWithBatchOptions()
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
         _simulationWithBatchOptions.AddSimulationBatchRunValues(new SimulationBatchRunValues
         {
            InitialValues = new[] { 10.0 },
            ParameterValues = new[] { 3.5, 0.53 },
            Id = "A"
         });
         _simulationWithBatchOptions.AddSimulationBatchRunValues(new SimulationBatchRunValues
         {
            InitialValues = new[] { 9.0 },
            ParameterValues = new[] { 3.4, 0.50 },
            Id = "B"
         });
         _simulationWithBatchOptions.AddSimulationBatchRunValues(new SimulationBatchRunValues
         {
            InitialValues = new[] { 10.5 },
            ParameterValues = new[] { 3.6, 0.55 },
            Id = "C"
         });
      }

      protected override void Context()
      {
         base.Context();

         sut = Api.GetConcurrentSimulationRunner();
         sut.AddSimulationBatchOption(_simulationWithBatchOptions);
      }

      protected override void Because()
      {
         _results = sut.RunConcurrently();
      }

      [Observation]
      public void should_be_able_to_simulate_the_simulation_for_multiple_runes()
      {
         for (var i = 0; i < _results.Length; i++)
         {
            var result = Api.GetSimulationBatchFactory().Create(_simulation, _simulationWithBatchOptions.SimulationBatchOptions).Run(_simulationWithBatchOptions.SimulationBatchRunValues.FirstOrDefault(v => v.Id == _results[i].AdditionalId));
            result.Time.Values.ShouldBeEqualTo(_results[i].SimulationResults.Time.Values);
            result.ResultsFor(0).ValuesAsArray().Select(qv => qv.Values).ShouldBeEqualTo(_results[i].SimulationResults.ResultsFor(0).ValuesAsArray().Select(qv => qv.Values));
         }
      }
   }
}
