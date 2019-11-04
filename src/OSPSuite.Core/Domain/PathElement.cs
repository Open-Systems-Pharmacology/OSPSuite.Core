using System;
using OSPSuite.Assets;
using OSPSuite.Utility.Collections;
using OSPSuite.Utility.Extensions;

namespace OSPSuite.Core.Domain
{
   /// <summary>
   ///    A cache returning for each path element the corresponding <see cref="PathElement" />
   /// </summary>
   public class PathElements : Cache<PathElementId, PathElement>
   {
      /// <summary>
      /// Allows for categorization of <see cref="PathElements"/> in the UI.
      /// </summary>
      public string Category { get; set; }

      public PathElements() : base(x => new PathElement())
      {
         Category = string.Empty;
      }
   }

   public class PathElement : IComparable, IComparable<PathElement>, IWithDescription
   {
      public string DisplayName { get; set; }
      public string Description { get; set; }
      public string IconName { get; set; }

      public PathElement()
      {
         DisplayName = string.Empty;
         Description = string.Empty;
         IconName = string.Empty;
      }

      public override string ToString()
      {
         return DisplayName;
      }

      public int CompareTo(PathElement other)
      {
         return string.Compare(DisplayName, other.DisplayName, StringComparison.Ordinal);
      }

      public int CompareTo(object obj)
      {
         return CompareTo(obj.DowncastTo<PathElement>());
      }

      public override bool Equals(object obj)
      {
         return Equals(obj as PathElement);
      }

      public bool Equals(PathElement other)
      {
         if (ReferenceEquals(null, other)) return false;
         if (ReferenceEquals(this, other)) return true;
         return Equals(other.DisplayName, DisplayName);
      }

      public override int GetHashCode()
      {
         return
            string.IsNullOrEmpty(DisplayName)
               ? string.Empty.GetHashCode()
               : DisplayName.GetHashCode();
      }
   }

   public class InvalidPathElement : PathElement
   {
      public InvalidPathElement()
      {
         DisplayName = Captions.InvalidObject;
         IconName = IconNames.ERROR;
      }
   }
}