using OSPSuite.Core.Domain;

namespace OSPSuite.Presentation.DTO
{
   public class SimulationParameterSelectionDTO : SimulationQuantitySelectionDTO
   {
      public bool IsFavorite { get; set; }
      public IParameterDTO ValueParameter { get; set; }
      public double Value => ValueParameter.Value;

      public SimulationParameterSelectionDTO(ISimulation simulation, QuantitySelectionDTO quantitySelectionDTO, string displayString) : base(simulation, quantitySelectionDTO, displayString)
      {
      }

   }
}