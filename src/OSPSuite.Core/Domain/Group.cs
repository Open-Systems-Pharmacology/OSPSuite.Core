using System;
using System.Collections.Generic;
using System.Linq;

namespace OSPSuite.Core.Domain
{
   public interface IGroup : IWithName, IWithDescription, IWithId, IComparable<IGroup>
   {
      IEnumerable<IGroup> Children { get; }
      string DisplayName { get; set; }
      bool Visible { get; set; }
      string IconName { get; set; }
      IGroup Parent { get; set; }
      IGroup Root { get; }
      int Sequence { get; set; }
      void AddChild(IGroup childGroup);
      bool ContainsGroup(string groupName);
      bool HasAncestorNamed(string groupName);
      bool IsAdvanced { get; set; }
      string PopDisplayName { get; set; }
      string FullName { get; set; }
   }

   public class Group : IGroup
   {
      private readonly IList<IGroup> _children;
      public string Name { get; set; }
      public int Sequence { get; set; }
      public string Description { get; set; }
      public string DisplayName { get; set; }
      public string IconName { get; set; }
      public bool Visible { get; set; }
      public bool IsAdvanced { get; set; }
      public string PopDisplayName { get; set; }
      public string FullName { get; set; }
      public IGroup Parent { get; set; }
      public string Id { get; set; }

      public Group()
      {
         _children = new List<IGroup>();
      }

      public IGroup Root
      {
         get { return Parent != null ? Parent.Root : this; }
      }

      public IEnumerable<IGroup> Children
      {
         get { return _children; }
      }

      public void AddChild(IGroup childGroup)
      {
         _children.Add(childGroup);
         childGroup.Parent = this;
      }

      public bool ContainsGroup(string groupName)
      {
         if (Name.Equals(groupName)) return true;
         return _children.Any(subGroup => subGroup.ContainsGroup(groupName));
      }

      public bool HasAncestorNamed(string groupName)
      {
         if (Name.Equals(groupName)) return true;
         if (Parent == null) return false;
         return Parent.HasAncestorNamed(groupName);
      }

      public override bool Equals(object obj)
      {
         var group = obj as IGroup;
         return group != null && Equals(group.Name, Name);
      }

      public override int GetHashCode()
      {
         return (Name != null ? Name.GetHashCode() : 0);
      }

      public int CompareTo(IGroup other)
      {
         return string.Compare(FullName, other.FullName, StringComparison.Ordinal);
      }

      public override string ToString()
      {
         return Name;
      }
   }
}