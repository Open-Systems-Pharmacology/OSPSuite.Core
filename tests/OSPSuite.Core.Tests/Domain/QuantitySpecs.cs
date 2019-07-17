using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;

namespace OSPSuite.Core.Domain
{
   public abstract class concern_for_Quantity : ContextSpecification<IQuantity>
   {
      protected override void Context()
      {
         sut = new TestQuantity();
      }

      private class TestQuantity : Quantity
      {
      }
   }

   
   public class When_setting_the_value_of_the_quantity : concern_for_Quantity
   {
      private double _value;
      private bool _valueChangedRaised;

      protected override void Context()
      {
         base.Context();
         _value = 3.2;
         sut.PropertyChanged += (o, e) =>
         {
            if (e.PropertyName.Equals("Value"))
               _valueChangedRaised = true;
         };
      }
      protected override void Because()
      {
         sut.Value = _value;
      }
      [Observation]
      public void should_set_is_fixed_value_property_to_true()
      {
         sut.IsFixedValue.ShouldBeTrue();
      }
      [Observation]
      public void Value_getter_should_return_the_value()
      {
         sut.Value.ShouldBeEqualTo(_value);
      }
      [Observation]
      public void should_throw_a_property_changed_event_for_Value()
      {
         _valueChangedRaised.ShouldBeTrue();
      }
   }
}	