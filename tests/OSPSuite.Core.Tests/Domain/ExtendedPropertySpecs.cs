using System.ComponentModel;
using FakeItEasy;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;

namespace OSPSuite.Core.Domain
{
   public abstract class concern_for_ExtendedProperty<T> : ContextSpecification<ExtendedProperty<T>>
   {

   }

   public class When_list_of_typed_properties_is_bool : concern_for_ExtendedProperty<bool>
   {
      [Observation]
      public void the_list_of_objects_should_not_throw_an_exception()
      {
         sut = new ExtendedProperty<bool>();
         sut.ListOfValuesAsObjects.ShouldBeEmpty();
      }
   }

   public class When_the_property_has_a_display_name :  concern_for_ExtendedProperty<string>
   {
      private PropertyChangedEventHandler _onPropertyChangedHandler;

      protected override void Context()
      {
         base.Context();
         _onPropertyChangedHandler = A.Fake<PropertyChangedEventHandler>();
         // order here so that handler is not triggered for assignment of display name
         sut = new ExtendedProperty<string> {FullName = "TheDisplayName"};
         sut.PropertyChanged += _onPropertyChangedHandler;
      }

      protected override void Because()
      {
         sut.Name = "TheName";
      }

      [Observation]
      public void the_event_handler_should_be_called_for_name_and_not_for_display_name()
      {
         A.CallTo(() => _onPropertyChangedHandler(A<object>._, A<PropertyChangedEventArgs>.That.Matches(args => Equals(args.PropertyName, "DisplayName")))).MustNotHaveHappened();
         A.CallTo(() => _onPropertyChangedHandler(A<object>._, A<PropertyChangedEventArgs>.That.Matches(args => Equals(args.PropertyName, "Name")))).MustHaveHappened();
      }

      [Observation]
      public void the_display_name_property_should_return_the_display_name()
      {
         sut.DisplayName.ShouldBeEqualTo("TheDisplayName");
      }
   }

   public class When_the_property_does_not_have_a_display_name : concern_for_ExtendedProperty<string>
   {
      private PropertyChangedEventHandler _onPropertyChangedHandler;

      protected override void Context()
      {
         base.Context();
         _onPropertyChangedHandler = A.Fake<PropertyChangedEventHandler>();
         sut = new ExtendedProperty<string>();
         sut.PropertyChanged += _onPropertyChangedHandler;  
      }

      protected override void Because()
      {
         sut.Name = "TheName";
      }

      [Observation]
      public void the_event_handler_should_be_called_for_display_name_and_for_name()
      {
         A.CallTo(() => _onPropertyChangedHandler(A<object>._, A<PropertyChangedEventArgs>.That.Matches(args => Equals(args.PropertyName, "DisplayName")))).MustHaveHappened();
         A.CallTo(() => _onPropertyChangedHandler(A<object>._, A<PropertyChangedEventArgs>.That.Matches(args => Equals(args.PropertyName, "Name")))).MustHaveHappened();
      }

      [Observation]
      public void the_display_name_property_should_return_the_name()
      {
         sut.DisplayName.ShouldBeEqualTo("TheName");
      }
   }

   public class When_list_of_typed_properties_is_string : concern_for_ExtendedProperty<string>
   {
      protected override void Context()
      {
         base.Context();
         sut = new ExtendedProperty<string>();
         sut.AddToListOfValues("new");
         sut.AddToListOfValues("string");
         sut.AddToListOfValues("value");
         sut.Value = "new";
      }

      [Observation]
      public void the_list_should_contain_all_the_objects()
      {
         sut.ListOfValuesAsObjects.ShouldContain("new");
         sut.ListOfValuesAsObjects.ShouldContain("string");
         sut.ListOfValuesAsObjects.ShouldContain("value");
      }

      [Observation]
      public void the_cloned_property_should_have_the_same_value_and_the_same_list_of_values()
      {
         var extendedProperty = sut.Clone();
         extendedProperty.ValueAsObject.ShouldBeEqualTo("new");
         extendedProperty.ListOfValuesAsObjects.ShouldContain("new");
         extendedProperty.ListOfValuesAsObjects.ShouldContain("string");
         extendedProperty.ListOfValuesAsObjects.ShouldContain("value");
      }
   }
}
