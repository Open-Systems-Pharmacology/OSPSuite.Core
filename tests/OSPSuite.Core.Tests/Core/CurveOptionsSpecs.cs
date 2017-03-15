using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Chart;

namespace OSPSuite.Core
{
   public abstract class concern_for_CurveOptions : ContextSpecification<CurveOptions>
   {
      protected override void Context()
      {
         sut = new CurveOptions();
      }
   }

   public class When_setting_the_curve_options_from_another_curve_options : concern_for_CurveOptions
   {
      private bool _propertyChangedRaised;

      protected override void Context()
      {
         base.Context();
         _propertyChangedRaised = false;
         sut.PropertyChanged += (o, e) => { _propertyChangedRaised = true; };
      }

      protected override void Because()
      {
         sut.UpdateFrom(new CurveOptions());
      }

      [Observation]
      public void should_not_raise_any_property_changed_event()
      {
         _propertyChangedRaised.ShouldBeFalse();
      }
   }
}	