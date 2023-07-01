using System.Collections.Generic;
using System.Linq;
using OSPSuite.Core.Extensions;
using OSPSuite.Utility.Collections;
using OSPSuite.Utility.Exceptions;

namespace OSPSuite.Core.Domain.UnitSystem
{
   public class DimensionFactory : IDimensionFactory
   {
      private readonly Cache<string, IDimension> _dimensions = new Cache<string, IDimension>(dim => dim.Name);
      private readonly List<IDimensionMergingInformation> _allMergingInformation = new List<IDimensionMergingInformation>();

      public IEnumerable<IDimension> Dimensions => _dimensions;

      public IDimension[] DimensionsSortedByName => Dimensions.OrderBy(x => x.Name).ToArray();

      public string[] DimensionNamesSortedByName => _dimensions.Keys.OrderBy(x => x).ToArray();

      public void AddDimension(IDimension dimension)
      {
         if (_dimensions.Contains(dimension.Name))
            throw new NotUniqueIdException(dimension.Name);

         _dimensions.Add(dimension.Name, dimension);
      }

      public IDimension Dimension(string name)
      {
         if (!Has(name))
            throw new KeyNotFoundException(unknownDimension(name));

         return _dimensions[name];
      }

      public bool TryGetDimension(string dimensionName, out IDimension dimension)
      {
         // Dimensions already registered. Returned
         if (Has(dimensionName))
         {
            dimension = Dimension(dimensionName);
            return true;
         }

         //It might be a RHS dimension. Let see if we can retrieve it
         try
         {
            dimension = getOrAddRHSDimensionForName(dimensionName);
            return true;
         }
         catch (KeyNotFoundException)
         {
            dimension = null;
            return false;
         }
      }

      public bool Has(string dimensionName) => _dimensions.Contains(dimensionName);

      public IDimension AddDimension(BaseDimensionRepresentation baseRepresentation, string dimensionName, string baseUnitName)
      {
         if (Has(dimensionName))
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

      private string rhsDimensionName(IDimension dimension) => $"{dimension.Name}{Constants.Dimension.RHS_DIMENSION_SUFFIX}";

      private string rhsDefaultUnitName(IDimension dimension)
      {
         var numerator = string.IsNullOrEmpty(dimension.BaseUnit.Name) ? "1" : dimension.BaseUnit.Name;
         if (string.Equals(numerator, "min"))
            return string.Empty;

         return $"{numerator}/min";
      }

      private IDimension findFirstEquivalentDimension(Dimension rhsDimension, string unitName) =>
         Dimensions.FirstOrDefault(x => x.IsEquivalentTo(rhsDimension) && x.HasUnit(unitName));

      public IDimension GetOrAddRHSDimensionFor(IDimension dimension)
      {
         var dimensionName = rhsDimensionName(dimension);
         if (Has(dimensionName))
            return Dimension(dimensionName);

         var unitName = rhsDefaultUnitName(dimension);

         // RHS is per Time hence -1
         var rhsDimensionRepresentation = new BaseDimensionRepresentation(dimension.BaseRepresentation);
         rhsDimensionRepresentation.TimeExponent -= 1;

         var rhsDimension = new Dimension(rhsDimensionRepresentation, dimensionName, unitName);
         var equivalentRHSDimension = findFirstEquivalentDimension(rhsDimension, unitName);
         if (equivalentRHSDimension != null)
            return equivalentRHSDimension;

         // Equivalent dimensions does noo exist. We add it and return it
         AddDimension(rhsDimension);
         return rhsDimension;
      }

      public IDimension DimensionForUnit(string unitName)
      {
         return Dimensions.DimensionForUnit(unitName);
      }

      public (IDimension dimension, Unit unit) FindUnit(string unitName)
      {
         var dimension = DimensionForUnit(unitName);
         return (dimension, dimension?.FindUnit(unitName, ignoreCase: true));
      }

      public IDimension CreateUserDefinedDimension(string dimensionName, string unit)
      {
         return new Dimension(Constants.Dimension.NO_DIMENSION.BaseRepresentation, dimensionName, unit);
      }

      /// <summary>
      ///    Try to find a dimension that could be the origin dimension for the given <paramref name="rhsDimensionName" />
      /// </summary>
      /// <exception cref="KeyNotFoundException">is thrown when match could not be found</exception>
      private IDimension getOrAddRHSDimensionForName(string rhsDimensionName)
      {
         if (string.IsNullOrEmpty(rhsDimensionName))
            throw new KeyNotFoundException();

         if (!rhsDimensionName.Contains(Constants.Dimension.RHS_DIMENSION_SUFFIX))
            throw new KeyNotFoundException(unknownDimension(rhsDimensionName));

         var dimensionName = rhsDimensionName.Replace(Constants.Dimension.RHS_DIMENSION_SUFFIX, "");
         var originDimension = Dimension(dimensionName);
         return GetOrAddRHSDimensionFor(originDimension);
      }

      private static string unknownDimension(string name) => $"Dimension '{name}' not available in DimensionFactory.";

      public bool HasMergingInformation(IDimension sourceDimension, IDimension targetDimension)
      {
         var targetDimensions = (from dimension in _allMergingInformation
            where dimension.SourceDimension == sourceDimension
            select dimension.TargetDimension).Distinct().ToList();

         return targetDimensions.Contains(targetDimension);
      }

      public void RemoveDimension(string dimensionName)
      {
         if (!Has(dimensionName))
            return;

         _dimensions.Remove(dimensionName);
      }

      public void RemoveDimension(IDimension dimension) => RemoveDimension(dimension.Name);

      /// <exception cref="OSPSuiteException">Thrown when no well-defined merging information for the dimensions found</exception>
      public IDimension MergedDimensionFor<T>(T hasDimension) where T : IWithDimension
      {
         if (hasDimension.Dimension == null)
            return null;

         var dimensionsToMerge = (from dimension in _allMergingInformation
            where dimension.SourceDimension.Equals(hasDimension.Dimension)
            select dimension.TargetDimension).Distinct().ToList();

         if (!dimensionsToMerge.Any())
            return hasDimension.Dimension;

         var converters = new List<IDimensionConverter>();
         var targetDimensions = new List<IDimension>();

         foreach (var dimensionToMerge in dimensionsToMerge)
         {
            var conv = CreateConverterFor(hasDimension.Dimension, dimensionToMerge, hasDimension);
            if (conv == null) continue;

            converters.Add(conv);
            targetDimensions.Add(dimensionToMerge);
         }

         //We have a real merged dimension
         var displayName = MergedDimensionNameFor(hasDimension.Dimension);
         return new MergedDimensionFor<T>(hasDimension.Dimension, targetDimensions, converters) { DisplayName = displayName };
      }

      public virtual string MergedDimensionNameFor(IDimension sourceDimension) => sourceDimension.DisplayName;

      public void AddMergingInformation(IDimensionMergingInformation mergingInformation) => _allMergingInformation.Add(mergingInformation);

      public IEnumerable<IDimensionMergingInformation> AllMergingInformation => _allMergingInformation;

      protected virtual IDimensionConverter CreateConverterFor<T>(IDimension dimension, IDimension dimensionToMerge, T hasDimension)
         where T : IWithDimension
      {
         /*should be implemented in derived methods*/
         return null;
      }
   }
}