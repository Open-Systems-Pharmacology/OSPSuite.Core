using OSPSuite.Assets;

namespace OSPSuite.Core.Domain
{
   public interface IObserver : IQuantity
   {
   }

   public class Observer : Quantity, IObserver
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