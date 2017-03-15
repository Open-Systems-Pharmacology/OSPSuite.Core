using System.Collections.Generic;
using OSPSuite.Utility.Extensions;

namespace OSPSuite.Core.Domain.Services
{
   public interface ISimulationPersistableUpdater
   {
      void UpdateSimulationPersistable(ISimulation simulation);
      void UpdateSimulationPersistable(IModelCoreSimulation simulation);
      void SetPersistable(IEnumerable<IQuantity> quantities, bool persistable);
   }

   public class SimulationPersistableUpdater : ISimulationPersistableUpdater
   {
      private readonly IEntitiesInSimulationRetriever _entitiesInSimulationRetriever;

      public SimulationPersistableUpdater(IEntitiesInSimulationRetriever entitiesInSimulationRetriever)
      {
         _entitiesInSimulationRetriever = entitiesInSimulationRetriever;
      }

      public void UpdateSimulationPersistable(ISimulation simulation)
      {
         var allQuantities = _entitiesInSimulationRetriever.QuantitiesFrom(simulation);
         updatePersitable(allQuantities, simulation.OutputSelections);
      }

      public void UpdateSimulationPersistable(IModelCoreSimulation simulation)
      {
         var allQuantities = _entitiesInSimulationRetriever.QuantitiesFrom(simulation);
         updatePersitable(allQuantities, simulation.BuildConfiguration.SimulationSettings.OutputSelections);
      }

      private void updatePersitable(PathCache<IQuantity> allQuantities, OutputSelections outputSelections)
      {
         SetPersistable(allQuantities, false);

         foreach (var selectedQuantity in outputSelections)
         {
            var quantity = allQuantities[selectedQuantity.Path];
            if (quantity != null)
               quantity.Persistable = true;
         }
      }

      public void SetPersistable(IEnumerable<IQuantity> quantities, bool persistable)
      {
         quantities.Each(x => x.Persistable = persistable);
      }
   }
}