using System.Collections.Generic;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Services;
using OSPSuite.Utility.Container;

namespace OSPSuite.Core
{
   public abstract class concern_for_SimulationAnalyzer : ContextWithLoadedSimulation<ISimulationAnalyzer>
   {
      protected IModelCoreSimulation _simulation;

      public override void GlobalContext()
      {
         base.GlobalContext();
         _simulation = LoadPKMLFile("simple").Simulation;
         sut= IoC.Resolve<ISimulationAnalyzer>();
      }
   }

   public class When_retrieving_the_name_of_all_parameters_used_in_a_simulation : concern_for_SimulationAnalyzer
   {
      private IReadOnlyList<string> _allUsedParameters;

      protected override void Because()
      {
         _allUsedParameters = sut.AllPathOfParametersUsedInSimulation(_simulation);
      }

      [Observation]
      public void should_only_return_the_parameter_used_in_the_simulation()
      {
         _allUsedParameters.ShouldOnlyContain("R1|k1");
      }
   }
}