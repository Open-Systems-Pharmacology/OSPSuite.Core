﻿using System.Collections.Generic;
using OSPSuite.Utility.Extensions;

namespace OSPSuite.Core.Domain.Services
{
   public interface ISimulationPersistableUpdater
   {
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

      public void UpdateSimulationPersistable(IModelCoreSimulation simulation)
      {
         var allQuantities = _entitiesInSimulationRetriever.QuantitiesFrom(simulation);
         updatePersistable(allQuantities, simulation.OutputSelections);
      }

      private void updatePersistable(PathCache<IQuantity> allQuantities, OutputSelections outputSelections)
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