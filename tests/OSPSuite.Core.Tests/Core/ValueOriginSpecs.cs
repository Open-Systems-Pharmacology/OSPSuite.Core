using NUnit.Framework;
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
            Id = 4,
            Source = ValueOriginSources.Database,
            Method = ValueOriginDeterminationMethods.ManualFit,
            Description = "Hello",
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
      public void should_return_a_value_origin_having_the_same_id_as_the_original_value_origin()
      {
         _clone.Id.ShouldBeEqualTo(sut.Id);
      }

      [Observation]
      public void should_return_a_value_origin_having_the_same_properties_at_the_source_value_origin()
      {
         _clone.Source.ShouldBeEqualTo(sut.Source);
         _clone.Description.ShouldBeEqualTo(sut.Description);
         _clone.Method.ShouldBeEqualTo(sut.Method);
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
            Id = 10,
            Description = "New description",
            Source = ValueOriginSources.Database,
            Method = ValueOriginDeterminationMethods.ManualFit,
         };
      }

      protected override void Because()
      {
         sut.UpdateFrom(_sourceValueOrigin);
      }

      [Observation]
      public void should_not_update_the_value_origin()
      {
         sut.Id.ShouldNotBeEqualTo(_sourceValueOrigin.Id);
      }

      [Observation]
      public void should_update_the_value_origin()
      {
         sut.Source.ShouldBeEqualTo(_sourceValueOrigin.Source);
         sut.Description.ShouldBeEqualTo(_sourceValueOrigin.Description);
         sut.Method.ShouldBeEqualTo(_sourceValueOrigin.Method);
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
         new ValueOrigin {Source = ValueOriginSources.Internet}.Display.ShouldBeEqualTo(ValueOriginSources.Internet.Display);
      }

      [Observation]
      public void should_return_the_description_if_only_the_description_is_set()
      {
         new ValueOrigin {Description = "Hello"}.Display.ShouldBeEqualTo("Hello");
      }

      [Observation]
      public void should_return_a_combination_of_type_and_descriptiion_if_those_properties_are_set()
      {
         var display = new ValueOrigin {Source = ValueOriginSources.Internet, Description = "Hello"}.Display;
         display.Contains("Hello").ShouldBeTrue();
         display.Contains(ValueOriginSources.Internet.Display).ShouldBeTrue();
      }
   }

   public class When_comparing_value_origins : concern_for_ValueOrigin
   {
      private ValueOrigin _valueOrigin1;
      private ValueOrigin _valueOrigin2;

      protected override void Context()
      {
         base.Context();
         _valueOrigin1 = new ValueOrigin();
         _valueOrigin2 = new ValueOrigin();
         _valueOrigin1.Description = "Hello";
         _valueOrigin1.Source = ValueOriginSources.Database;
         _valueOrigin1.Method = ValueOriginDeterminationMethods.InVitro;
         _valueOrigin2.UpdateFrom(_valueOrigin1);
      }

      [Observation]
      public void should_return_comparable_if_they_are_both_undefined()
      {
         _valueOrigin1 = new ValueOrigin();
         _valueOrigin2 = new ValueOrigin();
         _valueOrigin1.CompareTo(_valueOrigin2).ShouldBeEqualTo(0);
      }

      [Observation]
      public void should_return_comparable_if_they_have_the_same_method_source_and_description()
      {
         _valueOrigin1.CompareTo(_valueOrigin2).ShouldBeEqualTo(0);
      }

      [Observation]
      public void should_return_a_difference_if_they_have_a_different_source()
      {
         _valueOrigin2.Source = ValueOriginSources.Internet;
         _valueOrigin1.CompareTo(_valueOrigin2).ShouldNotBeEqualTo(0);
      }

      [Observation]
      public void should_return_a_difference_if_they_have_a_different_method()
      {
         _valueOrigin2.Method = ValueOriginDeterminationMethods.ManualFit;
         _valueOrigin1.CompareTo(_valueOrigin2).ShouldNotBeEqualTo(0);
      }

      [Observation]
      public void should_return_a_difference_if_they_have_a_different_description()
      {
         _valueOrigin2.Description = "New";
         _valueOrigin1.CompareTo(_valueOrigin2).ShouldNotBeEqualTo(0);
      }
   }

   public class When_checking_for_equality_for_value_origins : concern_for_ValueOrigin
   {
      private ValueOrigin _valueOrigin1;
      private ValueOrigin _valueOrigin2;

      protected override void Context()
      {
         base.Context();
         _valueOrigin1 = new ValueOrigin();
         _valueOrigin2 = new ValueOrigin();
         _valueOrigin1.Description = "Hello";
         _valueOrigin1.Source = ValueOriginSources.Database;
         _valueOrigin1.Method = ValueOriginDeterminationMethods.InVitro;
         _valueOrigin2.UpdateFrom(_valueOrigin1);
      }

      [Observation]
      public void should_return_equal_if_they_are_both_undefined()
      {
         _valueOrigin1 = new ValueOrigin();
         _valueOrigin2 = new ValueOrigin();
         _valueOrigin1.ShouldBeEqualTo(_valueOrigin2);
      }

      [Observation]
      public void should_return_equal_if_they_have_the_same_method_source_and_description()
      {
         _valueOrigin1.ShouldBeEqualTo(_valueOrigin2);
      }

      [Observation]
      public void should_not_return_equal_if_they_have_different_source()
      {
         _valueOrigin1.Source = ValueOriginSources.Internet;
         _valueOrigin1.ShouldNotBeEqualTo(_valueOrigin2);
      }

      [Observation]
      public void should_not_return_equal_if_they_have_different_method()
      {
         _valueOrigin1.Method = ValueOriginDeterminationMethods.InVivo;
         _valueOrigin1.ShouldNotBeEqualTo(_valueOrigin2);
      }

      [Observation]
      public void should_not_return_equal_if_they_have_different_description()
      {
         _valueOrigin1.Description = "New description";
         _valueOrigin1.ShouldNotBeEqualTo(_valueOrigin2);
      }
   }

   public class When_checking_for_equality_between_two_value_origins : concern_for_ValueOrigin
   {
      private ValueOrigin _sourceValueOrigin;
      private ValueOrigin _targetValueOrigin;

      protected override void Context()
      {
         base.Context();
         _sourceValueOrigin = new ValueOrigin();
         _targetValueOrigin = new ValueOrigin();

         _sourceValueOrigin.Source = ValueOriginSources.Database;
         _sourceValueOrigin.Method = ValueOriginDeterminationMethods.InVitro;
         _sourceValueOrigin.Description = "Hello";
      }

      [Observation]
      [TestCase(ValueOriginSourceId.Database, ValueOriginDeterminationMethodId.InVitro, "Hello", true)]
      [TestCase(ValueOriginSourceId.Database, ValueOriginDeterminationMethodId.InVitro, "NOT_SATE", false)]
      [TestCase(ValueOriginSourceId.Database, ValueOriginDeterminationMethodId.ManualFit, "Hello", false)]
      [TestCase(ValueOriginSourceId.Internet, ValueOriginDeterminationMethodId.InVitro, "Hello", false)]
      public void should_return_true_if_they_have_the_same_method_source_description_and_false_otherwise(ValueOriginSourceId source, ValueOriginDeterminationMethodId method, string description, bool equal)
      {
         _targetValueOrigin.Source = ValueOriginSources.ById(source);
         _targetValueOrigin.Method = ValueOriginDeterminationMethods.ById(method);
         _targetValueOrigin.Description = description;
         _sourceValueOrigin.Equals(_targetValueOrigin).ShouldBeEqualTo(equal);
      }

      [Observation]
      public void shoud_return_true_if_they_are_both_undefined()
      {
         _sourceValueOrigin = new ValueOrigin();
         _sourceValueOrigin.Equals(_targetValueOrigin).ShouldBeTrue();
      }
   }

   public class When_retrieving_the_unknown_value_origin : concern_for_ValueOrigin
   {
      private ValueOrigin _unknownValueOrigin;

      protected override void Because()
      {
         _unknownValueOrigin = ValueOrigin.Unknown;
      }

      [Observation]
      public void should_return_a_value_origin_with_an_undefined_source()
      {
         _unknownValueOrigin.Source.ShouldBeEqualTo(ValueOriginSources.Unknown);
      }

      [Observation]
      public void should_return_a_value_origin_with_an_unknown_method()
      {
         _unknownValueOrigin.Method.ShouldBeEqualTo(ValueOriginDeterminationMethods.Undefined);
      }

      [Observation]
      public void should_return_a_value_origin_with_an_empty_description()
      {
         string.IsNullOrEmpty(_unknownValueOrigin.Description).ShouldBeTrue();
      }
   }
}