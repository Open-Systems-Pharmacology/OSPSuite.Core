using System.Collections.Generic;
using OSPSuite.Utility.Extensions;
using OSPSuite.Core.Domain.UnitSystem;

namespace OSPSuite.Core.Domain
{
   public interface IFormulaUsablePath : IWithDimension, IObjectPath
   {
      string Alias { get; set; }
   }

   public class FormulaUsablePath : ObjectPath, IFormulaUsablePath
   {
      public string Alias { get; set; }
      public IDimension Dimension { get; set; }

      public FormulaUsablePath()
      {
      }

      public FormulaUsablePath(params string[] pathEntries) : base(pathEntries)
      {
      }

      public FormulaUsablePath(IEnumerable<string> pathEntries) : base(pathEntries)
      {
      }

      public override T Clone<T>()
      {
         return new FormulaUsablePath(_pathEntries)
         {
            Alias = Alias,
            Dimension = Dimension
         }
            .DowncastTo<T>();
      }

      public bool Equals(FormulaUsablePath other)
      {
         if (ReferenceEquals(null, other)) return false;
         if (ReferenceEquals(this, other)) return true;
         return base.Equals(other) && string.Equals(Alias, other.Alias) && Equals(Dimension, other.Dimension);
      }

      public override bool Equals(object obj)
      {
         if (ReferenceEquals(null, obj)) return false;
         if (ReferenceEquals(this, obj)) return true;
         return Equals(obj as FormulaUsablePath);
      }

      public override int GetHashCode()
      {
         unchecked
         {
            int hashCode = base.GetHashCode();
            hashCode = (hashCode * 397) ^ (Alias != null ? Alias.GetHashCode() : 0);
            hashCode = (hashCode * 397) ^ (Dimension != null ? Dimension.GetHashCode() : 0);
            return hashCode;
         }
      }
   }
}