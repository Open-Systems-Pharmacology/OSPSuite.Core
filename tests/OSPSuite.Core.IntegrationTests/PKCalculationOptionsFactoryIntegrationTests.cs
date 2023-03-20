using System.Linq;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Services;

namespace OSPSuite.Core
{
   public abstract class concern_for_PKCalculationOptionsFactory : ContextWithLoadedSimulation<PKCalculationOptionsFactory>
   {
      protected IModelCoreSimulation _simulation;

      public override void GlobalContext()
      {
         base.GlobalContext();
         _simulation = LoadPKMLFile("multiple").Simulation;
      }
   }

   public class When_creating_the_PK_calculation_options_for_a_multiple_simulation_with_three_applications : concern_for_PKCalculationOptionsFactory
   {
      [Observation]
      public void should_treat_the_application_has_multiple_application_when_setting_the_end_time_to_start_time_of_last_application()
      {
         _simulation.Settings.OutputSchema.Intervals.Last().EndTime.Value = 48 * 60; //48 hours
         var pkOptions = new PKCalculationOptionsFactory().CreateFor(_simulation, "drug");
         pkOptions.SingleDosing.ShouldBeFalse();
      }

      [Observation]
      public void should_treat_the_application_has_multiple_application_when_setting_the_end_time_before_the_start_time_of_last_application()
      {
         _simulation.Settings.OutputSchema.Intervals.Last().EndTime.Value = 47 * 60; //47 hours
         var pkOptions = new PKCalculationOptionsFactory().CreateFor(_simulation, "drug");
         pkOptions.SingleDosing.ShouldBeFalse();
      }
   }
}