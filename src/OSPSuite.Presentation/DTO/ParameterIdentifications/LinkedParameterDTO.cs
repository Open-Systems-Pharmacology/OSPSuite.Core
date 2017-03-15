using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.UnitSystem;

namespace OSPSuite.Presentation.DTO.ParameterIdentifications
{
   public class LinkedParameterDTO : ValidatableDTO, IPathRepresentableDTO
   {
      public virtual SimulationQuantitySelectionDTO SimulationQuantitySelectionDTO { get; }

      public LinkedParameterDTO(SimulationQuantitySelectionDTO simulationQuantitySimulationQuantitySelectionDTO)
      {
         SimulationQuantitySelectionDTO = simulationQuantitySimulationQuantitySelectionDTO;
      }

      public virtual double InitialValue => Quantity?.ValueInDisplayUnit ?? double.NaN;
      public virtual Unit DisplayUnit => Quantity?.DisplayUnit;
      public virtual IQuantity Quantity => SimulationQuantitySelectionDTO.Quantity;

      public PathElements PathElements
      {
         get { return SimulationQuantitySelectionDTO.PathElements; }
         set { SimulationQuantitySelectionDTO.PathElements = value; }
      }

      public virtual PathElementDTO SimulationPathElement => SimulationQuantitySelectionDTO.SimulationPathElement;
      public virtual PathElementDTO TopContainerPathElement => SimulationQuantitySelectionDTO.TopContainerPathElement;
      public virtual PathElementDTO ContainerPathElement => SimulationQuantitySelectionDTO.ContainerPathElement;
      public virtual PathElementDTO BottomCompartmentPathElement => SimulationQuantitySelectionDTO.BottomCompartmentPathElement;
      public virtual PathElementDTO MoleculePathElement => SimulationQuantitySelectionDTO.MoleculePathElement;
      public virtual PathElementDTO NamePathElement => SimulationQuantitySelectionDTO.NamePathElement;
      public virtual string Category => SimulationQuantitySelectionDTO.Category;
      public virtual string DisplayPathAsString => SimulationQuantitySelectionDTO.DisplayPathAsString;
   }

   public class NullLinkedParameterDTO : LinkedParameterDTO
   {
      public NullLinkedParameterDTO() : base(new NullSimulationQuantitySelectionDTO())
      {
      }
   }
}