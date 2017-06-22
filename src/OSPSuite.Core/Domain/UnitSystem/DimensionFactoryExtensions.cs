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
         return getMergedDimensions(dimensionFactory, defaultDimension).OrderBy(dimension => dimension.Name);
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

         if (defaultDimension != null)
            dimensionCache[defaultDimension.DisplayName] = defaultDimension;

         foreach (var dimension in dimensionFactory.Dimensions)
         {
            var optimalDimension = OptimalDimension(dimensionFactory, dimension);
            dimensionCache[optimalDimension.DisplayName] = optimalDimension;
         }

         return dimensionCache;
      }
   }
}