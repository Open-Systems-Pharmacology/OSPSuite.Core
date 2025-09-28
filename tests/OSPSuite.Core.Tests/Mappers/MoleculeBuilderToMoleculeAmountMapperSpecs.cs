using FakeItEasy;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Descriptors;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Core.Domain.Mappers;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Helpers;

namespace OSPSuite.Core.Mappers
{
   internal abstract class concern_for_MoleculeBuilderToMoleculeAmountMapper : ContextSpecification<IMoleculeBuilderToMoleculeAmountMapper>
   {
      protected IFormulaBuilderToFormulaMapper _formulaMapper;
      protected IObjectBaseFactory _objectBaseFactory;
      protected IParameterBuilderToParameterMapper _parameterMapper;
      protected IFormulaFactory _formulaFactory;
      protected IDimensionFactory _dimensionFactory;
      protected IParameterFactory _parameterFactory;
      protected SimulationConfiguration _simulationConfiguration;
      protected IEntityTracker _entityTracker;

      protected override void Context()
      {
         _objectBaseFactory = A.Fake<IObjectBaseFactory>();
         _formulaMapper = A.Fake<IFormulaBuilderToFormulaMapper>();
         _parameterMapper = A.Fake<IParameterBuilderToParameterMapper>();
         _formulaFactory = A.Fake<IFormulaFactory>();
         _dimensionFactory = A.Fake<IDimensionFactory>();
         _parameterFactory = A.Fake<IParameterFactory>();
         _entityTracker = A.Fake<IEntityTracker>();
         sut = new MoleculeBuilderToMoleculeAmountMapper(_objectBaseFactory, _formulaMapper, _parameterMapper, _dimensionFactory, _formulaFactory, _parameterFactory, _entityTracker);

         _simulationConfiguration = new SimulationConfiguration();
      }
   }

   internal class When_mapping_a_molecule_builder_to_a_molecule_amount : concern_for_MoleculeBuilderToMoleculeAmountMapper
   {
      private MoleculeBuilder _moleculeBuilder;
      private MoleculeAmount _moleculeAmount;
      private IFormula _mappedFormula;
      private IParameter _para1;
      private IParameter _para2;
      private IParameter _para3;
      private IDimension _amountDimension;
      private IContainer _targetContainer;
      private IParameter _parameterBuilder1;
      private IParameter _parameterBuilder2;
      private IParameter _parameterBuilder3;
      private SimulationBuilder _simulationBuilder;

      protected override void Context()
      {
         base.Context();
         _amountDimension = A.Fake<IDimension>();
         _targetContainer = new Container().WithName("TOTO");
         A.CallTo(() => _amountDimension.Name).Returns(Constants.Dimension.MOLAR_AMOUNT);
         _moleculeBuilder = new MoleculeBuilder().WithName("tralala").WithDimension(_amountDimension);
         _parameterBuilder1 = DomainHelperForSpecs.ConstantParameterWithValue(1).WithName("P1").WithMode(ParameterBuildMode.Local);
         _parameterBuilder2 = DomainHelperForSpecs.ConstantParameterWithValue(1).WithName("P2").WithMode(ParameterBuildMode.Local);
         _parameterBuilder3 = DomainHelperForSpecs.ConstantParameterWithValue(1).WithName("P3").WithMode(ParameterBuildMode.Local);

         _parameterBuilder2.ContainerCriteria = Create.Criteria(x => x.With("TOTO"));
         _parameterBuilder3.ContainerCriteria = Create.Criteria(x => x.With("TATA"));

         _moleculeBuilder.AddParameter(_parameterBuilder1);
         _moleculeBuilder.AddParameter(_parameterBuilder2);
         _moleculeBuilder.AddParameter(_parameterBuilder3);
         _mappedFormula = A.Fake<IFormula>();
         _para1 = new Parameter().WithName("P1");
         _para2 = new Parameter().WithName("P2");
         _para3 = new Parameter().WithName("P3");
         A.CallTo(() => _objectBaseFactory.Create<MoleculeAmount>()).Returns(new MoleculeAmount());
         A.CallTo(() => _dimensionFactory.Dimension(Constants.Dimension.MOLAR_AMOUNT)).Returns(_amountDimension);
         _simulationBuilder = A.Fake<SimulationBuilder>();
         A.CallTo(() => _formulaMapper.MapFrom(_moleculeBuilder.DefaultStartFormula, _simulationBuilder)).Returns(_mappedFormula);
         A.CallTo(() => _parameterMapper.MapFrom(_parameterBuilder1, _simulationBuilder)).Returns(_para1);
         A.CallTo(() => _parameterMapper.MapFrom(_parameterBuilder2, _simulationBuilder)).Returns(_para2);
         A.CallTo(() => _parameterMapper.MapFrom(_parameterBuilder3, _simulationBuilder)).Returns(_para3);
      }

      protected override void Because()
      {
         _moleculeAmount = sut.MapFrom(_moleculeBuilder, _targetContainer, _simulationBuilder);
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
      public void should_have_created_all_the_local_parameters_for_the_molecule_matching_the_criteria()
      {
         _moleculeAmount.Children.ShouldContain(_para1, _para2);
         _moleculeAmount.Children.ShouldNotContain(_para3);
      }

      [Observation]
      public void should_add_the_builder_to_the_build_configuration_cache()
      {
         A.CallTo(() => _entityTracker.Track(_moleculeAmount, _moleculeBuilder, _simulationBuilder)).MustHaveHappened();
      }
   }

   internal class When_mapping_a_molecule_builder_using_concentration_to_a_molecule_amount : concern_for_MoleculeBuilderToMoleculeAmountMapper
   {
      private MoleculeBuilder _moleculeBuilder;
      private MoleculeAmount _moleculeAmount;
      private IFormula _mappedFormula;
      private IDimension _concentrationDimension;
      private IFormula _startValueReferenceFormula;
      private IContainer _targetContainer;
      private SimulationBuilder _simulationBuilder;

      protected override void Context()
      {
         base.Context();
         _targetContainer = new Container();
         _concentrationDimension = A.Fake<IDimension>();
         _startValueReferenceFormula = A.Fake<IFormula>();
         A.CallTo(() => _concentrationDimension.Name).Returns(Constants.Dimension.MOLAR_CONCENTRATION);
         _moleculeBuilder = new MoleculeBuilder().WithDimension(_concentrationDimension);
         _moleculeBuilder.DisplayUnit = A.Fake<Unit>();
         _mappedFormula = A.Fake<IFormula>();
         _simulationBuilder = A.Fake<SimulationBuilder>();
         A.CallTo(() => _formulaMapper.MapFrom(_moleculeBuilder.DefaultStartFormula, _simulationBuilder)).Returns(_mappedFormula);
         A.CallTo(() => _objectBaseFactory.Create<MoleculeAmount>()).ReturnsLazily(() => new MoleculeAmount());
         var startValueParameter = new Parameter().WithName(Constants.Parameters.START_VALUE);
         A.CallTo(() => _parameterFactory.CreateStartValueParameter(A<MoleculeAmount>._, _mappedFormula, _moleculeBuilder.DisplayUnit)).Returns(startValueParameter);
         A.CallTo(() => _formulaFactory.CreateMoleculeAmountReferenceToStartValue(startValueParameter)).Returns(_startValueReferenceFormula);
      }

      protected override void Because()
      {
         _moleculeAmount = sut.MapFrom(_moleculeBuilder, _targetContainer, _simulationBuilder);
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