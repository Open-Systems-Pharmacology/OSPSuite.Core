using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using FakeItEasy;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Core.Services;

namespace OSPSuite.Core
{
   public abstract class concern_for_ParameterFactory : ContextSpecification<IParameterFactory>
   {
      protected IDimensionFactory _dimensionFactory;
      protected IFormulaFactory _formulaFactory;
      private IObjectBaseFactory _objectBaseFactory;
      protected IConcentrationBasedFormulaUpdater _concentrationBasedFormulaUpdater;
      protected IDisplayUnitRetriever _displayUnitRetriever;

      protected override void Context()
      {
         _dimensionFactory = A.Fake<IDimensionFactory>();
         _formulaFactory = A.Fake<IFormulaFactory>();
         _objectBaseFactory = A.Fake<IObjectBaseFactory>();
         _concentrationBasedFormulaUpdater = A.Fake<IConcentrationBasedFormulaUpdater>();
         _displayUnitRetriever = A.Fake<IDisplayUnitRetriever>();
         sut = new ParameterFactory(_formulaFactory, _objectBaseFactory, _dimensionFactory, _concentrationBasedFormulaUpdater, _displayUnitRetriever);
      }
   }

   public class When_the_parameter_factory_is_asked_to_create_a_concentration_parameter_for_a_given_building_block : concern_for_ParameterFactory
   {
      private IParameter _parameter;
      private IFormula _concentrationFormula;
      private FormulaCache _formulaCache;

      protected override void Context()
      {
         base.Context();
         _concentrationFormula = A.Fake<IFormula>();
         _formulaCache = new FormulaCache();
         A.CallTo(() => _formulaFactory.ConcentrationFormulaFor(_formulaCache)).Returns(_concentrationFormula);
      }

      protected override void Because()
      {
         _parameter = sut.CreateConcentrationParameter(_formulaCache);
      }

      [Observation]
      public void should_return_a_parameter_named_concentration()
      {
         _parameter.Name.ShouldBeEqualTo(Constants.Parameters.CONCENTRATION);
      }

      [Observation]
      public void the_returned_parameter_should_use_a_concentration_formula()
      {
         _parameter.Formula.ShouldBeEqualTo(_concentrationFormula);
      }

      [Observation]
      public void the_returned_parameter_should_be_readonly()
      {
         _parameter.Info.ReadOnly.ShouldBeTrue();
      }

      [Observation]
      public void the_returned_parameter_should_have_the_mode_set_to_local()
      {
         _parameter.BuildMode.ShouldBeEqualTo(ParameterBuildMode.Local);
      }
   }

   public class When_the_parameter_factory_is_creating_a_start_value_parameter_for_a_given_molecule_amount_and_formula : concern_for_ParameterFactory
   {
      private IFormula _modelFormulaToUse;
      private IMoleculeAmount _moleculeAmount;
      private IParameter _parameter;

      protected override void Context()
      {
         base.Context();
         _modelFormulaToUse = A.Fake<IFormula>();
         _moleculeAmount = A.Fake<IMoleculeAmount>();
      }

      protected override void Because()
      {
         _parameter = sut.CreateStartValueParameter(_moleculeAmount, _modelFormulaToUse);
      }

      [Observation]
      public void should_create_a_parameter_using_the_dimension_of_the_given_formula()
      {
         _parameter.Dimension.ShouldBeEqualTo(_modelFormulaToUse.Dimension);
      }

      [Observation]
      public void should_create_a_parameter_named_start_value()
      {
         _parameter.Name.ShouldBeEqualTo(Constants.Parameters.START_VALUE);
      }

      [Observation]
      public void the_created_parameter_should_use_the_formula()
      {
         _parameter.Formula.ShouldBeEqualTo(_modelFormulaToUse);
      }

      [Observation]
      public void should_have_updated_the_formula_paths()
      {
         A.CallTo(() => _concentrationBasedFormulaUpdater.UpdateRelativePathForStartValueMolecule(_moleculeAmount, _modelFormulaToUse)).MustHaveHappened();
      }
   }

   public class When_creating_a_parameter_for_which_the_dimension_was_not_specified : concern_for_ParameterFactory
   {
      private IParameter _parameter;
      private IFormula _constantFormula;
      private Unit _displayUnit;

      protected override void Context()
      {
         base.Context();
         _displayUnit = A.Fake<Unit>();
         _constantFormula = new ConstantFormula(5);
         A.CallTo(() => _formulaFactory.ConstantFormula(5, _dimensionFactory.NoDimension)).Returns(_constantFormula);
         A.CallTo(() => _displayUnitRetriever.PreferredUnitFor(_dimensionFactory.NoDimension)).Returns(_displayUnit);
      }

      protected override void Because()
      {
         _parameter = sut.CreateParameter("TOTO", 5);
      }

      [Observation]
      public void should_use_the_no_dimension_dimension()
      {
         _parameter.Dimension.ShouldBeEqualTo(_dimensionFactory.NoDimension);
      }

      [Observation]
      public void should_use_the_preffered_display_unit_for_this_dimension()
      {
         _parameter.DisplayUnit.ShouldBeEqualTo(_displayUnit);
      }

      [Observation]
      public void should_set_the_expected_formula()
      {
         _parameter.Formula.ShouldBeEqualTo(_constantFormula);
      }
   }
}