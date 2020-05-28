using System.Collections.Generic;
using System.Linq;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.SensitivityAnalyses;
using OSPSuite.Core.Domain.Services;
using OSPSuite.R.Domain;
using SensitivityAnalysis = OSPSuite.R.Domain.SensitivityAnalysis;

namespace OSPSuite.R.Services
{
   public abstract class concern_for_FullPathDisplayResolver : ContextForIntegration<IFullPathDisplayResolver>
   {
      protected Simulation _simulation;
      protected SensitivityAnalysis _sensitivityAnalysis;
      protected SensitivityAnalysisRunResult _sensitivityAnalysisRunResult;
      protected IReadOnlyList<PKParameterSensitivity> _allPKParameterSensitivities;
      protected IParameter _liverVolume;

      public override void GlobalContext()
      {
         base.GlobalContext();
         var simulationFile = HelperForSpecs.DataFile("S1.pkml");
         var simulationPersister = Api.GetSimulationPersister();
         var containerTask = Api.GetContainerTask();
         _simulation = simulationPersister.LoadSimulation(simulationFile);
         _liverVolume = containerTask.AllParametersMatching(_simulation, "Organism|Liver|Volume").First();

      }

      protected override void Context()
      {
         sut = Api.GetFullPathDisplayResolver();
      }
   }

   public class When_returning_the_display_path_for_a_known_parameter : concern_for_FullPathDisplayResolver
   {
      [Observation]
      public void should_return_the_expected_display_path()
      {
         sut.FullPathFor(_liverVolume).ShouldBeEqualTo("Liver-Volume");
      }
   }
}