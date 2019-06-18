using System.Linq;
using FakeItEasy;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Descriptors;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Core.Extensions;
using OSPSuite.Utility.Extensions;

namespace OSPSuite.Core
{
   public abstract class concern_for_SumFormula : ContextSpecification<SumFormula>
   {
      protected EntityDescriptorMapList<IFormulaUsable> _allFormulaUsable;
      protected IObjectPathFactory _objectPathFactory;
      private IParameter _fuParameter;
      protected IObjectBaseFactory _objectBaseFactory;

      protected override void Context()
      {
         var organism = new Container().WithName("Organism");
         _objectPathFactory = new ObjectPathFactory(new AliasCreator());
         _fuParameter = new Parameter().WithName("fu").WithValue(0.5);

         organism.Add(_fuParameter);
         var liver = new Container().WithName("Liver").WithParentContainer(organism);
         var liverVolume = new Parameter().WithName("Volume").WithValue(10).WithParentContainer(liver);
         var f_vas_liver = new Parameter().WithName("f_vas").WithValue(0.1);
         liver.Add(f_vas_liver);

         var kidney = new Container().WithName("Kidney").WithParentContainer(organism);
         var kidneyVolume = new Parameter().WithName("Volume").WithValue(20).WithParentContainer(kidney);
         kidney.Add(new Parameter().WithName("f_vas").WithValue(0.2));

         sut = new SumFormula();
         sut.Criteria = Create.Criteria(x => x.With("Volume"));
         sut.Variable = "V";
         sut.FormulaString = "fu * V_#i * f_vas_#i";
         sut.AddObjectPath(_objectPathFactory.CreateRelativeFormulaUsablePath(liverVolume, f_vas_liver).WithAlias("f_vas_#i"));
         sut.AddObjectPath(_objectPathFactory.CreateAbsoluteFormulaUsablePath(_fuParameter).WithAlias("fu"));

         sut.Dimension = new Dimension(new BaseDimensionRepresentation(), "dim1", "unit1");

         _allFormulaUsable = organism.GetAllChildren<IFormulaUsable>().ToEntityDescriptorMapList();

         _objectBaseFactory = A.Fake<IObjectBaseFactory>();
         A.CallTo(() => _objectBaseFactory.Create<ExplicitFormula>()).Returns(new ExplicitFormula());
      }
   }

   public class When_expanding_a_well_formed_sum_formula : concern_for_SumFormula
   {
      private ExplicitFormula _explicitFormula;

      protected override void Because()
      {
         _explicitFormula = sut.ExpandUsing(_allFormulaUsable, _objectPathFactory, _objectBaseFactory).DowncastTo<ExplicitFormula>();
      }

      [Observation]
      public void should_return_an_explicit_formula()
      {
         _explicitFormula.ShouldNotBeNull();
         _explicitFormula.FormulaString.ShouldBeEqualTo("fu * V_1 * f_vas_1 + fu * V_2 * f_vas_2");
         _explicitFormula.ObjectPaths.Count().ShouldBeEqualTo(5);
      }

      [Observation]
      public void dimension_of_expanded_formula_should_be_equal_to_dimension_of_orig_formula()
      {
         sut.Dimension.Equals(_explicitFormula.Dimension).ShouldBeTrue();
      }
   }

   public class When_expanding_a_well_formed_sum_formula_for_which_no_formula_usable_are_fulfilling_the_criteria : concern_for_SumFormula
   {
      private ExplicitFormula _explicitFormula;

      protected override void Context()
      {
         base.Context();
         sut.Criteria = Create.Criteria(x => x.With("does not exist"));
      }

      protected override void Because()
      {
         _explicitFormula = sut.ExpandUsing(_allFormulaUsable, _objectPathFactory, _objectBaseFactory).DowncastTo<ExplicitFormula>();
      }

      [Observation]
      public void should_return_an_explicit_formula_with_an_empty_formula()
      {
         _explicitFormula.ShouldNotBeNull();
         _explicitFormula.FormulaString.ShouldBeEqualTo("0");
      }
   }

   public class When_expanding_a_non_well_formed_sum_formula_for_which_a_relative_path_cannot_be_retrieved : concern_for_SumFormula
   {
      protected override void Context()
      {
         base.Context();
         sut.ClearObjectPaths();
         sut.AddObjectPath(_objectPathFactory.CreateFormulaUsablePathFrom("..", "wrong", "f_vas").WithAlias("f_vas_#i"));
      }

      [Observation]
      public void should_throw_an_exception()
      {
         The.Action(() => sut.ExpandUsing(_allFormulaUsable, _objectPathFactory, _objectBaseFactory)).ShouldThrowAn<UnableToResolvePathException>();
      }
   }

   public class When_setting_the_formula_string_of_a_sum_formula_with_the_extension : concern_for_SumFormula
   {
      private string _formulaString;

      protected override void Context()
      {
         base.Context();
         _formulaString = "tralala";
      }

      protected override void Because()
      {
         sut.WithFormulaString(_formulaString);
      }

      [Observation]
      public void should_have_set_the_formula_string_of_the_explicit_formula()
      {
         sut.FormulaString.ShouldBeEqualTo(_formulaString);
      }
   }

   public class When_validating_a_sum_formula_using_the_iteration_variable : concern_for_SumFormula
   {
      [Observation]
      public void should_return_that_the_formula_is_correct()
      {
         var (valid, message) = sut.IsValid();
         valid.ShouldBeTrue(message);
      }
   }

   public class When_validating_a_sum_formula_using_the_wrong_iteration_variable : concern_for_SumFormula
   {
      [Observation]
      public void should_return_that_the_formula_is_correct()
      {
         var (valid, message) = sut.IsValid("fu * P_#i * f_vas_#i");
         valid.ShouldBeFalse(message);
      }
   }
}