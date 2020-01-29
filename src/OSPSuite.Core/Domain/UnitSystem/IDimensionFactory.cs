using System.Collections.Generic;

namespace OSPSuite.Core.Domain.UnitSystem
{
   public interface IDimensionFactory
   {
      /// <summary>
      ///    All dimensions defined in the repository
      /// </summary>
      IEnumerable<IDimension> Dimensions { get; }

      IEnumerable<string> DimensionNames { get; }
      IDimension Dimension(string name);
      IDimension MergedDimensionFor<T>(T hasDimension) where T : IWithDimension;
      /// <summary>
      /// Returns <c>true</c> if a dimension named <paramref name="dimensionName"/> exists otherwise <c>false</c>.
      /// </summary>
      bool Has(string dimensionName);
      IDimension AddDimension(BaseDimensionRepresentation baseRepresentation, string dimensionName, string baseUnitName);
      void AddDimension(IDimension dimension);
      void AddMergingInformation(IDimensionMergingInformation mergingInformation);

      void RemoveDimension(string dimensionName);
      void RemoveDimension(IDimension dimension);

      void Clear();

      /// <summary>
      ///    Returns the one and unique dimension representation the 'dimensionsless' dimension
      /// </summary>
      IDimension NoDimension { get; }
   }
}