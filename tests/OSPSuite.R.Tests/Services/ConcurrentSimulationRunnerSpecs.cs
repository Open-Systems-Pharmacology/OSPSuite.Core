using NUnit.Framework;
using OSPSuite.BDDHelper;
using OSPSuite.Core;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Domain.Services;
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
      private SimulationResults[] _results;

      protected override void Context()
      {
         base.Context();

         _simulationPersister = Api.GetSimulationPersister();
         sut = new ConcurrentSimulationRunner(new ConcurrencyManager(new CoreUserSettings()));
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
         Assert.IsTrue(_results.All(r => r.ElementAt(0).AllValues.SelectMany(v => v.Values).Count() > 0));
      }
   }
}
