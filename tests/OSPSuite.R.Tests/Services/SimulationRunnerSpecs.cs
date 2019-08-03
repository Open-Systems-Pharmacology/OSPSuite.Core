using System.Linq;
using FakeItEasy;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Helpers;

namespace OSPSuite.R.Services
{
   public abstract class concern_for_SimulationRunner : ContextSpecification<ISimulationRunner>
   {
      protected IEntitiesInSimulationRetriever _entitiesInSimulationRetriever;

      protected override void Context()
      {
         _entitiesInSimulationRetriever= A.Fake<IEntitiesInSimulationRetriever>();  
         sut = new SimulationRunner(_entitiesInSimulationRetriever);
      }
   }

   public class When_running_a_simulation : concern_for_SimulationRunner
   {
      private IModelCoreSimulation _simulation;
      private PathCache<IQuantity> _quantityPathCache;
      private SimulationResults _results;

      protected override void Context()
      {
         base.Context();
         _quantityPathCache = new PathCacheForSpecs<IQuantity>();

         _quantityPathCache.Add("S1|Organism|Liver|Concentration", new Parameter{Persistable = true});
         _quantityPathCache.Add("S1|Organism|Kidney|Concentration", new Parameter { Persistable = true });
         _simulation = new ModelCoreSimulation();
         A.CallTo(_entitiesInSimulationRetriever).WithReturnType<PathCache<IQuantity>>().Returns(_quantityPathCache);
      }

      protected override void Because()
      {
         _results = sut.RunSimulation(_simulation);
      }
      [Observation]
      public void should_return_results_for_the_expected_outputs()
      {
         _results.AllIndividualResults.Count.ShouldBeEqualTo(1);
         _results.AllIndividualResults.ElementAt(0).AllValues.Count.ShouldBeEqualTo(2);
      }
   }
}