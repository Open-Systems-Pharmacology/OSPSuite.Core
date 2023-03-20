using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Utility.Extensions;

namespace OSPSuite.Core.Domain.Mappers
{
   public interface IReactionBuilderToReactionMapper : ILocalMapper<IReactionBuilder, IContainer, IReaction>
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

      public IReaction MapFromLocal(IReactionBuilder reactionBuilder, IContainer container, SimulationConfiguration simulationConfiguration)
      {
         var reaction = _objectBaseFactory.Create<IReaction>()
            .WithName(reactionBuilder.Name)
            .WithDescription(reactionBuilder.Description)
            .WithIcon(reactionBuilder.Icon)
            .WithDimension(reactionBuilder.Dimension)
            .WithFormula(createReactionKinetic(reactionBuilder, simulationConfiguration));
         reactionBuilder.Educts.Each(reactionPartnerBuilder => reaction.AddEduct(_reactionPartnerMapper.MapFromLocal(reactionPartnerBuilder, container, simulationConfiguration)));
         reactionBuilder.Products.Each(reactionPartnerBuilder => reaction.AddProduct(_reactionPartnerMapper.MapFromLocal(reactionPartnerBuilder, container, simulationConfiguration)));
         reactionBuilder.ModifierNames.Each(reaction.AddModifier);

         reaction.AddChildren(_parameterMapper.MapLocalFrom(reactionBuilder, simulationConfiguration));

         if (reactionBuilder.CreateProcessRateParameter)
            reaction.Add(processRateParameterFor(reactionBuilder, simulationConfiguration));

         simulationConfiguration.AddBuilderReference(reaction, reactionBuilder);
         return reaction;
      }

      private IFormula createReactionKinetic(IReactionBuilder reactionBuilder, SimulationConfiguration simulationConfiguration)
      {
         return _formulaMapper.MapFrom(reactionBuilder.Formula, simulationConfiguration);
      }

      private IParameter processRateParameterFor(IReactionBuilder reactionBuilder, SimulationConfiguration simulationConfiguration)
      {
         return _processRateParameterCreator.CreateProcessRateParameterFor(reactionBuilder, simulationConfiguration);
      }
   }
}