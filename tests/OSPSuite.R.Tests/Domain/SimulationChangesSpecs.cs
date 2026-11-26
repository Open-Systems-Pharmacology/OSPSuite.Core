using System.Linq;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Helpers;

namespace OSPSuite.R.Domain
{
   public abstract class concern_for_SimulationChanges : ContextSpecification<SimulationChanges>
   {
      protected IContainer _root;
      protected IParameter _constantParameter;
      protected IParameter _formulaParameter;
      protected IParameter _dependentParameter;
      protected MoleculeAmount _moleculeAmount;
      protected OriginalQuantityValue _originalQuantityValueBeforeLoad;

      protected override void Context()
      {
         _root = new Container().WithName("Root");
         _constantParameter = new Parameter { Name = "Constant", Formula = new ConstantFormula(1) };
         _formulaParameter = new Parameter { Name = "Formula", Formula = new ExplicitFormula("2") };

         var dependentFormula = new ExplicitFormula("Constant*3");
         dependentFormula.AddObjectPath(new FormulaUsablePath(ObjectPath.PARENT_CONTAINER, "Constant").WithAlias("Constant"));
         _dependentParameter = new Parameter { Name = "Dependent", Formula = dependentFormula };

         _moleculeAmount = new MoleculeAmount { Name = "Drug", Formula = new ConstantFormula(5), ScaleDivisor = 1 };
         _root.AddChildren(_constantParameter, _formulaParameter, _dependentParameter, _moleculeAmount);

         _originalQuantityValueBeforeLoad = new OriginalQuantityValue { Path = "Root|Other", Value = 4 };
         sut = new SimulationChanges(_root, new[] { _originalQuantityValueBeforeLoad }, hasUntraceableChanges: true);
      }
   }

   public class When_creating_the_changes_of_a_simulation : concern_for_SimulationChanges
   {
      [Observation]
      public void should_keep_the_changes_made_before_the_simulation_was_loaded()
      {
         sut.OriginalQuantityValuesBeforeLoad.ShouldOnlyContain(_originalQuantityValueBeforeLoad);
         sut.HasUntraceableChanges.ShouldBeTrue();
      }

      [Observation]
      public void should_not_report_any_change()
      {
         sut.ChangedQuantities().ShouldBeEmpty();
         sut.ChangedScaleDivisors().ShouldBeEmpty();
      }
   }

   public class When_values_of_a_simulation_are_changed : concern_for_SimulationChanges
   {
      protected override void Because()
      {
         _constantParameter.Value = 10;
         _formulaParameter.Value = 20;
         _moleculeAmount.Value = 50;
         _moleculeAmount.ScaleDivisor = 0.1;
      }

      [Observation]
      public void should_report_the_changed_quantities_with_the_values_they_had_initially()
      {
         sut.ChangedQuantities().ShouldOnlyContain<(IQuantity quantity, double initialValue)>((_constantParameter, 1), (_formulaParameter, 2), (_moleculeAmount, 5));
      }

      [Observation]
      public void should_not_report_a_formula_parameter_whose_value_changed_with_the_quantities_it_depends_on()
      {
         _dependentParameter.Value.ShouldBeEqualTo(30);
         sut.ChangedQuantities().Select(x => x.quantity).ShouldNotContain(_dependentParameter);
      }

      [Observation]
      public void should_report_the_changed_scale_divisor_with_its_initial_value()
      {
         sut.ChangedScaleDivisors().ShouldOnlyContain<(MoleculeAmount moleculeAmount, double initialScaleDivisor)>((_moleculeAmount, 1));
      }
   }

   public class When_values_of_a_simulation_are_set_back_to_their_initial_value : concern_for_SimulationChanges
   {
      protected override void Because()
      {
         _constantParameter.Value = 10;
         _constantParameter.Value = 1;
         _formulaParameter.Value = 2;
         _moleculeAmount.ScaleDivisor = 0.1;
         _moleculeAmount.ScaleDivisor = 1;
      }

      [Observation]
      public void should_not_report_any_change()
      {
         sut.ChangedQuantities().ShouldBeEmpty();
         sut.ChangedScaleDivisors().ShouldBeEmpty();
      }
   }

