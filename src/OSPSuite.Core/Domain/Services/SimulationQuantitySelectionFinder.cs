namespace OSPSuite.Core.Domain.Services
{
   public interface ISimulationQuantitySelectionFinder
   {
      bool SimulationHasSelection(SimulationQuantitySelection parameterSelection, IWithModel newSimulation);
   }

   public class SimulationQuantitySelectionFinder : ISimulationQuantitySelectionFinder
   {
      public bool SimulationHasSelection(SimulationQuantitySelection parameterSelection, IWithModel newSimulation)
      {
         var path = new ObjectPath(parameterSelection.PathArray);
         path.TryResolve<IEntity>(newSimulation.Model.Root, out var success);
         return success;
      }
   }
}