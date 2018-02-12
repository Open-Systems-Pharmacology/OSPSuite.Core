using System;
using System.Linq;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Utility.Extensions;
using FakeItEasy;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Descriptors;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Core.Extensions;

namespace OSPSuite.Core
{
   public abstract class concern_for_FormulaTask : ContextSpecification<IFormulaTask>
   {
      protected IObjectBaseFactory _objectBaseFactory;
      private IObjectPathFactory _objectPathFactory;
      private IAliasCreator _aliasCreator;
      private IDimensionFactory _dimensionFactory;

      protected override void Context()
      {
         _objectBaseFactory = A.Fake<IObjectBaseFactory>();
         _objectPathFactory = new ObjectPathFactory(new AliasCreator());
         _aliasCreator = new AliasCreator();
         _dimensionFactory = A.Fake<IDimensionFactory>();
         sut = new FormulaTask(_objectPathFactory, _objectBaseFactory, _aliasCreator, _dimensionFactory);
      }
   }

   public class When_checking_if_two_formula_of_different_types_are_the_same : concern_for_FormulaTask
   {
      [Observation]
      public void should_return_false()
      {
         sut.FormulasAreTheSame(new ExplicitFormula(), new ConstantFormula()).ShouldBeFalse();
         sut.FormulasAreTheSame(new ExplicitFormula(), new NormalDistributionFormula()).ShouldBeFalse();
         sut.FormulasAreTheSame(new ConstantFormula(), new LogNormalDistributionFormula()).ShouldBeFalse();
         sut.FormulasAreTheSame(new LogNormalDistributionFormula(), new NormalDistributionFormula()).ShouldBeFalse();
         sut.FormulasAreTheSame(new ExplicitFormula(), new TableFormula()).ShouldBeFalse();
         sut.FormulasAreTheSame(new BlackBoxFormula(), new TableFormula()).ShouldBeFalse();
         sut.FormulasAreTheSame(new BlackBoxFormula(), new ConstantFormula()).ShouldBeFalse();
      }
   }

   public class When_checking_if_two_blackbox_formulas_are_the_same : concern_for_FormulaTask
   {
      [Observation]
      public void should_return_true()
      {
         sut.FormulasAreTheSame(new BlackBoxFormula(), new BlackBoxFormula()).ShouldBeTrue();
      }
   }

   public class When_checking_if_two_distributed_formula_having_the_same_distribution_type_are_the_same : concern_for_FormulaTask
   {
      [Observation]
      public void should_return_true()
      {
         sut.FormulasAreTheSame(new LogNormalDistributionFormula(), new LogNormalDistributionFormula()).ShouldBeTrue();
         sut.FormulasAreTheSame(new NormalDistributionFormula(), new NormalDistributionFormula()).ShouldBeTrue();
         sut.FormulasAreTheSame(new DiscreteDistributionFormula(), new DiscreteDistributionFormula()).ShouldBeTrue();
         sut.FormulasAreTheSame(new UniformDistributionFormula(), new UniformDistributionFormula()).ShouldBeTrue();
      }
   }

   public class When_checking_if_two_constant_formula_are_the_same : concern_for_FormulaTask
   {
      [Observation]
      public void should_return_true_if_the_value_are_the_same()
      {
         sut.FormulasAreTheSame(new ConstantFormula(5), new ConstantFormula(5)).ShouldBeTrue();
      }

      [Observation]
      public void should_return_false_if_the_value_are_not_the_same()
      {
         sut.FormulasAreTheSame(new ConstantFormula(5), new ConstantFormula(6)).ShouldBeFalse();
      }
   }

   public class When_checking_if_two_explicit_formula_using_the_same_formula_string_and_the_references_are_the_same : concern_for_FormulaTask
   {
      private ExplicitFormula _formula1;
      private ExplicitFormula _formula2;