   public class When_an_overwritten_formula_parameter_is_reset_to_its_formula : concern_for_SimulationChanges
   {
      protected override void Context()
      {
         base.Context();
         _formulaParameter.Value = 20;
         sut = new SimulationChanges(_root, Enumerable.Empty<OriginalQuantityValue>(), hasUntraceableChanges: false);
      }

      protected override void Because()
      {
         _formulaParameter.IsFixedValue = false;
      }

      [Observation]
      public void should_report_the_parameter_with_the_value_it_had_initially()
      {
         sut.ChangedQuantities().ShouldOnlyContain<(IQuantity quantity, double initialValue)>((_formulaParameter, 20));
      }
   }

   public class When_the_value_of_a_distributed_parameter_is_changed : concern_for_SimulationChanges
   {
      private IDistributedParameter _distributedParameter;

      protected override void Context()
      {
         base.Context();
         _distributedParameter = DomainHelperForSpecs.NormalDistributedParameter(defaultMean: 10, defaultDeviation: 2).WithName("Distributed");
         _root.Add(_distributedParameter);
         sut = new SimulationChanges(_root, Enumerable.Empty<OriginalQuantityValue>(), hasUntraceableChanges: false);
      }

      protected override void Because()
      {
         _distributedParameter.Value = 12;
      }

      [Observation]
      public void should_report_the_distributed_parameter_with_the_value_of_its_initial_percentile_but_not_its_percentile()
      {
         sut.ChangedQuantities().ShouldOnlyContain<(IQuantity quantity, double initialValue)>((_distributedParameter, 10));
      }
   }

   public abstract class concern_for_SimulationChanges_with_a_circular_reference : ContextSpecification<SimulationChanges>
   {
      protected IContainer _root;
      protected IParameter _parameterA;
      protected IParameter _parameterB;

      protected override void Context()
      {
         _root = new Container().WithName("Root");
         _parameterA = new Parameter { Name = "A", Formula = formulaReferencing("B") };
         _parameterB = new Parameter { Name = "B", Formula = formulaReferencing("A") };
         _root.AddChildren(_parameterA, _parameterB);
      }

      private static ExplicitFormula formulaReferencing(string parameterName)
      {
         var formula = new ExplicitFormula(parameterName);
         formula.AddObjectPath(new FormulaUsablePath(ObjectPath.PARENT_CONTAINER, parameterName).WithAlias(parameterName));
         return formula;
      }
   }

   public class When_creating_the_changes_of_a_simulation_with_a_circular_reference : concern_for_SimulationChanges_with_a_circular_reference
   {
      protected override void Because()
      {
         sut = new SimulationChanges(_root, Enumerable.Empty<OriginalQuantityValue>(), hasUntraceableChanges: false);
      }

      [Observation]
      public void should_not_report_any_change()
      {
         sut.ChangedQuantities().ShouldBeEmpty();
      }
   }

   public class When_a_parameter_with_a_circular_reference_is_overwritten : concern_for_SimulationChanges_with_a_circular_reference
   {
      protected override void Context()
      {
         base.Context();
         sut = new SimulationChanges(_root, Enumerable.Empty<OriginalQuantityValue>(), hasUntraceableChanges: false);
      }

      protected override void Because()
      {
         _parameterA.Value = 5;
      }

      [Observation]
      public void should_report_the_parameter_without_an_initial_value()
      {
         sut.ChangedQuantities().ShouldOnlyContain<(IQuantity quantity, double initialValue)>((_parameterA, double.NaN));
      }
   }

   public class When_a_parameter_depending_on_a_circular_reference_is_overwritten : concern_for_SimulationChanges_with_a_circular_reference
   {
      private IParameter _dependentParameter;

      protected override void Context()
      {
         base.Context();
         var formula = new ExplicitFormula("A");
         formula.AddObjectPath(new FormulaUsablePath(ObjectPath.PARENT_CONTAINER, "A").WithAlias("A"));
         _dependentParameter = new Parameter { Name = "Dependent", Formula = formula };
         _root.Add(_dependentParameter);
         sut = new SimulationChanges(_root, Enumerable.Empty<OriginalQuantityValue>(), hasUntraceableChanges: false);
      }

      protected override void Because()
      {
         _dependentParameter.Value = 5;
      }

      [Observation]
      public void should_report_the_parameter_without_an_initial_value()
      {
         sut.ChangedQuantities().ShouldOnlyContain<(IQuantity quantity, double initialValue)>((_dependentParameter, double.NaN));
      }
   }
}
