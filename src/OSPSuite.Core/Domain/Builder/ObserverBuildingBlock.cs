using System.Collections.Generic;
using System.Linq;
using OSPSuite.Assets;

namespace OSPSuite.Core.Domain.Builder
{
   public interface IObserverBuildingBlock : IBuildingBlock<IObserverBuilder>
   {
      IEnumerable<IAmountObserverBuilder> AmountObserverBuilders { get; }
      IEnumerable<IContainerObserverBuilder> ContainerObserverBuilders { get; }
   }

   public class ObserverBuildingBlock : BuildingBlock<IObserverBuilder>, IObserverBuildingBlock
   {
      public ObserverBuildingBlock()
      {
         Icon = IconNames.OBSERVER;
      }

      public IEnumerable<IAmountObserverBuilder> AmountObserverBuilders => getTypedObserverBuilder<IAmountObserverBuilder>();

      public IEnumerable<IContainerObserverBuilder> ContainerObserverBuilders => getTypedObserverBuilder<IContainerObserverBuilder>();

      private IEnumerable<T> getTypedObserverBuilder<T>() where T : class, IObserverBuilder
      {
         return from observerBuilder in this
            let amountObserverBuilder = observerBuilder as T
            where amountObserverBuilder != null
            select amountObserverBuilder;
      }
   }
}