using System;
using System.Collections.Generic;
using System.Linq;
using OSPSuite.Assets;
using OSPSuite.Utility.Collections;
using OSPSuite.Utility.Exceptions;
using OSPSuite.Utility.Reflection;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Core.Extensions;

namespace OSPSuite.Core.Domain.Data
{
   /// <summary>
   ///    A single Column of calculated or imported Data. Values correspond to Values in BaseGrid
   /// </summary>
   /// <remarks>
   ///    If BaseGrid is changed (Count,Value) the using Application has to handle changes on column
   ///    DataColumn can belong only to one DataRepository.
   /// </remarks>
   public class DataColumn : IWithId, IWithName, IWithDisplayUnit
   {
      private readonly Cache<AuxiliaryType, DataColumn> _relatedColumnsCache = new Cache<AuxiliaryType, DataColumn>(x => x.DataInfo.AuxiliaryType);
      public QuantityInfo QuantityInfo { get; set; }
      public DataInfo DataInfo { get; set; }
      public string Id { get; set; }
      public string Name { get; set; }
      public BaseGrid BaseGrid { get; set; }
      private WeakRef<DataRepository> _repository;
      protected List<float> _values;
      public IDimension Dimension { get; set; }

      public bool ColumnValueIsBelowLLOQ(int rowIndex)
      {
         return rowIndex < Values.Count &&  DataInfo.LLOQ != null && this[rowIndex] < DataInfo.LLOQ.Value;
      }
      /// <summary>
      ///    Indicates whether the column should be displayed by default or is use for internal use only
      /// </summary>
      public bool IsInternal { get; set; }

      private List<float> _cachedValues;

      public DataColumn() : this(string.Empty, Constants.Dimension.NO_DIMENSION, null)
      {
      }

      public DataColumn(string name, IDimension dimension, BaseGrid baseGrid)
         : this(Guid.NewGuid().ToString(), name, dimension, baseGrid)
      {
      }

      public DataColumn(string id, string name, IDimension dimension, BaseGrid baseGrid)
      {
         Id = id;
         Name = name;
         Dimension = dimension;
         BaseGrid = baseGrid;
         QuantityInfo = new QuantityInfo(string.Empty, new List<string>(), QuantityType.Undefined);
         var defaultUnitName = dimension != null ? dimension.DefaultUnitName : string.Empty;
         DataInfo = new DataInfo(ColumnOrigins.Undefined) {DisplayUnitName = defaultUnitName};
         IsInternal = false;
      }

      public DataRepository Repository
      {
         set => _repository = value == null ? null : new WeakRef<DataRepository>(value);
         get => _repository?.Target;
      }

      public virtual bool IsInRepository() => _repository?.Target != null;

      public virtual float this[int index]
      {
         get
         {
            if (Values == null)
               return float.NaN;

            return Values[index];
         }
         set
         {
            if (Values == null)
               Values = new float[BaseGrid.Count].ToList();

            //values is set now
            _values[index] = value;
         }
      }

      /// <summary>
      ///    Returns true if the column values only contain one element otherwise false
      /// </summary>
      public virtual bool HasSingleValue => _values?.Count == 1;

      /// <summary>
      ///    Set or gets the values of the given columns. Always returns an array having the same dimension as the base grid
      /// </summary>
      public virtual IReadOnlyList<float> Values
      {
         get
         {
            //most common case: length of values is equal to the length of base grid
            //than just return the array
            if ((BaseGrid == null) || (_values == null) || (_values.Count == BaseGrid.Count))
               return _values;

            //2nd case: constant array. create array with the same size as base grid
            //          and fill it with the (only) value

            if (_values.Count != 1) 
               throw new OSPSuiteException(Error.WrongColumnDimensions(Name, _values.Count, BaseGrid.Count));

            if (_cachedValues == null)
               _cachedValues = new List<float>(new float[BaseGrid.Count].InitializeWith(_values[0]));

            return _cachedValues;
         }
         set
         {
            if (value != null && BaseGrid != null && value.Count != BaseGrid.Count && value.Count != 1)
               throw new ArgumentException($"Values.Length = {value.Count} != {BaseGrid.Count} = BaseGrid.Count");

            _values = value?.ToList();
            _cachedValues = null;
         }
      }

      /// <summary>
      ///    Returns the internal array of values. Its size is in either 1 (constant array) or equal to baseGrid dimension
      /// </summary>
      public virtual List<float> InternalValues
      {
         get => _values;
         internal set => _values = value;
      }

      public string PathAsString => QuantityInfo.PathAsString;

