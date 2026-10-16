using System;
using System.Linq;
using FakeItEasy;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.PKAnalyses;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Helpers;

namespace OSPSuite.Core.Domain
{
   public abstract class concern_for_PKCalculationOptionsFactory : ContextSpecification<PKCalculationOptionsFactory>
   {
      protected const string MOLECULE_NAME = "Drug";
      protected IModelCoreSimulation _simulation;
      protected IContainer _events;
      protected PKCalculationOptions _result;

      protected override void Context()
      {
         _events = new Container().WithName(Constants.EVENTS);
         _simulation = A.Fake<IModelCoreSimulation>();
         A.CallTo(() => _simulation.Model).Returns(new Model {Root = new Container {_events}});
         A.CallTo(() => _simulation.Reactions).Returns(Array.Empty<ReactionBuildingBlock>());
         A.CallTo(() => _simulation.BodyWeight).Returns(DomainHelperForSpecs.ConstantParameterWithValue(10));
         A.CallTo(() => _simulation.TotalDrugMassFor(MOLECULE_NAME)).Returns(DomainHelperForSpecs.ConstantParameterWithValue(30));
         sut = new PKCalculationOptionsFactory();
      }

      protected void AddApplication(string name, double startTime, double drugMass)
      {
         var application = new Container {Name = name, ContainerType = ContainerType.Application};
         application.Add(new MoleculeAmount {Name = MOLECULE_NAME});
         application.Add(DomainHelperForSpecs.ConstantParameterWithValue(startTime).WithName(Constants.Parameters.START_TIME));
         application.Add(DomainHelperForSpecs.ConstantParameterWithValue(drugMass).WithName(Constants.Parameters.DRUG_MASS));
         _events.Add(application);
      }

      protected override void Because()
      {
         _result = sut.CreateFor(_simulation, MOLECULE_NAME);
      }
   }

   public class When_creating_the_pk_calculation_options_for_a_multiple_dosing_simulation_with_an_additional_zero_dose_application_at_the_first_start_time : concern_for_PKCalculationOptionsFactory
   {
      protected override void Context()
      {
         base.Context();
         A.CallTo(() => _simulation.EndTime).Returns(4320);
         AddApplication("Disabled", startTime: 0, drugMass: 0);
         AddApplication("App1", startTime: 0, drugMass: 10);
         AddApplication("App2", startTime: 1440, drugMass: 10);
         AddApplication("App3", startTime: 2880, drugMass: 10);
      }

      [Observation]
      public void should_treat_the_simulation_as_multiple_dosing()
      {
         _result.SingleDosing.ShouldBeFalse();
      }

      [Observation]
      public void should_create_one_dosing_interval_per_application_ending_at_the_next_later_application()
      {
         _result.DosingIntervals.Select(x => x.StartValue).ShouldOnlyContainInOrder(0f, 0f, 1440f, 2880f);
         _result.DosingIntervals.Select(x => x.EndValue).ShouldOnlyContainInOrder(1440f, 1440f, 2880f, 4320f);
         _result.DosingIntervals.Select(x => x.DrugMassPerBodyWeight).ShouldOnlyContainInOrder(0d, 1d, 1d, 1d);
      }

      [Observation]
      public void should_return_the_first_and_last_intervals_between_application_start_times()
      {
         _result.FirstInterval.StartValue.ShouldBeEqualTo(0f);
         _result.FirstInterval.EndValue.ShouldBeEqualTo(1440f);
         _result.FirstInterval.DrugMassPerBodyWeight.ShouldBeEqualTo(1d);
         _result.LastMinusOneInterval.StartValue.ShouldBeEqualTo(1440f);
         _result.LastInterval.StartValue.ShouldBeEqualTo(2880f);
         _result.LastInterval.EndValue.ShouldBeEqualTo(4320f);
      }
   }

   public class When_creating_the_pk_calculation_options_for_a_multiple_dosing_simulation_with_two_applications_at_the_first_start_time : concern_for_PKCalculationOptionsFactory
   {
      protected override void Context()
      {
         base.Context();
         A.CallTo(() => _simulation.EndTime).Returns(2880);
         AddApplication("IV", startTime: 0, drugMass: 10);
         AddApplication("Oral", startTime: 0, drugMass: 5);
         AddApplication("App2", startTime: 1440, drugMass: 10);
      }

      [Observation]
      public void should_create_one_dosing_interval_per_application_ending_at_the_next_later_application()
      {
         _result.DosingIntervals.Select(x => x.StartValue).ShouldOnlyContainInOrder(0f, 0f, 1440f);
         _result.DosingIntervals.Select(x => x.EndValue).ShouldOnlyContainInOrder(1440f, 1440f, 2880f);
         _result.DosingIntervals.Select(x => x.DrugMassPerBodyWeight).ShouldOnlyContainInOrder(1d, 0.5d, 1d);
      }

      [Observation]
      public void should_set_the_drug_mass_of_the_first_interval_to_the_sum_of_the_drug_masses_applied_at_its_start_time()
      {
         _result.SingleDosing.ShouldBeFalse();
         _result.FirstInterval.DrugMassPerBodyWeight.ShouldBeEqualTo(1.5d);
         _result.LastMinusOneInterval.StartValue.ShouldBeEqualTo(0f);
         _result.LastInterval.StartValue.ShouldBeEqualTo(1440f);
         _result.LastInterval.DrugMassPerBodyWeight.ShouldBeEqualTo(1d);
      }

      [Observation]
      public void should_resolve_the_application_indexes_of_a_user_defined_pk_parameter_to_the_applications()
      {
         var userDefinedPKParameter = new UserDefinedPKParameter {StartApplicationIndex = 1, EndApplicationIndex = 2};
         userDefinedPKParameter.EstimateStartTimeFrom(_result).ShouldBeEqualTo(0f);
         userDefinedPKParameter.EstimateEndTimeFrom(_result).ShouldBeEqualTo(1440f);
         userDefinedPKParameter.EstimateDrugMassPerBodyWeight(_result).ShouldBeEqualTo(0.5d);
      }
   }

   public class When_creating_the_pk_calculation_options_for_a_simulation_with_two_applications_at_the_same_start_time_only : concern_for_PKCalculationOptionsFactory
   {
      protected override void Context()
      {
         base.Context();
         A.CallTo(() => _simulation.EndTime).Returns(1440);
         AddApplication("IV", startTime: 0, drugMass: 10);
         AddApplication("Oral", startTime: 0, drugMass: 5);
      }

      [Observation]
      public void should_treat_the_simulation_as_single_dosing()
      {
         _result.SingleDosing.ShouldBeTrue();
      }

      [Observation]
      public void should_return_a_first_interval_with_the_sum_of_the_drug_masses_applied_at_its_start_time()
      {
         _result.FirstInterval.StartValue.ShouldBeEqualTo(0f);
         _result.FirstInterval.EndValue.ShouldBeEqualTo(1440f);
         _result.FirstInterval.DrugMassPerBodyWeight.ShouldBeEqualTo(1.5d);
      }
   }
}
