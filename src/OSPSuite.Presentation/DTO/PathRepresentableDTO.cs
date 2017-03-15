using System.Linq;
using OSPSuite.Utility.Extensions;
using OSPSuite.Core.Domain;

namespace OSPSuite.Presentation.DTO
{
   public interface IPathRepresentableDTO : IValidatableDTO
   {
      PathElements PathElements { get; set; }
      PathElementDTO SimulationPathElement { get; }
      PathElementDTO TopContainerPathElement { get; }
      PathElementDTO ContainerPathElement { get; }
      PathElementDTO BottomCompartmentPathElement { get; }
      PathElementDTO MoleculePathElement { get; }
      PathElementDTO NamePathElement { get; }
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

      public PathElementDTO SimulationPathElement => PathElements[PathElement.Simulation];

      public PathElementDTO TopContainerPathElement => PathElements[PathElement.TopContainer];

      public PathElementDTO ContainerPathElement => PathElements[PathElement.Container];

      public PathElementDTO BottomCompartmentPathElement => PathElements[PathElement.BottomCompartment];

      public PathElementDTO MoleculePathElement => PathElements[PathElement.Molecule];

      public PathElementDTO NamePathElement => PathElements[PathElement.Name];

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