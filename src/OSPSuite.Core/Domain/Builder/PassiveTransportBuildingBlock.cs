using OSPSuite.Assets;

namespace OSPSuite.Core.Domain.Builder
{
   /// <summary>
   ///    Collection of passiveTransportBuilders
   /// </summary>
   public class PassiveTransportBuildingBlock : BuildingBlock<TransportBuilder>
   {
      public PassiveTransportBuildingBlock()
      {
         Icon = IconNames.PASSIVE_TRANSPORT;
      }
   }
}