      public virtual void AddRelatedColumn(DataColumn relatedColumn)
      {
         if (!relatedColumn.BaseGrid.Equals(BaseGrid))
            throw new InvalidArgumentException($"Different BaseGrid {relatedColumn.BaseGrid.Name} instead of {BaseGrid.Name}");

         switch (relatedColumn.DataInfo.AuxiliaryType)
         {
            case AuxiliaryType.ArithmeticStdDev:
               validateDimensionIs(AuxiliaryType.ArithmeticStdDev, relatedColumn.Dimension, Dimension);
               break;
            case AuxiliaryType.GeometricStdDev:
               validateDimensionIsDimensionsLess(AuxiliaryType.GeometricStdDev, relatedColumn.Dimension);
               break;
            case AuxiliaryType.ArithmeticMeanPop:
               validateDimensionIs(AuxiliaryType.ArithmeticMeanPop, relatedColumn.Dimension, Dimension);
               break;
            case AuxiliaryType.GeometricMeanPop:
               if(!Dimension.IsEquivalentTo(Constants.Dimension.NO_DIMENSION))
                  validateDimensionIs(AuxiliaryType.GeometricMeanPop, relatedColumn.Dimension, Dimension);
               break;
         }
         _relatedColumnsCache.Add(relatedColumn);
      }

      private static void validateDimensionIsDimensionsLess(AuxiliaryType auxiliaryType, IDimension dimension)
      {
         if (!dimension.IsEquivalentTo(Constants.Dimension.NO_DIMENSION))
            throw new InvalidArgumentException($"Wrong Dimension for {auxiliaryType}: {dimension.Name} instead of Dimensionless");
      }

      private static void validateDimensionIs(AuxiliaryType auxiliaryType, IDimension dimension, IDimension targetDimension)
      {
         if (!dimension.Equals(targetDimension))
            throw new InvalidArgumentException($"Wrong Dimension for {auxiliaryType}: {dimension.Name} instead of {targetDimension.Name}");
      }

      public virtual void RemoveRelatedColumn(AuxiliaryType auxiliaryType)
      {
         _relatedColumnsCache.Remove(auxiliaryType);
      }

      public virtual bool ContainsRelatedColumn(AuxiliaryType auxiliaryType)
      {
         return _relatedColumnsCache.Contains(auxiliaryType);
      }

      public virtual DataColumn GetRelatedColumn(AuxiliaryType auxiliaryType)
      {
         return _relatedColumnsCache[auxiliaryType];
      }

      public virtual IReadOnlyCollection<DataColumn> RelatedColumns => _relatedColumnsCache;

      public virtual float GetValue(float baseValue)
      {
         if (Values == null || !Values.Any())
            return float.NaN;

         int index = BaseGrid.IndexOf(baseValue);
         if (index >= 0)
            return Values[index];

         int leftIndex = BaseGrid.LeftIndexOf(baseValue);
         int rightIndex = BaseGrid.RightIndexOf(baseValue);

         if (leftIndex < 0)
            return float.NaN;

         if (rightIndex >= BaseGrid.Count)
            return float.NaN;

         if (leftIndex == rightIndex)
            return float.NaN;

         return Values[leftIndex] +
                (Values[rightIndex] - Values[leftIndex]) * (baseValue - BaseGrid[leftIndex]) /
                (BaseGrid[rightIndex] - BaseGrid[leftIndex]);
      }

      internal Cache<AuxiliaryType, DataColumn> RelatedColumnsCache => _relatedColumnsCache;

      public override string ToString()   
      {
         return Name;
      }

      internal void InsertValueAt(int index, float value)
      {
         _values.Insert(index, value);
      }

      public Unit DisplayUnit
      {
         get => Dimension.UnitOrDefault(DataInfo.DisplayUnitName);
         set => DataInfo.DisplayUnitName = value == null ? Dimension.DefaultUnit.Name : value.Name;
      }

      internal void RemoveValueAt(int index)
      {
         _values.RemoveAt(index);
      }

      /// <summary>
      ///    Returns the full path minus the first element for a <see cref="ColumnOrigins.Calculation" /> column or the full path
      ///    otherwise
      /// </summary>
      public string TemplatePath
      {
         get
         {
            if (QuantityInfo == null)
               return string.Empty;

            if (DataInfo == null || DataInfo.Origin != ColumnOrigins.Calculation)
               return QuantityInfo.PathAsString;

            return QuantityInfo.Path.Skip(1).ToPathString();
         }
      }
   }
}