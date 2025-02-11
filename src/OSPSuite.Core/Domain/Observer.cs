using OSPSuite.Assets;

namespace OSPSuite.Core.Domain
{
   public class Observer : Quantity
   {
      public Observer()
      {
         //default type for observer
         QuantityType = QuantityType.Observer;
         Icon = IconNames.OBSERVER;
         NegativeValuesAllowed = true;
      }
   }
}