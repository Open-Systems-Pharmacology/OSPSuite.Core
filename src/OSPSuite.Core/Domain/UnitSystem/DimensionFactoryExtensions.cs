using System.Collections.Generic;
using System.Linq;
using OSPSuite.Utility.Collections;
using OSPSuite.Core.Domain.Data;

namespace OSPSuite.Core.Domain.UnitSystem
{
   public static class DimensionFactoryExtensions
   {
      public static IEnumerable<IDimension> AllDimensionsForEditors(this IDimensionFactory dimensionFactory, IDimension defaultDimension)
      {
         return getMergedDimensions(dimensionFactory, defaultDimension).OrderBy(dimension => dimension.DisplayName);
      }

      public static IDimension OptimalDimension(this IDimensionFactory dimensionFactory, IDimension defaultDimension)
      {
         if (defaultDimension == null)
            return null;

         var templateColumn = new DataColumn {Dimension = defaultDimension};
         return dimensionFactory.MergedDimensionFor(templateColumn);
      }

      private static Cache<string, IDimension> getMergedDimensions(IDimensionFactory dimensionFactory, IDimension defaultDimension)
      {
         var dimensionCache = new Cache<string, IDimension>();

         foreach (var dimension in dimensionFactory.Dimensions)
         {
            var optimalDimension = OptimalDimension(dimensionFactory, dimension);
            dimensionCache[optimalDimension.DisplayName] = optimalDimension;
         }

         // Add the axis's own dimension last so that its exact instance wins the DisplayName key.
         // MergedDimensionFor mints a new merged dimension instance on every call, so the loop above
         // produces a different instance with the same DisplayName for mergeable dimensions. Editors
         // select the combo value by reference, so the list must contain the instance held by the axis
         // for it to be selectable. See issue #2891.
         if (defaultDimension != null)
            dimensionCache[defaultDimension.DisplayName] = defaultDimension;

         return dimensionCache;
      }
   }
}