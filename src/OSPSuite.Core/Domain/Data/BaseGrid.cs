﻿using System;
using System.Collections.Generic;
using OSPSuite.Assets;
using OSPSuite.Core.Domain.UnitSystem;

namespace OSPSuite.Core.Domain.Data
{
   /// <summary>
   ///    BaseGrid must be a strictly monotonic increasing sequence
   /// </summary>
   public class BaseGrid : DataColumn
   {
      [Obsolete("For serialization")]
      public BaseGrid() : this(Guid.NewGuid().ToString(), string.Empty, null)
      {
         //for deserialization
      }

      public BaseGrid(string name, IDimension dimension)
         : this(Guid.NewGuid().ToString(), name, dimension)
      {
      }

      public BaseGrid(string id, string name, IDimension dimension) : base(id, name, dimension, null)
      {
         BaseGrid = this;
         _values = new List<float>();
         var defaultUnitName = dimension != null ? dimension.DefaultUnitName : string.Empty;
         DataInfo = new DataInfo(ColumnOrigins.BaseGrid) {DisplayUnitName = defaultUnitName};
         QuantityInfo.Type = QuantityType.BaseGrid;
      }

      public override IReadOnlyList<float> Values
      {
         get { return _values; }
         set
         {
            //check strong monotony of values first (values are sorted and unique)
            for (int i = 1; i < value.Count; i++)
            {
               if (value[i] <= value[i - 1])
                  throw new InvalidArgumentException(Error.TimeNotStrictlyMonotone);
            }

            _values.Clear();
            _values.AddRange(value);
         }
      }

      public override List<float> InternalValues
      {
         get { return _values; }
         internal set { _values = value; }
      }

      public override bool HasSingleValue
      {
         get { return false; }
      }

      /// <summary>
      /// Insert the given values and returns the index where the value should be inserted
      /// </summary>
      public virtual void Insert(float value)
      {
         int index = _values.BinarySearch(value);
         if (index < 0) _values.Insert(~index, value);
      }

      internal virtual int InsertOrReplace(float value, out bool replaced)
      {
         int index = _values.BinarySearch(value);
         replaced = true;
         if (index < 0)
         {
            index = ~index;
            _values.Insert(index, value);
            replaced = false;
         }
         return index;

      }
      public virtual void InsertInterval(float start, float end, int numberOfTimePoints)
      {
         if (end <= start) throw new InvalidArgumentException("end <= start");
         if (numberOfTimePoints < 2) throw new InvalidArgumentException("numberOfTimePoints < 2");

         float timeInterval = (end - start) / (numberOfTimePoints - 1);
         for (int i = 0; i < numberOfTimePoints - 1; i++)
         {
            Insert(start + i * timeInterval);
         }
         Insert(end);
      }

      public virtual bool Remove(float value)
      {
         return _values.Remove(value);
      }

      public virtual void Clear()
      {
         _values.Clear();
      }

      public virtual int Count
      {
         get { return _values.Count; }
      }

      public virtual int IndexOf(float value)
      {
         return _values.IndexOf(value);
      }

      public virtual int LeftIndexOf(float value)
      {
         int index = _values.BinarySearch(value);
         return index >= 0 ? index : ~index - 1;
      }

      public virtual int RightIndexOf(float value)
      {
         int index = _values.BinarySearch(value);
         return index >= 0 ? index : ~index;
      }
   }
}