      protected override void Context()
      {
         base.Context();
         _formula1 = new ExplicitFormula("A+B");
         _formula2 = new ExplicitFormula("A+B");
         _formula1.AddObjectPath(new FormulaUsablePath(new[] {"A", "B", "C"}) {Alias = "A"});
         _formula1.AddObjectPath(new FormulaUsablePath(new[] {"D", "E", "F"}) {Alias = "B"});

         _formula2.AddObjectPath(new FormulaUsablePath(new[] {"A", "B", "C"}) {Alias = "A"});
         _formula2.AddObjectPath(new FormulaUsablePath(new[] {"D", "E", "F"}) {Alias = "B"});
      }

      [Observation]
      public void should_return_true()
      {
         sut.FormulasAreTheSame(_formula1, _formula2).ShouldBeTrue();
      }
   }

   public class When_checking_if_two_explicit_formula_using_the_same_formula_string_but_different_references_are_the_same : concern_for_FormulaTask
   {
      private ExplicitFormula _formula1;
      private ExplicitFormula _formula2;

      protected override void Context()
      {
         base.Context();
         _formula1 = new ExplicitFormula("A+B");
         _formula2 = new ExplicitFormula("A+B");
         _formula1.AddObjectPath(new FormulaUsablePath(new[] {"A", "B", "C"}) {Alias = "A"});
         _formula1.AddObjectPath(new FormulaUsablePath(new[] {"D", "E", "F"}) {Alias = "B"});

         _formula2.AddObjectPath(new FormulaUsablePath(new[] {"A", "B", "C"}) {Alias = "A"});
         _formula2.AddObjectPath(new FormulaUsablePath(new[] {"E", "F", "F"}) {Alias = "B"});
      }

      [Observation]
      public void should_return_true()
      {
         sut.FormulasAreTheSame(_formula1, _formula2).ShouldBeFalse();
      }
   }

   public class When_checking_if_two_explicit_formula_using_diffent_formula_string_but_the_same_references_are_the_same : concern_for_FormulaTask
   {
      private ExplicitFormula _formula1;
      private ExplicitFormula _formula2;

      protected override void Context()
      {
         base.Context();
         _formula1 = new ExplicitFormula("A+B");
         _formula2 = new ExplicitFormula("A+C");
         _formula1.AddObjectPath(new FormulaUsablePath(new[] {"A", "B", "C"}) {Alias = "A"});
         _formula1.AddObjectPath(new FormulaUsablePath(new[] {"D", "E", "F"}) {Alias = "B"});

         _formula2.AddObjectPath(new FormulaUsablePath(new[] {"A", "B", "C"}) {Alias = "A"});
         _formula2.AddObjectPath(new FormulaUsablePath(new[] {"D", "E", "F"}) {Alias = "B"});
      }

      [Observation]
      public void should_return_true()
      {
         sut.FormulasAreTheSame(_formula1, _formula2).ShouldBeFalse();
      }
   }

   public class When_checking_if_two_explicit_formula_using_different_formula_string_and_different_references_are_the_same : concern_for_FormulaTask
   {
      private ExplicitFormula _formula1;
      private ExplicitFormula _formula2;

      protected override void Context()
      {
         base.Context();
         _formula1 = new ExplicitFormula("A+B");
         _formula2 = new ExplicitFormula("A+C");
         _formula1.AddObjectPath(new FormulaUsablePath(new[] {"A", "B", "C"}) {Alias = "A"});
         _formula1.AddObjectPath(new FormulaUsablePath(new[] {"D", "E", "F"}) {Alias = "B"});

         _formula2.AddObjectPath(new FormulaUsablePath(new[] {"A", "B", "C"}) {Alias = "A"});
      }

      [Observation]
      public void should_return_true()
      {
         sut.FormulasAreTheSame(_formula1, _formula2).ShouldBeFalse();
      }
   }

   public class When_checking_if_two_explicit_formula_using_the_same_formula_string_and_the_references_but_diffent_aliases_are_the_same : concern_for_FormulaTask
   {
      private ExplicitFormula _formula1;
      private ExplicitFormula _formula2;

