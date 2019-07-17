using FakeItEasy;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Core.Domain.Mappers;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Domain.UnitSystem;

namespace OSPSuite.Core.Mappers
{
   public abstract class concern_for_MoleculeBuilderToMoleculeAmountMapper : ContextSpecification<IMoleculeBuilderToMoleculeAmountMapper>
   {
      protected IFormulaBuilderToFormulaMapper _formulaMapper;
      protected IObjectBaseFactory _objectBaseFactory;
      protected IParameterBuilderCollectionToParameterCollectionMapper _parameterMapper;
      protected IKeywordReplacerTask _keywordReplacerTask;
      protected IFormulaFactory _formulaFactory;
      protected IDimensionFactory _dimensionFactory;
      protected IParameterFactory _parameterFactory;

      protected override void Context()
      {
         _objectBaseFactory = A.Fake<IObjectBaseFactory>();
         _formulaMapper = A.Fake<IFormulaBuilderToFormulaMapper>();
         _parameterMapper = A.Fake<IParameterBuilderCollectionToParameterCollectionMapper>();
         _keywordReplacerTask = A.Fake<IKeywordReplacerTask>();
         _formulaFactory = A.Fake<IFormulaFactory>();
         _dimensionFactory = A.Fake<IDimensionFactory>();
         _parameterFactory = A.Fake<IParameterFactory>();
         sut = new MoleculeBuilderToMoleculeAmountMapper(_objectBaseFactory, _formulaMapper, _parameterMapper, _dimensionFactory,
            _keywordReplacerTask, _formulaFactory,  _parameterFactory);
      }
   }

   public class When_mapping_a_molecule_builder_to_a_molecule_amount : concern_for_MoleculeBuilderToMoleculeAmountMapper
   {
      private IMoleculeBuilder _moleculeBuilder;
      private IMoleculeAmount _moleculeAmount;
      private IFormula _mappedFormula;
      private IParameter _para1;
      private IParameter _para2;
      private IBuildConfiguration _buildConfiguration;
      private IDimension _amountDimension;

      protected override void Context()
      {
         base.Context();
         _amountDimension = A.Fake<IDimension>();
         A.CallTo(() => _amountDimension.Name).Returns(Constants.Dimension.AMOUNT);
         _moleculeBuilder = A.Fake<IMoleculeBuilder>().WithName("tralala").WithDimension(_amountDimension);
         _mappedFormula = A.Fake<IFormula>();
         _buildConfiguration = A.Fake<IBuildConfiguration>();
         _para1 = A.Fake<IParameter>();
         _para2 = A.Fake<IParameter>();
         A.CallTo(() => _dimensionFactory.Dimension(Constants.Dimension.AMOUNT)).Returns(_amountDimension);
         A.CallTo(() => _formulaMapper.MapFrom(_moleculeBuilder.DefaultStartFormula, _buildConfiguration)).Returns(_mappedFormula);
         A.CallTo(() => _parameterMapper.MapLocalFrom(_moleculeBuilder, _buildConfiguration)).Returns(new[] {_para1, _para2});
      }

      protected override void Because()
      {
         _moleculeAmount = sut.MapFrom(_moleculeBuilder, _buildConfiguration);
      }

      [Observation]
      public void should_return_a_molecule_whose_formula_was_set_from_the_default_builder_formula()
      {
         _moleculeAmount.Formula.ShouldBeEqualTo(_mappedFormula);
      }

      [Observation]
      public void should_return_a_molecule_whose_name_was_set_to_the_name_of_the_builder()
      {
         _moleculeAmount.Name.ShouldBeEqualTo(_moleculeBuilder.Name);
      }

      [Observation]
      public void should_have_created_all_the_local_parameters_for_the_molecule()
      {
         A.CallTo(() => _moleculeAmount.Add(_para1)).MustHaveHappened();
         A.CallTo(() => _moleculeAmount.Add(_para2)).MustHaveHappened();
      }

      [Observation]
      public void should_add_the_builder_to_the_build_configuration_cache()
      {
         A.CallTo(() => _buildConfiguration.AddBuilderReference(_moleculeAmount, _moleculeBuilder)).MustHaveHappened();
      }

      [Observation]
      public void should_replace_keywords_including_molecule()
      {
         A.CallTo(() => _keywordReplacerTask.ReplaceIn(_moleculeAmount)).MustHaveHappened();
      }
   }

   public class When_mapping_a_molecule_builder_using_concentration_to_a_molecule_amount : concern_for_MoleculeBuilderToMoleculeAmountMapper
   {
      private IMoleculeBuilder _moleculeBuilder;
      private IMoleculeAmount _moleculeAmount;
      private IFormula _mappedFormula;
      private IBuildConfiguration _buildConfiguration;
      private IDimension _concentrationDimension;
      private IFormula _startValueReferenceFormula;

      protected override void Context()
      {
         base.Context();
         _concentrationDimension = A.Fake<IDimension>();
         _startValueReferenceFormula= A.Fake<IFormula>();
         A.CallTo(() => _concentrationDimension.Name).Returns(Constants.Dimension.MOLAR_CONCENTRATION);
         _moleculeBuilder = A.Fake<IMoleculeBuilder>().WithDimension(_concentrationDimension);
         _moleculeBuilder.DisplayUnit= A.Fake<Unit>();
         _mappedFormula = A.Fake<IFormula>();
         _buildConfiguration = A.Fake<IBuildConfiguration>();
         A.CallTo(() => _formulaMapper.MapFrom(_moleculeBuilder.DefaultStartFormula, _buildConfiguration)).Returns(_mappedFormula);
         A.CallTo(() => _objectBaseFactory.Create<IMoleculeAmount>()).ReturnsLazily(()=>new MoleculeAmount());
         var startValueParameter = new Parameter().WithName(Constants.Parameters.START_VALUE);
         A.CallTo(() => _parameterFactory.CreateStartValueParameter(A<IMoleculeAmount>._, _mappedFormula, _moleculeBuilder.DisplayUnit)).Returns(startValueParameter);
         A.CallTo(() => _formulaFactory.CreateMoleculeAmountReferenceToStartValue(startValueParameter)).Returns(_startValueReferenceFormula);
      }

      protected override void Because()
      {
         _moleculeAmount = sut.MapFrom(_moleculeBuilder, _buildConfiguration);
      }

      [Observation]
      public void should_return_a_molecule_whose_formula_was_set_from_the_updated_formula()
      {
         _moleculeAmount.Formula.ShouldBeEqualTo(_startValueReferenceFormula);
      }

      [Observation]
      public void should_have_added_the_start_value_parameter_to_the_molecule()
      {
         _moleculeAmount.ContainsName(Constants.Parameters.START_VALUE).ShouldBeTrue();
      }
   }
}