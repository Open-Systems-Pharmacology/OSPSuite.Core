using System.Collections.Generic;
using FakeItEasy;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Core.Domain.Mappers;
using OSPSuite.Core.Domain.UnitSystem;

namespace OSPSuite.Core.Mappers
{
   public abstract class concern_for_ReactionBuilderToReactionMapper : ContextSpecification<IReactionBuilderToReactionMapper>
   {
      protected IReactionPartnerBuilderToReactionPartnerMapper _reactionPartnerMapper;
      protected IObjectBaseFactory _objectBaseFactory;
      protected IFormulaBuilderToFormulaMapper _formulaMapper;
      protected IParameterBuilderCollectionToParameterCollectionMapper _parameterMapper;
      protected IReaction _reaction;
      protected IReactionBuilder _reactionBuilder;
      protected IReactionPartner _educt1;
      protected IReactionPartner _educt2;
      protected IReactionPartner _prod1;
      protected IReactionPartner _prod2;
      protected IContainer _container;
      protected string _modifier;
      protected IBuildConfiguration _buildConfiguration;
      protected IProcessRateParameterCreator _processRateParameterCreator;
      private IDimensionFactory _dimensionFactory;
      protected IDimension _amountPerTimeDimension;
      protected IDimension _concentrationPerTimeDimension;

      protected override void Context()
      {
         _amountPerTimeDimension = A.Fake<IDimension>();
         _concentrationPerTimeDimension = A.Fake<IDimension>();
         A.CallTo(() => _amountPerTimeDimension.Name).Returns(Constants.Dimension.AMOUNT_PER_TIME);
         A.CallTo(() => _concentrationPerTimeDimension.Name).Returns(Constants.Dimension.MOLAR_CONCENTRATION_PER_TIME);

         _reactionPartnerMapper = A.Fake<IReactionPartnerBuilderToReactionPartnerMapper>();
         _objectBaseFactory = A.Fake<IObjectBaseFactory>();
         _formulaMapper = A.Fake<IFormulaBuilderToFormulaMapper>();
         _parameterMapper = A.Fake<IParameterBuilderCollectionToParameterCollectionMapper>();
         _dimensionFactory= A.Fake<IDimensionFactory>();
         _modifier = "MOD";
         _reactionBuilder = A.Fake<IReactionBuilder>().WithName("MyReaction");
         _reactionBuilder.Dimension = _amountPerTimeDimension;
         _buildConfiguration = A.Fake<IBuildConfiguration>();
         _reactionBuilder.Formula = A.Fake<IFormula>();
         var edPartner1 = A.Fake<IReactionPartnerBuilder>();
         var edPartner2 = A.Fake<IReactionPartnerBuilder>();
         var prodPartner1 = A.Fake<IReactionPartnerBuilder>();
         var prodPartner2 = A.Fake<IReactionPartnerBuilder>();
         _educt1 = A.Fake<IReactionPartner>();
         _prod1 = A.Fake<IReactionPartner>();
         _educt2 = A.Fake<IReactionPartner>();
         _prod2 = A.Fake<IReactionPartner>();
         _container = A.Fake<IContainer>();
         A.CallTo(() => _dimensionFactory.Dimension(Constants.Dimension.AMOUNT_PER_TIME)).Returns(_amountPerTimeDimension);
         A.CallTo(() => _reactionPartnerMapper.MapFromLocal(edPartner1, _container, _buildConfiguration)).Returns(_educt1);
         A.CallTo(() => _reactionPartnerMapper.MapFromLocal(edPartner2, _container, _buildConfiguration)).Returns(_educt2);
         A.CallTo(() => _reactionPartnerMapper.MapFromLocal(prodPartner1, _container, _buildConfiguration)).Returns(_prod1);
         A.CallTo(() => _reactionPartnerMapper.MapFromLocal(prodPartner2, _container, _buildConfiguration)).Returns(_prod2);
         A.CallTo(() => _formulaMapper.MapFrom(_reactionBuilder.Formula, _buildConfiguration)).Returns(_reactionBuilder.Formula);
         A.CallTo(() => _reactionBuilder.Educts).Returns(new[] { edPartner1, edPartner2 });
         A.CallTo(() => _reactionBuilder.Products).Returns(new[] { prodPartner1, prodPartner2 });
         A.CallTo(() => _reactionBuilder.Parameters).Returns(new IParameter[0]);
         A.CallTo(() => _reactionBuilder.ModifierNames).Returns(new[] { _modifier });
         A.CallTo(() => _objectBaseFactory.Create<IReaction>()).Returns(new Reaction());
         A.CallTo(() => _parameterMapper.MapFrom(_reactionBuilder.Parameters, _buildConfiguration, ParameterBuildMode.Local)).Returns(new List<IParameter>());
         _processRateParameterCreator = new ProcessRateParameterCreator(_objectBaseFactory,_formulaMapper);
         sut = new ReactionBuilderToReactionMapper(_objectBaseFactory, _reactionPartnerMapper, _formulaMapper, _parameterMapper,_processRateParameterCreator);
      }
   }

