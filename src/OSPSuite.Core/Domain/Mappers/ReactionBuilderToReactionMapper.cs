using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Utility.Extensions;

namespace OSPSuite.Core.Domain.Mappers
{
   public interface IReactionBuilderToReactionMapper : ILocalMapper<ReactionBuilder, IContainer, Reaction>
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
      private readonly IObjectTracker _objectTracker;

      public ReactionBuilderToReactionMapper(IObjectBaseFactory objectBaseFactory,
         IReactionPartnerBuilderToReactionPartnerMapper reactionPartnerMapper,
         IFormulaBuilderToFormulaMapper formulaMapper,
         IParameterBuilderCollectionToParameterCollectionMapper parameterMapper,
         IProcessRateParameterCreator processRateParameterCreator,
         IObjectTracker objectTracker)
      {
         _reactionPartnerMapper = reactionPartnerMapper;
         _formulaMapper = formulaMapper;
         _objectBaseFactory = objectBaseFactory;

         _parameterMapper = parameterMapper;
         _processRateParameterCreator = processRateParameterCreator;
         _objectTracker = objectTracker;
      }

      public Reaction MapFromLocal(ReactionBuilder reactionBuilder, IContainer container, SimulationBuilder simulationBuilder)
      {
         var reaction = _objectBaseFactory.Create<Reaction>()
            .WithName(reactionBuilder.Name)
            .WithDescription(reactionBuilder.Description)
            .WithIcon(reactionBuilder.Icon)
            .WithDimension(reactionBuilder.Dimension)
            .WithFormula(createReactionKinetic(reactionBuilder, simulationBuilder));
         reactionBuilder.Educts.Each(reactionPartnerBuilder => reaction.AddEduct(_reactionPartnerMapper.MapFromLocal(reactionPartnerBuilder, container, simulationBuilder)));
         reactionBuilder.Products.Each(reactionPartnerBuilder => reaction.AddProduct(_reactionPartnerMapper.MapFromLocal(reactionPartnerBuilder, container, simulationBuilder)));
         reactionBuilder.ModifierNames.Each(reaction.AddModifier);

         reaction.AddChildren(_parameterMapper.MapLocalFrom(reactionBuilder, simulationBuilder));

         if (reactionBuilder.CreateProcessRateParameter || simulationBuilder.CreateAllProcessRateParameters)
            reaction.Add(processRateParameterFor(reactionBuilder, simulationBuilder));

         simulationBuilder.AddBuilderReference(reaction, reactionBuilder);
         _objectTracker.TrackObject(reaction, reactionBuilder, simulationBuilder);

         return reaction;
      }

      private IFormula createReactionKinetic(ReactionBuilder reactionBuilder, SimulationBuilder simulationBuilder)
      {
         return _formulaMapper.MapFrom(reactionBuilder.Formula, simulationBuilder);
      }

      private IParameter processRateParameterFor(ReactionBuilder reactionBuilder, SimulationBuilder simulationBuilder)
      {
         return _processRateParameterCreator.CreateProcessRateParameterFor(reactionBuilder, simulationBuilder);
      }
   }
}