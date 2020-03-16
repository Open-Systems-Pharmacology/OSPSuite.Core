using System.Linq;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain.PKAnalyses;
using OSPSuite.Core.Domain.Services;

namespace OSPSuite.Core.Domain
{
   public abstract class concern_for_UserDefinedPKParameter : ContextSpecification<UserDefinedPKParameter>
   {
      protected PKCalculationOptions _options;
      protected DosingInterval _interval0;
      protected DosingInterval _interval1;
      protected DosingInterval _interval2;
      protected DosingInterval _interval3;
      protected DosingInterval _interval4;

      protected override void Context()
      {
         sut = new UserDefinedPKParameter();
         _options = new PKCalculationOptions {TotalDrugMassPerBodyWeight = 20};
         _interval0 = new DosingInterval {DrugMassPerBodyWeight = 2};
         _interval1 = new DosingInterval {DrugMassPerBodyWeight = 3};
         _interval2 = new DosingInterval {DrugMassPerBodyWeight = 4};
         _interval3 = new DosingInterval {DrugMassPerBodyWeight = 5};
         _interval4 = new DosingInterval {DrugMassPerBodyWeight = 6};
         _options.AddInterval(_interval0);
         _options.AddInterval(_interval1);
         _options.AddInterval(_interval2);
         _options.AddInterval(_interval3);
         _options.AddInterval(_interval4);
      }
   }

   public class When_estimating_the_drug_mass_per_body_weight_for_a_user_defined_parameter_with_time_define_in_min : concern_for_UserDefinedPKParameter
   {
      [Observation]
      public void should_return_null_for_start_time()
      {
         sut.StartTime = 15;
         sut.EstimateDrugMassPerBodyWeight(_options).ShouldBeNull();
      }

      [Observation]
      public void should_return_null_for_start_time_offset()
      {
         sut.StartTimeOffset = 15;
         sut.EstimateDrugMassPerBodyWeight(_options).ShouldBeNull();
      }

      [Observation]
      public void should_return_null_for_end_time_offset()
      {
         sut.EndTimeOffset = 15;
         sut.EstimateDrugMassPerBodyWeight(_options).ShouldBeNull();
      }

      [Observation]
      public void should_return_null_for_end_time()
      {
         sut.EndTime = 15;
         sut.EstimateDrugMassPerBodyWeight(_options).ShouldBeNull();
      }
   }

   public class When_estimating_the_drug_mass_per_body_weight_for_a_user_defined_parameter_without_any_interval_definition : concern_for_UserDefinedPKParameter
   {
      [Observation]
      public void should_return_the_total_drug_mass()
      {
         sut.EstimateDrugMassPerBodyWeight(_options).ShouldBeEqualTo(_options.TotalDrugMassPerBodyWeight);
      }
   }


   public class When_estimating_the_drug_mass_per_body_weight_for_a_user_defined_parameter_with_interval_definition_not_matching_the_options : concern_for_UserDefinedPKParameter
   {
      [Observation]
      public void should_return_null()
      {
         sut.StartApplicationIndex = 2;
         sut.EndApplicationIndex = 8;
         sut.EstimateDrugMassPerBodyWeight(_options).ShouldBeNull();


         sut.StartApplicationIndex = 9;
         sut.EndApplicationIndex = 10;
         sut.EstimateDrugMassPerBodyWeight(_options).ShouldBeNull();


         sut.StartApplicationIndex = 3;
         sut.EndApplicationIndex = 2;
         sut.EstimateDrugMassPerBodyWeight(_options).ShouldBeNull();

         sut.StartApplicationIndex = 1;
         sut.EndApplicationIndex = 1;
         sut.EstimateDrugMassPerBodyWeight(_options).ShouldBeNull();
      }
   }

   public class When_estimating_the_drug_mass_per_body_weight_for_a_user_defined_parameter_with_interval_definition_matching_the_options : concern_for_UserDefinedPKParameter
   {
      [Observation]
      public void should_return_null()
      {
         sut.StartApplicationIndex = 0;
         var intervals = new[] {_interval0, _interval1, _interval2, _interval3, _interval4};
         sut.EstimateDrugMassPerBodyWeight(_options).ShouldBeEqualTo(intervals.Sum(x=>x.DrugMassPerBodyWeight));


         sut.StartApplicationIndex = 1;
         intervals = new[] { _interval1, _interval2, _interval3, _interval4 };
         sut.EstimateDrugMassPerBodyWeight(_options).ShouldBeEqualTo(intervals.Sum(x => x.DrugMassPerBodyWeight));

         sut.StartApplicationIndex = null;
         sut.EndApplicationIndex = 3;
         intervals = new[] { _interval0, _interval1, _interval2};
         sut.EstimateDrugMassPerBodyWeight(_options).ShouldBeEqualTo(intervals.Sum(x => x.DrugMassPerBodyWeight));



         sut.StartApplicationIndex = 1;
         sut.EndApplicationIndex = 3;
         intervals = new[] { _interval1, _interval2 };
         sut.EstimateDrugMassPerBodyWeight(_options).ShouldBeEqualTo(intervals.Sum(x => x.DrugMassPerBodyWeight));

      }

   }

}