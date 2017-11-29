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

      IDimension AddDimension(BaseDimensionRepresentation baseRepresentation, string dimensionName, string baseUnitName);
      void AddDimension(IDimension dimension);
      void AddMergingInformation(IDimensionMergingInformation mergingInformation);

      void RemoveDimension(string dimensionName);
      void RemoveDimension(IDimension dimension);

      
      void Clear();

      /// <summary>
      ///    Returns the one and unique dimension represention the 'dimensionsless' dimension
      /// </summary>
      IDimension NoDimension { get; }


   }
}