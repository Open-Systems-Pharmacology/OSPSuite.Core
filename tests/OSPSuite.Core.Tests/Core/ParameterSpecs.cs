using System.Globalization;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Core.Domain.UnitSystem;

namespace OSPSuite.Core
{
   public abstract class concern_for_Parameter : ContextSpecification<IParameter>
   {
      protected override void Context()
      {
         sut = new Parameter();
      }
   }

   public class When_setting_a_parameter_to_a_fixed_value_that_is_not_the_default_value_then_to_default_and_again_to_the_same_fixed_value : concern_for_Parameter
   {
      private int _originValue;
      private bool _valueChangedEventRaised;

      protected override void Context()
      {
         _originValue = 15;
         sut = new Parameter();
         sut.Formula = new ExplicitFormula(_originValue.ToString(new NumberFormatInfo()));
         sut.Value = 20;
         sut.IsFixedValue = false;

         sut.PropertyChanged += (o, e) =>
         {
            if (e.PropertyName.Equals("Value"))
               _valueChangedEventRaised = true;
         };
      }

      protected override void Because()
      {
         sut.Value = 20;
      }

      [Observation]
      public void the_value_changed_event_should_have_been_raised()
      {
         _valueChangedEventRaised.ShouldBeTrue();
      }
   }

   public class When_setting_a_parameter_to_a_fixed_value : concern_for_Parameter
   {
      private int _originValue;
      private double _raisedValue;
      private double _setValue;

      protected override void Context()
      {
         _originValue = 15;
         _setValue = 20;
         sut = new Parameter();
         sut.Formula = new ExplicitFormula(_originValue.ToString(new NumberFormatInfo()));
         sut.IsFixedValue = false;

         sut.PropertyChanged += (o, e) =>
         {
            if (e.PropertyName.Equals("Value"))
               _raisedValue = sut.Value;
         };
      }

      protected override void Because()
      {
         sut.Value = _setValue;
      }

      [Observation]
      public void the_value_changed_event_should_have_been_raised()
      {
         _raisedValue.ShouldBeEqualTo(_setValue);
      }
   }

   public class When_setting_the_value_of_a_parameter_used_in_the_formula_of_another_parameter_for_which_the_references_were_not_resolved : concern_for_Parameter
   {
      private Parameter _dependentParameter;
      private Container _container;

      protected override void Context()
      {
         _container = new Container();
         sut = new Parameter().WithName("MolWeight").WithParentContainer(_container);
         sut.Formula = new ConstantFormula(400);

         _dependentParameter = new Parameter().WithName("MolWeightEff").WithParentContainer(_container);
         _dependentParameter.Formula = new ExplicitFormula("MolWeight * 2");
         _dependentParameter.Formula.AddObjectPath(new FormulaUsablePath(new[] {"..", "MolWeight"}).WithAlias("MolWeight"));

         //Evaluate the value once to trigger caching
         var value = _dependentParameter.Value;
      }

      protected override void Because()
      {
         sut.Value = 800;
      }

      [Observation]
      public void should_always_be_able_to_retrieve_the_accurate_value_of_the_dependent_parameter()
      {
         _dependentParameter.Value.ShouldBeEqualTo(sut.Value * 2);
      }
   }

   public class When_changing_the_formula_of_a_parameter_with_a_fixed_value : concern_for_Parameter
   {
      protected override void Context()
      {
         base.Context();
         sut.Formula = new ConstantFormula(1);
         sut.IsFixedValue = true;
      }

      protected override void Because()
      {
         sut.Formula = new ConstantFormula(5);
      }

      [Observation]
      public void the_fixed_value_should_not_be_overwritten()
      {
         sut.Value.ShouldBeEqualTo(1);
      }
   }

   public class When_changing_the_formula_of_a_parameter_whose_value_was_cached : concern_for_Parameter
   {
      protected override void Context()
      {
         base.Context();
         sut.Formula = new ConstantFormula(1);
         //trigger the caching
         double value = sut.Value;
      }

      protected override void Because()
      {
         sut.Formula = new ConstantFormula(5);
      }

      [Observation]
      public void should_be_able_to_retrieve_the_accurate_value()
      {
         sut.Value.ShouldBeEqualTo(5);
      }
   }

   public class When_unfixing_a_parameter_with_a_fixed_value_whose_value_was_cached : concern_for_Parameter
   {
      protected override void Context()
      {
         base.Context();
         sut.Formula = new ConstantFormula(5);
         sut.IsFixedValue = true;
      }

      protected override void Because()
      {
         sut.IsFixedValue = false;
      }

      [Observation]
      public void should_be_able_to_retrieve_the_original_value()
      {
         sut.Value.ShouldBeEqualTo(5);
      }
   }

   public class When_setting_the_display_unit_for_a_parameter : concern_for_Parameter
   {
      private IDimension _dimension;
      private Unit _displayUnit;

      protected override void Context()
      {
         base.Context();
         _dimension = new Dimension(new BaseDimensionRepresentation {TimeExponent = 1}, "Time", "s");
         _displayUnit = _dimension.AddUnit("min", 2, 0);
         sut.Dimension = _dimension;
      }

      protected override void Because()
      {
         sut.DisplayUnit = _displayUnit;
      }

      [Observation]
      public void should_be_able_to_retrieve_the_dislay_unit()
      {
         sut.DisplayUnit.ShouldBeEqualTo(_displayUnit);
      }
   }

   public class When_retrieving_the_display_unit_for_a_parameter_for_which_the_display_unit_was_not_set : concern_for_Parameter
   {
      private IDimension _dimension;

      protected override void Context()
      {
         base.Context();
         _dimension = new Dimension(new BaseDimensionRepresentation {TimeExponent = 1}, "Time", "s");
         sut.Dimension = _dimension;
      }

      [Observation]
      public void should_return_the_default_unit_of_the_dimension()
      {
         sut.DisplayUnit.ShouldBeEqualTo(_dimension.DefaultUnit);
      }
   }
}