using System.Linq;
using OSPSuite.Utility.Extensions;
using OSPSuite.Core.Domain;

namespace OSPSuite.Presentation.DTO
{
   public interface IPathRepresentableDTO : IValidatableDTO
   {
      PathElements PathElements { get; set; }
      PathElement SimulationPathElement { get; }
      PathElement TopContainerPathElement { get; }
      PathElement ContainerPathElement { get; }
      PathElement BottomCompartmentPathElement { get; }
      PathElement MoleculePathElement { get; }
      PathElement NamePathElement { get; }
      string Category { get; }

      /// <summary>
      ///    Display path of the quantity
      /// </summary>
      string DisplayPathAsString { get; }
   }

   public class PathRepresentableDTO : ValidatableDTO, IPathRepresentableDTO
   {
      public PathElements PathElements { get; set; }

      public PathRepresentableDTO()
      {
         PathElements = new PathElements();
      }

      public PathElement SimulationPathElement => PathElements[PathElementId.Simulation];

      public PathElement TopContainerPathElement => PathElements[PathElementId.TopContainer];

      public PathElement ContainerPathElement => PathElements[PathElementId.Container];

      public PathElement BottomCompartmentPathElement => PathElements[PathElementId.BottomCompartment];

      public PathElement MoleculePathElement => PathElements[PathElementId.Molecule];

      public PathElement NamePathElement => PathElements[PathElementId.Name];

      public string Category => PathElements.Category;

      /// <summary>
      ///    Display path of the quantity
      /// </summary>
      public string DisplayPathAsString => PathAsString(Constants.DISPLAY_PATH_SEPARATOR);

      /// <summary>
      ///    Display path of the quantity
      /// </summary>
      public string PathAsString(string separator)
      {
         return PathElements.Select(x => x.DisplayName).ToString(separator);
      }
   }
}