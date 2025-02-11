using System.Collections.Generic;
using System.Linq;
using OSPSuite.Assets;

namespace OSPSuite.Core.Domain.Builder
{
   public class ObserverBuildingBlock : BuildingBlock<ObserverBuilder>
   {
      public ObserverBuildingBlock()
      {
         Icon = IconNames.OBSERVER;
      }

      public IEnumerable<AmountObserverBuilder> AmountObserverBuilders => getTypedObserverBuilder<AmountObserverBuilder>();

      public IEnumerable<ContainerObserverBuilder> ContainerObserverBuilders => getTypedObserverBuilder<ContainerObserverBuilder>();

      private IEnumerable<T> getTypedObserverBuilder<T>() where T : ObserverBuilder
      {
         return from observerBuilder in this
            let amountObserverBuilder = observerBuilder as T
            where amountObserverBuilder != null
            select amountObserverBuilder;
      }
   }
}