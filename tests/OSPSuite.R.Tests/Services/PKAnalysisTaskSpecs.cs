using System.Linq;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.R.Domain;

namespace OSPSuite.R.Services
{
   public abstract class concern_for_PKAnalysisTask : ContextForIntegration<IPKAnalysisTask>
   {
      protected string _pkParameterFile;
      protected Simulation _simulation;
      protected string _outputPath;

      public override void GlobalContext()
      {
         base.GlobalContext();
         _outputPath = "Organism|PeripheralVenousBlood|Caffeine|Plasma (Peripheral Venous Blood)";

         _pkParameterFile = HelperForSpecs.DataFile("20 Values for peripheral venous blood.csv");
         var simulationFile = HelperForSpecs.DataFile("S1.pkml");
         var simulationPersister = Api.GetSimulationPersister();
         _simulation = simulationPersister.LoadSimulation(simulationFile);
         sut = Api.GetPKAnalysisTask();
      }
   }

   public class When_importing_a_valid_pk_parameter_files : concern_for_PKAnalysisTask
   {
      private PopulationSimulationPKAnalyses _result;

      protected override void Because()
      {
         _result = sut.ImportPKAnalysesFromCSV(_pkParameterFile, _simulation);
      }

      [Observation]
      public void should_return_a_pk_analysis_object_with_the_expected_data()
      {
         _result.All().Count().ShouldBeEqualTo(2);
         _result.HasPKParameterFor(_outputPath, "My PK-Parameter").ShouldBeTrue();
         _result.HasPKParameterFor(_outputPath, "C_max").ShouldBeTrue();
      }

      [Observation]
      public void should_have_converted_display_dimension_into_core_dimension()
      {
         var c_max = _result.PKParameterFor(_outputPath, "C_max");
         c_max.Dimension.Name.ShouldBeEqualTo(Constants.Dimension.MOLAR_CONCENTRATION);
      }

   }
}