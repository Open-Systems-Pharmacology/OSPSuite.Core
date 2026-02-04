using System;
using System.Collections.Generic;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Utility.Extensions;

namespace OSPSuite.Core.Domain.Data
{
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
         DataInfo = new DataInfo(ColumnOrigins.BaseGrid) { DisplayUnitName = defaultUnitName };
         QuantityInfo.Type = QuantityType.BaseGrid;
      }

      public override IReadOnlyList<float> Values
      {
         get => _values;
         set
         {
            _values.Clear();
            _values.AddRange(value);
         }
      }

      public override List<float> InternalValues
      {
         get => _values;
         internal set => _values = value;
      }

      public override bool HasSingleValue => false;

      public virtual bool Remove(float value)
      {
         return _values.Remove(value);
      }

      public virtual void Clear()
      {
         _values.Clear();
      }

      public virtual int Count => _values.Count;

      public virtual int IndexOf(float value)
      {
         return _values.IndexOf(value);
      }

      public virtual int IndexOfNextLowest(float value)
      {
         int nextLowestIndex = -1;
         Values.Each((x, i) =>
         {
            if(x <= value && (nextLowestIndex < 0 || x > Values[nextLowestIndex]))
               nextLowestIndex = i;
         });

         return nextLowestIndex;
      }

      public virtual int IndexOfNextHighest(float value)
      {
         int nextHighestIndex = Values.Count;
         Values.Each((x, i) =>
         {
            if (x >= value && (nextHighestIndex >= Values.Count || x < Values[nextHighestIndex]))
               nextHighestIndex = i;
         });

         return nextHighestIndex;
      }
   }
}