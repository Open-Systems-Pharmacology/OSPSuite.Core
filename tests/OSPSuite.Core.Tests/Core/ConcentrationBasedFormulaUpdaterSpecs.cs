using System.Linq;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using FakeItEasy;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Core.Extensions;

namespace OSPSuite.Core
{
   public abstract class concern_for_ConcentrationBasedFormulaUpdater : ContextSpecification<IConcentrationBasedFormulaUpdater>
   {
      protected ICloneManagerForModel _cloneManagerForModel;
      private IObjectBaseFactory _objectBaseFactory;
      private IDimensionFactory _dimensionFactory;
      protected IFormulaTask _formulaTask;

      protected override void Context()
      {
         _cloneManagerForModel = A.Fake<ICloneManagerForModel>();
         _objectBaseFactory = A.Fake<IObjectBaseFactory>();
         _dimensionFactory = A.Fake<IDimensionFactory>();
         _formulaTask = A.Fake<IFormulaTask>();
         sut = new ConcentrationBasedFormulaUpdater(_cloneManagerForModel, _objectBaseFactory, _dimensionFactory, _formulaTask);
      }
   }

   public class When_creating_an_amount_formula_for_a_formula_defined_in_a_molecule_amount : concern_for_ConcentrationBasedFormulaUpdater
   {
      private ExplicitFormula _concentrationFormula;
      private ExplicitFormula _amountFormula;

      protected override void Context()
      {
         base.Context();
         _concentrationFormula = new ExplicitFormula("A+B");
         A.CallTo(_formulaTask).WithReturnType<string>().Returns(Constants.VOLUME_ALIAS);
         A.CallTo(_cloneManagerForModel).WithReturnType<ExplicitFormula>().Returns(new ExplicitFormula("A+B"));
      }

      protected override void Because()
      {
         _amountFormula = sut.CreateAmountBaseFormulaFor(_concentrationFormula);
      }

      [Observation]
      public void should_add_a_reference_to_the_volume_of_the_container()
      {
         _amountFormula.FormulaString.ShouldBeEqualTo(string.Format("(A+B)*{0}", Constants.VOLUME_ALIAS));
      }
   }

   public class When_creating_an_amount_formula_for_a_formula_defined_in_a_molecule_amount_that_already_has_a_reference_to_a_volume_parameter : concern_for_ConcentrationBasedFormulaUpdater
   {
      private ExplicitFormula _amountFormula;
      private ExplicitFormula _concentrationFormula;

      protected override void Context()
      {
         base.Context();
         _concentrationFormula = new ExplicitFormula("A+B");
         A.CallTo(_cloneManagerForModel).WithReturnType<ExplicitFormula>().Returns(_concentrationFormula);
         A.CallTo(_formulaTask).WithReturnType<string>().Returns("TOTO");

      }

      protected override void Because()
      {
         _amountFormula = sut.CreateAmountBaseFormulaFor(_concentrationFormula);
      }

      [Observation]
      public void should_add_a_reference_to_the_volume_of_the_container()
      {
         _amountFormula.FormulaString.ShouldBeEqualTo("(A+B)*TOTO");
      }
   }

   public class When_creating_an_amount_formula_for_a_formula_defined_in_a_molecule_amount_that_already_has_a_reference_to_a_parameter_with_alias_V : concern_for_ConcentrationBasedFormulaUpdater
   {
      private ExplicitFormula _concentrationFormula;
      private ExplicitFormula _amountFormula;

      protected override void Context()
      {
         base.Context();
         _concentrationFormula = new ExplicitFormula("A+B");
         A.CallTo(_cloneManagerForModel).WithReturnType<ExplicitFormula>().Returns(_concentrationFormula);
         A.CallTo(_formulaTask).WithReturnType<string>().Returns("V1");
      }

      protected override void Because()
      {
         _amountFormula = sut.CreateAmountBaseFormulaFor(_concentrationFormula);
      }

      [Observation]
      public void should_add_a_reference_to_the_volume_of_the_container()
      {
         _amountFormula.FormulaString.ShouldBeEqualTo("(A+B)*V1");
      }
   }

   public class When_updating_the_references_used_in_a_start_value_formula : concern_for_ConcentrationBasedFormulaUpdater
   {
      private IFormula _formula;
      private IMoleculeAmount _molecule;

      protected override void Context()
      {
         base.Context();
         _formula = new ExplicitFormula();
         _molecule = new MoleculeAmount();
         _molecule.Add(new Parameter().WithName("k1"));

         _formula.AddObjectPath(new FormulaUsablePath(ObjectPath.PARENT_CONTAINER, "A"));
         _formula.AddObjectPath(new FormulaUsablePath("ORGANISM", "B"));
         _formula.AddObjectPath(new FormulaUsablePath("k1"));
      }

      protected override void Because()
      {
         sut.UpdateRelativePathForStartValueMolecule(_molecule, _formula);
      }

      [Observation]
      public void should_update_the_references_in_relative_paths()
      {
         shouldHaveObjectPath(ObjectPath.PARENT_CONTAINER, ObjectPath.PARENT_CONTAINER, "A");
         shouldHaveObjectPath(ObjectPath.PARENT_CONTAINER, "k1");
      }

      [Observation]
      public void should_not_update_the_references_in_absolute_paths()
      {
         shouldHaveObjectPath("ORGANISM", "B");
      }

      private void shouldHaveObjectPath(params string[] pathArray)
      {
         var pathAsString = pathArray.ToPathString();
         _formula.ObjectPaths.Any(x => x.PathAsString.Equals(pathAsString)).ShouldBeTrue();
      }
   }
}