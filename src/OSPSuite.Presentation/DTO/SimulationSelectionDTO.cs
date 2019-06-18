using OSPSuite.Core.Domain;

namespace OSPSuite.Presentation.DTO
{
   public class SimulationSelectionDTO : ObjectSelectionDTO<ISimulation>
   {
      public SimulationSelectionDTO(ISimulation simulation) : base(simulation)
      {
      }

      public ISimulation Simulation => Object;
   }

}