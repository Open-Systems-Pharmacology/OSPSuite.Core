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
         get => SimulationQuantitySelectionDTO.PathElements;
         set => SimulationQuantitySelectionDTO.PathElements = value;
      }

      public virtual PathElement SimulationPathElement => SimulationQuantitySelectionDTO.SimulationPathElement;
      public virtual PathElement TopContainerPathElement => SimulationQuantitySelectionDTO.TopContainerPathElement;
      public virtual PathElement ContainerPathElement => SimulationQuantitySelectionDTO.ContainerPathElement;
      public virtual PathElement BottomCompartmentPathElement => SimulationQuantitySelectionDTO.BottomCompartmentPathElement;
      public virtual PathElement MoleculePathElement => SimulationQuantitySelectionDTO.MoleculePathElement;
      public virtual PathElement NamePathElement => SimulationQuantitySelectionDTO.NamePathElement;
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