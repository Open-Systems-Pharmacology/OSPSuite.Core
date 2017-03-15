using OSPSuite.Core.Domain.Builder;

namespace OSPSuite.Core.Domain.Mappers
{
   public interface IReactionPartnerBuilderToReactionPartnerMapper : ILocalMapper<IReactionPartnerBuilder, IReactionPartner, IContainer>
   {
   }

   /// <summary>
   /// 
   /// </summary>
   public class ReactionPartnerBuilderToReactionPartnerMapper : IReactionPartnerBuilderToReactionPartnerMapper
   {
      /// <summary>
      /// Maps from input to output using data provided by local.
      /// </summary>
      /// <param name="reactionPartnerBuilder">The reaction partner builder used to create reaction partner from</param>
      /// <param name="container">The container there we found used molecules amounts in, should be normally the same as there the reaction is created</param>
      /// <param name="buildConfiguration">build configuration used to create model</param>
      public IReactionPartner MapFromLocal(IReactionPartnerBuilder reactionPartnerBuilder, IContainer container,IBuildConfiguration buildConfiguration)
      {
         var moleculeAmount = container.GetSingleChildByName<IMoleculeAmount>(reactionPartnerBuilder.MoleculeName);
         if (moleculeAmount == null)
            throw new MissingMoleculeAmountException(container.Name, reactionPartnerBuilder.MoleculeName);

         return new ReactionPartner(reactionPartnerBuilder.StoichiometricCoefficient, moleculeAmount);
      }
   }
}