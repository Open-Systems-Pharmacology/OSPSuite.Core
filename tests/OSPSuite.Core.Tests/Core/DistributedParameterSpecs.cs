using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Formulas;

namespace OSPSuite.Core
{
   public abstract class concern_for_DistributedParameter : ContextSpecification<IDistributedParameter>
   {
      protected IParameter _percentileParameter;
      protected IParameter _meanParameter;
      protected IParameter _stdParameter;

      protected override void Context()
      {
         sut = new DistributedParameter();
         var pathFactory = new ObjectPathFactory(new AliasCreator());
         _meanParameter = new Parameter {Name = Constants.Distribution.MEAN}.WithFormula(new ExplicitFormula("0"));
         _stdParameter = new Parameter {Name = Constants.Distribution.DEVIATION}.WithFormula(new ExplicitFormula("1"));
         _percentileParameter = new Parameter {Name = Constants.Distribution.PERCENTILE}.WithFormula(new ExplicitFormula("0.5"));
         sut.Add(_meanParameter);
         sut.Add(_stdParameter);
         sut.Add(_percentileParameter);
         sut.Formula = new NormalDistributionFormula();
         sut.Formula.AddObjectPath(pathFactory.CreateRelativeFormulaUsablePath(sut, _meanParameter));
         sut.Formula.AddObjectPath(pathFactory.CreateRelativeFormulaUsablePath(sut, _stdParameter));
         sut.Formula.ResolveObjectPathsFor(sut);
      }
   }

   public class When_adding_a_parameter_to_a_distributed_parameter : concern_for_DistributedParameter
   {
      protected override void Because()
      {
         sut.Add(_meanParameter);
      }

      [Observation]
      public void Paremters_parentcontainer_should_be_sut()
      {
         _meanParameter.ParentContainer.ShouldBeEqualTo(sut);
      }
   }

   public class When_the_percentile_of_a_distribued_parameter_is_set : concern_for_DistributedParameter
   {
      private double _percentileValue;

      protected override void Because()
      {
         _percentileValue = 0.7;
         sut.Percentile = _percentileValue;
      }

      [Observation]
      public void the_percentile_value_should_be_equal_to_the_set_value()
      {
         sut.Percentile.ShouldBeEqualTo(_percentileValue);
      }

      [Observation]
      public void the_parameter_should_have_been_marked_as_having_a_fixed_value()
      {
         sut.IsFixedValue.ShouldBeTrue();
      }
   }

   public class When_the_value_of_a_distributed_parameter_is_set : concern_for_DistributedParameter
   {
      private double _valueToSet;

      protected override void Context()
      {
         base.Context();
         _valueToSet = 1.2;
      }

      protected override void Because()
      {
         sut.Value = _valueToSet;
      }

      [Observation]
      public void the_parameter_should_have_been_marked_as_having_a_fixed_value()
      {
         sut.IsFixedValue.ShouldBeTrue();
      }

      [Observation]
      public void the_value_returned_by_the_parameter_should_be_the_set_value_()
      {
         sut.Value.ShouldBeEqualTo(_valueToSet);
      }
   }

   public class When_a_percentile_is_set_an_then_a_value_is_set : concern_for_DistributedParameter
   {
      private double _valueToSet;

      protected override void Context()
      {
         base.Context();
         _valueToSet = 1.3;
         sut.Percentile = 0.5;
      }

      protected override void Because()
      {
         sut.Value = _valueToSet;
      }

      [Observation]
      public void the_value_returned_by_the_parameter_should_be_the_set_value()
      {
         sut.Value.ShouldBeEqualTo(_valueToSet);
      }
   }

   public class When_a_percentile_value_is_set_twice : concern_for_DistributedParameter
   {
      private double _valueBeforeSet;

      protected override void Context()
      {
         base.Context();
         sut.Percentile = 0.5;
         _valueBeforeSet = sut.Value;
      }

      protected override void Because()
      {
         sut.Percentile = 0.7;
      }

      [Observation]
      public void the_value_for_the_new_percentile_should_not_be_equal_to_the_value_calculated_for_the_first_percentile()
      {
         sut.Value.ShouldNotBeEqualTo(_valueBeforeSet);
      }
   }

   public class When_a_distributed_parameter_is_set_to_not_fixed : concern_for_DistributedParameter
   {
      protected override void Context()
      {
         base.Context();
         sut.Percentile = 0.5;
         sut.Value = 1.1;
      }

      protected override void Because()
      {
         sut.IsFixedValue = false;
      }

      [Observation]
      public void the_value_of_the_percentile_should_be_the_original_value()
      {
         sut.Percentile.ShouldBeEqualTo(0.5);
      }
   }
}