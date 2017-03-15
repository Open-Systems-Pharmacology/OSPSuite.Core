using OSPSuite.Assets;

namespace OSPSuite.Core.Domain.Builder
{
   /// <summary>
   /// Collection of passiveTransportBuilders 
   /// </summary>
   public interface IPassiveTransportBuildingBlock : IBuildingBlock<ITransportBuilder>
   {
   }

   /// <summary>
   /// Collection of passiveTransportBuilders 
   /// </summary>
   public class PassiveTransportBuildingBlock : BuildingBlock<ITransportBuilder>, IPassiveTransportBuildingBlock
   {
      public PassiveTransportBuildingBlock()
      {
         Icon = IconNames.PASSIVE_TRANSPORT;
      }
   }
}