using System.Collections.Generic;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Extensions;

namespace OSPSuite.Presentation.DTO
{
   public static class PathRepresentableDTOExtensions
   {
      public static IReadOnlyList<string> AllDistinctValuesAt(this IEnumerable<IPathRepresentableDTO> pathRepresentables, PathElementId pathElementId)
      {
         return pathRepresentables.AllDistinctValues(p => p.PathElements[pathElementId].DisplayName);
      }

      public static IReadOnlyList<string> AllDistinctCategories(this IEnumerable<IPathRepresentableDTO> pathRepresentables)
      {
         return pathRepresentables.AllDistinctValues(p => p.Category);
      }

      public static bool HasDistinctValuesAt(this IEnumerable<IPathRepresentableDTO> pathRepresentables, PathElementId pathElementId)
      {
         return pathRepresentables.AllDistinctValuesAt(pathElementId).Count > 1;
      }

      public static bool HasOnlyEmptyValuesAt(this IEnumerable<IPathRepresentableDTO> pathRepresentables, PathElementId pathElementId)
      {
         var allValues = pathRepresentables.AllDistinctValuesAt(pathElementId);
         return allValues.Count == 1 && string.IsNullOrEmpty(allValues[0]);
      }

      public static bool HasDistinctCategories(this IEnumerable<IPathRepresentableDTO> pathRepresentables)
      {
         return pathRepresentables.AllDistinctCategories().Count > 1;
      }
   }
}