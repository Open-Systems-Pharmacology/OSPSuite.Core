using Castle.Core.Internal;
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

   public class When_setting_properties : concern_for_CurveOptions
   {
      private string _propertyName;

      protected override void Context()
      {
         base.Context();
         sut.InterpolationMode = InterpolationModes.xLinear;
         sut.PropertyChanged += (o, e) => { _propertyName = e.PropertyName; };
      }

      [Observation]
      public void should_not_raise_property_change_event_if_the_value_has_not_changed()
      {
         sut.InterpolationMode = InterpolationModes.xLinear;
         _propertyName.IsNullOrEmpty().ShouldBeTrue();
      }

      [Observation]
      public void should_raise_property_change_event_if_the_value_has_changed()
      {
         sut.InterpolationMode = InterpolationModes.yLinear;
         _propertyName.ShouldBeEqualTo("InterpolationMode");
      }
   }
}