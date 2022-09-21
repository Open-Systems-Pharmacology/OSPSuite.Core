using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.R.Services;

namespace OSPSuite.R.Domain
{
   public class concern_for_ConcurrentRunSimulationBatch : ContextForIntegration<ConcurrentRunSimulationBatch>
   {
      protected ISimulationPersister _simulationPersister;

      public override void GlobalContext()
      {
         base.GlobalContext();
         _simulationPersister = Api.GetSimulationPersister();
      }
   }

   public class When_creating_a_new_simulation_batch : concern_for_ConcurrentRunSimulationBatch
   {
      private Simulation _simulation;
      

      protected override void Context()
      {
         base.Context();
         _simulation = _simulationPersister.LoadSimulation(HelperForSpecs.DataFile("S1.pkml"));
      }

      protected override void Because()
      {
         sut = new ConcurrentRunSimulationBatch
         (
            _simulation,
            new SimulationBatchOptions()
         );
      }

      [Observation]
      public void should_not_be_reference_to_the_original_simulation()
      {
         sut.Simulation.ShouldNotBeEqualTo(_simulation);
      }
   }
}