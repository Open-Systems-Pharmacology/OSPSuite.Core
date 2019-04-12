using System;

namespace OSPSuite.Core.Domain.Descriptors
{
   public class MatchTagCondition : ITagCondition
   {
      public string Tag { get; private set; }

      public string Condition => Tag;

      [Obsolete("For serialization")]
      public MatchTagCondition()
      {
      }

      public override string ToString() => Condition;

      public MatchTagCondition(string tag)
      {
         Tag = tag;
      }

      public bool IsSatisfiedBy(EntityDescriptor entityDescriptor)
      {
         return entityDescriptor.Tags.Contains(Tag);
      }

      public IDescriptorCondition CloneCondition()
      {
         return new MatchTagCondition(Tag);
      }

      public void Replace(string keyword, string replacement)
      {
         if (string.Equals(Tag, keyword))
            Tag = replacement;
      }

      public override bool Equals(object otherObject)
      {
         var other = otherObject as MatchTagCondition;
         if (other == null) return false;
         return Tag.Equals(other.Tag);
      }

      public bool Equals(MatchTagCondition other)
      {
         if (other == null) return false;
         return string.Equals(Tag, other.Tag);
      }

      public override int GetHashCode() => Condition?.GetHashCode() ?? 0;
   }
}