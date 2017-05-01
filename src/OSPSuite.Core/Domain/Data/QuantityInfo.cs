using System;
using System.Collections.Generic;
using OSPSuite.Core.Extensions;

namespace OSPSuite.Core.Domain.Data
{
   public class QuantityInfo 
   {
      private IList<string> _path;

      /// <summary>
      ///   Typically name of the underlying quantity. This will only be used and displayed in the 
      ///   DataColumn QuantityName from the DataBrowser
      /// </summary>
      public string Name { get; set; }

      /// <summary>
      ///   Typically the quantity type such as species, observer etc..This will only be used and displayed in the 
      ///   DataColumn QuantityType from the DataBrowser
      /// </summary>
      public QuantityType Type { get; set; }

      /// <summary>
      ///   Used for sorting in lists and trees.
      /// </summary>
      public int OrderIndex { get; set; }

      [Obsolete("For serialization")]
      public QuantityInfo() : this(string.Empty, new List<string>(), QuantityType.Undefined)
      {
      }

      /// <summary>
      ///   Create a new quantity info
      /// </summary>
      /// <param name = "name">Name of quantity but could be set to sthg else. Only used in DataColumn.QuantityName</param>
      /// <param name = "quantityPath">Full path of quantity</param>
      /// <param name = "quantityType">Type of quantity (such as species, observer, parameter). </param>
      public QuantityInfo(string name, IEnumerable<string> quantityPath, QuantityType quantityType)
      {
         Name = name;
         Type = quantityType;
         _path = new List<string>(quantityPath);
      }

      /// <summary>
      ///   Returns the full path as string
      /// </summary>
      public string PathAsString => _path.ToPathString();

      /// <summary>
      ///   Full path of quantity for which data were saved.
      /// </summary>
      public IEnumerable<string> Path
      {
         get { return _path; }
         set { _path = new List<string>(value); }
      }

      /// <summary>
      ///   Returns a clone of the quantity info
      /// </summary>
      public QuantityInfo Clone()
      {
         return new QuantityInfo(Name, _path, Type);
      }

      public override bool Equals(object obj)
      {
         if (ReferenceEquals(null, obj)) return false;
         if (ReferenceEquals(this, obj)) return true;
         if (obj.GetType() != typeof (QuantityInfo)) return false;
         return Equals((QuantityInfo) obj);
      }

      public bool Equals(QuantityInfo other)
      {
         if (ReferenceEquals(null, other)) return false;
         if (ReferenceEquals(this, other)) return true;
         return Equals(other.PathAsString, PathAsString);
      }

      public override int GetHashCode()
      {
         return PathAsString?.GetHashCode() ?? 0;
      }

      public override string ToString()
      {
         return PathAsString;
      }
   }
}