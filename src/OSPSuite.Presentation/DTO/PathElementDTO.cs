using System;
using OSPSuite.Assets;
using OSPSuite.Utility.Extensions;
using OSPSuite.Core.Domain;

namespace OSPSuite.Presentation.DTO
{
   public class PathElementDTO : IComparable, IComparable<PathElementDTO>, IWithDescription
   {
      public string DisplayName { get; set; }
      public string Description { get; set; }
      public string IconName { get; set; }

      public PathElementDTO()
      {
         DisplayName = string.Empty;
         Description = string.Empty;
         IconName = string.Empty;
      }

      public override string ToString()
      {
         return DisplayName;
      }

      public int CompareTo(PathElementDTO other)
      {
         return string.Compare(DisplayName, other.DisplayName, StringComparison.Ordinal);
      }

      public int CompareTo(object obj)
      {
         return CompareTo(obj.DowncastTo<PathElementDTO>());
      }

      public override bool Equals(object obj)
      {
         return Equals(obj as PathElementDTO);
      }

      public bool Equals(PathElementDTO other)
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

   public class InvalidPathElementDTO : PathElementDTO
   {
      public InvalidPathElementDTO()
      {
         DisplayName = Captions.InvalidObject;
         IconName = ApplicationIcons.Error.IconName;
      }
   }
}