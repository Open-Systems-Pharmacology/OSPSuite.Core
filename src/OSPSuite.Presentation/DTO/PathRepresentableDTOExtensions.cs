using System.Collections.Generic;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Extensions;

namespace OSPSuite.Presentation.DTO
{
   public static class PathRepresentableDTOExtensions
   {
      public static IReadOnlyList<string> AllDistincValuesAt(this IEnumerable<IPathRepresentableDTO> pathRepresentables, PathElement pathElement)
      {
         return pathRepresentables.AllDistinctValues(p => p.PathElements[pathElement].DisplayName);
      }

      public static IReadOnlyList<string> AllDistincCategories(this IEnumerable<IPathRepresentableDTO> pathRepresentables)
      {
         return pathRepresentables.AllDistinctValues(p => p.Category);
      }

      public static bool HasDistinctValuesAt(this IEnumerable<IPathRepresentableDTO> pathRepresentables, PathElement pathElement)
      {
         return pathRepresentables.AllDistincValuesAt(pathElement).Count > 1;
      }

      public static bool HasOnlyEmptyValuesAt(this IEnumerable<IPathRepresentableDTO> pathRepresentables, PathElement pathElement)
      {
         var allValues = pathRepresentables.AllDistincValuesAt(pathElement);
         return allValues.Count == 1 && string.IsNullOrEmpty(allValues[0]);
      }

      public static bool HasDistinctCategories(this IEnumerable<IPathRepresentableDTO> pathRepresentables)
      {
         return pathRepresentables.AllDistincCategories().Count > 1;
      }
   }
}