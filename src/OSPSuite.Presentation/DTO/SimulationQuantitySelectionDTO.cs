using OSPSuite.Core.Domain;

namespace OSPSuite.Presentation.DTO
{
   public class SimulationQuantitySelectionDTO : ValidatableDTO, IPathRepresentableDTO
   {
      public virtual QuantitySelectionDTO QuantitySelectionDTO { get; }
      public virtual ISimulation Simulation { get; }
      public virtual string DisplayString { get; }

      public SimulationQuantitySelectionDTO(ISimulation simulation, QuantitySelectionDTO quantitySelectionDTO, string displayString)
      {
         DisplayString = displayString;
         QuantitySelectionDTO = quantitySelectionDTO;
         Simulation = simulation;
      }

      public virtual string QuantityPath => QuantitySelectionDTO?.QuantityPath;
      public virtual IQuantity Quantity => QuantitySelectionDTO?.Quantity;

      public override string ToString()
      {
         return DisplayString;
      }

      public PathElements PathElements
      {
         get { return QuantitySelectionDTO?.PathElements; }
         set
         {
            if (QuantitySelectionDTO != null)
               QuantitySelectionDTO.PathElements = value;
         }
      }

      public virtual PathElementDTO SimulationPathElement => QuantitySelectionDTO?.SimulationPathElement;
      public virtual PathElementDTO TopContainerPathElement => QuantitySelectionDTO?.TopContainerPathElement;
      public virtual PathElementDTO ContainerPathElement => QuantitySelectionDTO?.ContainerPathElement;
      public virtual PathElementDTO BottomCompartmentPathElement => QuantitySelectionDTO?.BottomCompartmentPathElement;
      public virtual PathElementDTO MoleculePathElement => QuantitySelectionDTO?.MoleculePathElement;
      public virtual PathElementDTO NamePathElement => QuantitySelectionDTO?.NamePathElement;

      public string Category => QuantitySelectionDTO?.Category;
      public string DisplayPathAsString => QuantitySelectionDTO?.DisplayPathAsString;
   }

   public class NullSimulationQuantitySelectionDTO : SimulationQuantitySelectionDTO
   {
      public NullSimulationQuantitySelectionDTO() : base(null, new QuantitySelectionDTO(), string.Empty)
      {
         QuantitySelectionDTO.PathElements[PathElement.Simulation] = new InvalidPathElementDTO();
         QuantitySelectionDTO.PathElements[PathElement.Name] = new InvalidPathElementDTO();
      }
   }
}