using System;
using System.Collections.Generic;
using System.Linq;
using OSPSuite.Utility.Collections;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.UnitSystem;

namespace OSPSuite.Core.Converter.v5_2
{
   /// <summary>
   ///    Special dimension factory that is used for deserialization only. It contains older dimensions that
   ///    are required only for deserialization
   /// </summary>
   public interface ISerializationDimensionFactory : IDimensionFactory
   {
   }

   public class SerializationDimensionFactory : ISerializationDimensionFactory
   {
      private readonly IDimensionFactory _dimensionFactory;
      private readonly ICache<string, IDimension> _allDimensions;

      public SerializationDimensionFactory(IDimensionFactory dimensionFactory, IDimensionMapper dimensionMapper)
      {
         _dimensionFactory = dimensionFactory;
         _allDimensions = new Cache<string, IDimension>(x => x.Name);
         _allDimensions.AddRange(dimensionMapper.DummyDimensionsForConversion);
      }

      public IDimension MergedDimensionFor<T>(T hasDimension) where T : IWithDimension
      {
         return _dimensionFactory.MergedDimensionFor(hasDimension);
      }

      public IDimension AddDimension(BaseDimensionRepresentation baseRepresentation, string dimensionName, string baseUnitName)
      {
         throw new InvalidOperationException("Should never be called");
      }

      public void AddDimension(IDimension dimension)
      {
         throw new InvalidOperationException("Should never be called");
      }

      public void AddMergingInformation(IDimensionMergingInformation mergingInformation)
      {
         throw new InvalidOperationException("Should never be called");
      }

      public IEnumerable<string> DimensionNames => _allDimensions.Keys.Union(_dimensionFactory.DimensionNames);

      public IDimension Dimension(string name)
      {
         if (string.IsNullOrEmpty(name))
            return NoDimension;

         if (_allDimensions.Contains(name))
            return _allDimensions[name];

         return _dimensionFactory.Dimension(name);
      }

      public void Clear()
      {
         throw new InvalidOperationException("Should never be called");
      }

      public IEnumerable<IDimension> Dimensions
      {
         get { return _allDimensions; }
      }

      public IDimension NoDimension
      {
         get { return _dimensionFactory.NoDimension; }
      }

      public void RemoveDimension(string dimensionName)
      {
         throw new InvalidOperationException("Should never be called");
      }

      public void RemoveDimension(IDimension dimension)
      {
         throw new InvalidOperationException("Should never be called");
      }
   }
}