      protected override void Context()
      {
         base.Context();
         _formula1 = new ExplicitFormula("A+B");
         _formula2 = new ExplicitFormula("A+B");
         _formula1.AddObjectPath(new FormulaUsablePath(new[] {"A", "B", "C"}) {Alias = "A"});
         _formula1.AddObjectPath(new FormulaUsablePath(new[] {"D", "E", "F"}) {Alias = "B"});

         _formula2.AddObjectPath(new FormulaUsablePath(new[] {"A", "B", "C"}) {Alias = "A"});
         _formula2.AddObjectPath(new FormulaUsablePath(new[] {"D", "E", "F"}) {Alias = "C"});
      }

      [Observation]
      public void should_return_true()
      {
         sut.FormulasAreTheSame(_formula1, _formula2).ShouldBeFalse();
      }
   }

   public class When_expanding_all_dynamic_formula_in_a_model : concern_for_FormulaTask
   {
      private IModel _model;
      private IParameter _parameter;
      private IParameter _p1;
      private IParameter _p2;

      protected override void Context()
      {
         base.Context();
         A.CallTo(() => _objectBaseFactory.Create<ExplicitFormula>()).Returns(new ExplicitFormula());
         _model = new Model();
         _p1 = new Parameter().WithName("p1").WithFormula(new ConstantFormula(1));
         _p1.AddTag("toto");
         _p2 = new Parameter().WithName("p2").WithFormula(new ConstantFormula(2));
         _p2.AddTag("toto");
         var root = new Container();
         _model.Root = root;
         _parameter = new Parameter().WithName("Dynamic");
         root.Add(_parameter);
         root.Add(_p1);
         root.Add(_p2);
         root.Add(new Parameter().WithName("p3").WithFormula(new ConstantFormula(3)));
         var sumFormula = new SumFormula();
         sumFormula.Criteria = Create.Criteria(x => x.With("toto"));
         _parameter.Formula = sumFormula;

         root.Add(_parameter);
      }

      protected override void Because()
      {
         sut.ExpandDynamicFormulaIn(_model);
      }

      [Observation]
      public void should_replace_all_dynamic_formula_with_the_corresponding_explicit_formula()
      {
         _parameter.Formula.IsExplicit().ShouldBeTrue();
      }

      [Observation]
      public void should_have_created_an_explicit_formula_that_is_the_sum_of_the_defined_parameers()
      {
         var explicitFormula = _parameter.Formula.DowncastTo<ExplicitFormula>();
         explicitFormula.FormulaString.ShouldBeEqualTo("P_1 + P_2");
         explicitFormula.ObjectPaths.Count().ShouldBeEqualTo(2);
      }
   }

   public class When_expanding_a_dynamic_formula__in_a_model_at_a_formula_useable_satisfying_their_own_criteria :
      concern_for_FormulaTask
   {
      private IModel _model;
      private IParameter _parameter;
      private IParameter _p1;
      private IParameter _p2;
      private Exception _exception;

      protected override void Context()
      {
         base.Context();
         A.CallTo(() => _objectBaseFactory.Create<ExplicitFormula>()).Returns(new ExplicitFormula());
         _model = new Model();
         _p1 = new Parameter().WithName("p1").WithFormula(new ConstantFormula(1));
         _p1.AddTag("toto");
         _p2 = new Parameter().WithName("p2").WithFormula(new ConstantFormula(2));
         _p2.AddTag("toto");
         var root = new Container();
         _model.Root = root;
         _parameter = new Parameter().WithName("Dynamic");
         root.Add(_parameter);
         root.Add(_p1);
         root.Add(_p2);
         root.Add(new Parameter().WithName("p3").WithFormula(new ConstantFormula(3)));

         var sumFormula = new SumFormula();
         sumFormula.Criteria = Create.Criteria(x => x.With("toto"));
         _parameter.Formula = sumFormula;
         _parameter.AddTag("toto");
         root.Add(_parameter);
      }

      protected override void Because()
      {
         try
         {
            sut.ExpandDynamicFormulaIn(_model);
         }
         catch (Exception e)
         {
            _exception = e;
         }
      }

      [Observation]
      public void should_throw_circular_reference_from_sum_formula_Exception()
      {
         _exception.ShouldBeAnInstanceOf<CircularReferenceInSumFormulaException>();
      }
   }

