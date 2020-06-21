using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Presentation.Core;

namespace OSPSuite.Presentation.Presentation
{
   public abstract class concern_for_DefaultPresenterSettings : ContextSpecification<DefaultPresentationSettings>
   {
      protected override void Context()
      {
         sut = new DefaultPresentationSettings();
      }
   }

   public class when_using_boolean_extension_methods_for_cache : concern_for_DefaultPresenterSettings
   {
      protected override void Context()
      {
         base.Context();
         sut.SetTrue("registered_setting");
      }

      [Observation]
      public void should_be_according_to_setting_when_value_is_registered()
      {
         sut.IsTrue("registered_setting").ShouldBeTrue();
         sut.IsFalse("registered_setting").ShouldBeFalse();
      }

      [Observation]
      public void should_be_false_when_value_is_not_registered()
      {
         sut.IsFalse("non_registered_setting").ShouldBeTrue();
      }
   }

   public class when_testing_for_equality_of_non_existing_item_in_cache : concern_for_DefaultPresenterSettings
   {
      [Observation]
      public void should_be_true_for_string()
      {
         sut.IsEqual("setting_string", default(string)).ShouldBeTrue();
      }

      [Observation]
      public void should_be_true_for_bool()
      {
         sut.IsEqual("setting_bool", default(bool)).ShouldBeTrue();
      }
   }

   public enum ParameterGroupingModeId
   {
      Simple,
      Advanced
   }

   public class when_retrieving_enum_settings : concern_for_DefaultPresenterSettings
   {
      enum TestEnum
      {
         One,
         Two
      }

      protected override void Context()
      {
         base.Context();
         sut.SetSetting("name", TestEnum.Two);
      }

      [Observation]
      public void should_test_equal_on_the_equals_convenience_method_for_enum()
      {
         sut.IsEqual("name", TestEnum.Two);
      }

      [Observation]
      public void should_allow_retrieval_of_enum_type()
      {
         sut.GetSetting<TestEnum>("name").ShouldBeEqualTo(TestEnum.Two);
      }
   }

   public class when_retrieving_settings_that_have_not_been_initialized_with_a_value : concern_for_DefaultPresenterSettings
   {
      [Observation]
      public void should_return_the_default_value_and_add_to_cache()
      {
         sut.PresenterPropertyCache.Count.ShouldBeEqualTo(0);
         sut.GetSetting<string>("noProperty").ShouldBeEqualTo(default(string));
         sut.PresenterPropertyCache.Count.ShouldBeEqualTo(1);
      }
   }
}
