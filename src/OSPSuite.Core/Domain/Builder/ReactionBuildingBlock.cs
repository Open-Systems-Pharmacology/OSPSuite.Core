using OSPSuite.Assets;

namespace OSPSuite.Core.Domain.Builder
{
   /// <summary>
   ///    a Collection of ReactionBuilder
   /// </summary>
   public interface IReactionBuildingBlock : IBuildingBlock<IReactionBuilder>
   {
   }

   public class ReactionBuildingBlock : BuildingBlock<IReactionBuilder>, IReactionBuildingBlock
   {
      public ReactionBuildingBlock()
      {
         Icon = IconNames.REACTION;
      }
   }
}