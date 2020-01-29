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

      /// <summary>
      /// Returns the dimension with name <paramref name="name"/>. 
      /// </summary>
      /// <exception cref="KeyNotFoundException"> is thrown if the dimension does not exist with the specified name</exception>
      IDimension Dimension(string name);


      /// <summary>
      /// Returns <c>true</c> if a dimension with <paramref name="dimensionName"/> if it exists or if a RHS dimensions can be derived from the <paramref name="dimensionName"/>.
      /// In that case, the <paramref name="dimension"/> will be set to the dimension. Otherwise returns <c>false</c>
      /// </summary>
      bool TryGetDimension(string dimensionName, out IDimension dimension);


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
      ///    Returns the one and unique dimension representation the 'dimensionless' dimension
      /// </summary>
      IDimension NoDimension { get; }

      /// <summary>
      /// Returns the RHS dimension corresponding to the given <paramref name="dimension"/>. If it does not exist already in the list of available dimension, a corresponding RHS dimension
      /// will be created, added to the list of available dimensions and returned
      /// </summary>
      IDimension GetOrAddRHSDimensionFor(IDimension dimension);
      
      /// <summary>
      ///    Retrieve the dimension given a unit string
      /// </summary>
      /// <param name="unitName">The unit string that will be searched for. For example 'mg' or 's'</param>
      /// <returns>The dimension associated with the unit name</returns>
      IDimension DimensionForUnit(string unitName);

   }
}