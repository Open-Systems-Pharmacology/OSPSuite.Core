using System.Collections.Generic;
using System.Linq;
using OSPSuite.Utility.Collections;
using OSPSuite.Utility.Exceptions;

namespace OSPSuite.Core.Domain.UnitSystem
{
   public class DimensionFactory : IDimensionFactory
   {
      private readonly ICache<string, IDimension> _dimensions;
      private readonly IList<IDimensionMergingInformation> _allMergingInformation;

      public DimensionFactory()
      {
         _dimensions = new Cache<string, IDimension>(dim => dim.Name);
         _allMergingInformation = new List<IDimensionMergingInformation>();
      }

      public void AddDimension(IDimension dimension)
      {
         if (_dimensions.Contains(dimension.Name))
            throw new NotUniqueIdException(dimension.Name);

         _dimensions.Add(dimension.Name, dimension);
      }

      public IEnumerable<IDimension> Dimensions => _dimensions;

      public IEnumerable<string> DimensionNames => _dimensions.Keys;

      public IDimension Dimension(string name)
      {
         if (!_dimensions.Keys.Contains(name))
            throw new KeyNotFoundException("Dimension " + name + " not available in DimensionFactory.");

         return _dimensions[name];
      }

      public IDimension AddDimension(BaseDimensionRepresentation baseRepresentation, string dimensionName, string baseUnitName)
      {
         if (_dimensions.Contains(dimensionName))
            throw new NotUniqueIdException(dimensionName);

         var dimension = new Dimension(baseRepresentation, dimensionName, baseUnitName);
         _dimensions.Add(dimension.Name, dimension);
         return dimension;
      }

      public void Clear()
      {
         _dimensions.Clear();
      }

      public IDimension NoDimension => _dimensions[Constants.Dimension.DIMENSIONLESS];

      public void RemoveDimension(string dimensionName)
      {
         if (!_dimensions.Contains(dimensionName))
            return;

         _dimensions.Remove(dimensionName);
      }

      public void RemoveDimension(IDimension dimension)
      {
         RemoveDimension(dimension.Name);
      }

      /// <exception cref="OSPSuiteException">Thrown when no well-defined merging Informations for the dimensions found</exception>
      public IDimension MergedDimensionFor<T>(T hasDimension) where T : IWithDimension
      {
         if (hasDimension.Dimension == null)
            return null;

         var dimensionsToMerge = (from dimension in _allMergingInformation
            where dimension.SourceDimension.Equals(hasDimension.Dimension)
            select dimension.TargetDimension).Distinct().ToList();

         if (!dimensionsToMerge.Any())
            return hasDimension.Dimension;

         var converters = new List<IDimensionConverterFor>();
         var targedDimensions = new List<IDimension>();

         foreach (var dimensionToMerge in dimensionsToMerge)
         {
            var conv = CreateConverterFor(hasDimension.Dimension, dimensionToMerge, hasDimension);
            if (conv == null) continue;

            converters.Add(conv);
            targedDimensions.Add(dimensionToMerge);
         }

         //We have a real merged dimension
         var displayName = MergedDimensionNameFor(hasDimension.Dimension);
         return new MergedDimensionFor<T>(hasDimension.Dimension, targedDimensions, converters) {DisplayName = displayName};
      }

      public virtual string MergedDimensionNameFor(IDimension sourceDimension)
      {
         return sourceDimension.DisplayName;
      }

      public void AddMergingInformation(IDimensionMergingInformation mergingInformation)
      {
         _allMergingInformation.Add(mergingInformation);
      }

      public IEnumerable<IDimensionMergingInformation> AllMergingInformation => _allMergingInformation;

      protected virtual IDimensionConverterFor CreateConverterFor<T>(IDimension dimension, IDimension dimensionToMerge, T hasDimension) where T : IWithDimension
      {
         /*should be implemented in derived methods*/
         return null;
      }
   }
}