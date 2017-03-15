using OSPSuite.Assets;

namespace OSPSuite.Core.Domain.Builder
{
   /// <summary>
   ///    EventGroups building block
   /// </summary>
   public interface IEventGroupBuildingBlock : IBuildingBlock<IEventGroupBuilder>
   {
   }

   public class EventGroupBuildingBlock : BuildingBlock<IEventGroupBuilder>, IEventGroupBuildingBlock
   {
      public EventGroupBuildingBlock()
      {
         Icon = IconNames.EVENT_GROUP;
      }
   }
}