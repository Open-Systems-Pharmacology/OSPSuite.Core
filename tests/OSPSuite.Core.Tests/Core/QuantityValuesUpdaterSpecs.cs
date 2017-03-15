using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using FakeItEasy;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Core.Domain.Services;

namespace OSPSuite.Core
{
   public abstract class concern_for_QuantityValuesUpdater : ContextSpecification<IQuantityValuesUpdater>
   {
      protected IConcentrationBasedFormulaUpdater _concentrationBasedFormulaUpdater;
      protected IKeywordReplacerTask _keywordReplacerTask;
      protected ICloneManagerForModel _cloneManagerForModel;
      protected IFormulaFactory _formulaFactory;
      protected IModel _model;
      protected IBuildConfiguration _buildConfiguration;

      protected override void Context()
      {
         _concentrationBasedFormulaUpdater = A.Fake<IConcentrationBasedFormulaUpdater>();
         _keywordReplacerTask = A.Fake<IKeywordReplacerTask>();
         _cloneManagerForModel = A.Fake<ICloneManagerForModel>();
         _formulaFactory = A.Fake<IFormulaFactory>();

         sut = new QuantityValuesUpdater(_keywordReplacerTask,_cloneManagerForModel,_formulaFactory,_concentrationBasedFormulaUpdater);

         _model= A.Fake<IModel>();
         _buildConfiguration= A.Fake<IBuildConfiguration>();
      }
   }

   public class When_updating_the_parameter_of_a_given_model_with_a_parameter_start_value_that_should_not_override_the_parameter_formula : concern_for_QuantityValuesUpdater
   {
      private IParameterStartValue _psv;
      private IFormula _explicitFormula;
      private Parameter _parameter;

      protected override void Context()
      {
         base.Context();
         _explicitFormula = new ExplicitFormula("1+2");
         _parameter = new Parameter().WithName("P").WithFormula(_explicitFormula);
         _model.Root = new Container {new Container {_parameter}.WithName("Organism")};
         _psv = new ParameterStartValue
         {
            OverrideFormulaWithValue = false,
            StartValue = 10,
            Path = new ObjectPath("Organism","P")
         };
         _buildConfiguration.ParameterStartValues = new ParameterStartValuesBuildingBlock {_psv};
         A.CallTo(() => _keywordReplacerTask.CreateModelPathFor(_psv.Path,_model.Root)).Returns(_psv.Path);

      }
      protected override void Because()
      {
         sut.UpdateQuantitiesValues(_model,_buildConfiguration);
      }

      [Observation]
      public void should_keep_the_formula_and_update_the_value()
      {
         _parameter.Formula.ShouldBeEqualTo(_explicitFormula);
         _parameter.Value.ShouldBeEqualTo(10);
         _parameter.IsFixedValue.ShouldBeTrue();
      }
   }

   public class When_updating_the_parameter_of_a_given_model_with_a_parameter_start_value_that_should_override_the_parameter_formula : concern_for_QuantityValuesUpdater
   {
      private IParameterStartValue _psv;
      private IFormula _explicitFormula;
      private Parameter _parameter;

      protected override void Context()
      {
         base.Context();
         _explicitFormula = new ExplicitFormula("1+2");
         _parameter = new Parameter().WithName("P").WithFormula(_explicitFormula);
         _model.Root = new Container { new Container { _parameter }.WithName("Organism") };
         _psv = new ParameterStartValue
         {
            OverrideFormulaWithValue = true,
            StartValue = 10,
            Path = new ObjectPath("Organism", "P")
         };
         _buildConfiguration.ParameterStartValues = new ParameterStartValuesBuildingBlock { _psv };
         A.CallTo(() => _keywordReplacerTask.CreateModelPathFor(_psv.Path, _model.Root)).Returns(_psv.Path);
         A.CallTo(() => _formulaFactory.ConstantFormula(10, _parameter.Dimension)).Returns(new ConstantFormula(10));

      }
      protected override void Because()
      {
         sut.UpdateQuantitiesValues(_model, _buildConfiguration);
      }

      [Observation]
      public void should_override_the_formula_with_a_constant_formula()
      {
         _parameter.Formula.IsConstant().ShouldBeTrue();
         _parameter.Value.ShouldBeEqualTo(10);
         _parameter.IsFixedValue.ShouldBeFalse();
      }
   }
}	