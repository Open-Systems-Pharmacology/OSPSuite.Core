using OSPSuite.Assets;

namespace OSPSuite.Core.Domain.Builder
{
   public class EventGroupBuildingBlock : BuildingBlock<EventGroupBuilder>
   {
      public EventGroupBuildingBlock()
      {
         Icon = IconNames.EVENT;
      }
   }
}