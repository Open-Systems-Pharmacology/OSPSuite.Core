using System.Linq;
using System.Threading.Tasks;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain.Data;
using OSPSuite.SimModel;

namespace OSPSuite.Core.Domain
{
   public abstract class concern_for_PopulationRunResults : ContextSpecification<PopulationRunResults>
   {
      protected override void Context()
      {
         sut = new PopulationRunResults();
      }
   }

   public class When_adding_individual_results_and_warnings_concurrently_from_multiple_threads : concern_for_PopulationRunResults
   {
      private const int NUMBER_OF_INDIVIDUALS = 100;
      private const int NUMBER_OF_RUNS = 5000;

      protected override void Because()
      {
         //the race only shows while the run info cache is small and resizes often, so it takes many short runs to hit it
         for (var run = 0; run < NUMBER_OF_RUNS; run++)
         {
            var runResults = new PopulationRunResults();
            //same sequence as PopulationRunner: an individual is added, then its warnings
            Parallel.For(0, NUMBER_OF_INDIVIDUALS, individualId =>
            {
               runResults.Add(new IndividualResults {IndividualId = individualId});
               runResults.AddWarnings(individualId, Enumerable.Empty<SolverWarning>());
            });
            sut = runResults;
         }
      }

      [Observation]
      public void should_have_a_run_info_for_every_individual() => sut.IndividualRunInfos.Count().ShouldBeEqualTo(NUMBER_OF_INDIVIDUALS);
   }
}
