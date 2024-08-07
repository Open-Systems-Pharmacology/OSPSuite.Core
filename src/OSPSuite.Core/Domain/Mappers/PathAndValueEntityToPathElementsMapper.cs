using System.Collections.Generic;
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
            case ExpressionParameter expressionParameter:
               return mapAsExpressionParameter(expressionParameter);
            case IndividualParameter individualParameter:
               return mapAsIndividualParameter(individualParameter);
            case ParameterValue parameterValue:
               return mapAsParameterValue(parameterValue);
            case InitialCondition initialCondition:
               return mapAsInitialCondition(initialCondition);
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

      private PathElements mapAsInitialCondition(InitialCondition initialCondition)
      {
         var bottomCompartmentIndex = secondLastPathElementIndex(initialCondition);
         var pathElements = mapAsPathAndValueEntity(initialCondition, bottomCompartmentIndex);
         addMolecule(initialCondition, pathElements, bottomCompartmentIndex);

         return pathElements;
      }

      private void addMolecule(ExpressionParameter expressionParameter, PathElements pathElements, int bottomCompartmentIndex)
      {
         if (bottomCompartmentIndex < 0)
            addMolecule(pathElements, expressionParameter.Path.ElementAt(0));
         else
            addMolecule(pathElements, expressionParameter.Path.ElementAt(moleculeNameIndex(bottomCompartmentIndex)));
      }

      private static int moleculeNameIndex(int bottomCompartmentIndex)
      {
         // If a Molecule Name exists it is always the next one after the bottom compartment
         return bottomCompartmentIndex + 1;
      }

      private void addMolecule(InitialCondition initialCondition, PathElements pathElements, int bottomCompartmentIndex)
      {
         addMolecule(pathElements, initialCondition.Path.ElementAt(moleculeNameIndex(bottomCompartmentIndex)));
      }

      private static void addMolecule(PathElements pathElements, string displayName)
      {
         pathElements.Add(PathElementId.Molecule, new PathElement {DisplayName = displayName});
      }

      private PathElements mapAsParameterValue(ParameterValue parameterValue)
      {
         return mapAsPathAndValueEntity(parameterValue, secondLastPathElementIndex(parameterValue));
      }

      private PathElements mapAsPathAndValueEntity(PathAndValueEntity pathAndValueEntity, int bottomCompartmentIndex)
      {
         var pathElements = new PathElements();

         if (hasContainerPaths(pathAndValueEntity, bottomCompartmentIndex))
            addContainerPaths(pathAndValueEntity, pathElements, bottomCompartmentIndex);
         if (hasBottomCompartment(pathAndValueEntity, bottomCompartmentIndex))
            addBottomCompartment(pathAndValueEntity, pathElements, bottomCompartmentIndex);
         if (hasNameAndTopContainer(pathAndValueEntity))
            addNameAndTopContainer(pathAndValueEntity, pathElements);

         return pathElements;
      }

      private static bool hasNameAndTopContainer(PathAndValueEntity pathAndValueEntity)
      {
         // Name and top container are present as long as the path has at least 2 elements
         return pathAndValueEntity.Path.Count >= 2;
      }

      private static bool hasBottomCompartment(PathAndValueEntity pathAndValueEntity, int bottomCompartmentIndex)
      {
         // Bottom compartment is present if there are enough elements
         return pathAndValueEntity.Path.Count >= bottomCompartmentIndex && bottomCompartmentIndex > 0;
      }

      private static bool hasContainerPaths(PathAndValueEntity pathAndValueEntity, int bottomCompartmentIndex)
      {
         // Container paths are present if there are more elements than the bottom compartment
         return pathAndValueEntity.Path.Count > bottomCompartmentIndex && bottomCompartmentIndex > 0;
      }

      // Index of the next-to-last element in the path (-1 to offset from count to 0-based index and -1 for next-to-last)
      private static int secondLastPathElementIndex(PathAndValueEntity pathAndValueEntity) => pathAndValueEntity.Path.Count - 2;

      // Index of the next-to-next-to-last element in the path (-1 to offset from count to 0-based index and -2 for next-to-next-to-last)
      private static int thirdLastPathElementIndex(PathAndValueEntity pathAndValueEntity) => pathAndValueEntity.Path.Count - 3;

      private void addContainerPaths(PathAndValueEntity pathAndValueEntity, PathElements pathElements, int bottomCompartmentIndex)
      {
         var containerPath = allElementsBetweenTopContainerAndBottomCompartment(pathAndValueEntity, bottomCompartmentIndex).ToString(Constants.DISPLAY_PATH_SEPARATOR);
         if (string.IsNullOrEmpty(containerPath))
            return;

         pathElements.Add(PathElementId.Container, new PathElement {DisplayName = containerPath});
      }

      private static IEnumerable<string> allElementsBetweenTopContainerAndBottomCompartment(PathAndValueEntity pathAndValueEntity, int bottomCompartmentIndex)
      {
         // Skip 1 (the top container) and take until one before the bottom compartment to create a combined ContainerPathElement
         return pathAndValueEntity.Path.Skip(1).Take(bottomCompartmentIndex - 1);
      }

      private void addBottomCompartment(PathAndValueEntity pathAndValueEntity, PathElements pathElements, int bottomCompartmentIndex)
      {
         pathElements.Add(PathElementId.BottomCompartment, new PathElement {DisplayName = pathAndValueEntity.Path.ElementAt(bottomCompartmentIndex)});
      }

      private void addNameAndTopContainer(PathAndValueEntity pathAndValueEntity, PathElements pathElements)
      {
         pathElements.Add(PathElementId.Name, new PathElement {DisplayName = pathAndValueEntity.Path.Last()});
         pathElements.Add(PathElementId.TopContainer, new PathElement {DisplayName = pathAndValueEntity.Path.First()});
      }
   }
}