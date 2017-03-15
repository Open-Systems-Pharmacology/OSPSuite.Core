using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Extensions;

namespace OSPSuite.Core
{
   public abstract class concern_for_ExtendedPropertyStore : ContextSpecification<ExtendedPropertyStore<IParameter>>
   {
      protected ExtendedProperties _extendedProperties;

      protected override void Context()
      {
         _extendedProperties = new ExtendedProperties();
         sut = new ExtendedPropertyStore<IParameter>(_extendedProperties);
      }
   }

   public class When_setting_the_display_name_and_description : concern_for_ExtendedPropertyStore
   {
      protected override void Because()
      {
         sut.ConfigureProperty(x => x.GroupName, "fullName", "description");
      }

      [Observation]
      public void Observation()
      {
         sut.Property(x => x.GroupName).FullName.ShouldBeEqualTo("fullName");
         sut.Property(x => x.GroupName).Description.ShouldBeEqualTo("description");
      }
   }

   public class When_getting_a_property_from_the_store : concern_for_ExtendedPropertyStore
   {

      [Observation]
      public void when_the_property_already_exists()
      {
         sut.Set(x => x.GroupName, "groupName");
         sut.Property(x => x.GroupName).Value.ShouldBeEqualTo("groupName");
      }

      [Observation]
      public void when_the_property_does_not_exist()
      {
         var property = sut.Property(x => x.GroupName);
         string.IsNullOrEmpty(property.Value).ShouldBeTrue();
      }
   }

   public class When_setting_a_value_for_a_property_into_the_store_that_does_not_exist : concern_for_ExtendedPropertyStore
   {
      private string _description;

      protected override void Context()
      {
         base.Context();
         _description = "tralala";
      }

      protected override void Because()
      {
         sut.Set(param => param.Description, _description);
      }

      [Observation]
      public void should_add_the_property_to_the_store()
      {
         _extendedProperties.Contains("Description").ShouldBeTrue();
      }

      [Observation]
      public void should_store_the_value_for_that_property()
      {
         _extendedProperties["Description"].ValueAsObject.ShouldBeEqualTo(_description);
      }
   }

   public class When_setting_a_value_for_a_property_into_the_store_that_was_already_defined : concern_for_ExtendedPropertyStore
   {
      private string _description;

      protected override void Context()
      {
         base.Context();
         _description = "tralala";
         sut.Set(param => param.Description, "oldValue");
      }

      protected override void Because()
      {
         sut.Set(param => param.Description, _description);
      }

      [Observation]
      public void should_return_the_value_for_that_property()
      {
         sut.Get(param => param.Description).ShouldBeEqualTo(_description);
      }
   }

   public class When_getting_a_value_for_a_property_that_does_not_exist_in_the_store : concern_for_ExtendedPropertyStore
   {
      [Observation]
      public void should_return_the_default_value_for_that_property()
      {
         string.IsNullOrEmpty(sut.Get(param => param.Description)).ShouldBeTrue();
      }
   }

   public class When_getting_a_value_for_a_property_that_was_in_the_store : concern_for_ExtendedPropertyStore
   {
      protected override void Context()
      {
         base.Context();
         sut.Set(param => param.Description, "toto");
      }

      [Observation]
      public void should_return_the_default_value_for_that_property()
      {
         sut.Get(param => param.Description).ShouldBeEqualTo("toto");
      }
   }
}