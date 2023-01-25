using System.Linq;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Utility;
using OSPSuite.Utility.Extensions;

namespace OSPSuite.Core.Domain.Mappers
{
   public interface IPathAndValueEntityToPathElementsMapper : IMapper<PathAndValueEntity, PathElements>
   {
   }

   public class PathAndValueEntityToPathElementsMapper : IPathAndValueEntityToPathElementsMapper
   {
      public PathElements MapFrom(PathAndValueEntity input)
      {
         switch (input)
         {
            case ParameterStartValue psv:
               return mapAsParameterStartValue(psv);
            case MoleculeStartValue msv:
               return mapAsMoleculeStartValue(msv);
            case ExpressionParameter expressionParameter:
               return mapAsExpressionParameter(expressionParameter);
            case IndividualParameter individualParameter:
               return mapAsIndividualParameter(individualParameter);
            default:
               return new PathElements();
         }
      }

      private PathElements mapAsIndividualParameter(IndividualParameter individualParameter)
      {
         return mapAsPathAndValueEntity(individualParameter, secondLastPathElementIndex(individualParameter));
      }

      private PathElements mapAsExpressionParameter(ExpressionParameter expressionParameter)
      {
         var bottomCompartmentIndex = thirdLastPathElementIndex(expressionParameter);
         var pathElements = mapAsPathAndValueEntity(expressionParameter, bottomCompartmentIndex);
         addMolecule(expressionParameter, pathElements, bottomCompartmentIndex);

         return pathElements;
      }

      private PathElements mapAsMoleculeStartValue(MoleculeStartValue msv)
      {
         var bottomCompartmentIndex = secondLastPathElementIndex(msv);
         var pathElements = mapAsPathAndValueEntity(msv, bottomCompartmentIndex);
         addMolecule(msv, pathElements, bottomCompartmentIndex);

         return pathElements;
      }

      private void addMolecule(ExpressionParameter expressionParameter, PathElements pathElements, int bottomCompartmentIndex)
      {
         addMolecule(pathElements, expressionParameter.Path.ElementAt(bottomCompartmentIndex + 1));
      }

      private void addMolecule(MoleculeStartValue msv, PathElements pathElements, int bottomCompartmentIndex)
      {
         addMolecule(pathElements, msv.Path.ElementAt(bottomCompartmentIndex + 1));
      }

      private static void addMolecule(PathElements pathElements, string displayName)
      {
         pathElements.Add(PathElementId.Molecule, new PathElement { DisplayName = displayName });
      }

      private PathElements mapAsParameterStartValue(ParameterStartValue psv)
      {
         return mapAsPathAndValueEntity(psv, secondLastPathElementIndex(psv));
      }

      private PathElements mapAsPathAndValueEntity(PathAndValueEntity pathAndValueEntity, int bottomCompartmentIndex)
      {
         var pathElements = new PathElements();


         if (pathAndValueEntity.Path.Count > bottomCompartmentIndex)
            addContainerPaths(pathAndValueEntity, pathElements, bottomCompartmentIndex);
         if (pathAndValueEntity.Path.Count >= bottomCompartmentIndex)
            addBottomCompartment(pathAndValueEntity, pathElements, bottomCompartmentIndex);
         if (pathAndValueEntity.Path.Count >= 2)
            addNameAndTopContainer(pathAndValueEntity, pathElements);

         return pathElements;
      }

      private static int secondLastPathElementIndex(PathAndValueEntity pathAndValueEntity) => pathAndValueEntity.Path.Count - 2;
      private static int thirdLastPathElementIndex(PathAndValueEntity pathAndValueEntity) => pathAndValueEntity.Path.Count - 3;

      private void addContainerPaths(PathAndValueEntity pathAndValueEntity, PathElements pathElements, int bottomCompartmentIndex)
      {
         // Skip 1 (the top container) and take until one before the bottom compartment to create a combined ContainerPathElement
         var containerPath = pathAndValueEntity.Path.Skip(1).Take(bottomCompartmentIndex - 1).ToString(Constants.DISPLAY_PATH_SEPARATOR);
         if (string.IsNullOrEmpty(containerPath))
            return;

         pathElements.Add(PathElementId.Container, new PathElement { DisplayName = containerPath });
      }

      private void addBottomCompartment(PathAndValueEntity pathAndValueEntity, PathElements pathElements, int bottomCompartmentIndex)
      {
         pathElements.Add(PathElementId.BottomCompartment, new PathElement { DisplayName = pathAndValueEntity.Path.ElementAt(bottomCompartmentIndex) });
      }

      private void addNameAndTopContainer(PathAndValueEntity pathAndValueEntity, PathElements pathElements)
      {
         pathElements.Add(PathElementId.Name, new PathElement { DisplayName = pathAndValueEntity.Path.Last() });
         pathElements.Add(PathElementId.TopContainer, new PathElement { DisplayName = pathAndValueEntity.Path.First() });
      }
   }
}