   public class When_mapping_a_reaction_builder_to_a_reaction : concern_for_ReactionBuilderToReactionMapper
   {
      protected override void Because()
      {
         _reaction = sut.MapFromLocal(_reactionBuilder, _container,_buildConfiguration);
      }

      [Observation]
      public void should_return_a_reaction_with_the_educts_intitialized_as_specified_in_the_builder()
      {
         _reaction.Educts.ShouldOnlyContain(_educt1, _educt2);
      }

      [Observation]
      public void should_return_a_reaction_with_the_products_intitialized_as_specified_in_the_builder()
      {
         _reaction.Products.ShouldOnlyContain(_prod1, _prod2);
      }

      [Observation]
      public void should_have_set_the_name_of_the_reaction_to_the_name_of_the_builder()
      {
         _reaction.Name.ShouldBeEqualTo(_reactionBuilder.Name);
      }

      [Observation]
      public void should_have_updated_the_formula_according_to_the_defined_kinetic_in_the_builder()
      {
         _reaction.Formula.ShouldBeEqualTo(_reactionBuilder.Formula);
      }
    
      [Observation]
      public void should_return_a_reaction_with_the_modifiers_intitialized_as_specified_in_the_builder()
      {
         _reaction.ModifierNames.ShouldOnlyContain(_modifier);
      }

      [Observation]
      public void should_return_a_reaction_with_the_dimension_set_to_amount_per_time()
      {
         _reaction.Dimension.ShouldBeEqualTo(_amountPerTimeDimension);
      }
   }


   public class When_mapping_a_reaction_builder_to_a_reaction_for_which_a_parameter_rate_should_be_generated : concern_for_ReactionBuilderToReactionMapper
   {
      private IFormula _kinetic;
      private IParameter _processRateParameter;

      protected override void Context()
      {
         base.Context();
         _reactionBuilder.CreateProcessRateParameter = true;
         _kinetic = A.Fake<IFormula>();
         _reactionBuilder.CreateProcessRateParameter = true;
         _reactionBuilder.ProcessRateParameterPersistable = true;
         A.CallTo(() => _formulaMapper.MapFrom(_kinetic, _buildConfiguration)).ReturnsLazily(x => new ExplicitFormula("clone"));
         _reactionBuilder.Name = "Reaction";
         _reactionBuilder.Formula = _kinetic;
         A.CallTo(() => _parameterMapper.MapFrom(_reactionBuilder.Parameters, _buildConfiguration)).Returns(new List<IParameter>());
         _processRateParameter = new Parameter();
         A.CallTo(() => _objectBaseFactory.Create<IParameter>()).Returns(_processRateParameter);

      }

      protected override void Because()
      {
         _reaction = sut.MapFromLocal(_reactionBuilder,_container, _buildConfiguration);
         _processRateParameter = _reaction.GetSingleChildByName<IParameter>(Constants.Parameters.PROCESS_RATE);
      }

      [Observation]
      public void should_have_created_the_parameter()
      {
         _processRateParameter.ShouldNotBeNull();
      }

      [Observation]
      public void created_Parameter_should_be_persistable()
      {
         _processRateParameter.Persistable.ShouldBeTrue();
      }

   }
}