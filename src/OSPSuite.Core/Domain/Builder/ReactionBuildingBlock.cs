using OSPSuite.Assets;

namespace OSPSuite.Core.Domain.Builder
{
   public class ReactionBuildingBlock : BuildingBlock<ReactionBuilder>
   {
      public ReactionBuildingBlock()
      {
         Icon = IconNames.REACTION;
      }
   }
}