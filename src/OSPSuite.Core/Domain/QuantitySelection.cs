using System;

namespace OSPSuite.Core.Domain
{
   public class QuantitySelection
   {
      /// <summary>
      ///    Full path of selected quantity which serves as id for the quantity
      /// </summary>
      public virtual string Path { get; }

      public virtual QuantityType QuantityType { get; }

      [Obsolete("For serialization")]
      public QuantitySelection()
      {
      }

      public QuantitySelection(string path, QuantityType quantityType)
      {
         Path = path;
         QuantityType = quantityType;
      }

      public QuantitySelection Clone()
      {
         return new QuantitySelection(Path, QuantityType);
      }

      public override string ToString()
      {
         return Path;
      }

      public override bool Equals(object obj)
      {
         if (ReferenceEquals(null, obj)) return false;
         if (ReferenceEquals(this, obj)) return true;
         if (obj.GetType() != GetType()) return false;
         return Equals((QuantitySelection) obj);
      }

      protected bool Equals(QuantitySelection other)
      {
         return string.Equals(Path, other.Path);
      }

      public override int GetHashCode()
      {
         return Path?.GetHashCode() ?? 0;
      }
   }
}