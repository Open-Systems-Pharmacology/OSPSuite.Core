using OSPSuite.Assets;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain;

namespace OSPSuite.Core
{
   public abstract class concern_for_ValueOrigin : ContextSpecification<ValueOrigin>
   {
      protected override void Context()
      {
         sut = new ValueOrigin
         {
            Source = ValueOriginSources.Database,
            Method = ValueOriginDeterminationMethods.ManualFit,
            Description = "Hello",
            Default = true
         };
      }
   }

   public class When_cloning_a_value_origin : concern_for_ValueOrigin
   {
      private ValueOrigin _clone;

      protected override void Because()
      {
         _clone = sut.Clone();
      }

      [Observation]
      public void should_return_a_value_origin_having_the_same_properties_at_the_source_value_origin()
      {
         _clone.Source.ShouldBeEqualTo(sut.Source);
         _clone.Description.ShouldBeEqualTo(sut.Description);
         _clone.Method.ShouldBeEqualTo(sut.Method);
         _clone.Default.ShouldBeEqualTo(sut.Default);
      }
   }

   public class When_updating_a_value_origin_from_an_undefined_value_origin : concern_for_ValueOrigin
   {
      protected override void Because()
      {
         sut.UpdateFrom(null);
      }

      [Observation]
      public void should_not_update_the_value_origin()
      {
         sut.Source.ShouldBeEqualTo(ValueOriginSources.Database);
         sut.Description.ShouldBeEqualTo("Hello");
      }
   }

   public class When_updating_a_value_origin_from_a_well_defined_value_origin : concern_for_ValueOrigin
   {
      private ValueOrigin _sourceValueOrigin;

      protected override void Context()
      {
         base.Context();
         _sourceValueOrigin = new ValueOrigin
         {
            Description = "New description",
            Source = ValueOriginSources.Database,
            Method = ValueOriginDeterminationMethods.ManualFit,
            Default = true,
         };
      }

      protected override void Because()
      {
         sut.UpdateFrom(_sourceValueOrigin);
      }

      [Observation]
      public void should_update_the_value_origin()
      {
         sut.Source.ShouldBeEqualTo(_sourceValueOrigin.Source);
         sut.Description.ShouldBeEqualTo(_sourceValueOrigin.Description);
         sut.Method.ShouldBeEqualTo(_sourceValueOrigin.Method);
         sut.Default.ShouldBeEqualTo(_sourceValueOrigin.Default);
      }
   }

   public class When_returning_the_display_value_for_a_value_origin : concern_for_ValueOrigin
   {
      [Observation]
      public void should_return_undefined_if_description_and_type_are_not_set()
      {
         new ValueOrigin().Display.ShouldBeEqualTo(Captions.ValueOrigins.Undefined);
      }

      [Observation]
      public void should_return_the_type_if_only_type_is_set_()
      {
         new ValueOrigin { Source = ValueOriginSources.Internet}.Display.ShouldBeEqualTo(ValueOriginSources.Internet.Display);
      }

      [Observation]
      public void should_return_the_description_if_only_the_description_is_set()
      {
         new ValueOrigin { Description = "Hello" }.Display.ShouldBeEqualTo("Hello");
      }
      [Observation]
      public void should_return_a_combination_of_type_and_descriptiion_if_those_properties_are_set()
      {
         var display = new ValueOrigin {Source = ValueOriginSources.Internet, Description = "Hello"}.Display;
         display.Contains("Hello").ShouldBeTrue();
         display.Contains(ValueOriginSources.Internet.Display).ShouldBeTrue();
      }

   }
}