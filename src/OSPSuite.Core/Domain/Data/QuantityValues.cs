using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using OSPSuite.Utility.Extensions;
using OSPSuite.Core.Extensions;

namespace OSPSuite.Core.Domain.Data
{
   /// <summary>
   ///    Represents the simulation values for a quantity identified by quantity path
   /// </summary>
   public class QuantityValues
   {
      /// <summary>
      ///    This id is only used for optimal serialization and should not be changed
      /// </summary>
      [EditorBrowsable(EditorBrowsableState.Never)]
      public virtual int Id { get; set; }

      private float[] _values;

      /// <summary>
      ///    Only required because charts requires ColumnId to defined curves
      /// </summary>
      public virtual string ColumnId { get; set; }

      /// <summary>
      ///    Path of the quantity for which values are stored
      /// </summary>
      public virtual string QuantityPath { get; set; }

      /// <summary>
      ///    Simple reference to corresponding time
      /// </summary>
      public virtual QuantityValues Time { get; internal set; }

      public QuantityValues()
      {
         QuantityPath = string.Empty;
         Values = new float[] {};
      }

      public virtual float[] Values
      {
         get { return _values; }
         set { _values = value ?? new float[] {}; }
      }

      public virtual IReadOnlyList<string> PathList
      {
         get { return QuantityPath.ToPathArray().ToList(); }
         set { QuantityPath = value.ToPathString(); }
      }

      public virtual byte[] Data
      {
         get { return Values.ToByteArray(); }
         set { Values = value.ToFloatArray(); }
      }

      /// <summary>
      ///    Returns the number of values defined for the quantities
      /// </summary>
      public virtual int Length => Values?.Length ?? 0;

      /// <summary>
      ///    Returns the value at the given index or <c>float.NaN</c> if the index is out of bound
      /// </summary>
      public virtual float this[int index0] => ValueAt(index0);

      /// <summary>
      ///    Returns the value at the given index or <c>float.NaN</c> if the index is out of bound
      /// </summary>
      public virtual float ValueAt(int index0)
      {
         try
         {
            return Values[index0];
         }
         catch (IndexOutOfRangeException)
         {
            return float.NaN;
         }
      }
   }

   public class NullQuantityValues : QuantityValues
   {
      public NullQuantityValues()
      {
         Time=new QuantityValues();
      }
   }
}