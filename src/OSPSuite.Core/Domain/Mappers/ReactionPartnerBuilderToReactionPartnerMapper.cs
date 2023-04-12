using OSPSuite.Core.Domain.Builder;

namespace OSPSuite.Core.Domain.Mappers
{
   public interface IReactionPartnerBuilderToReactionPartnerMapper : ILocalMapper<IReactionPartnerBuilder, IContainer, IReactionPartner>
   {
   }

   /// <summary>
   /// </summary>
   internal class ReactionPartnerBuilderToReactionPartnerMapper : IReactionPartnerBuilderToReactionPartnerMapper
   {
      /// <summary>
      ///    Maps from input to output using data provided by local.
      /// </summary>
      /// <param name="reactionPartnerBuilder">The reaction partner builder used to create reaction partner from</param>
      /// <param name="container">
      ///    The container there we found used molecules amounts in, should be normally the same as there
      ///    the reaction is created
      /// </param>
      /// <param name="simulationBuilder">Simulation Builder</param>
      public IReactionPartner MapFromLocal(IReactionPartnerBuilder reactionPartnerBuilder, IContainer container, SimulationBuilder simulationBuilder)
      {
         var moleculeAmount = container.GetSingleChildByName<IMoleculeAmount>(reactionPartnerBuilder.MoleculeName);
         if (moleculeAmount == null)
            throw new MissingMoleculeAmountException(container.Name, reactionPartnerBuilder.MoleculeName);

         return new ReactionPartner(reactionPartnerBuilder.StoichiometricCoefficient, moleculeAmount);
      }
   }
}