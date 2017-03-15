using OSPSuite.Assets;
using OSPSuite.Core.Domain;
using OSPSuite.Presentation.DTO;

namespace OSPSuite.Presentation.Mappers
{
   public abstract class AbstractQuantityToSimulationQuantitySelectionDTOMapper
   {
      protected void UpdateContainerDisplayNameAndIconsIfEmpty(QuantitySelectionDTO quantitySelectionDTO, IQuantity quantity)
      {
         var containerPathElement = quantitySelectionDTO.ContainerPathElement;
         if (!string.IsNullOrEmpty(containerPathElement.DisplayName) && !string.IsNullOrEmpty(containerPathElement.IconName))
            return;

         var parentContainer = quantity.ParentContainer;
         if (parentContainer == null)
            return;

         if (parentContainer.ContainerType == ContainerType.Molecule)
            updatePathElement(containerPathElement, ObjectTypes.Molecule, parentContainer.Icon);

         if (parentContainer.ContainerType == ContainerType.Formulation)
            updatePathElement(containerPathElement, ObjectTypes.Formulation, ApplicationIcons.Formulation);

         if (parentContainer.ContainerType == ContainerType.Organism)
            updatePathElement(containerPathElement, Captions.Organism, ApplicationIcons.Organism);

         if (parentContainer.ContainerType == ContainerType.Reaction)
            updatePathElement(containerPathElement, ObjectTypes.Reaction, ApplicationIcons.Reaction);

         if (quantity.HasAncestorWith(x => x.ContainerType == ContainerType.Application))
            updatePathElement(containerPathElement, ObjectTypes.Application, ApplicationIcons.Application);

         quantitySelectionDTO.PathElements[PathElement.Container] = containerPathElement;
      }

      private void updatePathElement(PathElementDTO pathElement, string displayName, ApplicationIcon icon)
      {
         updatePathElement(pathElement, displayName, icon.IconName);
      }

      private void updatePathElement(PathElementDTO pathElement, string displayName, string iconName)
      {
         if (string.IsNullOrEmpty(pathElement.DisplayName))
            pathElement.DisplayName = displayName;

         pathElement.IconName = iconName;
      }
   }
}