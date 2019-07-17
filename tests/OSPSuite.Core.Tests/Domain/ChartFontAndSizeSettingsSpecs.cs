using System.Linq;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Chart;
using OSPSuite.Utility.Validation;

namespace OSPSuite.Core.Domain
{
   public abstract class concern_for_ChartFontAndSizeSettings : ContextSpecification<ChartFontAndSizeSettings>
   {
      protected override void Context()
      {
         sut = new ChartFontAndSizeSettings();
      }

      protected bool FailsValidation(IBusinessRule rule)
      {
         return rule.IsSatisfiedBy(sut) == false;
      }
   }

   public abstract class When_validating_export_sizes : concern_for_ChartFontAndSizeSettings
   {
      protected override void Context()
      {
         base.Context();
         SetExportValues();
      }

      protected abstract void SetExportValues();
   }

   public abstract class When_validating_failing_export_sizes : When_validating_export_sizes
   {
      [Observation]
      public void values_under_the_minimum_should_fail_validation()
      {
         sut.Rules.All().Any(FailsValidation).ShouldBeTrue();
      }
   }

   public abstract class When_validating_passing_export_sizes : When_validating_export_sizes
   {
      [Observation]
      public void values_under_the_minimum_should_fail_validation()
      {
         sut.Rules.All().Any(FailsValidation).ShouldBeFalse();
      }
   }

   public class When_values_are_within_range : When_validating_passing_export_sizes
   {
      protected override void SetExportValues()
      {
         sut.ChartHeight = 200;
         sut.ChartWidth = 2000;
      }
   }

   public class When_values_are_not_set_for_chart_export_sizes : When_validating_passing_export_sizes
   {
      protected override void SetExportValues()
      {
         // Test is validating the case when no values are set
      }
   }


   public class When_validating_and_the_width_is_too_large : When_validating_failing_export_sizes
   {
      protected override void SetExportValues()
      {
         sut.ChartWidth = 2001;
      }
   }

   public class When_validating_and_the_width_is_too_small : When_validating_failing_export_sizes
   {
      protected override void SetExportValues()
      {
         sut.ChartWidth = 199;
      }
   }

   public class When_validating_and_the_height_is_too_large : When_validating_failing_export_sizes
   {
      protected override void SetExportValues()
      {
         sut.ChartHeight = 2001;
      }
   }

   public class When_validating_and_the_height_is_too_small : When_validating_failing_export_sizes
   {
      protected override void SetExportValues()
      {
         sut.ChartHeight = 199;
      }
   }
}
