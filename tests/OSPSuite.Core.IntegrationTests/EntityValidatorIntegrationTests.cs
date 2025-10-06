using System.Linq;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Utility.Container;

namespace OSPSuite.Core
{
   public abstract class concern_for_EntityValidator : ContextForModelConstructorIntegration
   {
      protected IModelCoreSimulation _simulation;
      protected EntityValidator _entityValidator;

      public override void GlobalContext()
      {
         base.GlobalContext();
         _simulation = LoadPKMLFile("simple").Simulation;
         _entityValidator = IoC.Resolve<IEntityValidatorFactory>().Create();
      }
   }

   public class When_validating_a_simulation_with_a_resolution_resulting_in_a_very_large_number_of_points : concern_for_EntityValidator
   {
      private ValidationResult _result;

      public override void GlobalContext()
      {
         base.GlobalContext();
         var interval = _simulation.Settings.OutputSchema.Intervals.First();
         //120 pts per hour
         interval.Resolution.ValueInDisplayUnit = 120;

         //min= >let's simulate for a long time (10 months)
         interval.EndTime.Value = 60 * 24 * 30 * 10;
      }

      protected override void Because()
      {
         _result = _entityValidator.Validate(_simulation);
      }

      [Observation]
      public void should_return_a_valid_state_with_warnings()
      {
         _result.ValidationState.ShouldBeEqualTo(ValidationState.ValidWithWarnings);
      }
   }

   public class When_validating_a_simulation_with_a_resolution_resulting_in_a_realistic_number_of_points : concern_for_EntityValidator
   {
      private ValidationResult _result;

      public override void GlobalContext()
      {
         base.GlobalContext();
         var interval = _simulation.Settings.OutputSchema.Intervals.First();
         //4 pts per hour
         interval.Resolution.ValueInDisplayUnit = 1;

         //min= >let's simulate for a long time (10 months)
         interval.EndTime.Value = 60 * 24 * 30 * 10;
      }

      protected override void Because()
      {
         _result = _entityValidator.Validate(_simulation);
      }

      [Observation]
      public void should_not_return_warnings()
      {
         _result.ValidationState.ShouldBeEqualTo(ValidationState.Valid);
      }
   }

   public class When_validating_a_simulation_with_a_resolution_resulting_in_a_realistic_number_of_points_based_on_start_time_and_end_time_of_interval : concern_for_EntityValidator
   {
      private ValidationResult _result;

      public override void GlobalContext()
      {
         base.GlobalContext();
         var interval = _simulation.Settings.OutputSchema.Intervals.First();
         //10 pts per hour
         interval.Resolution.ValueInDisplayUnit = 10;

         //9_months_start
         interval.StartTime.Value = 60 * 24 * 30 * 9;
         //10_months_end
         interval.EndTime.Value = 60 * 24 * 30 * 10;
      }

      protected override void Because()
      {
         _result = _entityValidator.Validate(_simulation);
      }

      [Observation]
      public void should_not_return_warnings()
      {
         _result.ValidationState.ShouldBeEqualTo(ValidationState.Valid);
      }
   }
}