using OSPSuite.Utility.Extensions;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Formulas;

namespace OSPSuite.Core.Domain.Mappers
{
   public interface IReactionBuilderToReactionMapper : ILocalMapper<IReactionBuilder, IReaction, IContainer>
   {
   }

   /// <summary>
   ///    Mapper for the creation of a Reaction in the Model
   /// </summary>
   internal class ReactionBuilderToReactionMapper : IReactionBuilderToReactionMapper
   {
      private readonly IReactionPartnerBuilderToReactionPartnerMapper _reactionPartnerMapper;
      private readonly IFormulaBuilderToFormulaMapper _formulaMapper;
      private readonly IObjectBaseFactory _objectBaseFactory;
      private readonly IParameterBuilderCollectionToParameterCollectionMapper _parameterMapper;
      private readonly IProcessRateParameterCreator _processRateParameterCreator;

      public ReactionBuilderToReactionMapper(IObjectBaseFactory objectBaseFactory,
         IReactionPartnerBuilderToReactionPartnerMapper reactionPartnerMapper,
         IFormulaBuilderToFormulaMapper formulaMapper, IParameterBuilderCollectionToParameterCollectionMapper parameterMapper,
         IProcessRateParameterCreator processRateParameterCreator)
      {
         _reactionPartnerMapper = reactionPartnerMapper;
         _formulaMapper = formulaMapper;
         _objectBaseFactory = objectBaseFactory;

         _parameterMapper = parameterMapper;
         _processRateParameterCreator = processRateParameterCreator;
      }

      public IReaction MapFromLocal(IReactionBuilder reactionBuilder, IContainer container, IBuildConfiguration buildConfiguration)
      {
         var reaction = _objectBaseFactory.Create<IReaction>()
            .WithName(reactionBuilder.Name)
            .WithDescription(reactionBuilder.Description)
            .WithIcon(reactionBuilder.Icon)
            .WithDimension(reactionBuilder.Dimension)
            .WithFormula(createReactionKinetic(reactionBuilder, buildConfiguration));
         reactionBuilder.Educts.Each(reactionPartnerBuilder => reaction.AddEduct(_reactionPartnerMapper.MapFromLocal(reactionPartnerBuilder, container, buildConfiguration)));
         reactionBuilder.Products.Each(reactionPartnerBuilder => reaction.AddProduct(_reactionPartnerMapper.MapFromLocal(reactionPartnerBuilder, container, buildConfiguration)));
         reactionBuilder.ModifierNames.Each(reaction.AddModifier);
         
         reaction.AddChildren(_parameterMapper.MapLocalFrom(reactionBuilder, buildConfiguration));
   
         if (reactionBuilder.CreateProcessRateParameter)
            reaction.Add(processRateParameterFor(reactionBuilder, buildConfiguration));

         buildConfiguration.AddBuilderReference(reaction, reactionBuilder);
         return reaction;
      }

      private IFormula createReactionKinetic(IReactionBuilder reactionBuilder, IBuildConfiguration buildConfiguration)
      {
         return _formulaMapper.MapFrom(reactionBuilder.Formula, buildConfiguration);
      }

      private IParameter processRateParameterFor(IReactionBuilder reactionBuilder, IBuildConfiguration buildConfiguration)
      {
         return _processRateParameterCreator.CreateProcessRateParameterFor(reactionBuilder, buildConfiguration);
      }
   }
}