   public class When_adding_a_reference_to_the_volume_of_parent_container_for_a_formula_without_any_reference_to_volume : concern_for_FormulaTask
   {
      private IFormula _explicitFormula;
      private string _alias;

      protected override void Context()
      {
         base.Context();
         _explicitFormula = new ExplicitFormula();
      }

      protected override void Because()
      {
         _alias = sut.AddParentVolumeReferenceToFormula(_explicitFormula);
      }

      [Observation]
      public void should_simply_add_it_and_return_the_volume_alias()
      {
         _alias.ShouldBeEqualTo(Constants.VOLUME_ALIAS);
         var volumeReference = _explicitFormula.ObjectPaths.Find(x => x.Alias == Constants.VOLUME_ALIAS);
         volumeReference.ShouldNotBeNull();
         volumeReference.PathAsString.ShouldBeEqualTo(new[] {ObjectPath.PARENT_CONTAINER, Constants.Parameters.VOLUME}.ToPathString());
      }
   }

   public class When_adding_a_reference_to_the_volume_of_parent_container_for_an_existing_reference_to_volume : concern_for_FormulaTask
   {
      private IFormula _explicitFormula;
      private string _alias;

      protected override void Context()
      {
         base.Context();
         _explicitFormula = new ExplicitFormula();
         _explicitFormula.AddObjectPath(new FormulaUsablePath(ObjectPath.PARENT_CONTAINER, Constants.Parameters.VOLUME).WithAlias(Constants.VOLUME_ALIAS));
      }

      protected override void Because()
      {
         _alias = sut.AddParentVolumeReferenceToFormula(_explicitFormula);
      }

      [Observation]
      public void should_simply_use_it()
      {
         _alias.ShouldBeEqualTo(Constants.VOLUME_ALIAS);
      }
   }

   public class When_adding_a_reference_to_the_volume_of_parent_container_for_an_existing_reference_to_volume_but_with_a_different_path : concern_for_FormulaTask
   {
      private IFormula _explicitFormula;
      private string _alias;

      protected override void Context()
      {
         base.Context();
         _explicitFormula = new ExplicitFormula();
         //another path
         _explicitFormula.AddObjectPath(new FormulaUsablePath(ObjectPath.PARENT_CONTAINER, ObjectPath.PARENT_CONTAINER, Constants.Parameters.VOLUME, Constants.Parameters.VOLUME).WithAlias(Constants.VOLUME_ALIAS));
      }

      protected override void Because()
      {
         _alias = sut.AddParentVolumeReferenceToFormula(_explicitFormula);
      }

      [Observation]
      public void should_not_use_the_default_volume_alias()
      {
         _alias.ShouldNotBeEqualTo(Constants.VOLUME_ALIAS);
      }
   }

   public class When_validating_the_formula_origin_id : concern_for_FormulaTask
   {
      private IModel _model;
      private ExplicitFormula _rhsFormula1;
      private ExplicitFormula _rhsFormula2;

      protected override void Context()
      {
         base.Context();
         _model = new Model {Root = new Container()};
         var parameter1 = new Parameter().WithName("P1").WithFormula(new ConstantFormula(1));
         _rhsFormula1 = new ExplicitFormula("A+B").WithOriginId("ORIGIN");
         _rhsFormula1.AddObjectPath(new FormulaUsablePath("A", "B"));
         parameter1.RHSFormula = _rhsFormula1;

         var parameter2 = new Parameter().WithName("P2").WithFormula(new ConstantFormula(2));
         _rhsFormula2 = new ExplicitFormula("A+B").WithOriginId("ORIGIN");
         _rhsFormula2.AddObjectPath(new FormulaUsablePath("A", "C"));

         parameter2.RHSFormula = _rhsFormula2;

         _model.Root.Add(parameter1);
         _model.Root.Add(parameter2);
      }

      protected override void Because()
      {
         sut.CheckFormulaOriginIn(_model);
      }

      [Observation]
      public void should_reset_the_origin_id_of_rhs_formula_with_different_object_path()
      {
         _rhsFormula2.OriginId.ShouldBeEmpty();
      }
   }
}