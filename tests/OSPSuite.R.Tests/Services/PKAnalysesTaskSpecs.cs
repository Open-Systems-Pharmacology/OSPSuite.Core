using System.Linq;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.R.Domain;

namespace OSPSuite.R.Services
{
   public abstract class concern_for_PKAnalysesTask : ContextForIntegration<IPKAnalysesTask>
   {
      protected string _pkParameterFile;
      protected Simulation _simulation;

      public override void GlobalContext()
      {
         base.GlobalContext();
         _pkParameterFile = HelperForSpecs.DataFile("20 Values for peripheral venous blood.csv");
         var simulationFile = HelperForSpecs.DataFile("S1.pkml");
         var simulationPersister = Api.GetSimulationPersister();
         _simulation = simulationPersister.LoadSimulation(simulationFile);
         sut = Api.GetPKAnalysesTask();
      }
   }

   public class When_importing_a_valid_pk_parameter_files : concern_for_PKAnalysesTask
   {
      private PopulationSimulationPKAnalyses _result;

      protected override void Because()
      {
         _result = sut.ImportPKAnalysesFromCSV(_pkParameterFile, _simulation);
      }

      [Observation]
      public void should_return_a_pk_analysis_object_with_the_expected_dat()
      {
         _result.All().Count().ShouldBeEqualTo(1);
         _result.HasPKParameterFor("Organism|PeripheralVenousBlood|Caffeine|Plasma (Peripheral Venous Blood)", "My PK-Parameter").ShouldBeTrue();
      }
   }
}